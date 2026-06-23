# Database Design
## Enums
### AuditLogCategory
- AccountCreation
- StatusChange
- Topup
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
### TopupLogicalOperator
- And
- Or
### TopupConditionField
- Age
- Balance
- SchoolingStatus
### TopupConditionOperator
- Equals
- NotEquals
- GreaterThan
- GreaterThanOrEqual
- LessThan
- LessThanOrEqual
- Between
### SystemTopupStatus
- Active
- Inactive
### ScheduleTopUpStatus
- Active
- Inactive
- Completed
### ScheduleTopUpFrequency
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
- `SystemTopupId` (int?)
- `SystemTopup` (SystemTopup?)
- `ScheduleTopUpId` (int?)
- `ScheduleTopUp` (ScheduleTopUp?)
- `IdempotencyKey` (string)
- `ManualAmount` (decimal?)
- `ManualReason` (string?)
- `Status` (TopupExecutionStatus)
- `TotalTargetCount` (int)
- `SuccessCount` (int)
- `FailedCount` (int)
- `TotalExecutedAmount` (decimal)
- `TopupNameSnapshot` (string?)
- `TopupAmountSnapshot` (decimal?)
- `ConditionsSnapshot` (string?)
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
### SystemTopup
- `Name` (string)
- `TopupAmount` (decimal?)
- `Status` (SystemTopupStatus)
- `ConditionGroups` (ICollection<SystemTopupConditionGroup>)
- `Executions` (ICollection<TopupExecution>)
- `Applications` (ICollection<TopupSystemApplication>)
### SystemTopupConditionGroup
- `SystemTopupId` (int)
- `SystemTopup` (SystemTopup)
- `ParentGroupId` (int?)
- `ParentGroup` (SystemTopupConditionGroup?)
- `LogicalOperator` (TopupLogicalOperator)
- `DisplayOrder` (int)
- `ChildGroups` (ICollection<SystemTopupConditionGroup>)
- `Conditions` (ICollection<SystemTopupCondition>)
### SystemTopupCondition
- `GroupId` (int)
- `Group` (SystemTopupConditionGroup)
- `Field` (TopupConditionField)
- `Operator` (TopupConditionOperator)
- `ValueText` (string?)
- `ValueNumber` (decimal?)
- `ValueNumberTo` (decimal?)
- `DisplayOrder` (int)
### ScheduleTopUp
- `Name` (string)
- `TopupAmount` (decimal?)
- `Frequency` (ScheduleTopUpFrequency)
- `OneTimeExecutionAt` (DateTime?)
- `ExecuteAtDay` (int?)
- `ExecuteAtMonth` (int?)
- `ExecutionTime` (TimeOnly)
- `NextExecutionAt` (DateTime?)
- `Status` (ScheduleTopUpStatus)
- `ConditionGroups` (ICollection<ScheduleTopUpConditionGroup>)
- `Executions` (ICollection<TopupExecution>)
### ScheduleTopUpConditionGroup
- `ScheduleTopUpId` (int)
- `ScheduleTopUp` (ScheduleTopUp)
- `ParentGroupId` (int?)
- `ParentGroup` (ScheduleTopUpConditionGroup?)
- `LogicalOperator` (TopupLogicalOperator)
- `DisplayOrder` (int)
- `ChildGroups` (ICollection<ScheduleTopUpConditionGroup>)
- `Conditions` (ICollection<ScheduleTopUpCondition>)
### ScheduleTopUpCondition
- `GroupId` (int)
- `Group` (ScheduleTopUpConditionGroup)
- `Field` (TopupConditionField)
- `Operator` (TopupConditionOperator)
- `ValueText` (string?)
- `ValueNumber` (decimal?)
- `ValueNumberTo` (decimal?)
- `DisplayOrder` (int)
### TopupSystemApplication
- `SystemTopupId` (int)
- `SystemTopup` (SystemTopup)
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
