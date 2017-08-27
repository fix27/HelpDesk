using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Collections.Generic;
using System;
using System.Linq;
using HelpDesk.DTO.FileUpload.Interface;
using HelpDesk.DTO.FileUpload;
using HelpDesk.DataService.Common;
using HelpDesk.DataService.Resources;
using HelpDesk.DataService.Specification;
using HelpDesk.Common.Aspects;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с файлами, прикрепляемыми к заявкам
    /// </summary>
    [Transaction]
    public class RequestFileService : BaseService, IRequestFileService
    {
        private readonly IRepository repository;
        private readonly IBaseRepository<RequestFile> requestFileRepository;
        private readonly ISettingsRepository settingsRepository;
        private readonly IRequestConstraintsService requestConstraintsService;

        public RequestFileService(IRepository repository,
            IBaseRepository<RequestFile> requestFileRepository,
            ISettingsRepository settingsRepository,
            IRequestConstraintsService requestConstraintsService)
        {
            
            this.repository             = repository;
            this.requestFileRepository  = requestFileRepository;
            this.settingsRepository     = settingsRepository;
            this.requestConstraintsService = requestConstraintsService;
        }


        static object lockObj = new object();
        public long SaveFile(IFileUploadDTO dto)
        {
            lock (lockObj)
            {
                Settings settings = settingsRepository.Get();
                                
                int c = requestFileRepository.GetList(
                    new RequestFileByNameAndForignKeyOrTempKeySpecification(dto.Name, dto.ForignKeyId, dto.TempRequestKey))
                    .Count();

                if (c > 0)
                    return 0;


                c = requestFileRepository.Count(t => t.RequestId != null && t.RequestId == dto.ForignKeyId ||
                    t.TempRequestKey != null && t.TempRequestKey == dto.TempRequestKey);

                if (c >= settings.MaxRequestFileCount)
                    throw new DataServiceException(String.Format(Resource.MaxRequestFileCountConstraintMsg, settings.MaxRequestFileCount));

                if (dto.Size / 1024 >= settings.MaxRequestFileSize)
                    throw new DataServiceException(String.Format(Resource.MaxRequestFileSizeConstraintMsg, settings.MaxRequestFileSize, dto.Name));

                if (String.IsNullOrWhiteSpace(dto.Name))
                    throw new DataServiceException(Resource.EmptyFileNameConstraintMsg);

                if (dto.Name.Length > settings.MaxFileNameLength)
                    throw new DataServiceException(String.Format(Resource.MaxFileNameConstraintMsg, settings.MaxFileNameLength, dto.Name));

                if (dto.Body == null || dto.Body.Length == 0)
                    throw new DataServiceException(Resource.EmptyFileBodyConstraintMsg);

                RequestFile file = new RequestFile()
                {
                    Body = dto.Body,
                    Name = dto.Name,
                    RequestId = dto.ForignKeyId,
                    Size = dto.Size,
                    TempRequestKey = dto.TempRequestKey,
                    Thumbnail = dto.Thumbnail,
                    Type = dto.Type
                };

                requestFileRepository.Save(file);
                repository.SaveChanges();

                return file.Id;
            }

        }

        public IEnumerable<IFileUploadInfoDTO> GetListFileInfo(long forignKeyId)
        {
            return requestFileRepository.GetList(t => t.RequestId == forignKeyId)
                .Select(t => new RequestFileInfoDTO()
                {
                    Id = t.Id,
                    ForignKeyId = t.RequestId,
                    Name = t.Name,
                    Size = t.Size,
                    TempRequestKey = t.TempRequestKey,
                    Type = t.Type
                })
                .ToList();
        }

        public IFileUploadThumbnailDTO GetFileThumbnail(long id)
        {
            return requestFileRepository.GetList(t => t.Id == id)
                .Select(t => new RequestFileThumbnailDTO()
                {
                    Id = t.Id,
                    ForignKeyId = t.RequestId,
                    Name = t.Name,
                    Size = t.Size,
                    TempRequestKey = t.TempRequestKey,
                    Type = t.Type,
                    Thumbnail = t.Thumbnail
                }).FirstOrDefault();
        }

        public IFileUploadDTO GetFile(long id)
        {
            return requestFileRepository.GetList(t => t.Id == id)
                .Select(t => new RequestFileDTO()
                {
                    Id = t.Id,
                    ForignKeyId = t.RequestId,
                    Name = t.Name,
                    Size = t.Size,
                    TempRequestKey = t.TempRequestKey,
                    Type = t.Type,
                    Body = t.Body
                }).FirstOrDefault();
        }

        public void DeleteFile(long id)
        {
            long? requestId = requestFileRepository.GetList(t => t.Id == id)
                .Select(t => t.RequestId)
                .FirstOrDefault();

            if (requestId.HasValue)
                requestConstraintsService.CheckExistsRequest(requestId.Value);

            requestFileRepository.Delete(id);
            repository.SaveChanges();
        }

        public IFileUploadDTO GetNew()
        {
            return new RequestFileDTO();
        }
    }
}
