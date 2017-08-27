using System;

namespace HelpDesk.DTO
{
    /// <summary>
    /// Новая заявка
    /// </summary>
    public class CreateOrUpdateRequestDTO
    {
        /// <summary>
        /// Id существующей заявки
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Id профиля пользователя
        /// </summary>
        public long PersonalProfileId { get; set; }

        /// <summary>
        /// Id объекта заявки
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// Наименование объекта заявки
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Описание проблемы
        /// </summary>
        public string DescriptionProblem { get; set; }

        /// <summary>
        /// Временный PK заявки. Используется для привязки файлов
        /// </summary>
        public Guid TempRequestKey { get; set; }
    }
}
