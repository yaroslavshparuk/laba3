namespace Invoices.Data.Records
{
    public record WorkItemRowRecord
    {
        public WorkItemRowRecord(int id, string name,int dayFrom, int duration) => 
            (Id, Name, DayFrom,Duration) =
            (id, name,dayFrom,duration);

        public int Id { get; init; }
        public string Name { get; init; }
        public int DayFrom { get; init; }
        public int Duration { get; set; }
        
    }
}
