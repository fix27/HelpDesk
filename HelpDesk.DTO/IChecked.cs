namespace HelpDesk.DTO
{
    /// <summary>
    /// Используется, когда значения DTO выбираются при помощи чекбокса в UI
    /// </summary>
    /// <typeparam name="TValue">Тип выбранного значения</typeparam>
    public interface IChecked<TValue> where TValue: struct
    {
        /// <summary>
        /// Выбрано
        /// </summary>
        bool Checked { get; set; }

        /// <summary>
        /// Выбранное значение
        /// </summary>
        TValue Value { get; }
    }
}
