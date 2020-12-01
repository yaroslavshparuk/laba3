namespace Invoices.Records
{
    public record UserRecord
           (string Id,
            string Name,
            string Email = null);
}
