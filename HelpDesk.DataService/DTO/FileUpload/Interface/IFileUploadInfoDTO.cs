using System;

namespace HelpDesk.DataService.DTO.FileUpload.Interface
{
    /// <summary>
    /// Загружаемый файл - только общие сведения
    /// </summary>
    public interface IFileUploadInfoDTO
    {
        long Id { get; set; }

        string Name { get; set; }

        string Type { get; set; }

        int Size { get; set; }

        Guid TempRequestKey { get; set; }

        long? ForignKeyId { get; set; }
   
    }
}
