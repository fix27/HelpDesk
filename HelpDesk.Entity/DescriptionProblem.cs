namespace HelpDesk.Entity
{
    /// <summary>
    /// Типовая проблема ПО или ТО
    /// </summary>
    public class DescriptionProblem : SimpleEntity
    {
        /// <summary>
        /// ПО
        /// </summary>
        public RequestObject RequestObject { get; set; }

        /// <summary>
        /// ТО
        /// </summary>
        public HardType HardType { get; set; }
    }
}
