using System.Net;
using Enums;

namespace Emails
{
    public class EmailTemplateBuilder
    {
        public EmailTemplate BuildWelcomeEmail(string appName, string displayName)
        {
            var safeAppName = Html(appName);
            var safeDisplayName = Html(displayName);

            var html = WrapHtml(
                safeAppName,
                $"Welcome to {safeAppName}",
                $"""
                <div style="text-align:center;margin:0 0 24px;">
                  <div style="font-size:46px;line-height:1;">🎉</div>
                </div>

                <p style="margin:0 0 16px;">Hi {safeDisplayName},</p>

                <p style="margin:0 0 16px;">
                  Welcome to <strong>{safeAppName}</strong>. Your account has been created successfully.
                </p>

                <div style="background:#ecfdf5;border:1px solid #bbf7d0;padding:16px 18px;
                            border-radius:10px;margin:24px 0;">
                  <div style="font-weight:700;color:#065f46;">Your account is ready</div>
                  <div style="font-size:14px;color:#047857;margin-top:4px;">
                    You can now sign in and start using the platform.
                  </div>
                </div>

                <p style="margin:0;color:#6b7280;">
                  If you need help, please contact your administrator.
                </p>
                """);

            var text = $"""
                Welcome to {appName}

                Hi {displayName},

                Your {appName} account has been created successfully.
                You can now sign in and start using the platform.
                """;

            return new EmailTemplate($"Welcome to {appName}", html, text);
        }

        public EmailTemplate BuildOtpEmail(string appName, string otpCode, DateTime expiresAt)
        {
            var safeAppName = Html(appName);
            var safeOtpCode = Html(otpCode);
            var expiryText = FormatUtc(expiresAt);

            var html = WrapHtml(
                safeAppName,
                "Your verification code",
                $"""
                <p style="margin:0 0 16px;">
                  Use this verification code to continue with <strong>{safeAppName}</strong>.
                </p>

                <div style="text-align:center;margin:32px 0;">
                  <div style="display:inline-block;background:#f3f6fb;border:2px dashed #0078d4;
                              border-radius:14px;padding:20px 32px;font-size:36px;font-weight:800;
                              letter-spacing:8px;color:#0078d4;">
                    {safeOtpCode}
                  </div>
                </div>

                <div style="background:#fff7ed;border-left:4px solid #f97316;padding:16px 18px;
                            border-radius:8px;margin:24px 0;">
                  <div style="font-size:13px;color:#9a3412;margin-bottom:4px;">This code expires at</div>
                  <div style="font-size:15px;font-weight:700;color:#7c2d12;">{Html(expiryText)}</div>
                </div>

                <p style="margin:0;color:#6b7280;">
                  If you did not request this code, you can safely ignore this email.
                </p>
                """);

            var text = $"""
                Your verification code

                Use this verification code to continue with {appName}: {otpCode}
                This code expires at {expiryText}.

                If you did not request this code, you can safely ignore this email.
                """;

            return new EmailTemplate("Your verification code", html, text);
        }

        public EmailTemplate BuildAutoDeductSuccessEmail(string appName, string displayName, decimal deductedAmount, decimal remainingOutstanding)
        {
            var safeAppName = Html(appName);
            var safeDisplayName = Html(displayName);

            var html = WrapHtml(
                safeAppName,
                "Outstanding Fees Auto-Deduction Report",
                $"""
                <p style="margin:0 0 16px;">Hi {safeDisplayName},</p>

                <p style="margin:0 0 16px;">
                  We have automatically processed the monthly deduction from your top-up balance to settle your outstanding fees.
                </p>

                <div style="background:#ecfdf5;border:1px solid #bbf7d0;padding:16px 18px;
                            border-radius:10px;margin:24px 0;">
                  <div style="font-weight:700;color:#065f46;">Deduction Details</div>
                  <div style="font-size:14px;color:#047857;margin-top:4px;">
                    Amount Deducted: <strong>${deductedAmount:N2}</strong>
                  </div>
                  <div style="font-size:14px;color:#047857;margin-top:4px;">
                    Remaining Outstanding: <strong>${remainingOutstanding:N2}</strong>
                  </div>
                </div>

                <p style="margin:0;color:#6b7280;">
                  Thank you. If you have any questions, please contact the finance administrator.
                </p>
                """);

            var text = $"""
                Outstanding Fees Auto-Deduction Report

                Hi {displayName},

                We have automatically processed the monthly deduction from your top-up balance to settle your outstanding fees.

                Amount Deducted: ${deductedAmount:N2}
                Remaining Outstanding: ${remainingOutstanding:N2}

                Thank you.
                """;

            return new EmailTemplate("Outstanding Fees Auto-Deduction Report", html, text);
        }

