
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Artaxias.Document.Generators
{
    public abstract class DocumentGenerator<T> : IDocumentGenerator where T : IDisposable
    {
        public string GenerateDocumnet(string templatePath, Dictionary<string, string> mappings)
        {
            using T doc = GetDocument(templatePath);
            Customize(doc);
            string text = ExtractText(doc);
            text = SearchAndReplace(text, mappings);
            text = Process(text);
            WriteText(doc, text);
            return SaveDocument(doc);
        }

        protected abstract T GetDocument(string templatePath);

        protected abstract void Customize(T doc);

        protected abstract string ExtractText(T doc);

        protected virtual string SearchAndReplace(string text, Dictionary<string, string> mappings)
        {
            //Regex regexText = new Regex(DocumentConstants._pattern, RegexOptions.IgnoreCase);
            //docText = regexText.Replace(docText, "Admin Admin");

            foreach (KeyValuePair<string, string> keyValue in mappings)
            {
                text = Regex.Replace(text, "{{" + keyValue.Key + "}}", keyValue.Value, RegexOptions.CultureInvariant);
            }
            return text;
        }

        protected abstract string Process(string mappedText);

        protected abstract void WriteText(T doc, string mappedText);

        protected abstract string SaveDocument(T doc);
    }
}
