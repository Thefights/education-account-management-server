using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EnrollmentSeedBuilder : ISeedBuilder
    {
        public int Priority => 120;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var primaryNames = new[]
            {
                "Sterling Quach", "Amelia Tan", "Marcus Lim", "Priya Nair", "Ethan Koh",
                "Hannah Lee", "Daniel Wong", "Sofia Chen", "Lucas Nguyen", "Maya Rahman",
                "Noah Teo", "Aisha Fernandez", "Ryan Chua", "Chloe Goh", "Irfan Hassan"
            };
            var givenNames = new[]
            {
                "Alina", "Benjamin", "Clara", "Darius", "Elena", "Farhan", "Grace", "Haruto",
                "Isabelle", "Jasper", "Keira", "Leon", "Mei Lin", "Nathan", "Olivia", "Pranav",
                "Qistina", "Rafael", "Selina", "Terence", "Umairah", "Victor", "Wen Jie", "Xavier",
                "Yasmin", "Zachary", "Adeline", "Brandon", "Celeste", "Damien", "Evelyn", "Faris",
                "Giselle", "Haziq", "Irene", "Jonas", "Kavya", "Lydia", "Malcolm", "Nadia"
            };
            var familyNames = new[]
            {
                "Ang", "Bala", "Chew", "Das", "Eng", "Foo", "Gan", "Ho", "Ismail", "Jeyaratnam",
                "Kwek", "Lim", "Mohamed", "Ng", "Ong", "Pillai", "Quek", "Rao", "Sim", "Tan",
                "Uddin", "Vasquez", "Wong", "Xu", "Yeo", "Zainal"
            };
            var schoolNames = new[]
            {
                "Northview Secondary School", "Eastbridge Secondary School", "Westhaven Secondary School",
                "Southpoint Secondary School", "Central Heights School", "Riverside Learning Institute",
                "Lakeside Technical School", "Greenfield Academy", "Harbourfront School",
                "Hillcrest Education Centre"
            };
            static string GetCitizenName(int id, string[] primaryNames, string[] givenNames, string[] familyNames)
            {
                if (id <= primaryNames.Length) return primaryNames[id - 1];
                if (id == 16) return "Natalie Seah";
                var givenName = givenNames[(id - 1) % givenNames.Length];
                var familyName = familyNames[((id - 1) / givenNames.Length + id) % familyNames.Length];
                return $"{givenName} {familyName}";
            }

            var enrollments = new List<Enrollment>
            {
                new Enrollment { Id = 1, CourseId = 1, SchoolStudentId = 1, SchoolNameSnapshot = "Northview Secondary School", CourseNameSnapshot = "Quantitative Problem Solving", CourseDescriptionSnapshot = "Applied numeracy and structured problem-solving for academic pathways.", CitizenNricSnapshot = SingaporeNricUtil.Generate(1), CitizenFullNameSnapshot = "Sterling Quach", CitizenEmailSnapshot = "sterling.quach@example.com", CitizenPhoneNumberSnapshot = "+6590000001", AccountNumberSnapshot = "EDU-2026-00000000001", CreatedAt = createdAt },
                new Enrollment { Id = 2, CourseId = 2, SchoolStudentId = 2, SchoolNameSnapshot = "Eastbridge Secondary School", CourseNameSnapshot = "Software Foundations with C#", CourseDescriptionSnapshot = "Core programming concepts, debugging, and application structure.", CitizenNricSnapshot = SingaporeNricUtil.Generate(2), CitizenFullNameSnapshot = "Amelia Tan", CitizenEmailSnapshot = "amelia.tan@example.com", CitizenPhoneNumberSnapshot = "+6590000002", AccountNumberSnapshot = "EDU-2026-00000000002", CreatedAt = createdAt },
                new Enrollment { Id = 3, CourseId = 3, SchoolStudentId = 3, SchoolNameSnapshot = "Westhaven Secondary School", CourseNameSnapshot = "Professional Communication Lab", CourseDescriptionSnapshot = "Practical writing, presentation, and workplace communication skills.", CitizenNricSnapshot = SingaporeNricUtil.Generate(3), CitizenFullNameSnapshot = "Marcus Lim", CitizenEmailSnapshot = "marcus.lim@example.com", CitizenPhoneNumberSnapshot = "+6590000003", AccountNumberSnapshot = "EDU-2026-00000000003", CreatedAt = createdAt },
                new Enrollment { Id = 4, CourseId = 4, SchoolStudentId = 4, SchoolNameSnapshot = "Southpoint Secondary School", CourseNameSnapshot = "Sustainability Science Workshop", CourseDescriptionSnapshot = "Environmental systems, resource planning, and sustainability practice.", CitizenNricSnapshot = SingaporeNricUtil.Generate(4), CitizenFullNameSnapshot = "Priya Nair", CitizenEmailSnapshot = "priya.nair@example.com", CitizenPhoneNumberSnapshot = "+6590000004", AccountNumberSnapshot = "EDU-2026-00000000004", CreatedAt = createdAt },
                new Enrollment { Id = 5, CourseId = 5, SchoolStudentId = 5, SchoolNameSnapshot = "Central Heights School", CourseNameSnapshot = "Digital Media Production", CourseDescriptionSnapshot = "Digital storytelling, layout, and media production workflows.", CitizenNricSnapshot = SingaporeNricUtil.Generate(5), CitizenFullNameSnapshot = "Ethan Koh", CitizenEmailSnapshot = "ethan.koh@example.com", CitizenPhoneNumberSnapshot = "+6590000005", AccountNumberSnapshot = "EDU-2026-00000000005", CreatedAt = createdAt },
                new Enrollment { Id = 6, CourseId = 6, SchoolStudentId = 6, SchoolNameSnapshot = "Riverside Learning Institute", CourseNameSnapshot = "Service Operations Practicum", CourseDescriptionSnapshot = "Customer operations, service standards, and scenario-based practice.", CitizenNricSnapshot = SingaporeNricUtil.Generate(6), CitizenFullNameSnapshot = "Hannah Lee", CitizenEmailSnapshot = "hannah.lee@example.com", CitizenPhoneNumberSnapshot = "+6590000006", AccountNumberSnapshot = "EDU-2026-00000000006", CreatedAt = createdAt },
                new Enrollment { Id = 7, CourseId = 7, SchoolStudentId = 7, SchoolNameSnapshot = "Lakeside Technical School", CourseNameSnapshot = "Electrical Systems Fundamentals", CourseDescriptionSnapshot = "Foundational electrical theory, components, and safety practices.", CitizenNricSnapshot = SingaporeNricUtil.Generate(7), CitizenFullNameSnapshot = "Daniel Wong", CitizenEmailSnapshot = "daniel.wong@example.com", CitizenPhoneNumberSnapshot = "+6590000007", AccountNumberSnapshot = "EDU-2026-00000000007", CreatedAt = createdAt },
                new Enrollment { Id = 8, CourseId = 8, SchoolStudentId = 8, SchoolNameSnapshot = "Greenfield Academy", CourseNameSnapshot = "Creative Writing Studio", CourseDescriptionSnapshot = "Narrative craft, editing practice, and guided writing critique.", CitizenNricSnapshot = SingaporeNricUtil.Generate(8), CitizenFullNameSnapshot = "Sofia Chen", CitizenEmailSnapshot = "sofia.chen@example.com", CitizenPhoneNumberSnapshot = "+6590000008", AccountNumberSnapshot = "EDU-2026-00000000008", CreatedAt = createdAt },
                new Enrollment { Id = 9, CourseId = 9, SchoolStudentId = 9, SchoolNameSnapshot = "Harbourfront School", CourseNameSnapshot = "Data Analytics Essentials", CourseDescriptionSnapshot = "Data preparation, analysis, visualization, and reporting fundamentals.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt },
                new Enrollment { Id = 10, CourseId = 10, SchoolStudentId = 10, SchoolNameSnapshot = "Hillcrest Education Centre", CourseNameSnapshot = "Office Productivity for Business", CourseDescriptionSnapshot = "Document, spreadsheet, and presentation workflows for business users.", CitizenNricSnapshot = SingaporeNricUtil.Generate(10), CitizenFullNameSnapshot = "Maya Rahman", CitizenEmailSnapshot = "maya.rahman@example.com", CitizenPhoneNumberSnapshot = "+6590000010", AccountNumberSnapshot = "EDU-2026-00000000010", CreatedAt = createdAt },
            
                // Upcoming courses (CourseId: 1, 2, 3)
                new Enrollment { Id = 11, CourseId = 1, SchoolStudentId = 9, SchoolNameSnapshot = "Northview Secondary School", CourseNameSnapshot = "Quantitative Problem Solving", CourseDescriptionSnapshot = "Applied numeracy and structured problem-solving for academic pathways.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt },
                new Enrollment { Id = 13, CourseId = 2, SchoolStudentId = 9, SchoolNameSnapshot = "Eastbridge Secondary School", CourseNameSnapshot = "Software Foundations with C#", CourseDescriptionSnapshot = "Core programming concepts, debugging, and application structure.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt },
                new Enrollment { Id = 14, CourseId = 3, SchoolStudentId = 9, SchoolNameSnapshot = "Westhaven Secondary School", CourseNameSnapshot = "Professional Communication Lab", CourseDescriptionSnapshot = "Practical writing, presentation, and workplace communication skills.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt },
            
                // InProgress courses (CourseId: 4, 5, 6)
                new Enrollment { Id = 12, CourseId = 4, SchoolStudentId = 9, SchoolNameSnapshot = "Southpoint Secondary School", CourseNameSnapshot = "Sustainability Science Workshop", CourseDescriptionSnapshot = "Environmental systems, resource planning, and sustainability practice.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt },
                new Enrollment { Id = 15, CourseId = 5, SchoolStudentId = 9, SchoolNameSnapshot = "Central Heights School", CourseNameSnapshot = "Digital Media Production", CourseDescriptionSnapshot = "Digital storytelling, layout, and media production workflows.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt },
                new Enrollment { Id = 16, CourseId = 6, SchoolStudentId = 9, SchoolNameSnapshot = "Riverside Learning Institute", CourseNameSnapshot = "Service Operations Practicum", CourseDescriptionSnapshot = "Customer operations, service standards, and scenario-based practice.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt },
            
                // Closed courses (CourseId: 7, 8, 9, 10) - 9 is already above, adding 7, 8, 10
                new Enrollment { Id = 17, CourseId = 7, SchoolStudentId = 9, SchoolNameSnapshot = "Lakeside Technical School", CourseNameSnapshot = "Electrical Systems Fundamentals", CourseDescriptionSnapshot = "Foundational electrical theory, components, and safety practices.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt },
                new Enrollment { Id = 18, CourseId = 8, SchoolStudentId = 9, SchoolNameSnapshot = "Greenfield Academy", CourseNameSnapshot = "Creative Writing Studio", CourseDescriptionSnapshot = "Narrative craft, editing practice, and guided writing critique.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt },
                new Enrollment { Id = 19, CourseId = 10, SchoolStudentId = 9, SchoolNameSnapshot = "Hillcrest Education Centre", CourseNameSnapshot = "Office Productivity for Business", CourseDescriptionSnapshot = "Document, spreadsheet, and presentation workflows for business users.", CitizenNricSnapshot = SingaporeNricUtil.Generate(9), CitizenFullNameSnapshot = "Lucas Nguyen", CitizenEmailSnapshot = "lucas.nguyen@example.com", CitizenPhoneNumberSnapshot = "+6590000009", AccountNumberSnapshot = "EDU-2026-00000000009", CreatedAt = createdAt }
            };

            var topics = new[]
            {
                "Academic Writing", "Business Numeracy", "Digital Literacy", "Career Readiness",
                "Applied Science", "Financial Literacy", "Project Collaboration", "Data Skills",
                "Workplace Communication"
            };
            var enrollmentId = 20;
            for (var schoolId = 1; schoolId <= 10; schoolId++)
            {
                for (var index = 0; index < 9; index++)
                {
                    var schoolStudentId = 11 + (schoolId - 1) * 9 + index;
                    var courseId = 11 + (schoolId - 1) * 9 + index;
                    var citizenName = GetCitizenName(
                        schoolStudentId,
                        primaryNames,
                        givenNames,
                        familyNames);
                    enrollments.Add(new Enrollment
                    {
                        Id = enrollmentId++,
                        CourseId = courseId,
                        SchoolStudentId = schoolStudentId,
                        SchoolNameSnapshot = schoolNames[schoolId - 1],
                        CourseNameSnapshot = $"{topics[index]} - School {schoolId} Cohort {index + 1}",
                        CourseDescriptionSnapshot = "Structured lessons with guided practice and applied assessments.",
                        CitizenNricSnapshot = SingaporeNricUtil.Generate(schoolStudentId),
                        CitizenFullNameSnapshot = citizenName,
                        CitizenEmailSnapshot = $"{citizenName.ToLowerInvariant().Replace(" ", ".")}.{schoolStudentId}@example.com",
                        CitizenPhoneNumberSnapshot = $"+659{schoolStudentId:0000000}",
                        AccountNumberSnapshot = $"EDU-2026-{schoolStudentId:00000000000}",
                        CreatedAt = createdAt
                    });
                }
            }

            modelBuilder.Entity<Enrollment>().HasData(enrollments);

            return modelBuilder;
        }
    }
}
