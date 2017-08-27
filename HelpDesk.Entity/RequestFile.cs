using System;

namespace HelpDesk.Entity
{
    /// <summary>
    /// Файл заявки
    /// </summary>
    public class RequestFile : BaseEntity
    {
        /// <summary>
        /// Наименование файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Размер файла
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Временный Guid-FK на заявку. Используется при создании заявки
        /// </summary>
        public Guid TempRequestKey { get; set; }

        /// <summary>
        /// Заявка
        /// </summary>
        public long? RequestId { get; set; }

        /// <summary>
        /// Уменьшенное изображение
        /// </summary>
        public byte[] Thumbnail { get; set; }

        /// <summary>
        /// Изображение
        /// </summary>
        public byte[] Body { get; set; }
       
    }
}
