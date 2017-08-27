namespace HelpDesk.WebApp.Models
{
    /// <summary>
    /// Модель данных для одноименного js-компонента
    /// </summary>
    public class jstree
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; }
    }
}