
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ETicaret.Infrastructure;
using ETicaret.Infrastructure.Operations;
namespace ETicaret.Infrastructure.Services
{
    public class FileService
    {
        private readonly IHostingEnvironment _env;

        public FileService(IHostingEnvironment env)
        {
            _env = env;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fs = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fs);
                await fs.FlushAsync();
                return true;
            }
            catch (Exception)
            {
                //TODO LOG!
                throw;
            }
        }

        private async Task<string> FileRenameAsync(string path, string fileName)
        {
            return  await Task.Run(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = $"{NameOperations.CharacterRegulatory(oldName)}";
             
                var resultFileName = await FileNameİndexer(path, newFileName, extension);
                return resultFileName;
            });
        }

        private async Task<string> FileNameİndexer(string path, string newFileName, string extension)
        {
            return await Task.Run(() =>
            {
                var index = 0;
                while (File.Exists($"{path}\\{newFileName}{(index != 0 ? "-" + index.ToString() : "")}{extension}"))
                    index++;
                newFileName = index != 0 ? newFileName + "-" + index.ToString() + extension : newFileName + extension;
                return newFileName;
            });
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection formFiles)
        {

            string uploadPath = Path.Combine(_env.WebRootPath, path);

            if (File.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new(); 

            List<bool> results = new();
            foreach (IFormFile file in formFiles)
            {
               string fileNewName = await FileRenameAsync(uploadPath,file.FileName);
               bool result = await CopyFileAsync(Path.Combine(uploadPath, fileNewName), file);
               datas.Add((fileNewName, path));
               results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true)))
            {
                return datas;
            }

            return null;
        }
    }
}
