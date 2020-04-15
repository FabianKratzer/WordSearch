
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;

using Microsoft.Win32;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WordSearch
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = "";
        private WordFileReader docReader;

        private Dictionary<string, WordFile> allWordFiles = new Dictionary<string, WordFile>();
        private Dictionary<string, WordFile> filteredWordFiles = new Dictionary<string, WordFile>();

        private List<string> keyWords = new List<string>();

        private List<PropertyInfo> propsOfWordFile = new List<PropertyInfo>();
        private List<string> propNames = new List<string>();
        private GridView grdView = new GridView();
        private List<ViewObject> listItems = new List<ViewObject>();

        public MainWindow()
        {
            InitializeComponent();
            docReader = new WordFileReader(this);

            propsOfWordFile = typeof(WordFile).GetProperties().ToList();

            foreach (var prop in propsOfWordFile)
            {
                propNames.Add(prop.Name);
            }

            //Load Gridview
            UpdateView(new Dictionary<string, WordFile>());
            //statusbar
            setStatus("Ready!");
        }

        private void button_browse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.path = folderBrowserDialog1.SelectedPath;
                textBox_path.Text = this.path;
            }

            // searching files
            setStatus("searching files!");
            string[] filePaths = Directory.GetFiles(this.path, "*.docx",SearchOption.AllDirectories);
            setStatus("found " + filePaths.Count() + " *.docx files!");
            label_results.Content = "found "+filePaths.Count()+ " *.docx files!";
             
            foreach (string p in filePaths)
            {
                string filename = System.IO.Path.GetFileName(p);
                string subpath = p.Replace(path, "");
                subpath = subpath.Replace(filename, "");
                allWordFiles.Add(p,new WordFile(subpath, filename, ""));
            }
            UpdateView(allWordFiles);


            // reading docx files
            setStatus("reading " + filePaths.Count()+" *.docx files!");
            label_results.Content = "reading " + filePaths.Count() + " *.docx files!";
            
            foreach (var entry in allWordFiles)
            {

                // await Task.Run(() => );
                entry.Value.content = docReader.readContent(entry.Key.ToString()).Replace("\r\n", "");
            }
            setStatus("readed " + filePaths.Count() + " *.docx files!");
            label_results.Content = "readed " + filePaths.Count() + " *.docx files!";
            UpdateView(allWordFiles);

        }


        public void UpdateView(Dictionary<string, WordFile> files)
        {
            filteredWordFiles = files;

            // item source
            listItems.Clear();

            //Erzeuge Listobjekte
            foreach (var item in files)
            {
                ViewObject newObject = new ViewObject();
                newObject.AddProperties(propsOfWordFile);
                foreach (PropertyInfo prop in propsOfWordFile)
                {
                    newObject.UpdateProperty(prop.Name, prop.GetValue(item.Value) as string);
                }
                listItems.Add(newObject);
            }
            listView1.ItemsSource = listItems;


            //columns
            grdView.Columns.Clear();
            foreach (string prop in propNames)
            {
                GridViewColumn Column = new GridViewColumn();
                //Column.DisplayMemberBinding = new Binding("_propertyValues["+prop+"]");

                DataTemplate templateID = new DataTemplate();
                FrameworkElementFactory factoryID = new FrameworkElementFactory(typeof(TextBlock));
                //set the Textblock's HorizontalAlignment property to Right
                factoryID.SetValue(TextBlock.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Left);
                factoryID.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                factoryID.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("_propertyValues[" + prop + "]"));

                templateID.VisualTree = factoryID;

                Column.CellTemplate = templateID;

                Column.Header = prop;
                Column.Width = 100;
                grdView.Columns.Add(Column);
            }
 
            listView1.View = grdView;
            //update ListItems
            listView1.Items.Refresh();
        }


        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_filter_Click(object sender, RoutedEventArgs e)
        {
            keyWords.Clear();
            string query = textBox_search.Text;
            keyWords.AddRange(query.Split(' '));
            //keyWords.Add(query);

            Dictionary<string,WordFile> results = new Dictionary<string, WordFile>();
            foreach(var entry in allWordFiles)
            {
                int hits = 0;
                foreach(string keyword in keyWords) {
                    if(entry.Value.content.Contains(keyword))
                    {
                        hits++;
                    }
                }
                if(hits == keyWords.Count())
                {
                    results.Add(entry.Key,entry.Value);
                }
            }
            filteredWordFiles = results;
            UpdateView(filteredWordFiles);

        }

        private void button_reset_Click(object sender, RoutedEventArgs e)
        {
            textBox_search.Text = "";
            filteredWordFiles = allWordFiles;
            UpdateView(allWordFiles);
        }


        //update status
        public void setStatus(string status)
        {
            if (lb_status.Dispatcher.CheckAccess())
            {
                lb_status.Content = status;
            }
            else
            {
                Action act = () => { lb_status.Content = status; };
                lb_status.Dispatcher.Invoke(act);
            }
        }
    }
}
