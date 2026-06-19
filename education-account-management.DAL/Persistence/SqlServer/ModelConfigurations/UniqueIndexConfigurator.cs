using Models;

namespace Persistence.SqlServer.ModelConfigurations
{
    public static class UniqueIndexConfigurator
    {
        public static void ConfigureUniqueIndexes(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var uniqueIndexes = entityType.GetIndexes().Where(i => i.IsUnique).ToList();
                var uniqueProperties = entityType.ClrType.GetProperties()
                    .Where(p => Attribute.IsDefined(p, typeof(UniqueAttribute)))
                    .ToList();

                if (typeof(AuditEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Create unique indexes with soft-delete filter and allow multiple NULLs by excluding NULLs
                    foreach (var index in uniqueIndexes)
                    {
                        var properties = index.Properties.Select(p => p.Name).ToArray();
                        entityType.RemoveIndex(index.Properties);
                        var nullFilters = properties.Select(p => $"\"{p}\" IS NOT NULL");
                        var filterParts = IsPermanentUniqueIndex(entityType.ClrType, properties)
                            ? nullFilters
                            : new[] { $"\"{nameof(AuditEntity.IsDeleted)}\" = 0" }.Concat(nullFilters);
                        var filter = string.Join(" AND ", filterParts);
                        modelBuilder.Entity(entityType.ClrType)
                                    .HasIndex(properties)
                                    .IsUnique()
                                    .HasFilter(filter);
                    }

                    // Create unique indexes for properties with [Unique] attribute
                    foreach (var property in uniqueProperties)
                    {
                        var filter = IsPermanentUniqueIndex(entityType.ClrType, [property.Name])
                            ? $"\"{property.Name}\" IS NOT NULL"
                            : $"\"{nameof(AuditEntity.IsDeleted)}\" = 0 AND \"{property.Name}\" IS NOT NULL";
                        modelBuilder.Entity(entityType.ClrType)
                                    .HasIndex(property.Name)
                                    .IsUnique()
                                    .HasFilter(filter);
                    }
                }
                else
                {
                    foreach (var property in uniqueProperties)
                    {
                        // For non-audited entities, still allow multiple NULLs in unique indexes
                        modelBuilder.Entity(entityType.ClrType)
                                    .HasIndex(property.Name)
                                    .IsUnique()
                                    .HasFilter($"\"{property.Name}\" IS NOT NULL");
                    }
                }
            }
        }

        private static bool IsPermanentUniqueIndex(Type entityType, IReadOnlyCollection<string> properties)
        {
            var propertySet = properties.ToHashSet(StringComparer.Ordinal);

            return entityType == typeof(Citizen) &&
                   (propertySet.SetEquals([nameof(Citizen.Nric)]) ||
                    propertySet.SetEquals([nameof(Citizen.SingpassSubjectId)])) ||
                   entityType == typeof(EducationCreditTransaction) &&
                   propertySet.SetEquals([nameof(EducationCreditTransaction.TransactionCode)]) ||
                   entityType == typeof(SsoIdentity) &&
                   (propertySet.SetEquals([nameof(SsoIdentity.Provider), nameof(SsoIdentity.ProviderUserId)]) ||
                    propertySet.SetEquals([nameof(SsoIdentity.AuthAccountId), nameof(SsoIdentity.Provider)]));
        }
    }
}