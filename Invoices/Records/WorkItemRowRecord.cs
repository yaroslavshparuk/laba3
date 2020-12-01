namespace Invoices.Records
{
    public record WorkItemRowRecord
    {
        public WorkItemRowRecord(string id, string name,int dayFrom, int duration/*, bool isStared, bool isChanged, bool isClosed*/) => 
            (Id, Name, DayFrom,Duration/*, IsStarted, IsChanged, IsClosed*/) =
            (id, name,dayFrom,duration/*, isStared, isChanged, isClosed*/);

        public string Id { get; init; }
        public string Name { get; init; }
        public int DayFrom { get; init; }
        public int Duration { get; set; }
        
    }
}
