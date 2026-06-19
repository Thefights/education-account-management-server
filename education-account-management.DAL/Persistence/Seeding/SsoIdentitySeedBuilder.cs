using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class SsoIdentitySeedBuilder : ISeedBuilder
{
    public int Priority => 30;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<SsoIdentity>().HasData(
     Enumerable.Range(1, 15).Select(id => new SsoIdentity
     {
         Id = id,
         Provider = id <= 8 ? SsoProvider.AzureAD : SsoProvider.Singpass,
         ProviderUserId = id switch
         {
             1 => "ae5873a0-acbe-4976-ba46-69aee20cfa48",

             2 => "f8cdef31-a31e-4b4a-93e4-5f571e91255a",
             3 => "b1e7cdf2-43ef-4a3e-9eb8-4e63b3ae42f4",
             4 => "f116e09e-1a6f-4847-aa57-442705d242d0",
             5 => "db3c66a7-b7f4-47df-bd7a-3f70fcaaa73d",
             6 => "0446ecca-6483-4129-bd4f-906f970f18d5",

             7 => "azure-object-007",
             8 => "azure-object-008",

             _ => $"singpass-subject-{id:000}"
         },
         UserId = id,
         CreatedAt = createdAt
     }).ToArray());

        return modelBuilder;
    }

}