        public EmailTemplate BuildPaymentReminderEmail(string appName, string displayName, decimal totalOutstanding)
        {
            var safeAppName = Html(appName);
            var safeDisplayName = Html(displayName);

            var html = WrapHtml(
                safeAppName,
                "Payment Reminder: Outstanding Fees",
                $"""
                <p style="margin:0 0 16px;">Hi {safeDisplayName},</p>

                <p style="margin:0 0 16px;">
                  This is a reminder that you have outstanding fees that require your attention.
                </p>

                <div style="background:#fff7ed;border-left:4px solid #f97316;padding:16px 18px;
                            border-radius:8px;margin:24px 0;">
                  <div style="font-size:13px;color:#9a3412;margin-bottom:4px;">Outstanding Amount Due</div>
                  <div style="font-size:18px;font-weight:700;color:#7c2d12;">${totalOutstanding:N2}</div>
                </div>

                <p style="margin:0 0 16px;">
                  Please top up your education account balance to enable auto-deduction, or make payment as soon as possible.
                </p>

                <p style="margin:0;color:#6b7280;">
                  If you have already paid, please ignore this email.
                </p>
                """);

            var text = $"""
                Payment Reminder: Outstanding Fees

                Hi {displayName},

                This is a reminder that you have outstanding fees of ${totalOutstanding:N2} that require your attention.
                Please top up your education account balance to enable auto-deduction, or make payment as soon as possible.

                If you have already paid, please ignore this email.
                """;

            return new EmailTemplate("Payment Reminder: Outstanding Fees", html, text);
        }

