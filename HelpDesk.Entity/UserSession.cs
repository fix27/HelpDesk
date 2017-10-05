using System;

namespace HelpDesk.Entity
{
    /// <summary>
    /// Тип приложения
    /// </summary>
    public enum ApplicationType { Cabinet, Worker }

    /// <summary>
    /// Факт начала пользовательской сессии
    /// </summary>
    public class UserSession : BaseEntity
    {
        /// <summary>
        /// Id пользователя (из WorkerUser или CabinetUser)
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Дата вставки записи
        /// </summary>
        public DateTime DateInsert { get; set; }

        /// <summary>
        /// Тип приложения
        /// </summary>
        public ApplicationType ApplicationType { get; set; }

        /// <summary>
        /// IP-адрес, с которого осуществлен вход
        /// </summary>
        public string IP { get; set; }
    }
}
