using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSearch
{
    public class WordFile
    {
        public string subpath { get; set; }
        public string filename { get; set; }
        public string content { get; set; }

        public WordFile(string subpath, string filename, string content)
        {
            this.subpath = subpath;
            this.filename = filename;
            this.content = content;
        }
    }
}
