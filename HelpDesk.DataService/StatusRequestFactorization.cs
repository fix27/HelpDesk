using HelpDesk.DTO;
using System;
using System.Collections.Generic;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Прямое и обратное преобразование StatusRequestEnum - statusRequestId
    /// </summary>
    public static class StatusRequestFactorization 
    {
        private static Lazy<IDictionary<long, StatusRequestEnum>> equivalenceByElement = new Lazy<IDictionary<long, StatusRequestEnum>>(() =>
        {
            IDictionary<long, StatusRequestEnum> map = new Dictionary<long, StatusRequestEnum>()
            {
                {824,  StatusRequestEnum.Rejected },                //Отказано
                {825,  StatusRequestEnum.ExtendedDeadLine },        //Перенос
                {826,  StatusRequestEnum.NotApprovedComplete },     //Отказано в готовности
                {191,  StatusRequestEnum.New },                     //Рассмотрение
                {192,  StatusRequestEnum.Accepted },                //Принята
                {193,  StatusRequestEnum.ExtendedDeadLine },        //Подтверждение переноса
                {194,  StatusRequestEnum.Rejected },                //Подтверждение отказа
                {195,  StatusRequestEnum.Closing },                 //Подтверждение готовности
                {196,  StatusRequestEnum.ApprovedComplete },        //Выполнена
                {839,  StatusRequestEnum.Rejected },                //Отказ после принятия
                {22464,  StatusRequestEnum.Passive },               //Пасив
                {383343,  StatusRequestEnum.ExtendedConfirmation }  //Перенос готовности
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
