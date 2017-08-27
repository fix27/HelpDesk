namespace HelpDesk.WebApp.Models
{
    public class FilterGridDialogModel
    {
        public string DialogId { get; set; }
        public string DialogTitle { get; set; }

        public string NgModelValue { get; set; }
        public string NgModelValue1 { get; set; }
        public string NgModelValue2 { get; set; }
        public string NgModelItems { get; set; }
        public string NgModelItemValue { get; set; }
        public string NgModelItemName { get; set; }

        protected FilterGridDialogModel(string dialogId, string dialogTitle)
        {
            DialogId = dialogId;
            DialogTitle = dialogTitle;
        }

        public FilterGridDialogModel(string dialogId, string dialogTitle, string ngModelValue)
            :this(dialogId, dialogTitle)
        {
            NgModelValue = ngModelValue;
        }

        public FilterGridDialogModel(string dialogId, string dialogTitle, string ngModelValue1, string ngModelValue2)
            : this(dialogId, dialogTitle)
        {
            NgModelValue1 = ngModelValue1;
            NgModelValue2 = ngModelValue2;
        }

        public FilterGridDialogModel(string dialogId, string dialogTitle, string ngModelItems, string ngModelItemValue = null, string ngModelItemName = null)
            : this(dialogId, dialogTitle)
        {
            NgModelItems = ngModelItems;
            NgModelItemValue = ngModelItemValue == null? "Id": ngModelItemValue;
            NgModelItemName = ngModelItemName == null ? "Name" : ngModelItemName;
        }
    }
}