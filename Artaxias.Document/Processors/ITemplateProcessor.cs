using System.Collections.Generic;

namespace Artaxias.Document.Processors
{
    public interface ITemplateProcessor
    {
        IList<string> ProcessTemplate(string templatePath);
    }
}
