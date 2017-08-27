using HelpDesk.DTO.FileUpload.Interface;
using System;

namespace HelpDesk.DTO.FileUpload
{
    /// <summary>
    /// Файл заявки
    /// </summary>
    public class RequestFileDTO : SimpleDTO, IFileUploadDTO
    {
        /// <summary>
        /// Тип файла
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Размер файла
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Временный FK на заявку. Используется при создании заявки
        /// </summary>
        public Guid TempRequestKey { get; set; }

        /// <summary>
        /// Заявка
        /// </summary>
        public long? ForignKeyId { get; set; }

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
