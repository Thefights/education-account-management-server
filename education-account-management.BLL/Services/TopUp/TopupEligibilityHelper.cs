namespace Services.TopUp
{
    internal static class TopupEligibilityHelper
    {
        public static readonly System.Linq.Expressions.Expression<Func<EducationAccount, bool>> EligibleAccountPredicate =
            account => account.Status == EducationAccountStatus.Active
                && account.Citizen.IsSingaporean;

        public static bool IsEligible(EducationAccount account)
        {
            return account.Status == EducationAccountStatus.Active
                && account.Citizen.IsSingaporean;
        }

        public static string? GetIneligibilityReason(EducationAccount account)
        {
            if (account.Status != EducationAccountStatus.Active)
            {
                return $"Account is not Active (current status: {account.Status}).";
            }

            if (!account.Citizen.IsSingaporean)
            {
                return "Account holder is not a Singapore citizen.";
            }

            return null;
        }
    }
}
