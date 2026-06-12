namespace Validators
{
    public interface IValidator<in T>
    {
        void Validate(T entity);
    }
}
