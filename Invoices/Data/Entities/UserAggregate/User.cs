namespace Invoices.Data.Entities.UserAggregate

{
   public class User
    {
        public User(string externalId, string name)
        {
            ExternalId = externalId;
            Name = name;
        }
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
    }
}
