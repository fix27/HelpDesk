using System;

namespace HelpDesk.DTO.FileUpload.Interface
{
    /// <summary>
    /// Загружаемый файл c Thumbnail
    /// </summary>
    public interface IFileUploadThumbnailDTO
    {
        long Id { get; set; }

        string Name { get; set; }

        string Type { get; set; }

        int Size { get; set; }

        Guid TempRequestKey { get; set; }

        long? ForignKeyId { get; set; }

        byte[] Thumbnail { get; set; }

    }
}
