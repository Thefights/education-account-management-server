using System.Linq.Expressions;
using System.Reflection;

namespace Validators
{
    public static class UniqueConstraintValidator
    {
        public static async Task ValidateAsync<TModel>(
            IGenericRepository<TModel> repository,
            TModel entity,
            int? excludedId = null,
            CancellationToken cancellationToken = default)
            where TModel : BaseEntity
        {
            var uniqueProperties = typeof(TModel)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(property => Attribute.IsDefined(property, typeof(UniqueAttribute)))
                .ToList();

            foreach (var property in uniqueProperties)
            {
                var value = property.GetValue(entity);
                if (value == null || value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                {
                    continue;
                }

                var filter = BuildFilter<TModel>(property, value, excludedId);
                if (await repository.AnyAsync(filter, cancellationToken))
                {
                    throw new DataConflictException($"{typeof(TModel).Name} {property.Name} already exists.");
                }
            }
        }

        private static Expression<Func<TModel, bool>> BuildFilter<TModel>(
            PropertyInfo property,
            object value,
            int? excludedId)
            where TModel : BaseEntity
        {
            var parameter = Expression.Parameter(typeof(TModel), "entity");
            var propertyAccess = Expression.Property(parameter, property);
            var valueExpression = Expression.Constant(value, property.PropertyType);
            var equalsExpression = Expression.Equal(propertyAccess, valueExpression);

            Expression body = equalsExpression;
            if (excludedId.HasValue)
            {
                var idAccess = Expression.Property(parameter, nameof(BaseEntity.Id));
                var idExpression = Expression.Constant(excludedId.Value);
                body = Expression.AndAlso(
                    body,
                    Expression.NotEqual(idAccess, idExpression));
            }

            return Expression.Lambda<Func<TModel, bool>>(body, parameter);
        }
    }
}
