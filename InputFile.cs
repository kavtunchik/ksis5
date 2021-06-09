using Microsoft.AspNetCore.Http;

namespace laba5
{
    public class InputFile
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
