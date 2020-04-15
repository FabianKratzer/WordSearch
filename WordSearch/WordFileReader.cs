using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace WordSearch
{
    public class WordFileReader
    {
        private MainWindow window;

        public WordFileReader(MainWindow window)
        {
            this.window = window;
        }

        public void readAllDocFiles(Dictionary<string, WordFile> files)
        {
            foreach (var entry in files)
            {
                entry.Value.content = readContent(entry.Key.ToString()).Replace("\r\n", "");
            }
            window.UpdateView(files);

        }
        public string readContent(string path)  
        {
            try
            {
                IDataObject Data1;

                Word.Application wordApplication = new Word.Application();
                wordApplication.Visible = false;

                Word.Document wordDocument = new Word.Document();
                wordDocument = wordApplication.Documents.Open(path);

                //alles auswählen
                wordDocument.ActiveWindow.Selection.WholeStory();
                //kopieren
                wordDocument.ActiveWindow.Selection.Copy();
                Data1 = Clipboard.GetDataObject();
                String str = Data1.GetData(DataFormats.Text).ToString();
                //Word Document und Anwendung schließen  
                wordDocument.Close();
                wordApplication.Quit();

                return str;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading docx file: " + ex.ToString());
                return "ERROR";
            }
           
        }
    }
}
