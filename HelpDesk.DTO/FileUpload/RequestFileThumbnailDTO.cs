using HelpDesk.DTO.FileUpload.Interface;
using System;

namespace HelpDesk.DTO.FileUpload
{
    /// <summary>
    /// Файл заявки c Thumbnail
    /// </summary>
    public class RequestFileThumbnailDTO : SimpleDTO, IFileUploadThumbnailDTO
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
                
       
    }
}
