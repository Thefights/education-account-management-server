# Codex Project Instructions

## Migration Policy

- Do not run `dotnet ef migrations add` or otherwise create migrations automatically.
- The user will handle migrations manually.

## Data Access

- Do not use `DbContext` directly inside services.
- Services must access data through repositories.
- Prefer reusing generic repositories first.
- Create a dedicated repository file/interface only when generic repositories are not enough or when query logic is complex/reusable.
- Commit changes through `IUnitOfWork.SaveChangeAsync(...)`.
- Do not expose or call custom `SaveChangesAsync` methods from repositories.

## Utilities

- Extract reusable helper logic into utility/helper classes when appropriate.
- Do not extract a helper/function unless it is reused from at least two places, unless the extraction is required by a clear architectural boundary such as DI, an interface contract, or framework integration.
- Examples: token generation/hashing, JWT creation, password hashing, shared security helpers.
- Use `nameof(...)` for names that represent real system members such as entity properties, DTO properties, navigation paths, include paths, mapped target fields, sortable fields, searchable fields, filter fields, claim names, and policy/role names when they are backed by code symbols. Literal strings are acceptable only for external contracts, protocol values, messages, route templates, SQL/provider syntax, or infrastructure markers that are not code members.

## Validation

- Mappers must only map data. Do not trim, validate, call repositories, or apply business rules inside mapper classes.
- Entity/model annotations are the source of truth for persisted entity invariants. Keep required fields, max lengths, enum validity, FK/value-type default checks, and reusable format checks on entities when the rule must hold before saving from any source.
- Do not duplicate entity invariant annotations on DTOs just to validate the same mapped fields earlier.
- DTO/request annotations are allowed only for API-boundary, command, or transport-only checks that are not naturally validated by a mapped entity. Examples: upload file type/required file, list IDs requiring at least one item, command enum values, or request DTOs that do not map cleanly to a persisted entity.
- When a DTO maps to an entity, validate the mapped entity with `TryValidate()` after mapping and before persistence or side effects that depend on valid data.
- Do not introduce `IValidator<T>` abstractions or dedicated business validator classes.
- Keep cross-field, workflow, repository-backed, state-transition, authorization, and other business rules in the owning service.
- DTO annotations are only for API-boundary or transport rules that cannot be enforced by the mapped entity, such as OTP/session commands, uploads, and request-only list constraints.
- Do not duplicate persisted entity invariants on DTOs.
- Keep services focused on orchestration and business flow, not long inline validation blocks.
- Use `NotDefaultValueAttribute` for required value-type IDs/FKs that must not be `0`.
- Use `EnumDefinedAttribute` for enum properties that must reject undefined numeric enum values.

## DTO Conventions

- Enum properties returned by `Get*DTO` response DTOs must use `string?` rather than enum types, so public API responses expose enum names instead of numeric enum values.
- Request/command DTOs may keep enum-typed properties when the API should accept enum values and validate them with `EnumDefinedAttribute` or a business validator.
- Group DTOs by feature, domain, or workflow. CRUD request/response DTOs for the same resource can stay in one file, but split files that collect unrelated flows or domains, such as registration, login, MFA, and password reset contracts all living in one auth DTO file.

## Project Organization

- Put frequently reused imports in the project `GlobalUsings.cs` file.
- Use plural feature folders and namespaces when a singular feature name would collide with an entity type, for example `Services.Courses`, `DTOs.EducationAccounts`, and `Filters.Schools`.
- Do not introduce same-name entity aliases such as `using CourseEntity = Models.Course`; keep the plural namespace and use the entity's natural type name.
- Keep base service/controller types under `Base` folders.
- Keep management-specific services/controllers and their interfaces under `Management` folders.
- Keep regular services/controllers directly under their normal parent folder instead of nesting them under `Base` or `Management`.

## Constructor And Field Convention

- Use primary constructors for dependency injection.
- Immediately assign constructor parameters to private/protected readonly fields.
- Use `_camelCase` field names in the class body.

Example:

```csharp
public class ExampleService(IExampleRepository exampleRepository)
{
    private readonly IExampleRepository _exampleRepository = exampleRepository;

    public Task DoWorkAsync()
    {
        return _exampleRepository.DoWorkAsync();
    }
}
```