        public EmailTemplate BuildEducationAccountCreditedEmail(
            string displayName,
            decimal creditAmount,
            string reason,
            DateTime creditedAt,
            decimal availableBalance,
            string portalLink)
        {
            var safeDisplayName = Html(displayName);
            var safeReason = Html(reason);
            var creditedDateText = creditedAt.ToString("dd MMM yyyy");
            var safePortalLink = Html(portalLink);
            var creditAmountText = $"SGD {creditAmount:N2}";
            var availableBalanceText = $"SGD {availableBalance:N2}";

            var html = $"""
                <!doctype html>
                <html lang="en">
                <head>
                  <meta charset="utf-8">
                  <meta name="viewport" content="width=device-width, initial-scale=1">
                  <title>Credit added to your Education Account</title>
                </head>
                <body style="margin:0;padding:0;background:#f5f7fb;color:#1f2937;font-family:Segoe UI,Arial,Helvetica,sans-serif;">
                  <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="background:#f5f7fb;margin:0;padding:0;">
                    <tr>
                      <td align="center" style="padding:36px 16px;">
                        <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="max-width:600px;background:#ffffff;border-radius:12px;overflow:hidden;border:1px solid #e5e7eb;box-shadow:0 18px 44px rgba(15,23,42,0.08);">
                          <tr>
                            <td style="height:6px;background:#16a34a;font-size:0;line-height:0;">&nbsp;</td>
                          </tr>
                          <tr>
                            <td align="center" style="padding:38px 34px 22px;">
                              <div style="font-size:26px;line-height:1.25;font-weight:800;color:#0f5132;">MOE Support Team</div>
                              <div style="margin-top:6px;font-size:14px;line-height:1.4;color:#6b7280;">Education Account Management</div>
                              <div style="margin:28px auto 18px;width:76px;height:76px;border-radius:38px;background:#dcfce7;color:#15803d;text-align:center;font-size:34px;line-height:76px;font-weight:800;">$</div>
                              <h1 style="margin:0;font-size:28px;line-height:1.25;font-weight:800;color:#111827;">Credit added to your Education Account</h1>
                              <p style="margin:14px auto 0;max-width:480px;color:#6b7280;font-size:15px;line-height:1.7;">
                                Hello {safeDisplayName}, credit has been added to your Education Account. You may use this balance to pay eligible course fees.
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding:0 34px 34px;">
                              <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="border:1px solid #e5e7eb;border-radius:10px;overflow:hidden;">
                                <tr>
                                  <td style="padding:16px 18px;border-bottom:1px solid #eef2f7;color:#6b7280;font-size:14px;">Credit Amount</td>
                                  <td align="right" style="padding:16px 18px;border-bottom:1px solid #eef2f7;color:#111827;font-size:14px;font-weight:700;">{Html(creditAmountText)}</td>
                                </tr>
                                <tr>
                                  <td style="padding:16px 18px;border-bottom:1px solid #eef2f7;color:#6b7280;font-size:14px;">Reason</td>
                                  <td align="right" style="padding:16px 18px;border-bottom:1px solid #eef2f7;color:#111827;font-size:14px;font-weight:700;">{safeReason}</td>
                                </tr>
                                <tr>
                                  <td style="padding:16px 18px;border-bottom:1px solid #eef2f7;color:#6b7280;font-size:14px;">Date Credited</td>
                                  <td align="right" style="padding:16px 18px;border-bottom:1px solid #eef2f7;color:#111827;font-size:14px;font-weight:700;">{Html(creditedDateText)}</td>
                                </tr>
                                <tr>
                                  <td style="padding:16px 18px;color:#6b7280;font-size:14px;">Available Balance</td>
                                  <td align="right" style="padding:16px 18px;color:#15803d;font-size:14px;font-weight:800;">{Html(availableBalanceText)}</td>
                                </tr>
                              </table>

                              <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="margin-top:28px;">
                                <tr>
                                  <td align="center">
                                    <a href="{safePortalLink}" style="display:block;background:#16a34a;color:#ffffff;text-decoration:none;padding:15px 20px;border-radius:8px;font-size:15px;font-weight:700;">View Education Account</a>
                                  </td>
                                </tr>
                              </table>

                              <p style="margin:30px 0 0;text-align:center;color:#6b7280;font-size:14px;line-height:1.6;">
                                If you have questions about this credit, please contact MOE Support Team.
                              </p>
                              <p style="margin:14px 0 0;text-align:center;color:#9ca3af;font-size:12px;line-height:1.5;">
                                This is an automated email from MOE Support Team. Please do not reply directly to this email.
                              </p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>
                """;

            var text = $"""
                Credit added to your Education Account

                Hello {displayName},

                {creditAmountText} has been added to your Education Account.

                Reason: {reason}
                Date Credited: {creditedDateText}
                Available Balance: {availableBalanceText}

                View your Education Account: {portalLink}

                Thank you,
                MOE Support Team
                """;

            return new EmailTemplate("Credit added to your Education Account", html, text);
        }

