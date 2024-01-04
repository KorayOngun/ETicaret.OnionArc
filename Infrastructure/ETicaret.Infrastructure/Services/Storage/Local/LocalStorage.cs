using ETicaret.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage,ILocalStorage
    {
        private readonly IHostingEnvironment _env;

        public LocalStorage(IHostingEnvironment env)
        {
            _env = env;
        }
        public async Task DeleteAsync(string path, string fileName)
        {
           await Task.Run(()=> File.Delete(Path.Combine(path,fileName)));
        }

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
        {
            var result =  File.Exists(Path.Combine(path,fileName));
            return result;
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection formFiles)
        {
            string uploadPath = Path.Combine(_env.WebRootPath, path);

            if (!File.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();

            foreach (IFormFile file in formFiles)
            {
                var fileName = await FileRenameAsync(uploadPath, file.Name,HasFile);
                bool result = await CopyFileAsync(Path.Combine(uploadPath, fileName), file);
                datas.Add((fileName, path));
            }
            return datas;


            //TODO eğer ki yukarıdaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığına dair exception oluştur.
            //return null;
        }
        private async Task<bool> CopyFileAsync(string path, IFormFile file)
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
    }
}
