namespace HelpDesk.DTO
{
    /// <summary>
    /// Объект заявки - TO
    /// </summary>
    public class RequestObjectTODTO
    {
        public long Id { get; set; }

        public string HardTypeName { get; set; }
        public long HardTypeId { get; set; }
        public string ModelName { get; set; }
        public long ModelId { get; set; }
        public string ManufacturerName { get; set; }
        public long ManufacturerId { get; set; }

        public long ObjectTypeId { get; set; }
        
    }
}
