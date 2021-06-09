using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace laba5
{
    public class FileEl
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public FileEl(string Name, string Type)
        {
            this.Name = Name;
            this.Type = Type;
        }

    }
}