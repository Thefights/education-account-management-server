# Database Design
## Enums
### AuditLogCategory
- AccountCreation
- StatusChange
- TopupConfig
- Security
- Transaction
- Billing
- AI
### BatchType
- ManualTopup
- ScheduledTopup
### ChargeStatus
- Unpaid
- PartiallyPaid
- Paid
- Outstanding
- Cancelled
### CitizenshipStatus
- Active
- Revoked
- Renounced
### CourseStatus
- Active
- Inactive
### EducationAccountStatus
- Active
- Extended
- Closed
### EducationCreditTransactionDirection
- Credit
- Debit
### EducationCreditTransactionType
- Topup
- CourseFee
- Adjustment
### FileType
- Image
- Document
- Spreadsheet
- Pdf
- Video
- Other
- Custom
### OutboxMessageStatus
- Pending
- Processing
- Completed
- Failed
### PaymentMethod
- EducationBalance
- OnlinePayment
### PaymentStatus
- Pending
- Succeeded
- Failed
### SchoolStatus
- Active
- Inactive
### SsoProvider
- Singpass
- AzureAD
### SweepAction
- Create
- Close
- Extend
### SweepTargetStatus
- Pending
- Success
- Failed
### TopupExecutionSourceType
- System
- Schedule
- Manual
### TopupExecutionStatus
- Pending
- Executing
- Completed
### TopupMatchMode
- And
- Or
### TopupRuleConditionField
- Age
- Balance
- SchoolingStatus
### TopupRuleConditionOperator
- Equals
- NotEquals
- GreaterThan
- GreaterThanOrEqual
- LessThan
- LessThanOrEqual
### TopupRuleStatus
- Active
- Inactive
### TopupRuleType
- System
- Schedule
### TopupScheduleStatus
- Active
- Inactive
- Completed
### TopupScheduleType
- OneTime
- Monthly
- Yearly
### TopupTargetStatus
- Pending
- Processing
- Success
- Failed
### UserRole
- SystemAdmin
- FinanceAdmin
- SchoolAdmin
- AccountHolder
### UserStatus
- Active
- Inactive
## Models
### AdminProfile
- `StaffCode` (string)
- `FullName` (string)
- `Email` (string)
- `PhoneNumber` (string?)
- `UserId` (int)
- `Nric` (string)
- `User` (User)
- `SchoolId` (int?)
- `School` (School?)
### AiAssistantSetting
- `IsEnabled` (bool)
### AuditLog
- `Category` (AuditLogCategory)
- `Action` (string)
- `Nric` (string?)
- `IpAddress` (string)
- `OccurredAt` (DateTime)
- `ActorUserId` (int?)
- `ActorUser` (User?)
### Charge
- `EnrollmentId` (int)
- `Enrollment` (Enrollment)
- `Status` (ChargeStatus)
- `CourseFeeAmountSnapshot` (decimal)
- `MiscFeeAmountSnapshot` (decimal)
- `GstAmountSnapshot` (decimal)
- `GrossAmount` (decimal)
- `SubsidyAmount` (decimal)
- `NetAmount` (decimal)
- `PaidAmount` (decimal)
- `RemainingAmount` (decimal)
- `PaymentAllocations` (ICollection<PaymentAllocation>)
### Citizen
- `Nric` (string)
- `FullName` (string)
- `Email` (string?)
- `PhoneNumber` (string?)
- `ResidentialAddress` (string?)
- `MailingAddress` (string?)
- `DateOfBirth` (DateOnly)
- `CitizenshipStatus` (CitizenshipStatus)
- `SchoolingStatus` (string?)
- `User` (User?)
- `EducationAccount` (EducationAccount?)
### Course
- `SchoolId` (int)
- `School` (School)
- `Status` (CourseStatus)
- `CourseName` (string)
- `Description` (string?)
- `CourseFeeAmount` (decimal)
- `MiscFeeAmount` (decimal)
- `GstAmount` (decimal)
- `Enrollments` (ICollection<Enrollment>)
### EducationAccount
- `AccountNumber` (string)
- `EducationCreditBalance` (decimal)
- `Status` (EducationAccountStatus)
- `OpenedAt` (DateTime)
- `ClosedAt` (DateTime?)
- `CitizenId` (int)
- `Citizen` (Citizen)
- `RowVersion` (byte[])
- `TopupExecutionTargets` (ICollection<TopupExecutionTarget>)
- `TopupSystemApplications` (ICollection<TopupSystemApplication>)
- `EducationCreditTransactions` (ICollection<EducationCreditTransaction>)
- `StatusHistories` (ICollection<EducationAccountStatusHistory>)
### EducationAccountStatusHistory
- `EducationAccountId` (int)
- `EducationAccount` (EducationAccount)
- `PreviousStatus` (EducationAccountStatus)
- `NewStatus` (EducationAccountStatus)
- `Reason` (string)
- `ChangedAt` (DateTime)
- `ChangedByUserId` (int?)
- `ChangedByUser` (User?)
### EducationAccountSweepReport
- `BatchDate` (DateOnly)
- `StartedAt` (DateTime)
- `CompletedAt` (DateTime)
- `AccountsCreatedCount` (int)
- `AccountsClosedCount` (int)
- `AccountsExtendedCount` (int)
- `Targets` (ICollection<EducationAccountSweepTarget>)
### EducationAccountSweepTarget
- `SweepReportId` (int)
- `SweepReport` (EducationAccountSweepReport)
- `Nric` (string)
- `Action` (SweepAction)
- `Status` (SweepTargetStatus)
- `Reason` (string?)
### EducationCreditTransaction
- `TransactionCode` (Guid)
- `Type` (EducationCreditTransactionType)
- `Direction` (EducationCreditTransactionDirection)
- `Amount` (decimal)
- `BalanceBefore` (decimal)
- `BalanceAfter` (decimal)
- `Description` (string?)
- `EducationAccountId` (int)
- `EducationAccount` (EducationAccount)
- `TopupExecutionTarget` (TopupExecutionTarget?)
- `Payment` (Payment?)
### Enrollment
- `CourseId` (int)
- `Course` (Course)
- `EducationAccountId` (int)
- `EducationAccount` (EducationAccount)
- `SchoolNameSnapshot` (string)
- `CourseNameSnapshot` (string)
- `CourseDescriptionSnapshot` (string?)
- `CitizenNricSnapshot` (string)
- `CitizenFullNameSnapshot` (string)
- `CitizenEmailSnapshot` (string?)
- `CitizenPhoneNumberSnapshot` (string?)
- `AccountNumberSnapshot` (string)
- `EnrolledAt` (DateTime)
- `CompletedAt` (DateTime?)
- `WithdrawnAt` (DateTime?)
- `Charge` (Charge?)
### OutboxMessage
- `Type` (string)
- `PayloadJson` (string)
- `Status` (OutboxMessageStatus)
- `RetryCount` (int)
- `OccurredAt` (DateTime)
### Payment
- `EducationCreditTransactionId` (int?)
- `EducationCreditTransaction` (EducationCreditTransaction?)
- `PaymentMethod` (PaymentMethod)
- `Status` (PaymentStatus)
- `AccountNumberSnapshot` (string)
- `CitizenNricSnapshot` (string)
- `CitizenFullNameSnapshot` (string)
- `TotalAmount` (decimal)
- `PaidAt` (DateTime?)
- `ExternalReference` (string?)
- `PaymentAllocations` (ICollection<PaymentAllocation>)
### PaymentAllocation
- `PaymentId` (int)
- `Payment` (Payment)
- `ChargeId` (int)
- `Charge` (Charge)
- `CourseNameSnapshot` (string)
- `SchoolNameSnapshot` (string)
- `ChargeGrossAmountSnapshot` (decimal)
- `ChargeNetAmountSnapshot` (decimal)
- `ChargeRemainingAmountSnapshot` (decimal)
- `Amount` (decimal)
### RefreshToken
- `TokenHash` (string)
- `ExpiresAt` (DateTime)
- `RevokedAt` (DateTime?)
- `UserId` (int)
- `User` (User)
### School
- `Status` (SchoolStatus)
- `SchoolName` (string)
- `Address` (string)
- `PhoneNumber` (string)
- `Email` (string)
- `AdminProfiles` (ICollection<AdminProfile>)
- `Courses` (ICollection<Course>)
### SsoIdentity
- `Provider` (SsoProvider)
- `ProviderUserId` (string)
- `UserId` (int)
- `User` (User)
### TopupExecution
- `ExecutionCode` (string)
- `SourceType` (TopupExecutionSourceType)
- `TopupRuleId` (int?)
- `TopupRule` (TopupRule?)
- `TopupScheduleId` (int?)
- `TopupSchedule` (TopupSchedule?)
- `IdempotencyKey` (string)
- `ManualAmount` (decimal?)
- `ManualReason` (string?)
- `Status` (TopupExecutionStatus)
- `TotalTargetCount` (int)
- `SuccessCount` (int)
- `FailedCount` (int)
- `TotalExecutedAmount` (decimal)
- `RuleNameSnapshot` (string?)
- `RuleTypeSnapshot` (TopupRuleType?)
- `MatchModeSnapshot` (TopupMatchMode?)
- `TopupAmountSnapshot` (decimal?)
- `RuleConditionsSnapshot` (string?)
- `Targets` (ICollection<TopupExecutionTarget>)
### TopupExecutionTarget
- `TopupExecutionId` (int)
- `TopupExecution` (TopupExecution)
- `EducationAccountId` (int?)
- `EducationAccount` (EducationAccount?)
- `AccountNumber` (string)
- `Amount` (decimal)
- `Status` (TopupTargetStatus)
- `FailureReason` (string?)
- `MatchedConditionsSnapshot` (string?)
- `EducationCreditTransactionId` (int?)
- `EducationCreditTransaction` (EducationCreditTransaction?)
### TopupRule
- `RuleName` (string)
- `Type` (TopupRuleType)
- `MatchMode` (TopupMatchMode)
- `TopupAmount` (decimal?)
- `Status` (TopupRuleStatus)
- `Conditions` (ICollection<TopupRuleCondition>)
- `Schedule` (TopupSchedule?)
- `Executions` (ICollection<TopupExecution>)
- `SystemApplications` (ICollection<TopupSystemApplication>)
### TopupRuleCondition
- `Field` (TopupRuleConditionField)
- `Operator` (TopupRuleConditionOperator)
- `ValueText` (string?)
- `ValueNumber` (decimal?)
- `ConditionAmount` (decimal?)
- `DisplayOrder` (int)
- `TopupRuleId` (int)
- `TopupRule` (TopupRule)
### TopupSchedule
- `TopupRuleId` (int)
- `TopupRule` (TopupRule)
- `Frequency` (TopupScheduleType)
- `OneTimeExecutionAt` (DateTime?)
- `ExecuteAtDay` (int?)
- `ExecuteAtMonth` (int?)
- `ExecutionTime` (TimeOnly)
- `NextExecutionAt` (DateTime?)
- `Status` (TopupScheduleStatus)
- `Executions` (ICollection<TopupExecution>)
### TopupSystemApplication
- `TopupRuleId` (int)
- `TopupRule` (TopupRule)
- `EducationAccountId` (int)
- `EducationAccount` (EducationAccount)
- `TopupExecutionTargetId` (int)
- `TopupExecutionTarget` (TopupExecutionTarget)
### User
- `Role` (UserRole)
- `Status` (UserStatus)
- `FailedLoginCount` (int)
- `LockedUntil` (DateTime?)
- `LastLoginAt` (DateTime?)
- `CitizenId` (int?)
- `Citizen` (Citizen?)
- `AdminProfile` (AdminProfile?)
- `SsoIdentities` (ICollection<SsoIdentity>)
- `RefreshTokens` (ICollection<RefreshToken>)
- `StatusHistories` (ICollection<UserStatusHistory>)
### UserStatusHistory
- `UserId` (int)
- `User` (User)
- `PreviousStatus` (UserStatus)
- `NewStatus` (UserStatus)
- `Reason` (string)
- `ChangedAt` (DateTime)
- `ChangedByUserId` (int?)
- `ChangedByUser` (User?)
