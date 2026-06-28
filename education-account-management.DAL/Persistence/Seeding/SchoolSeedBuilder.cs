using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class SchoolSeedBuilder : ISeedBuilder
    {
        public int Priority => 50;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<School>().HasData(
                new School { Id = 1, SchoolName = "Northview Secondary School", Address = "10 Seed Campus Road, Singapore", PhoneNumber = "+656000001", Email = "school01@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 2, SchoolName = "Eastbridge Secondary School", Address = "20 Seed Campus Road, Singapore", PhoneNumber = "+656000002", Email = "school02@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 3, SchoolName = "Westhaven Secondary School", Address = "30 Seed Campus Road, Singapore", PhoneNumber = "+656000003", Email = "school03@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 4, SchoolName = "Southpoint Secondary School", Address = "40 Seed Campus Road, Singapore", PhoneNumber = "+656000004", Email = "school04@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 5, SchoolName = "Central Heights School", Address = "50 Seed Campus Road, Singapore", PhoneNumber = "+656000005", Email = "school05@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 6, SchoolName = "Riverside Learning Institute", Address = "60 Seed Campus Road, Singapore", PhoneNumber = "+656000006", Email = "school06@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 7, SchoolName = "Lakeside Technical School", Address = "70 Seed Campus Road, Singapore", PhoneNumber = "+656000007", Email = "school07@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 8, SchoolName = "Greenfield Academy", Address = "80 Seed Campus Road, Singapore", PhoneNumber = "+656000008", Email = "school08@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 9, SchoolName = "Harbourfront School", Address = "90 Seed Campus Road, Singapore", PhoneNumber = "+656000009", Email = "school09@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 10, SchoolName = "Hillcrest Education Centre", Address = "100 Seed Campus Road, Singapore", PhoneNumber = "+656000010", Email = "school10@example.edu.sg", Status = SchoolStatus.Inactive, CreatedAt = createdAt },
                new School { Id = 11, SchoolName = "Cedar Valley School", Address = "110 Seed Campus Road, Singapore", PhoneNumber = "+656000011", Email = "school11@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 12, SchoolName = "Maple Ridge Academy", Address = "120 Seed Campus Road, Singapore", PhoneNumber = "+656000012", Email = "school12@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 13, SchoolName = "Oceanview Institute", Address = "130 Seed Campus Road, Singapore", PhoneNumber = "+656000013", Email = "school13@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 14, SchoolName = "Brighton Learning Centre", Address = "140 Seed Campus Road, Singapore", PhoneNumber = "+656000014", Email = "school14@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 15, SchoolName = "Pioneer Technical College", Address = "150 Seed Campus Road, Singapore", PhoneNumber = "+656000015", Email = "school15@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 16, SchoolName = "Summit Arts School", Address = "160 Seed Campus Road, Singapore", PhoneNumber = "+656000016", Email = "school16@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 17, SchoolName = "Meridian Business School", Address = "170 Seed Campus Road, Singapore", PhoneNumber = "+656000017", Email = "school17@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 18, SchoolName = "Silverstream Polytechnic", Address = "180 Seed Campus Road, Singapore", PhoneNumber = "+656000018", Email = "school18@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 19, SchoolName = "Redwood Community College", Address = "190 Seed Campus Road, Singapore", PhoneNumber = "+656000019", Email = "school19@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 20, SchoolName = "Bluewater Skills Institute", Address = "200 Seed Campus Road, Singapore", PhoneNumber = "+656000020", Email = "school20@example.edu.sg", Status = SchoolStatus.Inactive, CreatedAt = createdAt },
                new School { Id = 21, SchoolName = "Golden Grove Academy", Address = "210 Seed Campus Road, Singapore", PhoneNumber = "+656000021", Email = "school21@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 22, SchoolName = "Sunrise Training Centre", Address = "220 Seed Campus Road, Singapore", PhoneNumber = "+656000022", Email = "school22@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 23, SchoolName = "Crescent School of Technology", Address = "230 Seed Campus Road, Singapore", PhoneNumber = "+656000023", Email = "school23@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 24, SchoolName = "Orchid City College", Address = "240 Seed Campus Road, Singapore", PhoneNumber = "+656000024", Email = "school24@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 25, SchoolName = "Evergreen Education Hub", Address = "250 Seed Campus Road, Singapore", PhoneNumber = "+656000025", Email = "school25@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 26, SchoolName = "Vista Applied Learning", Address = "260 Seed Campus Road, Singapore", PhoneNumber = "+656000026", Email = "school26@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 27, SchoolName = "Compass Point School", Address = "270 Seed Campus Road, Singapore", PhoneNumber = "+656000027", Email = "school27@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 28, SchoolName = "Newbridge Institute", Address = "280 Seed Campus Road, Singapore", PhoneNumber = "+656000028", Email = "school28@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 29, SchoolName = "Heritage Skills Academy", Address = "290 Seed Campus Road, Singapore", PhoneNumber = "+656000029", Email = "school29@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 30, SchoolName = "Frontier Science School", Address = "300 Seed Campus Road, Singapore", PhoneNumber = "+656000030", Email = "school30@example.edu.sg", Status = SchoolStatus.Inactive, CreatedAt = createdAt },
                new School { Id = 31, SchoolName = "Meadowbrook College", Address = "310 Seed Campus Road, Singapore", PhoneNumber = "+656000031", Email = "school31@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 32, SchoolName = "Peakview Education Centre", Address = "320 Seed Campus Road, Singapore", PhoneNumber = "+656000032", Email = "school32@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 33, SchoolName = "Bayfront Technical School", Address = "330 Seed Campus Road, Singapore", PhoneNumber = "+656000033", Email = "school33@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 34, SchoolName = "Queensway Learning Academy", Address = "340 Seed Campus Road, Singapore", PhoneNumber = "+656000034", Email = "school34@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 35, SchoolName = "Unity Continuing Education", Address = "350 Seed Campus Road, Singapore", PhoneNumber = "+656000035", Email = "school35@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 36, SchoolName = "Elmwood Professional School", Address = "360 Seed Campus Road, Singapore", PhoneNumber = "+656000036", Email = "school36@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 37, SchoolName = "Innovation Training Campus", Address = "370 Seed Campus Road, Singapore", PhoneNumber = "+656000037", Email = "school37@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 38, SchoolName = "Riverbend School", Address = "380 Seed Campus Road, Singapore", PhoneNumber = "+656000038", Email = "school38@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 39, SchoolName = "Stonefield Institute", Address = "390 Seed Campus Road, Singapore", PhoneNumber = "+656000039", Email = "school39@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 40, SchoolName = "Seaside Skills Centre", Address = "400 Seed Campus Road, Singapore", PhoneNumber = "+656000040", Email = "school40@example.edu.sg", Status = SchoolStatus.Inactive, CreatedAt = createdAt },
                new School { Id = 41, SchoolName = "Northstar Academy", Address = "410 Seed Campus Road, Singapore", PhoneNumber = "+656000041", Email = "school41@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 42, SchoolName = "Eastgate Technical College", Address = "420 Seed Campus Road, Singapore", PhoneNumber = "+656000042", Email = "school42@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 43, SchoolName = "Westlake Learning Hub", Address = "430 Seed Campus Road, Singapore", PhoneNumber = "+656000043", Email = "school43@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 44, SchoolName = "Southridge School", Address = "440 Seed Campus Road, Singapore", PhoneNumber = "+656000044", Email = "school44@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 45, SchoolName = "Central Park Institute", Address = "450 Seed Campus Road, Singapore", PhoneNumber = "+656000045", Email = "school45@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 46, SchoolName = "Hillview Polytechnic", Address = "460 Seed Campus Road, Singapore", PhoneNumber = "+656000046", Email = "school46@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 47, SchoolName = "Greenridge Academy", Address = "470 Seed Campus Road, Singapore", PhoneNumber = "+656000047", Email = "school47@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 48, SchoolName = "Harbour Bay College", Address = "480 Seed Campus Road, Singapore", PhoneNumber = "+656000048", Email = "school48@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 49, SchoolName = "Lighthouse Skills School", Address = "490 Seed Campus Road, Singapore", PhoneNumber = "+656000049", Email = "school49@example.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
                new School { Id = 50, SchoolName = "Civic Learning Centre", Address = "500 Seed Campus Road, Singapore", PhoneNumber = "+656000050", Email = "school50@example.edu.sg", Status = SchoolStatus.Inactive, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
