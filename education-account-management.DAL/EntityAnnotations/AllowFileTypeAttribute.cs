using Microsoft.AspNetCore.Http;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AllowFileTypeAttribute(params FileType[] fileTypes) : ValidationAttribute
    {
        public FileType[] FileTypes { get; } = fileTypes is { Length: > 0 } ? fileTypes : [FileType.Image];

        public string[] CustomExtensions { get; set; } = [];

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            var allowedExtensions = FileExtensionMap.GetAllowedExtensions(FileTypes, CustomExtensions);
            var memberName = validationContext.MemberName ?? validationContext.DisplayName;

            if (value is string filePathOrUrl)
            {
                return ValidateFileName(filePathOrUrl, allowedExtensions, memberName);
            }

            if (value is IFormFile file)
            {
                return ValidateFile(file, allowedExtensions, memberName);
            }

            if (value is IEnumerable<IFormFile> files)
            {
                foreach (var currentFile in files)
                {
                    var result = ValidateFile(currentFile, allowedExtensions, memberName);
                    if (result != ValidationResult.Success)
                    {
                        return result;
                    }
                }
            }

            return ValidationResult.Success;
        }

        private static ValidationResult? ValidateFile(IFormFile file, HashSet<string> allowedExtensions, string memberName)
        {
            return ValidateFileName(file.FileName, allowedExtensions, memberName);
        }

        private static ValidationResult? ValidateFileName(string fileName, HashSet<string> allowedExtensions, string memberName)
        {
            var normalizedFileName = NormalizeFileName(fileName);
            var extension = Path.GetExtension(normalizedFileName);

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                var allowed = string.Join(", ", allowedExtensions.Order());
                return new ValidationResult(
                    $"File '{fileName}' has an unsupported extension. Allowed: {allowed}",
                    [memberName]);
            }

            return ValidationResult.Success;
        }

        private static string NormalizeFileName(string fileName)
        {
            if (Uri.TryCreate(fileName, UriKind.Absolute, out var uri))
            {
                return uri.LocalPath;
            }

            var queryIndex = fileName.IndexOfAny(['?', '#']);
            return queryIndex >= 0 ? fileName[..queryIndex] : fileName;
        }
    }
}
