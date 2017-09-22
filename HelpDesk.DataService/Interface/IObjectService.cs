using HelpDesk.DataService.DTO;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IObjectService
    {
        IEnumerable<WareDTO> GetListWare();

        IEnumerable<SimpleDTO> GetListHardType(string name = null);

        IEnumerable<SimpleDTO> GetListModel(long manufacturerId, string name = null);

        IEnumerable<SimpleDTO> GetListManufacturer(string name = null);
    }
}
