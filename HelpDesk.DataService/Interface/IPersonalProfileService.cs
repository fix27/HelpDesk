using HelpDesk.DTO;

namespace HelpDesk.DataService.Interface
{
    public interface IPersonalProfileService
    {
        PersonalProfileDTO Get(long id);
        void Save(PersonalProfileDTO dto);
        bool IsComplete(long id);
    }
}
