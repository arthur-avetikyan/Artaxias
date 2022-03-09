using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

using System;
using System.IO;

namespace Artaxias.Document.Generators
{
    public class WordDocumentGenerator : DocumentGenerator<WordprocessingDocument>
    {
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

        protected override void Customize(WordprocessingDocument doc) { }

        protected override string ExtractText(WordprocessingDocument doc)
        {
            string docText = null;
            using (StreamReader sr = new(doc.MainDocumentPart.GetStream()))
            {
                docText = sr.ReadToEnd();
            }

            return docText;
        }

        protected override string Process(string mappedText)
        {
            return mappedText;
        }

        protected override void WriteText(WordprocessingDocument doc, string mappedText)
        {
            using StreamWriter sw = new StreamWriter(doc.MainDocumentPart.GetStream(FileMode.Create, FileAccess.Write));
            sw.Write(mappedText);
        }

        protected override string SaveDocument(WordprocessingDocument doc)
        {
            doc.ChangeDocumentType(WordprocessingDocumentType.Document);
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), DocumentConstants._rootFolder);
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{Guid.NewGuid()}{DocumentConstants._documentExtension}");
            using OpenXmlPackage output = doc.SaveAs(filePath);

            return filePath;
        }
    }
}
