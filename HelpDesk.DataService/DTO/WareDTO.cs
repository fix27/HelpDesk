using HelpDesk.DataService.DTO.Resources;

namespace HelpDesk.DataService.DTO
{
    public class WareDTO
    {
        public bool Id { get; set; }
        public string Name { get; set; }

        public static string GetName(bool soft)
        {
            if (soft)
                return Resource.Name_WareIS;
            else
                return Resource.Name_WareTO;
        }
    }
}
