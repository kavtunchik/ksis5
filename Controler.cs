using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.VisualBasic.FileIO;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace laba5
{
    [Route("api")]
    public class FileController : Controller
    {
        private readonly string _path = "C:\\laba5\\laba5\\wwwroot\\Files";

        [HttpPut("PUT")]
        public IActionResult  InputFile([FromForm] InputFile input)
        {
            try
            {
                using (var fileStream = new FileStream(Path.Combine(_path, input.Name), FileMode.Create))
                {
                    input.File.CopyTo(fileStream);
                }
                return Ok("Файл создан");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GET")]
        public IActionResult GetFile(string namefile)
        {
            if (System.IO.File.Exists(Path.Combine(_path, namefile)))
            {
                try
                {
                    string path = Path.Combine(_path, namefile);
                    FileStream file = new FileStream(path, FileMode.Open);
                    return File(file, "application/unknown", Path.GetFileName(namefile));
                }
                catch
                {
                    return NotFound("Такого файла не существует");
                }
            }
            else
            {
                string directoryname = namefile;
                try
                {
                    IReadOnlyCollection<string> files = FileSystem.GetFiles(Path.Combine(_path, directoryname));
                    IReadOnlyCollection<string> directories = FileSystem.GetDirectories(Path.Combine(_path, directoryname));
                    List<FileEl> content = new List<FileEl>();
                    foreach (var item in directories)
                    {
                        content.Add(new FileEl(Path.GetFileName(item), "Папка"));
                    }
                    foreach (var item in files)
                    {
                        content.Add(new FileEl(Path.GetFileName(item), "Файл"));
                    }
                    return new JsonResult(content, new JsonSerializerOptions { });
                }
                catch
                {
                    return NotFound("Такой директории не существует");
                }

            }
        }

        [HttpGet("INFO")]
        public IActionResult GetInfo(string namefile)
        {
            string path = Path.Combine(_path, namefile);
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                infoFile info = new infoFile();
                info.Name = fileInfo.Name;
                info.Path = fileInfo.DirectoryName;
                info.Size = fileInfo.Length;
                info.CreationDate = fileInfo.CreationTime.ToString();
                info.ChangedDate = fileInfo.LastWriteTime.ToString();
                return Ok(info);
            }
            else
            {
                return NotFound("Такого файла не существует");
            }
        }

        [HttpDelete("DELETE")]
        public IActionResult DeleteFile(string namefile)
        {
            if (System.IO.File.Exists(Path.Combine(_path, namefile)))
            {
                try
                {
                    FileSystem.DeleteFile(Path.Combine(_path, namefile));
                    return Ok("Файл успешно удален");
                }
                catch
                {
                    return NotFound("Такого файла не существует");
                }
            }
            else
            {
                try
                {
                    FileSystem.DeleteDirectory(Path.Combine(_path, namefile), DeleteDirectoryOption.DeleteAllContents);
                    return Ok("Директория успешно удалена");
                }
                catch
                {
                    return NotFound("Такого файла не существует");
                }
            }
        }

        
    }
}
