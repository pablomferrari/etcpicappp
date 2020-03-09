using System.IO;
using System.Threading.Tasks;

namespace EtcPicApp.Data
{
	public interface IFileSave
	{
		Task SaveAndView(string filename, string contentType, MemoryStream stream);
	}
}
