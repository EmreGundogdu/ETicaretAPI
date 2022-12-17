using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Services;
public interface IFileService
{
    Task UploadAsync(string path,IFormFileCollection files);
}
