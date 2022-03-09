using Artaxias.Web.Common.DataTransferObjects.File;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.FileManagement
{
    public class FileManager : IFileManager
    {
        private readonly string _contentRootPath;

        public FileManager()
        {
            System.Reflection.Assembly entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            _contentRootPath = entryAssembly != null
                ? Path.GetDirectoryName(entryAssembly.Location)
                : throw new DirectoryNotFoundException($"{nameof(entryAssembly)} is null");
        }

        public async Task<string> SaveFileAsync(FileDto file, string rootFolder)
        {

            if (file == null || file.FileData == null || !(file.FileData.Length > 0) ||
                string.IsNullOrWhiteSpace(file.FileName) || string.IsNullOrWhiteSpace(rootFolder))
            {
                return string.Empty;
            }

            string name = $"{Path.GetFileNameWithoutExtension(file.FileName)?.ToLower().Replace(" ", "_")}_{Guid.NewGuid()}";
            string extension = Path.GetExtension(file.FileName);
            string filesPath = CreateDirectoryIfNot(rootFolder);
            string filePathToSave = Path.Combine(filesPath, name + extension);
            try
            {
                await File.WriteAllBytesAsync(filePathToSave, file.FileData);
                return filePathToSave;

            }
            catch (Exception e)
            {
                if (!e.Message.Contains("exists"))
                {
                    Console.WriteLine(e);
                }
                //TODO maybe need to handle
            }

            return filePathToSave;
        }

        public void DeleteFile(string filePath)
        {
            if (filePath == null || !File.Exists(filePath))
            {
                return;
            }

            File.Delete(filePath);
        }

        public string CreateDirectoryIfNot(string folderPathFromRoot)
        {
            string rootFolder = Directory.GetCurrentDirectory();
            string fullPathToDir = Path.Combine(rootFolder, folderPathFromRoot);
            if (Directory.Exists(fullPathToDir))
            {
                return fullPathToDir;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(fullPathToDir);
                    return fullPathToDir;
                }
                catch (Exception)
                {
                    // Console.WriteLine(e);
                    // throw;
                    return string.Empty;
                }
            }
        }

        public async Task<FileDto> GetFileAsync(string filePath, string fileName)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File not found: {filePath}");
            }

            byte[] fileData = await File.ReadAllBytesAsync(filePath);

            return new FileDto
            {
                FileName = fileName,
                FileData = fileData
            };
        }

        public string ReadFile(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new InvalidOperationException($"{nameof(path)} is not correct");
            }

            IFileInfo fileInfo = new PhysicalFileInfo(new FileInfo(Path.Combine(_contentRootPath, path)));

            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"Template file located at \"{path}\" was not found");
            }

            using Stream fs = fileInfo.CreateReadStream();
            using StreamReader sr = new StreamReader(fs);
            return sr.ReadToEnd();
        }
    }
}
