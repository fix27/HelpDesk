namespace HelpDesk.WebApp.Controllers.InputParameters
{
    public class AmountTransactionInputParameter
    {
        /// <summary>
        /// Кому перевод
        /// </summary>
        public long UserToId { get; set; }

        /// <summary>
        /// Сумма перевода
        /// </summary>
        public decimal Amount { get; set; }
    }
}