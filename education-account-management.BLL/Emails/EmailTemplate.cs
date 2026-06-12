namespace Emails
{
    public sealed record EmailTemplate(string Subject, string HtmlBody, string TextBody);
}
