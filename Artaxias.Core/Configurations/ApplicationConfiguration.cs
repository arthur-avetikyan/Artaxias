
using Artaxias.Web.Server.Wrappers;

namespace Artaxias.Core.Configurations
{
    public class ApplicationConfiguration
    {
        public string Url { get; set; }

        public ApplicationAuthenticationConfiguration Authentication { get; set; }

        public string EmailTemplatesFilePath { get; set; }

        public string DocumentTemplatesFilePath { get; set; }
    }
}
