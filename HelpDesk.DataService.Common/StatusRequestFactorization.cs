using HelpDesk.DataService.Common.DTO;
using HelpDesk.Entity;
using System;
using System.Collections.Generic;

namespace HelpDesk.DataService.Common
{
    /// <summary>
    /// Прямое и обратное преобразование StatusRequestEnum - StatusRequestId
    /// </summary>
    public static class StatusRequestFactorization 
    {
        private static Lazy<IDictionary<long, StatusRequestEnum>> equivalenceByElement = new Lazy<IDictionary<long, StatusRequestEnum>>(() =>
        {
            IDictionary<long, StatusRequestEnum> map = new Dictionary<long, StatusRequestEnum>()
            {
                {(long)RawStatusRequestEnum.New,            StatusRequestEnum.New },                    //Рассмотрение
                {(long)RawStatusRequestEnum.Accepted,       StatusRequestEnum.Accepted },               //Принята
                {(long)RawStatusRequestEnum.Rejected,       StatusRequestEnum.Rejected },               //Отказано
                {(long)RawStatusRequestEnum.RejectedAfterAccepted,  StatusRequestEnum.Rejected },       //Отказ после принятия
                {(long)RawStatusRequestEnum.ExtendedDeadLine,  StatusRequestEnum.ExtendedDeadLine },    //Перенос
                {(long)RawStatusRequestEnum.Closing,        StatusRequestEnum.Closing },                //Выполнена
                {(long)RawStatusRequestEnum.ExtendedConfirmation,  StatusRequestEnum.ExtendedConfirmation },    //Перенос подтверждения
                {(long)RawStatusRequestEnum.NotApprovedComplete,  StatusRequestEnum.NotApprovedComplete },      //Отказано в готовности
                {(long)RawStatusRequestEnum.ApprovedRejected,  StatusRequestEnum.Rejected },            //Подтверждение отказа
                {(long)RawStatusRequestEnum.ApprovedComplete,  StatusRequestEnum.ApprovedComplete },    //Подтверждение выполнения
                {(long)RawStatusRequestEnum.Passive,        StatusRequestEnum.Passive }                 //Пасив
            };
            return map;
        });
        

        private static Lazy<IDictionary<StatusRequestEnum, IEnumerable<long>>> elementsByEquivalence = new Lazy<IDictionary<StatusRequestEnum, IEnumerable<long>>>(() =>
        {
            IDictionary<StatusRequestEnum, IEnumerable<long>> map = new Dictionary<StatusRequestEnum, IEnumerable<long>>();
            return map;
        });

        public static StatusRequestEnum GetEquivalenceByElement(long statusRequestId)
        {
            try
            {
                return equivalenceByElement.Value[statusRequestId];
            }
            catch
            {
                return StatusRequestEnum.Unknown;
            }
        }
        
        public static IEnumerable<long> GetElementsByEquivalence(StatusRequestEnum statusRequest)
        {
            if (elementsByEquivalence.Value.ContainsKey(statusRequest))
                return elementsByEquivalence.Value[statusRequest];
            else
            {
                IList<long> keys = new List<long>();
                foreach (long key in equivalenceByElement.Value.Keys)
                {
                    if (equivalenceByElement.Value[key] == statusRequest)
                        keys.Add(key);
                }

                elementsByEquivalence.Value[statusRequest] = keys;
                return keys;
            }
                
        }
    }
}
