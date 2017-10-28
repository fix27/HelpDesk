using System;

namespace HelpDesk.DataService.DTO.Parameters
{
    /// <summary>
    /// Параметр: Новая или сущуствующая заявка
    /// </summary>
    public class RequestParameter
    {
        /// <summary>
        /// Id существующей заявки
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Id заявки, на основе которой создается заявка
        /// </summary>
        public long? ByRequestId { get; set; }

        /// <summary>
        /// Id сотрудника
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// Id пользователя, создающего/редактирующего заявку. Если заявка создается из личного кабинета - UserId = 0
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id объекта заявки
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// Наименование объекта заявки
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Информация о сотруднике
        /// </summary>
        public string EmployeeInfo { get; set; }

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
