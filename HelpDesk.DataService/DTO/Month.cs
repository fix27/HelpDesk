namespace HelpDesk.DataService.DTO
{
    public class Month
    {
        /// <summary>
        /// Значение месяца от 1 до 12 (0 для "Все месяцы")
        /// </summary>
        public int Ord { get; set; }

        /// <summary>
        /// Наименование (может быть "Все месяцы")
        /// </summary>
        public string Name { get; set; }
    }
}
