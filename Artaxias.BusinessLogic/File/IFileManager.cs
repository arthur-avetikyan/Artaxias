using Artaxias.Web.Common.DataTransferObjects.File;

using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.FileManagement
{
    public interface IFileManager
    {
        void DeleteFile(string filePath);
        Task<string> SaveFileAsync(FileDto file, string rootFolder);
        Task<FileDto> GetFileAsync(string filePath, string fileName);
        string ReadFile(string path);
    }
}