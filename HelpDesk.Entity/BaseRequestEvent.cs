using System;

namespace HelpDesk.Entity
{
    /// <summary>
    /// Событие заявки
    /// </summary>
    public class BaseRequestEvent : BaseEntity
    {
        /// <summary>
        /// Заявка
        /// </summary>
        public virtual long RequestId { get; set; }

        /// <summary>
        /// Пользователь исполнитель/дипетчер
        /// </summary>
        public virtual WorkerUser User { get; set; }

        /// <summary>
        /// Состояние заявки
        /// </summary>
        public virtual StatusRequest StatusRequest { get; set; }

        /// <summary>
        /// Дата события
        /// </summary>
        public virtual DateTime DateEvent { get; set; }

        /// <summary>
        /// Дата вставки
        /// </summary>
        public virtual DateTime DateInsert { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// Итерация (начинается с нуля, каждый перенос срока инкрементирует итерацию)
        /// </summary>
        public virtual int OrdGroup { get; set; }

    }
}
