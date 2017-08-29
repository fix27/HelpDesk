using System;

namespace HelpDesk.Entity
{
    /// <summary>
    /// Заявка
    /// </summary>
    public class BaseRequest: BaseEntity
    {

        //все свойства помечены как virtual, потому что так нужно 
        //для маппинга union-subclass в NHibernate
        public virtual int Version { get; set; }

        /// <summary>
        /// Описание проблемы
        /// </summary>
        public virtual string DescriptionProblem{ get; set; }
        
        
        /// <summary>
        /// ID состояния заявки
        /// </summary>
        public virtual StatusRequest Status { get; set; }
        

        /// <summary>
        /// Дата занесения заявки
        /// </summary>
        public virtual DateTime DateInsert { get; set; }

        /// <summary>
        /// Дата последнего изменения заявки
        /// </summary>
        public virtual DateTime DateUpdate { get; set; }

        
        /// <summary>
        /// Плановая дата окончания работ по заявке 
        /// </summary>
        public virtual DateTime? DateEndPlan { get; set; }

        /// <summary>
        /// Фактическая дата окончания работ по заявке (проставляется при подтверждении закрытия заявки как текущая дата)
        /// </summary>
        public virtual DateTime? DateEndFact { get; set; }

        /// <summary>
        /// Пользователь, последним изменивший состояние заявки
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// Количество переносов срока
        /// </summary>
        public virtual int CountCorrectionDateEndPlan { get; set; }

        /// <summary>
        /// Объект заявки
        /// </summary>
        public virtual RequestObject Object { get; set; }

        /// <summary>
        /// Заявитель
        /// </summary>
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Worker Worker { get; set; }

        public virtual bool Archive { get; }

    }
}
