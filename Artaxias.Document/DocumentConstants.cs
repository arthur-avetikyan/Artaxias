namespace Artaxias.Document
{
    internal static class DocumentConstants
    {
        internal const string _pattern = @"(?<=\{{).*?(?=\}})";
        internal const string _documentExtension = ".docx";
        internal const string _templateExtension = ".dotx";
        internal const string _rootFolder = "Output";
    }
}
