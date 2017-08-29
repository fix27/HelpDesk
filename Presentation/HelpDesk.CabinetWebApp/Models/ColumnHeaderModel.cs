using System;

namespace HelpDesk.CabinetWebApp.Models
{
    public enum TypeColumnHeaderFilter
    {
        None,
        CheckBoxList,
        Date,
        Number,
        Text
    }
    public class ColumnHeaderModel
    {
        public string Name { get; set; }
        public string PropertyName { get; set; }

        public bool Sortable { get; set; }

        public TypeColumnHeaderFilter TypeFilter { get; set; }


        public string NgModelValue { get; set; }
        public string NgModelValue1 { get; set; }
        public string NgModelValue2 { get; set; }
        public string NgModelItems { get; set; }
        public string NgModelItemValue { get; set; }
        public string NgModelItemName { get; set; }

        

        public FilterGridDialogModel GetFilterGridDialogModel()
        {
            if (String.IsNullOrWhiteSpace(PropertyName) ||
                        String.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("In FilterGridDialogModel PropertyName and Name can't null");

            switch (TypeFilter)
            {
                case TypeColumnHeaderFilter.None:
                    return null;
                case TypeColumnHeaderFilter.CheckBoxList:
                    if (String.IsNullOrWhiteSpace(NgModelItems))
                        throw new ArgumentException("In FilterGridDialogModel for TypeColumnHeaderFilter.CheckBoxList NgModelItems can't null");
                    return new FilterGridDialogModel(PropertyName, Name, NgModelItems, NgModelItemValue, NgModelItemName);
                case TypeColumnHeaderFilter.Date:
                    if (String.IsNullOrWhiteSpace(NgModelValue1) ||
                        String.IsNullOrWhiteSpace(NgModelValue2))
                        throw new ArgumentException("In FilterGridDialogModel for TypeColumnHeaderFilter.Date NgModelValue1 and NgModelValue2 can't null");
                    return new FilterGridDialogModel(PropertyName, Name, NgModelValue1, NgModelValue2);
                case TypeColumnHeaderFilter.Number:
                    if (String.IsNullOrWhiteSpace(NgModelValue1) ||
                        String.IsNullOrWhiteSpace(NgModelValue2))
                        throw new ArgumentException("In FilterGridDialogModel for TypeColumnHeaderFilter.Number NgModelValue1 and NgModelValue2 can't null");
                    return new FilterGridDialogModel(PropertyName, Name, NgModelValue1, NgModelValue2);
                case TypeColumnHeaderFilter.Text:
                    if (String.IsNullOrWhiteSpace(NgModelValue))
                        throw new ArgumentException("In FilterGridDialogModel for TypeColumnHeaderFilter.Text NgModelValue can't null");
                    return new FilterGridDialogModel(PropertyName, Name, NgModelValue);
            }

            return null;
        }

        public string GetFilterFormUrl()
        {
            switch (TypeFilter)
            {
                case TypeColumnHeaderFilter.CheckBoxList:
                    return "~/App/Main/views/shared/filterGridCheckBoxList.cshtml";
                case TypeColumnHeaderFilter.Date:
                    return "~/App/Main/views/shared/filterGridDate.cshtml";
                case TypeColumnHeaderFilter.Number:
                    return "~/App/Main/views/shared/filterGridNumber.cshtml";
                case TypeColumnHeaderFilter.Text:
                    return "~/App/Main/views/shared/filterGridText.cshtml";
            }

            return null;
        }

        public ColumnHeaderModel()
        {
            TypeFilter = TypeColumnHeaderFilter.None;
            Sortable = true;
        }

    }
}