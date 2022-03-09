
using System.Threading.Tasks;

namespace Artaxias.Mailing
{
    public interface IEmailProcessor
    {
        Task SendAsync(EmailConfigurationParameters parameters);
    }
}
