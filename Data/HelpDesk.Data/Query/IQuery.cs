using HelpDesk.Entity;
using System.Linq;

namespace HelpDesk.Data.Query
{
	public interface IQuery<TParam, TDto>
	{
		TDto Get(TParam param);
	}
}
