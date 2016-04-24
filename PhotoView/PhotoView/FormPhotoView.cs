using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Controls;

namespace PhotoView
{
    public partial class FormPhotoView : Form
    {
        public FormPhotoView()
        {
            InitializeComponent();
            initRootListBox();
        }

        private IEnumerable<Tuple<int, string>> profilesToIdsAndTitles(IEnumerable<string> profiles)
        {
            string idtbl = GetPrivateProfileString(profiles, "DATA", "IDTBL", "失敗");
            if ( idtbl != "失敗" && idtbl != "" )
            {
                string[] nums = idtbl.Split(new char[] { ',' });
                foreach ( string strNum in nums )
                {
                    int n = Int32.Parse(strNum);
                    string id = "ID" + n.ToString("d5");
                    string title = GetPrivateProfileString(profiles, id, "TITLE", "失敗");
                    if ( title != "失敗" )
                    {
                        yield return new Tuple<int, string>(n, title);
                    }
                }
            }
        }

        private void addToListBox(ListBox lb, IEnumerable<Tuple<int, string>> items)
        {
            lb.Items.Clear();
            foreach ( Tuple<int, string> item in items )
            {
                lb.Items.Add(item.Item2);
            }
        }

        private string GetPrivateProfileString(IEnumerable<string> profiles, string section, string key, string defaultValue)
        {
            string currentSection = "";
            string currentKey = "";
            foreach ( string line in profiles )
            {
                if ( Regex.IsMatch(line, @"^\[.*\]$") )
                {
                    currentSection = Regex.Match(line, @"^\[(.*)\]$").Groups[1].Value;
                }
                else if ( Regex.IsMatch(line, @"^.*\=.*$") )
                {
                    currentKey = Regex.Match(line, @"^(.*)\=(.*)$").Groups[1].Value;
                    if ( currentSection == section && currentKey == key )
                    {
                        return Regex.Match(line, @"^(.*)\=(.*)$").Groups[2].Value;
                    }
                }
            }
            return defaultValue;
        }

        private IEnumerable<string> readLines(string fileName)
        {
            try
            {
                StreamReader sr = new StreamReader(fileName, Encoding.Default);

                List<string> lines = new List<string>();
                while ( !sr.EndOfStream )
                {
                    lines.Add(sr.ReadLine());
                }

                sr.Close();

                return lines;
            }
            catch ( Exception ex )
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\furuta\photo4\たっぷりデジカメ";
            openFileDialog1.Filter = "ini files (*.ini)|*.ini|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if ( openFileDialog1.ShowDialog() == DialogResult.OK )
            {
                IEnumerable<string> profiles = readLines(openFileDialog1.FileName);
                categoryFolder = Path.GetDirectoryName(openFileDialog1.FileName);
                categories = profilesToIdsAndTitles(profiles);
                addToListBox(listBoxCategory, categories);
            }
        }

        private List<string> rootFolders;

        private void initRootListBox()
        {
            rootFolders = new List<string>();
            rootFolders.Add(@"J:\furuta\photo");
            rootFolders.Add(@"J:\furuta\photo2");
            rootFolders.Add(@"J:\furuta\photo3");
            rootFolders.Add(@"J:\furuta\photo4");
            foreach ( string folder in rootFolders )
            {
                listBoxRoot.Items.Add(folder);
            }
        }

        private void listBoxRoot_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxRoot.SelectedIndex;
            // タイミングによって index が -1 になるのを回避
            if ( index >= 0 )
            {
                string inifile = rootFolders[index] + @"\たっぷりデジカメ\__bookbox.ini";
                IEnumerable<string> profiles = readLines(inifile);
                categoryFolder = Path.GetDirectoryName(inifile);
                categories = profilesToIdsAndTitles(profiles);
                addToListBox(listBoxCategory, categories);
            }
        }

        private string categoryFolder = "";
        private IEnumerable<Tuple<int, string>> categories = null;

        private void listBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxCategory.SelectedIndex;
            // タイミングによって index が -1 になるのを回避
            if ( index >= 0 )
            {
                if ( categories != null )
                {
                    int id = categories.ElementAt(index).Item1;
                    string inifile = categoryFolder + @"\a" + id.ToString("d5") + @"\__bookbox.ini";
                    albumFolder = Path.GetDirectoryName(inifile);
                    IEnumerable<string> lines = readLines(inifile);
                    if ( lines != null )
                    {
                        albums = profilesToIdsAndTitles(lines);
                        addToListBox(listBoxAlbum, albums);
                    }
                }
            }
        }

        private string albumFolder = "";
        private IEnumerable<Tuple<int, string>> albums = null;

        private void listBoxAlbum_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxAlbum.SelectedIndex;
            // タイミングによって index が -1 になるのを回避
            if ( index >= 0 )
            {
                if ( albums != null )
                {
                    int id = albums.ElementAt(index).Item1;
                    string photoFolder = albumFolder + @"\a" + id.ToString("d5") + @"\DCIM\100FFIMG";
                    explorerBrowserContents.Navigate(ShellObject.FromParsingName(photoFolder));
                }
            }
        }

        private void FormPhotoView_Load(object sender, EventArgs e)
        {
            explorerBrowserContents.NavigationOptions.PaneVisibility.Navigation = PaneVisibilityState.Hide;
            explorerBrowserContents.Navigate((ShellObject)KnownFolders.Desktop);
        }
    }
}