        public EmailTemplate BuildCourseEnrollmentConfirmedEmail(
            string displayName,
            string courseName,
            DateTime courseStartDate,
            DateTime courseEndDate,
            string portalLink)
        {
            return BuildModernEmail(
                "You've been enrolled in " + courseName,
                "You've been enrolled",
                $"Hello {displayName}, you have been enrolled in {courseName}. You can view the course details from your e-Service portal.",
                "#0b5cab",
                BuildDetails(
                    ("Course", courseName, false),
                    ("Course Start Date", FormatDate(courseStartDate), false),
                    ("Course End Date", FormatDate(courseEndDate), false)),
                "View course details",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildEnrollmentRemovedEmail(
            string displayName,
            string courseName,
            string portalLink)
        {
            return BuildModernEmail(
                "Your enrollment in " + courseName + " has been removed",
                "Enrollment removed",
                $"Hello {displayName}, your enrollment in {courseName} has been removed by the school admin. If you believe this is incorrect, please contact the school admin or MOE Support Team for assistance.",
                "#a14f0b",
                BuildDetails(("Course", courseName, false)),
                "View portal",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildFasApplicationSubmittedEmail(
            string displayName,
            string fasName,
            string applicationNumber,
            DateTime submittedDate,
            string portalLink)
        {
            return BuildModernEmail(
                "Your FAS application has been submitted",
                "FAS application submitted",
                $"Hello {displayName}, your application for {fasName} has been submitted successfully and is now pending review.",
                "#0b5cab",
                BuildDetails(
                    ("FAS", fasName, false),
                    ("Application ID", applicationNumber, false),
                    ("Submitted Date", FormatDate(submittedDate), false)),
                "View application",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildFasApplicationApprovedEmail(
            string displayName,
            string fasName,
            string applicationNumber,
            string approvedTier,
            string approvedSubsidy,
            DateTime? validityStartDate,
            DateTime? validityEndDate,
            string portalLink)
        {
            return BuildModernEmail(
                "Your FAS application has been approved",
                "FAS application approved",
                $"Hello {displayName}, your application for {fasName} has been approved. The approved subsidy will be reflected in your course fee calculation where applicable.",
                "#16a34a",
                BuildDetails(
                    ("FAS", fasName, false),
                    ("Application ID", applicationNumber, false),
                    ("Approved Tier", approvedTier, false),
                    ("Approved Subsidy", approvedSubsidy, true),
                    ("Validity Period", FormatDateRange(validityStartDate, validityEndDate), false)),
                "View application",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildFasApplicationRejectedEmail(
            string displayName,
            string fasName,
            string applicationNumber,
            string rejectionReason,
            string portalLink)
        {
            return BuildModernEmail(
                "Your FAS application has been rejected",
                "FAS application rejected",
                $"Hello {displayName}, your application for {fasName} has been rejected. You may review the details in your e-Service portal.",
                "#a14f0b",
                BuildDetails(
                    ("FAS", fasName, false),
                    ("Application ID", applicationNumber, false),
                    ("Reason", rejectionReason, false)),
                "View application",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildFasExpiredReapplyEmail(
            string displayName,
            string fasName,
            string applicationNumber,
            DateTime validityEndDate,
            string portalLink)
        {
            return BuildModernEmail(
                "Your FAS has expired - you may reapply",
                "FAS expired",
                $"Hello {displayName}, your approved {fasName} has expired. If you still require financial assistance and the FAS is available for your course, you may submit a new application.",
                "#a14f0b",
                BuildDetails(
                    ("FAS", fasName, false),
                    ("Application ID", applicationNumber, false),
                    ("Expired On", FormatDate(validityEndDate), false)),
                "View FAS applications",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildCourseFeePayableEmail(
            string displayName,
            string courseName,
            decimal totalFee,
            decimal fasSubsidy,
            decimal netPayable,
            DateTime paymentDueDate,
            string portalLink)
        {
            return BuildModernEmail(
                courseName + " fee is now payable",
                "Course fee is now payable",
                $"Hello {displayName}, the fee for {courseName} is now payable. Please log in to your e-Service portal to review the fee breakdown and complete payment.",
                "#0b5cab",
                BuildDetails(
                    ("Course", courseName, false),
                    ("Total Fee", Money(totalFee), false),
                    ("FAS Subsidy", Money(fasSubsidy), false),
                    ("Net Payable", Money(netPayable), true),
                    ("Payment Due Date", FormatDate(paymentDueDate), false)),
                "Pay course fee",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildPaymentOverdueEmail(
            string displayName,
            string courseName,
            decimal outstandingAmount,
            string portalLink)
        {
            return BuildModernEmail(
                "Outstanding payment for " + courseName,
                "Payment overdue",
                $"Hello {displayName}, your payment for {courseName} is now outstanding. Please log in to your e-Service portal to make payment as soon as possible.",
                "#a14f0b",
                BuildDetails(
                    ("Course", courseName, false),
                    ("Outstanding Amount", Money(outstandingAmount), true)),
                "Make payment",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildPaymentStatusEmail(
            string displayName,
            string courseName,
            PaymentStatus status,
            decimal paymentAmount,
            string paymentMethod,
            DateTime? paymentDate,
            string? paymentReference,
            string portalLink)
        {
            var isSuccess = status == PaymentStatus.Succeeded;
            var isCanceled = status == PaymentStatus.Canceled;
            var subject = isSuccess
                ? "Payment received for " + courseName
                : isCanceled
                    ? "Payment canceled for " + courseName
                    : "Payment failed for " + courseName;
            var heading = isSuccess
                ? "Payment received"
                : isCanceled
                    ? "Payment canceled"
                    : "Payment was not successful";
            var accent = isSuccess ? "#16a34a" : "#b91c1c";
            var intro = isSuccess
                ? $"Hello {displayName}, we have received your payment for {courseName}."
                : isCanceled
                    ? $"Hello {displayName}, your payment session for {courseName} was canceled. No payment has been applied to your course fee."
                    : $"Hello {displayName}, your payment for {courseName} was unsuccessful. No payment has been applied to your course fee.";

            return BuildModernEmail(
                subject,
                heading,
                intro,
                accent,
                BuildDetails(
                    ("Course", courseName, false),
                    (isSuccess ? "Payment Amount" : "Attempted Amount", Money(paymentAmount), true),
                    ("Payment Method", paymentMethod, false),
                    ("Payment Date", paymentDate.HasValue ? FormatDate(paymentDate.Value) : "N/A", false),
                    ("Reference No.", string.IsNullOrWhiteSpace(paymentReference) ? "N/A" : paymentReference!, false)),
                isSuccess ? "View payment details" : "Try again",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildEducationAccountExtendedEmail(
            string displayName,
            string accountNumber,
            decimal outstandingAmount,
            string portalLink)
        {
            return BuildModernEmail(
                "Your Education Account has been extended",
                "Education Account extended",
                $"Hello {displayName}, your Education Account remains open because unpaid charges remain. Please review the outstanding amount in your e-Service portal.",
                "#a14f0b",
                BuildDetails(
                    ("Account Number", accountNumber, false),
                    ("Outstanding Amount", Money(outstandingAmount), true)),
                "View account",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildInstallmentPlanCreatedEmail(
            string displayName,
            string courseName,
            int paymentPlanMonths,
            decimal installmentAmount,
            DateTime firstDueDate,
            string portalLink)
        {
            return BuildModernEmail(
                "Installment plan created for " + courseName,
                "Installment plan created",
                $"Hello {displayName}, your installment plan for {courseName} has been created.",
                "#0b5cab",
                BuildDetails(
                    ("Course", courseName, false),
                    ("Payment Plan", paymentPlanMonths + " month(s)", false),
                    ("Installment Amount", Money(installmentAmount), true),
                    ("Next Due Date", FormatDate(firstDueDate), false)),
                "View installments",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildInstallmentPaidEmail(
            string displayName,
            string courseName,
            decimal paidAmount,
            DateTime paidDate,
            string portalLink)
        {
            return BuildModernEmail(
                "Installment payment received for " + courseName,
                "Installment payment received",
                $"Hello {displayName}, we have received your installment payment for {courseName}.",
                "#16a34a",
                BuildDetails(
                    ("Course", courseName, false),
                    ("Amount Paid", Money(paidAmount), true),
                    ("Payment Date", FormatDate(paidDate), false)),
                "View installments",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildPaymentDueReminderEmail(
            string displayName,
            string courseName,
            decimal outstandingAmount,
            DateTime dueDate,
            string portalLink)
        {
            return BuildModernEmail(
                "Reminder: payment due for " + courseName,
                "Payment due reminder",
                $"Hello {displayName}, this is a reminder that payment for {courseName} is due soon.",
                "#a14f0b",
                BuildDetails(
                    ("Course", courseName, false),
                    ("Outstanding Amount", Money(outstandingAmount), true),
                    ("Payment Due Date", FormatDate(dueDate), false)),
                "Make payment",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildFasValidityExpiringSoonEmail(
            string displayName,
            string fasName,
            string applicationNumber,
            DateTime validityEndDate,
            string portalLink)
        {
            return BuildModernEmail(
                "Your approved FAS will expire soon",
                "FAS expiring soon",
                $"Hello {displayName}, your approved {fasName} is valid until {FormatDate(validityEndDate)}. Please check whether you are eligible to reapply if you still require financial assistance after the validity period.",
                "#a14f0b",
                BuildDetails(
                    ("FAS", fasName, false),
                    ("Application ID", applicationNumber, false),
                    ("Validity End Date", FormatDate(validityEndDate), false)),
                "View FAS applications",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildReceiptAvailableEmail(
            string displayName,
            string courseName,
            decimal paymentAmount,
            string paymentReference,
            string portalLink)
        {
            return BuildModernEmail(
                "Your payment receipt is available",
                "Payment receipt is available",
                $"Hello {displayName}, your payment receipt for {courseName} is now available.",
                "#16a34a",
                BuildDetails(
                    ("Course", courseName, false),
                    ("Amount Paid", Money(paymentAmount), true),
                    ("Reference No.", paymentReference, false)),
                "View receipt",
                portalLink,
                "MOE Support Team");
        }

        public EmailTemplate BuildInstallmentDueReminderEmail(
            string displayName,
            string courseName,
            decimal installmentAmount,
            DateTime dueDate,
            string portalLink)
        {
            return BuildModernEmail(
                "Reminder: installment due for " + courseName,
                "Installment due reminder",
                $"Hello {displayName}, this is a reminder that an installment for {courseName} is due soon.",
                "#a14f0b",
                BuildDetails(
                    ("Course", courseName, false),
                    ("Installment Amount", Money(installmentAmount), true),
                    ("Due Date", FormatDate(dueDate), false)),
                "View installments",
                portalLink,
                "MOE Support Team");
        }

        private static EmailTemplate BuildModernEmail(
            string subject,
            string heading,
            string intro,
            string accentColor,
            string detailsHtml,
            string buttonText,
            string portalLink,
            string senderName)
        {
            var safeSubject = Html(subject);
            var safeHeading = Html(heading);
            var safeIntro = Html(intro);
            var safeButtonText = Html(buttonText);
            var safePortalLink = Html(portalLink);
            var safeSenderName = Html(senderName);
            var textDetails = StripHtmlRows(detailsHtml);

            var html = $"""
                <!doctype html>
                <html lang="en">
                <head>
                  <meta charset="utf-8">
                  <meta name="viewport" content="width=device-width, initial-scale=1">
                  <title>{safeSubject}</title>
                </head>
                <body style="margin:0;padding:0;background:#f5f7fb;color:#1f2937;font-family:Segoe UI,Arial,Helvetica,sans-serif;">
                  <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="background:#f5f7fb;margin:0;padding:0;">
                    <tr>
                      <td align="center" style="padding:36px 16px;">
                        <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="max-width:600px;background:#ffffff;border-radius:12px;overflow:hidden;border:1px solid #e5e7eb;box-shadow:0 18px 44px rgba(15,23,42,0.08);">
                          <tr>
                            <td style="height:6px;background:{accentColor};font-size:0;line-height:0;">&nbsp;</td>
                          </tr>
                          <tr>
                            <td align="center" style="padding:38px 34px 22px;">
                              <div style="font-size:26px;line-height:1.25;font-weight:800;color:{accentColor};">{safeSenderName}</div>
                              <div style="margin-top:6px;font-size:14px;line-height:1.4;color:#6b7280;">Education Account Management</div>
                              <h1 style="margin:30px 0 0;font-size:28px;line-height:1.25;font-weight:800;color:#111827;">{safeHeading}</h1>
                              <p style="margin:14px auto 0;max-width:480px;color:#6b7280;font-size:15px;line-height:1.7;">{safeIntro}</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding:0 34px 34px;">
                              <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="border:1px solid #e5e7eb;border-radius:10px;overflow:hidden;">
                                {detailsHtml}
                              </table>
                              <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="margin-top:28px;">
                                <tr>
                                  <td align="center">
                                    <a href="{safePortalLink}" style="display:block;background:{accentColor};color:#ffffff;text-decoration:none;padding:15px 20px;border-radius:8px;font-size:15px;font-weight:700;">{safeButtonText}</a>
                                  </td>
                                </tr>
                              </table>
                              <p style="margin:30px 0 0;text-align:center;color:#6b7280;font-size:14px;line-height:1.6;">
                                If you need assistance, please contact MOE Support Team.
                              </p>
                              <p style="margin:14px 0 0;text-align:center;color:#9ca3af;font-size:12px;line-height:1.5;">
                                This is an automated email from MOE Support Team. Please do not reply directly to this email.
                              </p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>
                """;

            var text = $"""
                {subject}

                {intro}

                {textDetails}

                {buttonText}: {portalLink}

                Thank you,
                MOE Support Team
                """;

            return new EmailTemplate(subject, html, text);
        }

        private static string BuildDetails(params (string Label, string Value, bool Highlight)[] rows)
        {
            return string.Join(
                Environment.NewLine,
                rows.Select((row, index) =>
                {
                    var border = index == rows.Length - 1 ? string.Empty : "border-bottom:1px solid #eef2f7;";
                    var valueColor = "#111827";
                    var valueWeight = row.Highlight ? "800" : "700";
                    return $"""
                                <tr data-text="{Html(row.Label)}: {Html(row.Value)}">
                                  <td style="padding:16px 18px;{border}color:#6b7280;font-size:14px;">{Html(row.Label)}</td>
                                  <td align="right" style="padding:16px 18px;{border}color:{valueColor};font-size:14px;font-weight:{valueWeight};">{Html(row.Value)}</td>
                                </tr>
                        """;
                }));
        }

        private static string StripHtmlRows(string detailsHtml)
        {
            var rows = detailsHtml
                .Split("data-text=\"", StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(part => WebUtility.HtmlDecode(part.Split('"')[0]))
                .Where(value => !string.IsNullOrWhiteSpace(value));

            return string.Join(Environment.NewLine, rows);
        }

        private static string Money(decimal value)
        {
            return $"SGD {value:N2}";
        }

        private static string FormatDate(DateTime value)
        {
            return value.ToString("dd MMM yyyy");
        }

        private static string FormatDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue && !endDate.HasValue)
            {
                return "N/A";
            }

            if (!endDate.HasValue)
            {
                return $"{FormatDate(startDate!.Value)} onwards";
            }

            if (!startDate.HasValue)
            {
                return "Until " + FormatDate(endDate.Value);
            }

            return $"{FormatDate(startDate.Value)} to {FormatDate(endDate.Value)}";
        }

        private static string WrapHtml(string appName, string title, string body)
        {
            return $"""
                <!doctype html>
                <html lang="en">
                <head>
                  <meta charset="utf-8">
                  <meta name="viewport" content="width=device-width, initial-scale=1">
                  <meta name="color-scheme" content="light">
                  <title>{Html(title)}</title>
                </head>

                <body style="margin:0;padding:0;background:#f3f6fb;color:#111827;
                             font-family:'Segoe UI',Arial,Helvetica,sans-serif;">
                  <table role="presentation" width="100%" cellspacing="0" cellpadding="0"
                         style="background:#f3f6fb;margin:0;padding:0;">
                    <tr>
                      <td align="center" style="padding:40px 16px;">

                        <table role="presentation" width="100%" cellspacing="0" cellpadding="0"
                               style="max-width:600px;background:#ffffff;border-radius:18px;
                                      overflow:hidden;border:1px solid #e5e7eb;
                                      box-shadow:0 12px 32px rgba(15,23,42,0.08);">

                          <tr>
                            <td style="background:linear-gradient(135deg,#0078d4,#005a9e);
                                       padding:34px 32px;text-align:center;color:#ffffff;">
                              <div style="font-size:26px;font-weight:800;letter-spacing:-0.4px;">
                                {appName}
                              </div>
                              <div style="font-size:14px;opacity:0.9;margin-top:8px;">
                                Secure Identity & Access Management
                              </div>
                            </td>
                          </tr>

                          <tr>
                            <td style="padding:36px 32px;">
                              <h1 style="font-size:24px;line-height:1.3;margin:0 0 20px;
                                         color:#111827;letter-spacing:-0.3px;">
                                {Html(title)}
                              </h1>

                              <div style="font-size:15px;line-height:1.7;color:#374151;">
                                {body}
                              </div>
                            </td>
                          </tr>

                        </table>

                        <div style="max-width:600px;text-align:center;padding:22px 12px 0;
                                    color:#6b7280;font-size:12px;line-height:1.6;">
                          © {DateTime.UtcNow.Year} {appName}. All rights reserved.<br>
                          This is an automated email. Please do not reply.
                        </div>

                      </td>
                    </tr>
                  </table>
                </body>
                </html>
                """;
        }

        private static string Html(string value)
        {
            return WebUtility.HtmlEncode(value);
        }

        private static string FormatUtc(DateTime value)
        {
            return value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm 'UTC'");
        }
    }
}
