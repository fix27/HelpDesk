﻿using HelpDesk.Common.Cache;
using HelpDesk.Common.Cache.Interface;
using System;

namespace HelpDesk.DataService.DTO.Parameters
{
    /// <summary>
    /// Параметр: Новое состояние заявки
    /// </summary>
    public class RequestEventParameter: IForCacheKeyValue
    {
        /// <summary>
        /// Версия записи заявки для разруливания конкурентного доступа
        /// </summary>
        public int RequestVersion { get; set; }

        /// <summary>
        /// Id заявки
        /// </summary>
        public long RequestId { get; set; }

        /// <summary>
        /// Id нового состояния
        /// </summary>
        public long StatusRequestId { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Новая дата окончания срока выполнения работ по заявке
        /// </summary>
        public DateTime? NewDeadLineDate { get; set; }

        public string GetForCacheKeyValue()
        {
            return RequestId.ToString();
        }

    }
}
