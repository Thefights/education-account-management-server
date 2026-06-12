using DTOs.Auth;
using DTOs.Csv;
using Interfaces.Auth;
using Services.Base;

namespace Services.Csv
{
    public class AuthAccountCsvImportService(
        IUnitOfWork unitOfWork,
        IAuthEmailService authEmailService)
        : CsvImportService<AuthAccount, ImportAuthAccountCsvRowDTO>(unitOfWork)
    {
        private readonly IAuthEmailService _authEmailService = authEmailService;
        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();
        private readonly IGenericRepository<Role> _roleRepository = unitOfWork.Repository<Role>();

        public override async Task<BatchImportResultDTO> ImportAsync(
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            return await ImportAsync(file, sendEmail: true, cancellationToken);
        }

        public async Task<BatchImportResultDTO> ImportAsync(
            IFormFile file,
            bool sendEmail,
            CancellationToken cancellationToken = default)
        {
            var fileErrors = ValidateFile(file);
            if (fileErrors.Count != 0)
            {
                return CsvImportHelper.BuildFailureResult(0, fileErrors);
            }

            var rows = ReadRows(file);
            if (rows.Errors.Count != 0)
            {
                return CsvImportHelper.BuildFailureResult(rows.Total, rows.Errors);
            }

            if (rows.Items.Count == 0)
            {
                return CsvImportHelper.BuildFailureResult(0, [BatchImportErrorDTO.Create(0, "File", "CSV file must contain at least one data row.")]);
            }

            var errors = new List<BatchImportErrorDTO>();
            var parsedRows = new List<ParsedAuthAccountImportRow>();

            foreach (var item in rows.Items)
            {
                var parsedRow = ParseImportRow(item.Row, item.RowNumber, errors);
                if (parsedRow != null)
                {
                    parsedRows.Add(parsedRow);
                }
            }

            AddCsvDuplicateErrors(parsedRows, errors);
            await AddRepositoryBackedErrorsAsync(parsedRows, errors, cancellationToken);

            if (errors.Count != 0)
            {
                return CsvImportHelper.BuildFailureResult(rows.Total, errors);
            }

            var importedAccounts = new List<ImportedAuthAccount>();
            foreach (var parsedRow in parsedRows)
            {
                var importedAccount = MapImportRowToAccount(parsedRow);
                CsvImportHelper.AddEntityValidationErrors(errors, importedAccount.AuthAccount, parsedRow.RowNumber);
                CsvImportHelper.AddEntityValidationErrors(errors, importedAccount.AuthAccount.User, parsedRow.RowNumber);

                importedAccounts.Add(importedAccount);
            }

            if (errors.Count != 0)
            {
                return CsvImportHelper.BuildFailureResult(rows.Total, errors);
            }

            if (sendEmail)
            {
                foreach (var importedAccount in importedAccounts)
                {
                    await _authEmailService.SendWelcomeEmailAsync(
                        importedAccount.AuthAccount.Email,
                        importedAccount.AuthAccount.UserIdText,
                        cancellationToken);
                }
            }

            await Repository.AddRangeAsync(importedAccounts.Select(account => account.AuthAccount).ToList(), cancellationToken);
            await UnitOfWork.SaveChangeAsync(cancellationToken);

            return new BatchImportResultDTO
            {
                Total = rows.Total,
                Succeeded = rows.Total,
                Failed = 0,
                Errors = []
            };
        }

        private static ParsedAuthAccountImportRow? ParseImportRow(
            ImportAuthAccountCsvRowDTO row,
            int rowNumber,
            List<BatchImportErrorDTO> errors)
        {
            var userIdText = CsvImportHelper.RequireText(row.UserIdText, nameof(ImportAuthAccountCsvRowDTO.UserIdText), rowNumber, errors);
            var email = CsvImportHelper.RequireText(row.Email, nameof(ImportAuthAccountCsvRowDTO.Email), rowNumber, errors);
            var fullName = CsvImportHelper.RequireText(row.FullName, nameof(ImportAuthAccountCsvRowDTO.FullName), rowNumber, errors);
            var gender = CsvImportHelper.ParseRequiredEnum<UserGender>(row.Gender, nameof(ImportAuthAccountCsvRowDTO.Gender), rowNumber, errors);
            var roleIds = CsvImportHelper.ParseRequiredIds(row.RoleIds, nameof(ImportAuthAccountCsvRowDTO.RoleIds), rowNumber, errors);
            var phoneNumber = string.IsNullOrWhiteSpace(row.PhoneNumber) ? null : row.PhoneNumber.Trim();
            var imageUrl = string.IsNullOrWhiteSpace(row.ImageUrl) ? null : row.ImageUrl.Trim();

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                CsvImportHelper.ValidateImageUrl(errors, imageUrl, rowNumber, nameof(ImportAuthAccountCsvRowDTO.ImageUrl));
            }

            if (string.IsNullOrWhiteSpace(userIdText)
                || string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(fullName)
                || !gender.HasValue
                || roleIds.Count == 0)
            {
                return null;
            }

            return new ParsedAuthAccountImportRow(
                rowNumber,
                userIdText,
                email,
                fullName,
                gender.Value,
                roleIds,
                phoneNumber,
                imageUrl);
        }

