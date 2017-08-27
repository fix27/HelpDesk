using HelpDesk.DTO.FileUpload.Interface;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    /// <summary>
    /// Для возможности использовать разные сервисы для работы с загружаемыми файлами
    /// </summary>
    public interface IFileUploadService
    {
        long SaveFile(IFileUploadDTO dto);

        IEnumerable<IFileUploadInfoDTO> GetListFileInfo(long forignKeyId);

        IFileUploadThumbnailDTO GetFileThumbnail(long id);
        IFileUploadDTO GetFile(long id);

        void DeleteFile(long id);

        IFileUploadDTO GetNew();
    }
}
