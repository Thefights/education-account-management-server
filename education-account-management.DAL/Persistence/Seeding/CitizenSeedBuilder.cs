using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class CitizenSeedBuilder : ISeedBuilder
    {
        public int Priority => 10;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<Citizen>().HasData(
                new Citizen { Id = 1, Nric = SingaporeNricUtil.Generate(1), FullName = "Sterling Quach", Email = "phuckhang1088@gmail.com", PhoneNumber = "+6590000001", ResidentialAddress = "1 Learning Grove, Singapore", MailingAddress = "1 Learning Grove, Singapore", DateOfBirth = new DateOnly(1995, 1, 2), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 2, Nric = SingaporeNricUtil.Generate(2), FullName = "Amelia Tan", Email = "amelia.tan@example.com", PhoneNumber = "+6590000002", ResidentialAddress = "2 Learning Grove, Singapore", MailingAddress = "2 Learning Grove, Singapore", DateOfBirth = new DateOnly(1996, 2, 3), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 3, Nric = SingaporeNricUtil.Generate(3), FullName = "Marcus Lim", Email = "marcus.lim@example.com", PhoneNumber = "+6590000003", ResidentialAddress = "3 Learning Grove, Singapore", MailingAddress = "3 Learning Grove, Singapore", DateOfBirth = new DateOnly(1997, 3, 4), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 4, Nric = SingaporeNricUtil.Generate(4), FullName = "Priya Nair", Email = "priya.nair@example.com", PhoneNumber = "+6590000004", ResidentialAddress = "4 Learning Grove, Singapore", MailingAddress = "4 Learning Grove, Singapore", DateOfBirth = new DateOnly(1998, 4, 5), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 5, Nric = SingaporeNricUtil.Generate(5), FullName = "Ethan Koh", Email = "ethan.koh@example.com", PhoneNumber = "+6590000005", ResidentialAddress = "5 Learning Grove, Singapore", MailingAddress = "5 Learning Grove, Singapore", DateOfBirth = new DateOnly(1999, 5, 6), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 6, Nric = SingaporeNricUtil.Generate(6), FullName = "Hannah Lee", Email = "hannah.lee@example.com", PhoneNumber = "+6590000006", ResidentialAddress = "6 Learning Grove, Singapore", MailingAddress = "6 Learning Grove, Singapore", DateOfBirth = new DateOnly(2000, 6, 7), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 7, Nric = SingaporeNricUtil.Generate(7), FullName = "Daniel Wong", Email = "daniel.wong@example.com", PhoneNumber = "+6590000007", ResidentialAddress = "7 Learning Grove, Singapore", MailingAddress = "7 Learning Grove, Singapore", DateOfBirth = new DateOnly(2001, 7, 8), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 8, Nric = SingaporeNricUtil.Generate(8), FullName = "Sofia Chen", Email = "sofia.chen@example.com", PhoneNumber = "+6590000008", ResidentialAddress = "8 Learning Grove, Singapore", MailingAddress = "8 Learning Grove, Singapore", DateOfBirth = new DateOnly(2002, 8, 9), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 9, Nric = SingaporeNricUtil.Generate(9), FullName = "Lucas Nguyen", Email = "lucas.nguyen@example.com", PhoneNumber = "+6590000009", ResidentialAddress = "9 Learning Grove, Singapore", MailingAddress = "9 Learning Grove, Singapore", DateOfBirth = new DateOnly(2003, 9, 10), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 10, Nric = SingaporeNricUtil.Generate(10), FullName = "Maya Rahman", Email = "maya.rahman@example.com", PhoneNumber = "+6590000010", ResidentialAddress = "10 Learning Grove, Singapore", MailingAddress = "10 Learning Grove, Singapore", DateOfBirth = new DateOnly(2004, 10, 11), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 11, Nric = SingaporeNricUtil.Generate(11), FullName = "Noah Teo", Email = "noah.teo@example.com", PhoneNumber = "+6590000011", ResidentialAddress = "11 Learning Grove, Singapore", MailingAddress = "11 Learning Grove, Singapore", DateOfBirth = new DateOnly(2005, 11, 12), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 12, Nric = SingaporeNricUtil.Generate(12), FullName = "Aisha Fernandez", Email = "aisha.fernandez@example.com", PhoneNumber = "+6590000012", ResidentialAddress = "12 Learning Grove, Singapore", MailingAddress = "12 Learning Grove, Singapore", DateOfBirth = new DateOnly(1994, 12, 13), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 13, Nric = SingaporeNricUtil.Generate(13), FullName = "Ryan Chua", Email = "ryan.chua@example.com", PhoneNumber = "+6590000013", ResidentialAddress = "13 Learning Grove, Singapore", MailingAddress = "13 Learning Grove, Singapore", DateOfBirth = new DateOnly(1995, 1, 14), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 14, Nric = SingaporeNricUtil.Generate(14), FullName = "Chloe Goh", Email = "chloe.goh@example.com", PhoneNumber = "+6590000014", ResidentialAddress = "14 Learning Grove, Singapore", MailingAddress = "14 Learning Grove, Singapore", DateOfBirth = new DateOnly(1996, 2, 15), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 15, Nric = SingaporeNricUtil.Generate(15), FullName = "Irfan Hassan", Email = "irfan.hassan@example.com", PhoneNumber = "+6590000015", ResidentialAddress = "15 Learning Grove, Singapore", MailingAddress = "15 Learning Grove, Singapore", DateOfBirth = new DateOnly(1997, 3, 16), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 16, Nric = SingaporeNricUtil.Generate(16), FullName = "Natalie Seah", Email = "natalie.seah@example.com", PhoneNumber = "+6590000016", ResidentialAddress = "16 Learning Grove, Singapore", MailingAddress = "16 Learning Grove, Singapore", DateOfBirth = new DateOnly(1998, 4, 17), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 17, Nric = SingaporeNricUtil.Generate(17), FullName = "Alina Ang", Email = "alina.ang@example.com", PhoneNumber = "+6590000017", ResidentialAddress = "17 Learning Grove, Singapore", MailingAddress = "17 Learning Grove, Singapore", DateOfBirth = new DateOnly(1999, 5, 18), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 18, Nric = SingaporeNricUtil.Generate(18), FullName = "Benjamin Bala", Email = "benjamin.bala@example.com", PhoneNumber = "+6590000018", ResidentialAddress = "18 Learning Grove, Singapore", MailingAddress = "18 Learning Grove, Singapore", DateOfBirth = new DateOnly(2000, 6, 19), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 19, Nric = SingaporeNricUtil.Generate(19), FullName = "Clara Chew", Email = "clara.chew@example.com", PhoneNumber = "+6590000019", ResidentialAddress = "19 Learning Grove, Singapore", MailingAddress = "19 Learning Grove, Singapore", DateOfBirth = new DateOnly(2001, 7, 20), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 20, Nric = SingaporeNricUtil.Generate(20), FullName = "Darius Das", Email = "darius.das@example.com", PhoneNumber = "+6590000020", ResidentialAddress = "20 Learning Grove, Singapore", MailingAddress = "20 Learning Grove, Singapore", DateOfBirth = new DateOnly(2002, 8, 21), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 21, Nric = SingaporeNricUtil.Generate(21), FullName = "Elena Eng", Email = "elena.eng@example.com", PhoneNumber = "+6590000021", ResidentialAddress = "21 Learning Grove, Singapore", MailingAddress = "21 Learning Grove, Singapore", DateOfBirth = new DateOnly(2003, 9, 22), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 22, Nric = SingaporeNricUtil.Generate(22), FullName = "Farhan Foo", Email = "farhan.foo@example.com", PhoneNumber = "+6590000022", ResidentialAddress = "22 Learning Grove, Singapore", MailingAddress = "22 Learning Grove, Singapore", DateOfBirth = new DateOnly(2004, 10, 23), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 23, Nric = SingaporeNricUtil.Generate(23), FullName = "Grace Gan", Email = "grace.gan@example.com", PhoneNumber = "+6590000023", ResidentialAddress = "23 Learning Grove, Singapore", MailingAddress = "23 Learning Grove, Singapore", DateOfBirth = new DateOnly(2005, 11, 24), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 24, Nric = SingaporeNricUtil.Generate(24), FullName = "Haruto Ho", Email = "haruto.ho@example.com", PhoneNumber = "+6590000024", ResidentialAddress = "24 Learning Grove, Singapore", MailingAddress = "24 Learning Grove, Singapore", DateOfBirth = new DateOnly(1994, 12, 25), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 25, Nric = SingaporeNricUtil.Generate(25), FullName = "Isabelle Ismail", Email = "isabelle.ismail@example.com", PhoneNumber = "+6590000025", ResidentialAddress = "25 Learning Grove, Singapore", MailingAddress = "25 Learning Grove, Singapore", DateOfBirth = new DateOnly(1995, 1, 26), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 26, Nric = SingaporeNricUtil.Generate(26), FullName = "Jasper Jeyaratnam", Email = "jasper.jeyaratnam@example.com", PhoneNumber = "+6590000026", ResidentialAddress = "26 Learning Grove, Singapore", MailingAddress = "26 Learning Grove, Singapore", DateOfBirth = new DateOnly(1996, 2, 27), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 27, Nric = SingaporeNricUtil.Generate(27), FullName = "Keira Kwek", Email = "keira.kwek@example.com", PhoneNumber = "+6590000027", ResidentialAddress = "27 Learning Grove, Singapore", MailingAddress = "27 Learning Grove, Singapore", DateOfBirth = new DateOnly(1997, 3, 1), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 28, Nric = SingaporeNricUtil.Generate(28), FullName = "Leon Lim", Email = "leon.lim@example.com", PhoneNumber = "+6590000028", ResidentialAddress = "28 Learning Grove, Singapore", MailingAddress = "28 Learning Grove, Singapore", DateOfBirth = new DateOnly(1998, 4, 2), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 29, Nric = SingaporeNricUtil.Generate(29), FullName = "Mei Lin Mohamed", Email = "mei.lin.mohamed@example.com", PhoneNumber = "+6590000029", ResidentialAddress = "29 Learning Grove, Singapore", MailingAddress = "29 Learning Grove, Singapore", DateOfBirth = new DateOnly(1999, 5, 3), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 30, Nric = SingaporeNricUtil.Generate(30), FullName = "Nathan Ng", Email = "nathan.ng@example.com", PhoneNumber = "+6590000030", ResidentialAddress = "30 Learning Grove, Singapore", MailingAddress = "30 Learning Grove, Singapore", DateOfBirth = new DateOnly(2000, 6, 4), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 31, Nric = SingaporeNricUtil.Generate(31), FullName = "Olivia Ong", Email = "olivia.ong@example.com", PhoneNumber = "+6590000031", ResidentialAddress = "31 Learning Grove, Singapore", MailingAddress = "31 Learning Grove, Singapore", DateOfBirth = new DateOnly(2001, 7, 5), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 32, Nric = SingaporeNricUtil.Generate(32), FullName = "Pranav Pillai", Email = "pranav.pillai@example.com", PhoneNumber = "+6590000032", ResidentialAddress = "32 Learning Grove, Singapore", MailingAddress = "32 Learning Grove, Singapore", DateOfBirth = new DateOnly(2002, 8, 6), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 33, Nric = SingaporeNricUtil.Generate(33), FullName = "Qistina Quek", Email = "qistina.quek@example.com", PhoneNumber = "+6590000033", ResidentialAddress = "33 Learning Grove, Singapore", MailingAddress = "33 Learning Grove, Singapore", DateOfBirth = new DateOnly(2003, 9, 7), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 34, Nric = SingaporeNricUtil.Generate(34), FullName = "Rafael Rao", Email = "rafael.rao@example.com", PhoneNumber = "+6590000034", ResidentialAddress = "34 Learning Grove, Singapore", MailingAddress = "34 Learning Grove, Singapore", DateOfBirth = new DateOnly(2004, 10, 8), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 35, Nric = SingaporeNricUtil.Generate(35), FullName = "Selina Sim", Email = "selina.sim@example.com", PhoneNumber = "+6590000035", ResidentialAddress = "35 Learning Grove, Singapore", MailingAddress = "35 Learning Grove, Singapore", DateOfBirth = new DateOnly(2005, 11, 9), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 36, Nric = SingaporeNricUtil.Generate(36), FullName = "Terence Tan", Email = "terence.tan@example.com", PhoneNumber = "+6590000036", ResidentialAddress = "36 Learning Grove, Singapore", MailingAddress = "36 Learning Grove, Singapore", DateOfBirth = new DateOnly(1994, 12, 10), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 37, Nric = SingaporeNricUtil.Generate(37), FullName = "Umairah Uddin", Email = "umairah.uddin@example.com", PhoneNumber = "+6590000037", ResidentialAddress = "37 Learning Grove, Singapore", MailingAddress = "37 Learning Grove, Singapore", DateOfBirth = new DateOnly(1995, 1, 11), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 38, Nric = SingaporeNricUtil.Generate(38), FullName = "Victor Vasquez", Email = "victor.vasquez@example.com", PhoneNumber = "+6590000038", ResidentialAddress = "38 Learning Grove, Singapore", MailingAddress = "38 Learning Grove, Singapore", DateOfBirth = new DateOnly(1996, 2, 12), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 39, Nric = SingaporeNricUtil.Generate(39), FullName = "Wen Jie Wong", Email = "wen.jie.wong@example.com", PhoneNumber = "+6590000039", ResidentialAddress = "39 Learning Grove, Singapore", MailingAddress = "39 Learning Grove, Singapore", DateOfBirth = new DateOnly(1997, 3, 13), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 40, Nric = SingaporeNricUtil.Generate(40), FullName = "Xavier Xu", Email = "xavier.xu@example.com", PhoneNumber = "+6590000040", ResidentialAddress = "40 Learning Grove, Singapore", MailingAddress = "40 Learning Grove, Singapore", DateOfBirth = new DateOnly(1998, 4, 14), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 41, Nric = SingaporeNricUtil.Generate(41), FullName = "Yasmin Yeo", Email = "yasmin.yeo@example.com", PhoneNumber = "+6590000041", ResidentialAddress = "41 Learning Grove, Singapore", MailingAddress = "41 Learning Grove, Singapore", DateOfBirth = new DateOnly(1999, 5, 15), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 42, Nric = SingaporeNricUtil.Generate(42), FullName = "Zachary Zainal", Email = "zachary.zainal@example.com", PhoneNumber = "+6590000042", ResidentialAddress = "42 Learning Grove, Singapore", MailingAddress = "42 Learning Grove, Singapore", DateOfBirth = new DateOnly(2000, 6, 16), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 43, Nric = SingaporeNricUtil.Generate(43), FullName = "Adeline Ang", Email = "adeline.ang@example.com", PhoneNumber = "+6590000043", ResidentialAddress = "43 Learning Grove, Singapore", MailingAddress = "43 Learning Grove, Singapore", DateOfBirth = new DateOnly(2001, 7, 17), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 44, Nric = SingaporeNricUtil.Generate(44), FullName = "Brandon Bala", Email = "brandon.bala@example.com", PhoneNumber = "+6590000044", ResidentialAddress = "44 Learning Grove, Singapore", MailingAddress = "44 Learning Grove, Singapore", DateOfBirth = new DateOnly(2002, 8, 18), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 45, Nric = SingaporeNricUtil.Generate(45), FullName = "Celeste Chew", Email = "celeste.chew@example.com", PhoneNumber = "+6590000045", ResidentialAddress = "45 Learning Grove, Singapore", MailingAddress = "45 Learning Grove, Singapore", DateOfBirth = new DateOnly(2003, 9, 19), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 46, Nric = SingaporeNricUtil.Generate(46), FullName = "Damien Das", Email = "damien.das@example.com", PhoneNumber = "+6590000046", ResidentialAddress = "46 Learning Grove, Singapore", MailingAddress = "46 Learning Grove, Singapore", DateOfBirth = new DateOnly(2004, 10, 20), SchoolingStatus = "Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 47, Nric = SingaporeNricUtil.Generate(47), FullName = "Evelyn Eng", Email = "evelyn.eng@example.com", PhoneNumber = "+6590000047", ResidentialAddress = "47 Learning Grove, Singapore", MailingAddress = "47 Learning Grove, Singapore", DateOfBirth = new DateOnly(2005, 11, 21), SchoolingStatus = "Not Enrolled", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 48, Nric = SingaporeNricUtil.Generate(48), FullName = "Faris Foo", Email = "faris.foo@example.com", PhoneNumber = "+6590000048", ResidentialAddress = "48 Learning Grove, Singapore", MailingAddress = "48 Learning Grove, Singapore", DateOfBirth = new DateOnly(1994, 12, 22), SchoolingStatus = "Graduated", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 49, Nric = SingaporeNricUtil.Generate(49), FullName = "Giselle Gan", Email = "giselle.gan@example.com", PhoneNumber = "+6590000049", ResidentialAddress = "49 Learning Grove, Singapore", MailingAddress = "49 Learning Grove, Singapore", DateOfBirth = new DateOnly(1995, 1, 23), SchoolingStatus = "Suspended", IsSingaporean = true, CreatedAt = createdAt },
                new Citizen { Id = 50, Nric = SingaporeNricUtil.Generate(50), FullName = "Haziq Ho", Email = "haziq.ho@example.com", PhoneNumber = "+6590000050", ResidentialAddress = "50 Learning Grove, Singapore", MailingAddress = "50 Learning Grove, Singapore", DateOfBirth = new DateOnly(1996, 2, 24), SchoolingStatus = "Withdrawn", IsSingaporean = true, CreatedAt = createdAt });

            var sweepCitizens = new List<Citizen>();
            var totalSweepCitizens = SeedScenarioConstants.SweepAccountsPerDay * SeedScenarioConstants.SweepDayCount;
            for (int index = 0; index < totalSweepCitizens; index++)
            {
                var id = SeedScenarioConstants.SweepCitizenStartId + index;
                sweepCitizens.Add(new Citizen
                {
                    Id = id,
                    Nric = SingaporeNricUtil.Generate(id),
                    FullName = $"Sweep Citizen {index + 1:D3}",
                    Email = $"sweep.citizen.{index + 1:D3}@example.com",
                    PhoneNumber = $"+658{id:D8}"[..12],
                    ResidentialAddress = $"{index + 1} Sweep Road, Singapore",
                    MailingAddress = $"{index + 1} Sweep Road, Singapore",
                    DateOfBirth = new DateOnly(2000, ((index % 12) + 1), ((index % 28) + 1)),
                    SchoolingStatus = "Not Enrolled",
                    IsSingaporean = true,
                    CreatedAt = createdAt
                });
            }

            modelBuilder.Entity<Citizen>().HasData(sweepCitizens);

            var manualCitizens = new List<Citizen>();
            for (int index = 0; index < SeedScenarioConstants.ManualCitizenCount; index++)
            {
                var id = SeedScenarioConstants.ManualCitizenStartId + index;
                manualCitizens.Add(new Citizen
                {
                    Id = id,
                    Nric = SingaporeNricUtil.Generate(id),
                    FullName = $"Manual Account Test Citizen {index + 1:D2}",
                    Email = $"manual.account.test.{index + 1:D2}@example.com",
                    PhoneNumber = $"+659901{index + 1:D4}",
                    ResidentialAddress = $"{index + 1} Manual Test Lane, Singapore",
                    MailingAddress = $"{index + 1} Manual Test Lane, Singapore",
                    DateOfBirth = new DateOnly(2000, ((index % 12) + 1), ((index % 28) + 1)),
                    SchoolingStatus = "Not Enrolled",
                    IsSingaporean = true,
                    IsAutoSweepExcluded = true,
                    CreatedAt = createdAt
                });
            }

            modelBuilder.Entity<Citizen>().HasData(manualCitizens);

            return modelBuilder;
        }
    }
}
