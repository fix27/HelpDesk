using HelpDesk.DataService.DTO.FileUpload.Interface;
using System;

namespace HelpDesk.DataService.DTO.FileUpload
{
    /// <summary>
    /// Файл заявки - только общие сведения
    /// </summary>
    public class RequestFileInfoDTO : SimpleDTO, IFileUploadInfoDTO
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
                
       
    }
}
