using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class AdminProfileSeedBuilder : ISeedBuilder
    {
        public int Priority => 40;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<AdminProfile>().HasData(
                new AdminProfile { Id = 1, StaffCode = "STAFF-00001", FullName = "System Administrator", Nric = SingaporeNricUtil.Generate(101), Email = "admin001@example.com", PhoneNumber = "+6580000001", UserId = 1, CreatedAt = createdAt },
                new AdminProfile { Id = 2, StaffCode = "STAFF-00002", FullName = "Finance Administrator", Nric = SingaporeNricUtil.Generate(102), Email = "admin002@example.com", PhoneNumber = "+6580000002", UserId = 2, CreatedAt = createdAt },
                new AdminProfile { Id = 3, StaffCode = "STAFF-00003", FullName = "School Administrator", Nric = SingaporeNricUtil.Generate(103), Email = "admin003@example.com", PhoneNumber = "+6580000003", UserId = 3, SchoolId = 1, CreatedAt = createdAt },
                new AdminProfile { Id = 4, StaffCode = "STAFF-00004", FullName = "Demo Administrator 004", Nric = SingaporeNricUtil.Generate(104), Email = "admin004@example.com", PhoneNumber = "+6580000004", UserId = 5, CreatedAt = createdAt },
                new AdminProfile { Id = 5, StaffCode = "STAFF-00005", FullName = "Demo Administrator 005", Nric = SingaporeNricUtil.Generate(105), Email = "admin005@example.com", PhoneNumber = "+6580000005", UserId = 6, CreatedAt = createdAt },
                new AdminProfile { Id = 6, StaffCode = "STAFF-00006", FullName = "Demo Administrator 006", Nric = SingaporeNricUtil.Generate(106), Email = "admin006@example.com", PhoneNumber = "+6580000006", UserId = 7, SchoolId = 7, CreatedAt = createdAt },
                new AdminProfile { Id = 7, StaffCode = "STAFF-00007", FullName = "Demo Administrator 007", Nric = SingaporeNricUtil.Generate(107), Email = "admin007@example.com", PhoneNumber = "+6580000007", UserId = 8, CreatedAt = createdAt },
                new AdminProfile { Id = 8, StaffCode = "STAFF-00008", FullName = "Demo Administrator 008", Nric = SingaporeNricUtil.Generate(108), Email = "admin008@example.com", PhoneNumber = "+6580000008", UserId = 9, CreatedAt = createdAt },
                new AdminProfile { Id = 9, StaffCode = "STAFF-00009", FullName = "Demo Administrator 009", Nric = SingaporeNricUtil.Generate(109), Email = "admin009@example.com", PhoneNumber = "+6580000009", UserId = 10, SchoolId = 10, CreatedAt = createdAt },
                new AdminProfile { Id = 10, StaffCode = "STAFF-00010", FullName = "Demo Administrator 010", Nric = SingaporeNricUtil.Generate(110), Email = "admin010@example.com", PhoneNumber = "+6580000010", UserId = 11, CreatedAt = createdAt },
                new AdminProfile { Id = 11, StaffCode = "STAFF-00011", FullName = "Demo Administrator 011", Nric = SingaporeNricUtil.Generate(111), Email = "admin011@example.com", PhoneNumber = "+6580000011", UserId = 12, CreatedAt = createdAt },
                new AdminProfile { Id = 12, StaffCode = "STAFF-00012", FullName = "Demo Administrator 012", Nric = SingaporeNricUtil.Generate(112), Email = "admin012@example.com", PhoneNumber = "+6580000012", UserId = 13, SchoolId = 13, CreatedAt = createdAt },
                new AdminProfile { Id = 13, StaffCode = "STAFF-00013", FullName = "Demo Administrator 013", Nric = SingaporeNricUtil.Generate(113), Email = "admin013@example.com", PhoneNumber = "+6580000013", UserId = 14, CreatedAt = createdAt },
                new AdminProfile { Id = 14, StaffCode = "STAFF-00014", FullName = "Demo Administrator 014", Nric = SingaporeNricUtil.Generate(114), Email = "admin014@example.com", PhoneNumber = "+6580000014", UserId = 15, CreatedAt = createdAt },
                new AdminProfile { Id = 15, StaffCode = "STAFF-00015", FullName = "Demo Administrator 015", Nric = SingaporeNricUtil.Generate(115), Email = "admin015@example.com", PhoneNumber = "+6580000015", UserId = 16, SchoolId = 16, CreatedAt = createdAt },
                new AdminProfile { Id = 16, StaffCode = "STAFF-00016", FullName = "Demo Administrator 016", Nric = SingaporeNricUtil.Generate(116), Email = "admin016@example.com", PhoneNumber = "+6580000016", UserId = 17, CreatedAt = createdAt },
                new AdminProfile { Id = 17, StaffCode = "STAFF-00017", FullName = "Demo Administrator 017", Nric = SingaporeNricUtil.Generate(117), Email = "admin017@example.com", PhoneNumber = "+6580000017", UserId = 18, CreatedAt = createdAt },
                new AdminProfile { Id = 18, StaffCode = "STAFF-00018", FullName = "Demo Administrator 018", Nric = SingaporeNricUtil.Generate(118), Email = "admin018@example.com", PhoneNumber = "+6580000018", UserId = 19, SchoolId = 19, CreatedAt = createdAt },
                new AdminProfile { Id = 19, StaffCode = "STAFF-00019", FullName = "Demo Administrator 019", Nric = SingaporeNricUtil.Generate(119), Email = "admin019@example.com", PhoneNumber = "+6580000019", UserId = 20, CreatedAt = createdAt },
                new AdminProfile { Id = 20, StaffCode = "STAFF-00020", FullName = "Demo Administrator 020", Nric = SingaporeNricUtil.Generate(120), Email = "admin020@example.com", PhoneNumber = "+6580000020", UserId = 21, CreatedAt = createdAt },
                new AdminProfile { Id = 21, StaffCode = "STAFF-00021", FullName = "Demo Administrator 021", Nric = SingaporeNricUtil.Generate(121), Email = "admin021@example.com", PhoneNumber = "+6580000021", UserId = 22, SchoolId = 22, CreatedAt = createdAt },
                new AdminProfile { Id = 22, StaffCode = "STAFF-00022", FullName = "Demo Administrator 022", Nric = SingaporeNricUtil.Generate(122), Email = "admin022@example.com", PhoneNumber = "+6580000022", UserId = 23, CreatedAt = createdAt },
                new AdminProfile { Id = 23, StaffCode = "STAFF-00023", FullName = "Demo Administrator 023", Nric = SingaporeNricUtil.Generate(123), Email = "admin023@example.com", PhoneNumber = "+6580000023", UserId = 24, CreatedAt = createdAt },
                new AdminProfile { Id = 24, StaffCode = "STAFF-00024", FullName = "Demo Administrator 024", Nric = SingaporeNricUtil.Generate(124), Email = "admin024@example.com", PhoneNumber = "+6580000024", UserId = 25, SchoolId = 25, CreatedAt = createdAt },
                new AdminProfile { Id = 25, StaffCode = "STAFF-00025", FullName = "Demo Administrator 025", Nric = SingaporeNricUtil.Generate(125), Email = "admin025@example.com", PhoneNumber = "+6580000025", UserId = 26, CreatedAt = createdAt },
                new AdminProfile { Id = 26, StaffCode = "STAFF-00026", FullName = "Demo Administrator 026", Nric = SingaporeNricUtil.Generate(126), Email = "admin026@example.com", PhoneNumber = "+6580000026", UserId = 27, CreatedAt = createdAt },
                new AdminProfile { Id = 27, StaffCode = "STAFF-00027", FullName = "Demo Administrator 027", Nric = SingaporeNricUtil.Generate(127), Email = "admin027@example.com", PhoneNumber = "+6580000027", UserId = 28, SchoolId = 28, CreatedAt = createdAt },
                new AdminProfile { Id = 28, StaffCode = "STAFF-00028", FullName = "Demo Administrator 028", Nric = SingaporeNricUtil.Generate(128), Email = "admin028@example.com", PhoneNumber = "+6580000028", UserId = 29, CreatedAt = createdAt },
                new AdminProfile { Id = 29, StaffCode = "STAFF-00029", FullName = "Demo Administrator 029", Nric = SingaporeNricUtil.Generate(129), Email = "admin029@example.com", PhoneNumber = "+6580000029", UserId = 30, CreatedAt = createdAt },
                new AdminProfile { Id = 30, StaffCode = "STAFF-00030", FullName = "Demo Administrator 030", Nric = SingaporeNricUtil.Generate(130), Email = "admin030@example.com", PhoneNumber = "+6580000030", UserId = 31, SchoolId = 31, CreatedAt = createdAt },
                new AdminProfile { Id = 31, StaffCode = "STAFF-00031", FullName = "Demo Administrator 031", Nric = SingaporeNricUtil.Generate(131), Email = "admin031@example.com", PhoneNumber = "+6580000031", UserId = 32, CreatedAt = createdAt },
                new AdminProfile { Id = 32, StaffCode = "STAFF-00032", FullName = "Demo Administrator 032", Nric = SingaporeNricUtil.Generate(132), Email = "admin032@example.com", PhoneNumber = "+6580000032", UserId = 33, CreatedAt = createdAt },
                new AdminProfile { Id = 33, StaffCode = "STAFF-00033", FullName = "Demo Administrator 033", Nric = SingaporeNricUtil.Generate(133), Email = "admin033@example.com", PhoneNumber = "+6580000033", UserId = 34, SchoolId = 34, CreatedAt = createdAt },
                new AdminProfile { Id = 34, StaffCode = "STAFF-00034", FullName = "Demo Administrator 034", Nric = SingaporeNricUtil.Generate(134), Email = "admin034@example.com", PhoneNumber = "+6580000034", UserId = 35, CreatedAt = createdAt },
                new AdminProfile { Id = 35, StaffCode = "STAFF-00035", FullName = "Demo Administrator 035", Nric = SingaporeNricUtil.Generate(135), Email = "admin035@example.com", PhoneNumber = "+6580000035", UserId = 36, CreatedAt = createdAt },
                new AdminProfile { Id = 36, StaffCode = "STAFF-00036", FullName = "Demo Administrator 036", Nric = SingaporeNricUtil.Generate(136), Email = "admin036@example.com", PhoneNumber = "+6580000036", UserId = 37, SchoolId = 37, CreatedAt = createdAt },
                new AdminProfile { Id = 37, StaffCode = "STAFF-00037", FullName = "Demo Administrator 037", Nric = SingaporeNricUtil.Generate(137), Email = "admin037@example.com", PhoneNumber = "+6580000037", UserId = 38, CreatedAt = createdAt },
                new AdminProfile { Id = 38, StaffCode = "STAFF-00038", FullName = "Demo Administrator 038", Nric = SingaporeNricUtil.Generate(138), Email = "admin038@example.com", PhoneNumber = "+6580000038", UserId = 39, CreatedAt = createdAt },
                new AdminProfile { Id = 39, StaffCode = "STAFF-00039", FullName = "Demo Administrator 039", Nric = SingaporeNricUtil.Generate(139), Email = "admin039@example.com", PhoneNumber = "+6580000039", UserId = 40, SchoolId = 40, CreatedAt = createdAt },
                new AdminProfile { Id = 40, StaffCode = "STAFF-00040", FullName = "Demo Administrator 040", Nric = SingaporeNricUtil.Generate(140), Email = "admin040@example.com", PhoneNumber = "+6580000040", UserId = 41, CreatedAt = createdAt },
                new AdminProfile { Id = 41, StaffCode = "STAFF-00041", FullName = "Demo Administrator 041", Nric = SingaporeNricUtil.Generate(141), Email = "admin041@example.com", PhoneNumber = "+6580000041", UserId = 42, CreatedAt = createdAt },
                new AdminProfile { Id = 42, StaffCode = "STAFF-00042", FullName = "Demo Administrator 042", Nric = SingaporeNricUtil.Generate(142), Email = "admin042@example.com", PhoneNumber = "+6580000042", UserId = 43, SchoolId = 43, CreatedAt = createdAt },
                new AdminProfile { Id = 43, StaffCode = "STAFF-00043", FullName = "Demo Administrator 043", Nric = SingaporeNricUtil.Generate(143), Email = "admin043@example.com", PhoneNumber = "+6580000043", UserId = 44, CreatedAt = createdAt },
                new AdminProfile { Id = 44, StaffCode = "STAFF-00044", FullName = "Demo Administrator 044", Nric = SingaporeNricUtil.Generate(144), Email = "admin044@example.com", PhoneNumber = "+6580000044", UserId = 45, CreatedAt = createdAt },
                new AdminProfile { Id = 45, StaffCode = "STAFF-00045", FullName = "Demo Administrator 045", Nric = SingaporeNricUtil.Generate(145), Email = "admin045@example.com", PhoneNumber = "+6580000045", UserId = 46, SchoolId = 46, CreatedAt = createdAt },
                new AdminProfile { Id = 46, StaffCode = "STAFF-00046", FullName = "Demo Administrator 046", Nric = SingaporeNricUtil.Generate(146), Email = "admin046@example.com", PhoneNumber = "+6580000046", UserId = 47, CreatedAt = createdAt },
                new AdminProfile { Id = 47, StaffCode = "STAFF-00047", FullName = "Demo Administrator 047", Nric = SingaporeNricUtil.Generate(147), Email = "admin047@example.com", PhoneNumber = "+6580000047", UserId = 48, CreatedAt = createdAt },
                new AdminProfile { Id = 48, StaffCode = "STAFF-00048", FullName = "Demo Administrator 048", Nric = SingaporeNricUtil.Generate(148), Email = "admin048@example.com", PhoneNumber = "+6580000048", UserId = 49, SchoolId = 49, CreatedAt = createdAt },
                new AdminProfile { Id = 49, StaffCode = "STAFF-00049", FullName = "Demo Administrator 049", Nric = SingaporeNricUtil.Generate(149), Email = "admin049@example.com", PhoneNumber = "+6580000049", UserId = 50, CreatedAt = createdAt },
                new AdminProfile { Id = 50, StaffCode = "STAFF-00050", FullName = "Demo Administrator 050", Nric = SingaporeNricUtil.Generate(150), Email = "admin050@example.com", PhoneNumber = "+6580000050", UserId = 51, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
