using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Emiko.Models
{
    public class File
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private byte[] content;

        public byte[] Content
        {
            get { return content; }
            set { content = value; }
        }

        public String contentType = "application/docx";

        public File()
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "log.txt";
            name = "log.txt";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(path).Length;
            content = br.ReadBytes((int)numBytes);
            fs.Close();
        }
    }
}