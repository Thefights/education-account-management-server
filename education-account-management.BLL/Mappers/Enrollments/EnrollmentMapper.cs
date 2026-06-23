using DTOs.Enrollments;
using Riok.Mapperly.Abstractions;

namespace Mappers.Enrollments;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class EnrollmentMapper : IReadMapper<Enrollment, GetEnrollmentDTO>
{
    [MapProperty($"{nameof(Enrollment.Course)}.{nameof(Course.CourseCode)}", nameof(GetEnrollmentDTO.CourseCode))]
    [MapProperty($"{nameof(Enrollment.Course)}.{nameof(Course.Status)}", nameof(GetEnrollmentDTO.CourseStatus))]
    [MapProperty(nameof(Enrollment.SchoolNameSnapshot), nameof(GetEnrollmentDTO.SchoolName))]
    [MapProperty(nameof(Enrollment.CourseNameSnapshot), nameof(GetEnrollmentDTO.CourseName))]
    [MapProperty(nameof(Enrollment.CourseDescriptionSnapshot), nameof(GetEnrollmentDTO.CourseDescription))]
    [MapProperty(nameof(Enrollment.CitizenNricSnapshot), nameof(GetEnrollmentDTO.CitizenNric))]
    [MapProperty(nameof(Enrollment.CitizenFullNameSnapshot), nameof(GetEnrollmentDTO.CitizenFullName))]
    [MapProperty(nameof(Enrollment.CitizenEmailSnapshot), nameof(GetEnrollmentDTO.CitizenEmail))]
    [MapProperty(nameof(Enrollment.CitizenPhoneNumberSnapshot), nameof(GetEnrollmentDTO.CitizenPhoneNumber))]
    [MapProperty(nameof(Enrollment.AccountNumberSnapshot), nameof(GetEnrollmentDTO.AccountNumber))]
    [MapProperty(nameof(Enrollment.CreatedAt), nameof(GetEnrollmentDTO.EnrolledAt))]
    [MapProperty($"{nameof(Enrollment.Charge)}.{nameof(Charge.Status)}", nameof(GetEnrollmentDTO.ChargeStatus))]
    [MapProperty($"{nameof(Enrollment.Charge)}.{nameof(Charge.GrossAmount)}", nameof(GetEnrollmentDTO.GrossAmount))]
    [MapProperty($"{nameof(Enrollment.Charge)}.{nameof(Charge.PaidAmount)}", nameof(GetEnrollmentDTO.PaidAmount))]
    [MapProperty($"{nameof(Enrollment.Charge)}.{nameof(Charge.RemainingAmount)}", nameof(GetEnrollmentDTO.RemainingAmount))]
    public partial GetEnrollmentDTO MapToGetDTO(Enrollment model);

    public partial List<GetEnrollmentDTO> MapToGetDTOList(List<Enrollment> models);

    public partial IQueryable<GetEnrollmentDTO> ProjectToGetDTO(IQueryable<Enrollment> query);
}
