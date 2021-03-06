﻿namespace HelpDesk.Entity
{
    /// <summary>
    /// Различные системные настройки
    /// </summary>
    public class Settings : BaseEntity
    {
        /// <summary>
        /// Минимальный интервал (мин), через который можно пользователю подать очередную заявку
        /// </summary>
        public decimal MinInterval { get; set; }

        /// <summary>
        /// Ограничение на количество заявок в день
        /// </summary>
        public int LimitRequestCount { get; set; }

        /// <summary>
        /// Максимальное количество файлов, которые можно прикрепить к заявке
        /// </summary>
        public int MaxRequestFileCount { get; set; }

        /// <summary>
        /// Максимальный размер файла (Кб), который можно прикрепить к заявке
        /// </summary>
        public int MaxRequestFileSize { get; set; }

        /// <summary>
        /// Максимальная длина имени файла, который можно прикрепить к заявке
        /// </summary>
        public int MaxFileNameLength { get; set; }

        /// <summary>
        /// Ссылка на руководство пользователя
        /// </summary>
        public string ManualUrl { get; set; }

        /// <summary>
        /// Ссылка на соглашение об уровне сервиса
        /// </summary>
        public string ServiceLevelAgreementUrl { get; set; }

        /// <summary>
        /// Произвольный текст некоторого сообщения, которое пользователь должен видеть
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Телефон технической поддержки
        /// </summary>
        public string TechSupportPhones { get; set; }
        
        /// <summary>
        /// Час начала рабочего дня по умолчанию
        /// </summary>
        public int? StartWorkDay { get; set; }

        /// <summary>
        /// Час окончания рабочего дня по умолчанию
        /// </summary>
        public int? EndWorkDay { get; set; }

        /// <summary>
        /// Час начала обеденного перерыва по умолчанию
        /// </summary>
        public int? StartLunchBreak { get; set; }

        /// <summary>
        /// Час окончания обеденного перерыва по умолчанию
        /// </summary>
        public int? EndLunchBreak { get; set; }

        /// <summary>
        /// Минимальное количество дней по умолчанию, на которое устанавливается дата переноса заявки
        /// </summary>
        public int? MinCountTransferDay { get; set; }
        
        /// <summary>
        /// Максимальное количество дней, на которое возможен перенос заявки
        /// </summary>
        public int? MaxCountTransferDay { get; set; }

    }    
}
