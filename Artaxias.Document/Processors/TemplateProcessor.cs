
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Artaxias.Document.Processors
{
    public abstract class TemplateProcessor<T> : ITemplateProcessor where T : class, IDisposable
    {
        public IList<string> ProcessTemplate(string templatePath)
        {
            using T doc = GetDocument(templatePath);
            Customize(doc);
            string docText = ExtractText(doc);
            IList<string> mappings = GetMappingFields(docText);
            return Process(mappings);
        }

        protected abstract T GetDocument(string templatePath);

        protected abstract void Customize(T doc);

        protected abstract string ExtractText(T doc);

        public virtual IList<string> GetMappingFields(string text)
        {
            List<string> mappings = new();
            try
            {
                foreach (Match match in Regex.Matches(text, DocumentConstants._pattern, RegexOptions.IgnoreCase))
                {
                    mappings.Add(match.Value);
                }
            }
            catch (RegexMatchTimeoutException) { /*handle*/ }

            return mappings;
        }

        protected abstract IList<string> Process(IEnumerable<string> mappings);
    }
}
