using System.Collections.Generic;
using HelpDesk.Common.Helpers;
using System;
using HelpDesk.DataService.DTO;

namespace HelpDesk.DataService.Filters
{
    /// <summary>
    /// Для фильтрации заявок
    /// </summary>
    public class RequestFilter
    {
        /// <summary>
        /// Номер заявки, или номера заявок ч/з запятую
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Список Id заявок ч/з запятую
        /// </summary>
        public IEnumerable<long> Ids { get { return Id.ToEnumerable<long>(); } }

        /// <summary>
        /// Наименование объекта (включая тип, модель, производителя)
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Информация о заявителе (ФИО, телефон, кабинет, наименование/адрес обслуживаемой организации)
        /// </summary>
        public string EmployeeInfo { get; set; }

        /// <summary>
        /// Нименование исполнителя
        /// </summary>
        public string WorkerName { get; set; }

        /// <summary>
        /// Описание проблемы
        /// </summary>
        public string DescriptionProblem { get; set; }

        /// <summary>
        /// Список состояний заявки
        /// </summary>
        public IList<StatusRequestEnum> StatusIds { get; set; }

        /// <summary>
        /// Список RAW-состояний заявки
        /// </summary>
        public IList<long> RawStatusIds { get; set; }

        /// <summary>
        /// Дата подачи заявки "С"
        /// </summary>
        public DateTime? DateInsert1 { get; set; }

        /// <summary>
        /// Дата подачи заявки "По"
        /// </summary>
        public DateTime? DateInsert2 { get; set; }

        /// <summary>
        /// Дата окончания срока выполнения работ по заявке "С"
        /// </summary>
        public DateTime? DateEndPlan1 { get; set; }

        /// <summary>
        /// Дата окончания срока выполнения работ по заявке "По"
        /// </summary>
        public DateTime? DateEndPlan2 { get; set; }


        /// <summary>
        /// Год для поиска в архиве
        /// </summary>
        public int ArchiveYear { get; set; }

        /// <summary>
        /// Месяц для поиска в архиве
        /// </summary>
        public int ArchiveMonth { get; set; }

        /// <summary>
        /// Год для поиска в архиве, среди которых осуществляется выбор
        /// </summary>
        public IList<Year> ArchiveYears { get; set; }

        /// <summary>
        /// Месяц для поиска в архиве, среди которых осуществляется выбор
        /// </summary>
        public IList<Month> ArchiveMonths { get; set; }

        /// <summary>
        /// Поиск в архиве
        /// </summary>
        public bool Archive { get; set; }

    }
}
