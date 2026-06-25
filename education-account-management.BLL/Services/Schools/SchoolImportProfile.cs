using DTOs.Csv;
using DTOs.Schools;
using Interfaces.Csv;
using System.Text.RegularExpressions;

namespace Services.Schools
{
    public class SchoolImportProfile(
        SchoolMapper mapper,
        IUnitOfWork unitOfWork) : ICsvImportProfile<School, CreateSchoolDTO>
    {
        private readonly SchoolMapper _mapper = mapper;
        private readonly IGenericRepository<School> _schoolRepository = unitOfWork.Repository<School>();

        public string EntityName => nameof(School);

        public School MapToEntity(CreateSchoolDTO row)
        {
            return _mapper.MapFromCreateDTO(row);
        }

        public async Task<List<BatchImportErrorDTO>> ValidateRowAsync(CreateSchoolDTO row, int rowNumber, CancellationToken cancellationToken = default)
        {
            var errors = new List<BatchImportErrorDTO>();

            if (string.IsNullOrWhiteSpace(row.SchoolName))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.SchoolName), "School name is required."));
            }

            if (string.IsNullOrWhiteSpace(row.Email))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Email), "Email is required."));
            }
            else if (!Regex.IsMatch(row.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Email), "Invalid email format."));
            }
            else
            {
                var emailExists = await _schoolRepository.AnyAsync(s => s.Email == row.Email, cancellationToken);
                if (emailExists)
                {
                    errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Email), "Email must be unique."));
                }
            }

            if (string.IsNullOrWhiteSpace(row.PhoneNumber))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.PhoneNumber), "Phone number is required."));
            }

            if (string.IsNullOrWhiteSpace(row.Address))
            {
                errors.Add(BatchImportErrorDTO.Create(rowNumber, nameof(row.Address), "Address is required."));
            }

            return errors;
        }
    }
}
