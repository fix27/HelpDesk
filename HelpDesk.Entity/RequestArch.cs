namespace HelpDesk.Entity
{
    /// <summary>
    /// Архивная заявка
    /// </summary>
    public class RequestArch : BaseRequest
    {
        public override bool Archive { get { return true; } }
    }
}
