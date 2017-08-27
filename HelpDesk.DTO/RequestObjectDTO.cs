using HelpDesk.DTO.Resources;
using HelpDesk.Entity;
using System;

namespace HelpDesk.DTO
{
    public class RequestObjectDTO
    {
        public long Id { get; set; }
        
        /// <summary>
        /// Наименование ПО
        /// </summary>
        public string SoftName { get; set; }

        public string ObjectTypeName { get { return ObjectType.Name;  }  }
        
               
        /// <summary>
        /// Полное наименование объекта заявки (ТО и т.п.)
        /// </summary>
        public string ObjectName
        {
            get
            {
                return GetObjectName(ObjectTypeName, SoftName, HardType, Model);
            }
        }

        public static string GetObjectName(string objectTypeName, string softName, HardType hardType, Model model)
        {
            if (!String.IsNullOrEmpty(softName))
                return softName;

            if (model == null)
                return objectTypeName;



            return String.Format("{0}{1}{2}",
                        hardType.Name,
                        model.Manufacturer == null ? "" : String.Format(" - {0}", model.Manufacturer.Name),
                        String.Format(" - {0}: {1}", Resource.Name_Model, model.Name));
        }

        public string WorkTypeName
        {
            get
            {
                return Soft ? Resource.Name_WorkTypeWareIS : Resource.Name_WorkTypeWareTO;
            }
        }
                
        public bool Soft { get; set; }
        public ObjectType ObjectType { get; set; }
        public HardType HardType { get; set; }
        public Model Model { get; set; }

    }
}
