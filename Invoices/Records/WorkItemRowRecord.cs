namespace Invoices.Records
{
    public record WorkItemRowRecord
    {
        public WorkItemRowRecord(string id, string name,int dayFrom, int duration) => (Id, Name, DayFrom,Duration) = (id, name,dayFrom,duration);
        public string Id { get; init; }
        public string Name { get; init; }
        public int DayFrom { get; init; }
        public int Duration { get; set; }
    }
}
