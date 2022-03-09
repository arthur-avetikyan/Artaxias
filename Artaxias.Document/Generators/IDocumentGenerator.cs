using System.Collections.Generic;

namespace Artaxias.Document.Generators
{
    public interface IDocumentGenerator
    {
        string GenerateDocumnet(string templatePath, Dictionary<string, string> mappings);
    }
}