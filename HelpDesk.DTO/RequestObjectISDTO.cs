using System;

namespace HelpDesk.DTO
{
    /// <summary>
    /// Объект заявки - ПО
    /// </summary>
    public class RequestObjectISDTO
    {
        public long Id { get; set; }

        /// <summary>
        /// Наименование ПО
        /// </summary>
        public string SoftName { get; set; }

        /// <summary>
        /// Наименование типа работ
        /// </summary>
        public string ObjectTypeName { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string ObjectName
        {
            get
            {
                return String.IsNullOrWhiteSpace(SoftName) ? ObjectTypeName : SoftName;
            }
        }

    }
}
