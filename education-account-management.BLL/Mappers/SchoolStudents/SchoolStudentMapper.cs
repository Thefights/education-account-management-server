using DTOs.SchoolStudents;
using Riok.Mapperly.Abstractions;

namespace Mappers.SchoolStudents
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class SchoolStudentMapper :
        ICrudMapper<SchoolStudent, CreateSchoolStudentDTO, GetSchoolStudentDTO, UpdateSchoolStudentDTO>
    {
        public partial SchoolStudent MapFromCreateDTO(CreateSchoolStudentDTO createDTO);

        public partial void MapFromUpdateDTO(UpdateSchoolStudentDTO updateDTO, SchoolStudent model);

        [MapProperty($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.AccountNumber)}", nameof(GetSchoolStudentDTO.AccountNumber))]
        [MapProperty($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.Nric)}", nameof(GetSchoolStudentDTO.Nric))]
        [MapProperty($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}", nameof(GetSchoolStudentDTO.FullName))]
        [MapProperty($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.Email)}", nameof(GetSchoolStudentDTO.Email))]
        [MapProperty($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.PhoneNumber)}", nameof(GetSchoolStudentDTO.PhoneNumber))]
        [MapProperty($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.DateOfBirth)}", nameof(GetSchoolStudentDTO.DateOfBirth))]
        [MapProperty(nameof(SchoolStudent.Enrollments), nameof(GetSchoolStudentDTO.Courses))]
        public partial GetSchoolStudentDTO MapToGetDTO(SchoolStudent model);

        public partial List<GetSchoolStudentDTO> MapToGetDTOList(List<SchoolStudent> models);

        public partial IQueryable<GetSchoolStudentDTO> ProjectToGetDTO(IQueryable<SchoolStudent> query);

        [MapProperty($"{nameof(Enrollment.Course)}.{nameof(Course.Id)}", nameof(GetSchoolStudentCourseDTO.Id))]
        [MapProperty($"{nameof(Enrollment.Course)}.{nameof(Course.CourseCode)}", nameof(GetSchoolStudentCourseDTO.CourseCode))]
        [MapProperty($"{nameof(Enrollment.Course)}.{nameof(Course.CourseName)}", nameof(GetSchoolStudentCourseDTO.CourseName))]
        [MapProperty($"{nameof(Enrollment.Course)}.{nameof(Course.Status)}", nameof(GetSchoolStudentCourseDTO.Status))]
        public partial GetSchoolStudentCourseDTO MapEnrollmentToCourse(Enrollment enrollment);
    }
}
