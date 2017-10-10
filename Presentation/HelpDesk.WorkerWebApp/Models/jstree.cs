namespace HelpDesk.WorkerWebApp.Models
{
    /// <summary>
    /// Модель данных для узла jstree с ленивой подгрузкой
    /// </summary>
    public class jstree_node
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

    /// <summary>
    /// Модель данных для полностью загруженного (загружены все дочерние узлы) узла jstree.
    /// Для такого узла на клиентской стороне строится массив children
    /// </summary>
    public class jstree_all_loaded_node
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        

        public string type { get; set; }

        /// <summary>
        /// Может ли узел быть выбран в качестве значения справочника, или он нужен только для
        /// визуального представления древовидной структуры
        /// </summary>
        public bool selectable { get; set; }
    }
}