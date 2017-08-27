using HelpDesk.DataService.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace HelpDesk.DataService
{
    public abstract class BaseService: BaseDataService
    {
        protected bool checkStringConstraint(string propName, string propValue, bool notEmpty, int maxLength, int minLength)
        {
            return BaseService.CheckStringConstraint(this.errorMessages, this.extensions, propName, propValue, notEmpty, maxLength, minLength);
        }

        protected bool checkStringConstraint(int index, string propName, string propValue, bool notEmpty, int maxLength, int minLength)
        {
            return BaseService.CheckStringConstraint(index, this.indexErrorMessages, propName, propValue, notEmpty, maxLength, minLength);
        }

        protected List<object> extensions = new List<object>();

        protected Dictionary<string, List<string>> errorMessages = new Dictionary<string, List<string>>();
        protected Dictionary<int, Dictionary<string, List<string>>> indexErrorMessages = new Dictionary<int, Dictionary<string, List<string>>>();


        protected Dictionary<string, List<string>> warningMessages = new Dictionary<string, List<string>>();
        protected Dictionary<int, Dictionary<string, List<string>>> indexWarningMessages = new Dictionary<int, Dictionary<string, List<string>>>();

        protected void setErrorMsg(string key, string msg, object extension = null)
        {
            BaseService.SetMessages(this.errorMessages, this.extensions, key, msg, extension);
        }
        
        public static bool CheckStringConstraint(Dictionary<string, List<string>> messages, string propName, string propValue, bool notEmpty, int maxLength, int minLength)
        {
            return CheckStringConstraint(messages, null, propName, propValue, notEmpty, maxLength, minLength);
        }

        public static bool CheckStringConstraint(Dictionary<string, List<string>> messages, List<object> extensions, string propName, string propValue, bool notEmpty, int maxLength, int minLength)
        {
            if (propValue != null)
                propValue = Regex.Replace(propValue.Trim(), @"<[^>]*>", String.Empty);

            if (notEmpty)
            {
                if (propValue == null || propValue.Trim() == "")
                {
                    SetMessages(messages, extensions, propName, Resource.EmptyConstraintMsg);
                    return false;
                }
                else if (propValue.Length > maxLength)
                {
                    SetMessages(messages, extensions, propName, String.Format(Resource.LengthMaxConstraintMsg, propValue.Length, maxLength));
                    return false;
                }
                else if (minLength > 0 && propValue.Length < minLength)
                {
                    SetMessages(messages, extensions, propName, String.Format(Resource.LengthMinConstraintMsg, propValue.Length, minLength));
                    return false;
                }
            }
            else
            {
                if (propValue != null && propValue.Trim() != "" && propValue.Length > maxLength)
                {
                    SetMessages(messages, extensions, propName, String.Format(Resource.LengthMaxConstraintMsg, propValue.Length, maxLength));
                    return false;
                }
                else if (propValue != null && propValue.Trim() != "" && minLength > 0 && propValue.Length < minLength)
                {
                    SetMessages(messages, extensions, propName, String.Format(Resource.LengthMinConstraintMsg, propValue.Length, minLength));
                    return false;
                }
            }

            return true;
        }
        
        public static bool CheckStringConstraint(int index, Dictionary<int, Dictionary<string, List<string>>> indexMessages, string propName, string propValue, bool notEmpty, int maxLength, int minLength)
        {
            if (propValue != null)
                propValue = Regex.Replace(propValue.Trim(), @"<[^>]*>", String.Empty);

            if (notEmpty)
            {
                if (propValue == null || propValue.Trim() == "")
                {
                    SetMessages(index, indexMessages, propName, Resource.EmptyConstraintMsg);
                    return false;
                }
                else if (propValue.Length > maxLength)
                {
                    SetMessages(index, indexMessages, propName, String.Format(Resource.LengthMaxConstraintMsg, propValue.Length, maxLength));
                    return false;
                }
                else if (minLength > 0 && propValue.Length < minLength)
                {
                    SetMessages(index, indexMessages, propName, String.Format(Resource.LengthMinConstraintMsg, propValue.Length, minLength));
                    return false;
                }
            }
            else
            {
                if (propValue != null && propValue.Trim() != "" && propValue.Length > maxLength)
                {
                    SetMessages(index, indexMessages, propName, String.Format(Resource.LengthMaxConstraintMsg, propValue.Length, maxLength));
                    return false;
                }
                else if (propValue != null && propValue.Trim() != "" && minLength > 0 && propValue.Length < minLength)
                {
                    SetMessages(index, indexMessages, propName, String.Format(Resource.LengthMinConstraintMsg, propValue.Length, minLength));
                    return false;
                }
            }

            return true;
        }
        



        public static void SetMessages(Dictionary<string, List<string>> messages, List<object> extensions, string key, string msg, object extension = null)
        {
            if (!messages.Keys.Contains(key))
                messages[key] = new List<string>();
            messages[key].Add(msg);

            if (extension != null && extensions != null)
                extensions.Add(extension);
        }

        public static void SetMessages(int index, Dictionary<int, Dictionary<string, List<string>>> indexMessages, string key, string msg)
        {
            if (!indexMessages.Keys.Contains(index))
                indexMessages[index] = new Dictionary<string,List<string>>();

            if (!indexMessages[index].Keys.Contains(key))
                indexMessages[index][key] = new List<string>();
            indexMessages[index][key].Add(msg);
                        
        }

        public static void SetMessages(Dictionary<string, List<string>> messages, string key, string msg)
        {
            SetMessages(messages, null, key, msg);
        }
                

    }
}
