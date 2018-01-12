namespace HelpDesk.DataService.DTO
{
    /// <summary>
    /// Состояние заявки (чистое)
    /// </summary>
    public class RawStatusRequestDTO : IChecked<long>
    {
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
               

        #region IChecked
        public bool Checked { get; set; }

        public long Value { get { return Id; } }
        #endregion IChecked

        public override bool Equals(object obj)
        {
            RawStatusRequestDTO obj2 = obj as RawStatusRequestDTO;
            if (obj2 == null) return false;
            return Id == obj2.Id;
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }
    }
}
