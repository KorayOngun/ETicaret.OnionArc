using ETicaret.Application.Abstractions.Storage;
using ETicaret.Infrastructure.Operations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Services.Storage
{
    public abstract class Storage
    {
        public delegate bool HasFileHandler(string path, string fileName);
        
        protected async Task<string> FileRenameAsync(string path, string fileName, HasFileHandler hasFile)
        {
            return await Task.Run(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = $"{NameOperations.CharacterRegulatory(oldName)}";

                var resultFileName = await FileNameİndexer(path, newFileName, extension, hasFile);
                return resultFileName;
            });
        }

        protected async Task<string> FileNameİndexer(string path, string newFileName, string extension, HasFileHandler hasFile)
        {
            return await Task.Run(() =>
            {
                var index = 0;
                while (hasFile.Invoke(path,$"{newFileName}{(index != 0 ? "-" + index.ToString() : "")}{extension}"))
                    index++;
                newFileName = index != 0 ? newFileName + "-" + index.ToString() + extension : newFileName + extension;
                return newFileName;
            });
        }
      
    }
}
