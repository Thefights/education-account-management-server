namespace Enums
{
    public enum AuditLogAction
    {
        Register = 1,
        SendRegisterEmailOtp = 2,
        VerifyRegisterEmailOtp = 3,
        Login = 4,
        LoginFailed = 5,
        SocialLogin = 6,
        Logout = 7,
        RefreshToken = 8,
        VerifyMfaOtp = 9,
        ForgotPassword = 10,
        ResetPassword = 11,

        CreateAccount = 100,
        UpdateAccount = 101,
        DeleteAccount = 102,
        DeleteAccounts = 103,
        UpdateAccountStatus = 104,
        ImportAccounts = 105,
        UnlockAccount = 106,

        CreateProduct = 300,
        UpdateProduct = 301,
        DeleteProduct = 302,
        DeleteProducts = 303,
        ImportProducts = 304,
        UpdateProductStatus = 305,

        ViewAuditLogs = 500
    }
}