        private static void AddCsvDuplicateErrors(
            List<ParsedAuthAccountImportRow> rows,
            List<BatchImportErrorDTO> errors)
        {
            CsvImportHelper.AddDuplicateErrors(
                rows,
                row => row.UserIdText,
                row => row.RowNumber,
                nameof(ImportAuthAccountCsvRowDTO.UserIdText),
                "UserIdText is duplicated in the CSV file.",
                errors,
                StringComparer.OrdinalIgnoreCase);

            CsvImportHelper.AddDuplicateErrors(
                rows.Where(row => !string.IsNullOrWhiteSpace(row.PhoneNumber)),
                row => row.PhoneNumber!,
                row => row.RowNumber,
                nameof(ImportAuthAccountCsvRowDTO.PhoneNumber),
                "PhoneNumber is duplicated in the CSV file.",
                errors);
        }

        private async Task AddRepositoryBackedErrorsAsync(
            List<ParsedAuthAccountImportRow> rows,
            List<BatchImportErrorDTO> errors,
            CancellationToken cancellationToken)
        {
            var roleIds = rows.SelectMany(row => row.RoleIds).Distinct().ToList();
            var existingRoleIds = await _roleRepository
                .Query()
                .Where(role => roleIds.Contains(role.Id))
                .Select(role => role.Id)
                .ToListAsync(cancellationToken);
            var missingRoleIds = roleIds.Except(existingRoleIds).ToHashSet();

            var userIdTexts = rows.Select(row => row.UserIdText).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            var existingUserIdTexts = await Repository
                .Query()
                .Where(authAccount => userIdTexts.Contains(authAccount.UserIdText))
                .Select(authAccount => authAccount.UserIdText)
                .ToListAsync(cancellationToken);
            var existingUserIdTextSet = existingUserIdTexts.ToHashSet(StringComparer.OrdinalIgnoreCase);

            var phoneNumbers = rows
                .Where(row => !string.IsNullOrWhiteSpace(row.PhoneNumber))
                .Select(row => row.PhoneNumber!)
                .Distinct()
                .ToList();
            var existingPhoneNumbers = await _userRepository
                .Query()
                .Where(user => user.PhoneNumber != null && phoneNumbers.Contains(user.PhoneNumber))
                .Select(user => user.PhoneNumber!)
                .ToListAsync(cancellationToken);
            var existingPhoneNumberSet = existingPhoneNumbers.ToHashSet();

            foreach (var row in rows)
            {
                if (existingUserIdTextSet.Contains(row.UserIdText))
                {
                    errors.Add(BatchImportErrorDTO.Create(row.RowNumber, nameof(ImportAuthAccountCsvRowDTO.UserIdText), "AuthAccount UserIdText already exists."));
                }

                if (!string.IsNullOrWhiteSpace(row.PhoneNumber) && existingPhoneNumberSet.Contains(row.PhoneNumber))
                {
                    errors.Add(BatchImportErrorDTO.Create(row.RowNumber, nameof(ImportAuthAccountCsvRowDTO.PhoneNumber), "User PhoneNumber already exists."));
                }

                foreach (var roleId in row.RoleIds.Where(missingRoleIds.Contains))
                {
                    errors.Add(BatchImportErrorDTO.Create(row.RowNumber, nameof(ImportAuthAccountCsvRowDTO.RoleIds), $"Role {roleId} was not found."));
                }

            }
        }

        private static ImportedAuthAccount MapImportRowToAccount(ParsedAuthAccountImportRow row)
        {
            var authAccount = new AuthAccount
            {
                UserIdText = row.UserIdText,
                Email = row.Email,
                PasswordHash = null,
                User = new User
                {
                    FullName = row.FullName,
                    PhoneNumber = row.PhoneNumber,
                    Gender = row.Gender,
                    ImageUrl = row.ImageUrl,
                    UserRoles = row.RoleIds
                        .Select(roleId => new UserRole { RoleId = roleId })
                        .ToList()
                }
            };

            return new ImportedAuthAccount(authAccount);
        }

        private sealed record ParsedAuthAccountImportRow(
            int RowNumber,
            string UserIdText,
            string Email,
            string FullName,
            UserGender Gender,
            List<int> RoleIds,
            string? PhoneNumber,
            string? ImageUrl);

        private sealed record ImportedAuthAccount(AuthAccount AuthAccount);
    }
}
