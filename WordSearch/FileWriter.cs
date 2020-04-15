using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSearch
{
    public class FileWriter
    {
        private MainWindow window;

        //DateTime.Now.ToString();
        public FileWriter(MainWindow window)
        {
            this.window = window;
        }

        public bool createDirectory(string path)
        {
            string txtPath = path + @"\txt";
            try
            {
                if (Directory.Exists(txtPath))
                {
                    Console.WriteLine("That path exists already.");
                    return false;
                }
                DirectoryInfo di = Directory.CreateDirectory(txtPath);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(txtPath));
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error creating directory: "+ e.ToString());
                return false;
            }
        }
        public void writeTextFile(string path, string content)
        {
            try
            {
                System.IO.File.WriteAllText(path, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing file: " + ex.ToString());
            }
        }
    }
}
