using Common.HttpResults;
using Enums;
using Exceptions;
using Interfaces.Audit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Diagnostics;

namespace Middlewares
{
    public class ExceptionHandlingMiddleware(
        ILogger<ExceptionHandlingMiddleware> logger,
        IAuditLogWriter auditLogWriter,
        IUnitOfWork unitOfWork) : IMiddleware
    {
        private const string InternalServerErrorMessage = "Internal server error";

        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (OperationCanceledException ex)
            {
                await ExecuteMappedExceptionAsync(
                    context,
                    ex,
                    CreateErrorResult("Request was canceled", StatusCodes.Status499ClientClosedRequest),
                    logAsInternal: false);
            }
            catch (ValidationFailureException ex)
            {
                IActionResult result = ex.FieldErrors.Any()
                    ? Result.FailFieldErrors(ex.FieldErrors, "Validation failed", StatusCodes.Status400BadRequest)
                    : Result.FailErrors(
                        ex.GlobalErrors.Any() ? ex.GlobalErrors : ["Validation failed"],
                        "Validation failed",
                        StatusCodes.Status400BadRequest);

                await ExecuteMappedExceptionAsync(context, ex, result, logAsInternal: false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await ExecuteMappedExceptionAsync(
                    context,
                    ex,
                    CreateErrorResult(
                        "The record was changed or deleted by another request.",
                        StatusCodes.Status409Conflict),
                    logAsInternal: false);
            }
            catch (DbUpdateException ex)
            {
                var result = TryCreateSqlErrorResult(ex, out var logAsInternal)
                    ?? CreateErrorResult(InternalServerErrorMessage, StatusCodes.Status500InternalServerError);

                await ExecuteMappedExceptionAsync(context, ex, result, logAsInternal);
            }
            catch (Exception ex)
            {
                var (result, logAsInternal) = MapException(ex);
                await ExecuteMappedExceptionAsync(context, ex, result, logAsInternal);
            }
        }

        private static (IActionResult Result, bool LogAsInternal) MapException(Exception exception)
        {
            return exception switch
            {
                UserFacingException ex => (
                    CreateErrorResult(ex.Message, ex.StatusCode),
                    false),
                UnauthorizedAccessException ex => (
                    CreateErrorResult(ex.Message, StatusCodes.Status401Unauthorized),
                    false),
                InvalidDataException ex => (
                    CreateErrorResult(ex.Message, StatusCodes.Status400BadRequest),
                    false),
                TimeoutException => (
                    CreateErrorResult("Service request timeout", StatusCodes.Status504GatewayTimeout),
                    true),
                InternalAppException => (
                    CreateErrorResult(InternalServerErrorMessage, StatusCodes.Status500InternalServerError),
                    true),
                ArgumentNullException => (
                    CreateErrorResult(InternalServerErrorMessage, StatusCodes.Status500InternalServerError),
                    true),
                InvalidOperationException => (
                    CreateErrorResult(InternalServerErrorMessage, StatusCodes.Status500InternalServerError),
                    true),
                _ => (
                    CreateErrorResult(InternalServerErrorMessage, StatusCodes.Status500InternalServerError),
                    true)
            };
        }

        private static IActionResult? TryCreateSqlErrorResult(
            DbUpdateException exception,
            out bool logAsInternal)
        {
            logAsInternal = true;

            if (exception.GetBaseException() is not SqlException sqlException)
            {
                return null;
            }

            logAsInternal = sqlException.Number switch
            {
                2601 or 2627 or 547 or 515 or 8152 or 2628 => false,
                _ => true
            };

            return sqlException.Number switch
            {
                2601 or 2627 => CreateErrorResult(
                    "A record with the same value already exists.",
                    StatusCodes.Status409Conflict),
                547 => CreateErrorResult(
                    "The request references related data that does not exist or cannot be changed.",
                    StatusCodes.Status400BadRequest),
                515 => CreateErrorResult(
                    "Required data is missing.",
                    StatusCodes.Status400BadRequest),
                8152 or 2628 => CreateErrorResult(
                    "One or more values exceed the allowed length.",
                    StatusCodes.Status400BadRequest),
                _ => CreateErrorResult(
                    InternalServerErrorMessage,
                    StatusCodes.Status500InternalServerError)
            };
        }

        private static IActionResult CreateErrorResult(string message, int statusCode)
        {
            return Result.FailErrors([message], message, statusCode);
        }

        private async Task ExecuteMappedExceptionAsync(
            HttpContext context,
            Exception exception,
            IActionResult result,
            bool logAsInternal)
        {
            if (logAsInternal)
            {
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", traceId);
            }

            if (exception is UnauthorizedAccessException unauthEx)
            {
                await _auditLogWriter.LogAnonymousAsync(
                    AuditLogCategory.Security,
                    "UnauthorizedAccessException",
                    cancellationToken: CancellationToken.None
                );

                await _unitOfWork.SaveChangeAsync();
            }

            await Execute(context, result);
        }

        private static async Task Execute(HttpContext context, IActionResult result)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            context.Response.Headers["x-trace-id"] = traceId;

            var actionContext = new ActionContext { HttpContext = context };
            await result.ExecuteResultAsync(actionContext);
        }
    }
}
