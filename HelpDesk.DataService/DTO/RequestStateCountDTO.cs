namespace HelpDesk.DataService.DTO
{
    public class RequestStateCountDTO
    {
        public long StatusId { get; set; }
        public string StatusName { get; set; }

        public string StatusBackColor { get; set; }

        public int Count { get; set; }
    }
}
