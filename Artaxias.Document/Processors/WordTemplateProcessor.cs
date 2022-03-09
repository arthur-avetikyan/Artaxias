using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Artaxias.Document.Processors
{
    public class WordTemplateProcessor : TemplateProcessor<WordprocessingDocument>
    {
        protected override void Customize(WordprocessingDocument doc)
        {
        }

        protected override WordprocessingDocument GetDocument(string templatePath)
        {
            string fileType = Path.GetExtension(templatePath);
            return fileType switch
            {
                DocumentConstants._templateExtension => WordprocessingDocument.CreateFromTemplate(templatePath),
                DocumentConstants._documentExtension => WordprocessingDocument.Create(templatePath, WordprocessingDocumentType.Document),
                _ => throw new ArgumentException($"{templatePath} file type is not supported."),
            };
        }

        protected override string ExtractText(WordprocessingDocument doc)
        {
            string docText = null;
            using (StreamReader sr = new StreamReader(doc.MainDocumentPart.GetStream()))
            {
                docText = sr.ReadToEnd();
            }
            return docText;
        }

        protected override IList<string> Process(IEnumerable<string> mappings)
        {
            return mappings.ToList();
        }
    }
}
