using Microsoft.EntityFrameworkCore.ChangeTracking;

using Exceptions;

namespace Persistence.SqlServer.ModelConfigurations
{
    public static class SoftDeleteCascadeHandler
    {
        public static async Task ApplySoftDeleteCascadeAsync(
            DbContext context,
            CancellationToken cancellationToken = default)
        {
            var softDeletedEntries = context.ChangeTracker.Entries<AuditEntity>()
                .Where(e => e.State == EntityState.Modified &&
                            e.Property(nameof(AuditEntity.IsDeleted)).IsModified &&
                            e.Entity.IsDeleted)
                .ToList();

            foreach (var entry in softDeletedEntries)
            {
                await ProcessEntityCascadeAsync(context, entry.Entity, cancellationToken);
            }
        }

        private static async Task ProcessEntityCascadeAsync(
            DbContext context,
            object entity,
            CancellationToken cancellationToken)
        {
            var entityType = entity.GetType();
            var entry = context.Entry(entity);

            var efEntityType = context.Model.FindEntityType(entityType);
            if (efEntityType == null) return;

            var principalNavigations = efEntityType.GetNavigations()
                .Where(n => n.ForeignKey.PrincipalEntityType.ClrType == entityType
                            && n.PropertyInfo != null)
                .ToList();

            foreach (var navigation in principalNavigations)
            {
                await ProcessNavigationPropertyAsync(context, entry, navigation.PropertyInfo!, cancellationToken);
            }
        }

        private static async Task ProcessNavigationPropertyAsync(
            DbContext context,
            EntityEntry entry,
            PropertyInfo navProperty,
            CancellationToken cancellationToken)
        {
            var entityType = entry.Entity.GetType();
            var attribute = navProperty.GetCustomAttribute<OnDeleteAttribute>();
            var cascadeMode = attribute?.Mode ?? OnDeleteBehavior.Restrict;

            var navigation = entry.Navigation(navProperty.Name);
            if (!navigation.IsLoaded)
            {
                await navigation.LoadAsync(cancellationToken);
            }

            var relatedEntities = GetRelatedEntities(navProperty, entry.Entity);
            if (relatedEntities == null || !relatedEntities.Any())
            {
                return;
            }

            switch (cascadeMode)
            {
                case OnDeleteBehavior.Restrict:
                    throw new DataConflictException(
                        "This record cannot be deleted because related records are still using it. Remove the related records first, then try again.");

                case OnDeleteBehavior.Cascade:
                    foreach (var relatedEntity in relatedEntities)
                    {
                        if (relatedEntity is AuditEntity auditEntity && !auditEntity.IsDeleted)
                        {
                            auditEntity.IsDeleted = true;
                            auditEntity.DeletedAt = DateTime.UtcNow;

                            await ProcessEntityCascadeAsync(context, relatedEntity, cancellationToken);
                        }
                        else if (relatedEntity is Entity and not AuditEntity)
                        {
                            context.Remove(relatedEntity);
                            await ProcessEntityCascadeAsync(context, relatedEntity, cancellationToken);
                        }
                    }
                    break;

                case OnDeleteBehavior.SetNull:
                    await SetForeignKeyToNullAsync(context, entry, navProperty, relatedEntities);
                    break;

                case OnDeleteBehavior.NoAction:
                    break;
            }
        }

        private static IEnumerable<object> GetRelatedEntities(PropertyInfo navProperty, object entity)
        {
            var value = navProperty.GetValue(entity);
            if (value == null)
            {
                return [];
            }

            if (value is IEnumerable<object> collection)
            {
                return collection.ToList();
            }

            return [value];
        }

        private static async Task SetForeignKeyToNullAsync(
            DbContext context,
            EntityEntry entry,
            PropertyInfo navProperty,
            IEnumerable<object> relatedEntities)
        {
            var principalEntityType = context.Model.FindEntityType(entry.Entity.GetType());
            var navigation = principalEntityType?.FindNavigation(navProperty.Name);

            if (navigation == null)
            {
                return;
            }

            var foreignKey = navigation.ForeignKey;
            var fkPropertyNames = foreignKey.Properties.Select(p => p.Name).ToList();

            foreach (var related in relatedEntities)
            {
                var relatedEntry = context.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == related)
                                   ?? context.Entry(related);

                foreach (var fkPropName in fkPropertyNames)
                {
                    var fkProperty = relatedEntry.Property(fkPropName);
                    if (fkProperty != null)
                    {
                        var clrProperty = related.GetType().GetProperty(fkPropName);
                        if (clrProperty != null && IsNullable(clrProperty))
                        {
                            fkProperty.CurrentValue = null;
                            fkProperty.IsModified = true;
                        }
                    }
                }

                var navProp = related.GetType().GetProperty(navigation.ForeignKey.PrincipalToDependent?.Name ?? "")
                              ?? related.GetType().GetProperty(navigation.Inverse?.Name ?? "");
                navProp?.SetValue(related, null);
            }

            await Task.CompletedTask;
        }

        private static bool IsNullable(PropertyInfo property)
        {
            if (!property.PropertyType.IsValueType)
            {
                return true;
            }

            return Nullable.GetUnderlyingType(property.PropertyType) != null;
        }
    }
}
