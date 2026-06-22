# Database Design Decisions

## Snapshot Convention

- Do not create dedicated snapshot tables unless the business explicitly requires independently queryable or reusable snapshot records.
- Store transaction-specific snapshot values directly on the owning transaction entity.
- Every snapshot property must use the `Snapshot` suffix, consistent with `Enrollment`, `Payment`, and `PaymentAllocation`.
- Keep the original foreign key when the source entity relationship is still required. Snapshot fields preserve historical values after the source entity changes.

### Top-up Execution

`TopupExecution` stores the rule configuration that was effective when the execution started:

- `RuleNameSnapshot`
- `RuleTypeSnapshot`
- `MatchModeSnapshot`
- `TopupAmountSnapshot`
- `RuleConditionsSnapshot`

`TopupExecutionTarget` stores `MatchedConditionsSnapshot` to explain which conditions qualified that account.

Rule conditions have variable cardinality, so condition collections are stored as immutable JSON in the snapshot fields. These fields are business snapshots, not generic audit payloads.

Manual top-ups do not use rule snapshot fields because their amount and reason are already stored in `ManualAmount` and `ManualReason`.

## History Convention

Create a history table only for business state transitions that cannot be reconstructed from an existing immutable ledger or execution record.

Current required history tables:

- `EducationAccountStatusHistory`
- `UserStatusHistory`

Do not create duplicate history tables for top-up results, balance movements, payments, provisioning results, or enrollment source data. These are already represented by execution, transaction, allocation, report, and enrollment snapshot records.

## Audit Log Convention

`AuditLog` stores event metadata only:

- category
- action
- actor
- target NRIC when applicable
- IP address
- occurrence time

Do not add a generic `PayloadJson`, `TargetEntity`, or `TargetEntityId`. Business details belong to the relevant snapshot, history, ledger, execution, or report entity.
