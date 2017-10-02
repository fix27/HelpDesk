using HelpDesk.Entity;

namespace HelpDesk.DataService.DTO
{
    public class EmployeeObjectDTO
    {
        public long Id { get; set; }
        public long ObjectId { get; set; }
        
        /// <summary>
        /// Наименование ПО
        /// </summary>
        public string SoftName { get; set; }

        public string ObjectTypeName { get; set; }

        
        /// <summary>
        /// Полное наименование объекта заявки (ТО и т.п.)
        /// </summary>
        public string ObjectName
        {
            get
            {
                return RequestObjectDTO.GetObjectName(ObjectTypeName, SoftName, HardType, Model);
            }
        }

        public string WareName
        {
            get
            {
                return WareDTO.GetName(Soft);
            }
        }

         

        public ObjectType ObjectType { get; set; }
        public HardType HardType { get; set; }
        public Model Model { get; set; }

        public bool Soft{ get; set; }

    }
}
