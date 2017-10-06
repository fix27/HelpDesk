namespace HelpDesk.WorkerWebApp.Models
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

        public string type { get; set; }

        /// <summary>
        /// Может ли узел быть выбран в качестве значения справочника, или он нужен только для
        /// визуального представления древовидной структуры
        /// </summary>
        public bool selectable { get; set; }
    }
}