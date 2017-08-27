using System;


namespace HelpDesk.DTO
{
    /// <summary>
    /// Различные константы
    /// </summary>
    public static class Constants
    {
        private static Lazy<long[]> ignoredRawRequestStates = new Lazy<long[]>(() =>
        {
            return new long[]
                {
                    197,
                    1259,
                    1260,
                    1261,
                    2436,
                    2437,
                    2442,
                    127467,
                    240431,
                    240432,
                    840,
                    841
                };
        });

        /// <summary>
        /// Состояния заявок, которые не используются, и, соответственно, не отображаются в UI
        /// </summary>
        public static long[] IgnoredRawRequestStates
        {
            get
            {
                return ignoredRawRequestStates.Value;
            }
        }

        /// <summary>
        /// Псевдособытие "Дата окончания"
        /// </summary>
        public static long DateEndStatusRequest { get { return 197; } }

        /// <summary>
        /// Новая заявка
        /// </summary>
        public static long NewStatusRequest { get { return 191; } }
        
        /// <summary>
        /// Событие "Подтверждение готовности"
        /// </summary>
        public static long ConfirmationStatusRequest { get { return 195; } }
                
    }
}
