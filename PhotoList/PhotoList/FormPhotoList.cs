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

namespace PhotoList
{
    public partial class FormPhotoList : Form
    {
        public FormPhotoList()
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

        private string GetPrivateProfileString2(IEnumerable<string> profiles, string section, string key, string defaultValue)
        {
            // Picasa用 StartsWithを使ったバージョン
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
                    if ( currentSection.StartsWith(section) && currentKey == key )
                    {
                        return Regex.Match(line, @"^(.*)\=(.*)$").Groups[2].Value;
                    }
                }
            }
            return defaultValue;
        }

        private IEnumerable<string> readLines(string fileName, Encoding encoding)
        {
            try
            {
                List<string> lines = new List<string>();
                if ( File.Exists(fileName) )
                {
                    StreamReader sr = new StreamReader(fileName, encoding);

                    while ( !sr.EndOfStream )
                    {
                        lines.Add(sr.ReadLine());
                    }

                    sr.Close();
                }

                return lines;
            }
            catch ( Exception ex )
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        private IEnumerable<string> readLines(string fileName)
        {
            return readLines(fileName, Encoding.Default);
        }

        private List<string> rootFolders;
        private List<string> rootFoldersPicasa;

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

            rootFoldersPicasa = new List<string>();
            rootFoldersPicasa.Add(@"J:\furuta\photo-new");
            foreach ( string folder in rootFoldersPicasa )
            {
                listBoxPicasaRoot.Items.Add(folder);
            }
        }

        private void listBoxRoot_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxRoot.SelectedIndex;
            // タイミングによって index が -1 になるのを回避
            if ( index >= 0 )
            {
                List<string> infoList = new List<string>();
                string inifile = rootFolders[index] + @"\たっぷりデジカメ\__bookbox.ini";
                IEnumerable<string> profiles = readLines(inifile);
                string categoryFolder = Path.GetDirectoryName(inifile);
                IEnumerable<Tuple<int, string>> categories = profilesToIdsAndTitles(profiles);
                foreach ( Tuple<int, string> category in categories )
                {
                    infoList.Add(category.Item2);
                    infoList.AddRange(scanCategory(category, categoryFolder));
                }
                showList(infoList);
            }
        }

        private List<string> scanCategory(Tuple<int, string> category, string categoryFolder)
        {
            List<string> infoList = new List<string>();
            int id = category.Item1;
            string inifile = categoryFolder + @"\a" + id.ToString("d5") + @"\__bookbox.ini";
            string albumFolder = Path.GetDirectoryName(inifile);
            IEnumerable<string> lines = readLines(inifile);
            if ( lines != null )
            {
                IEnumerable<Tuple<int, string>> albums = profilesToIdsAndTitles(lines);
                foreach ( Tuple<int, string> album in albums )
                {
                    scanAlbum(album, albumFolder);
                    infoList.Add(scanAlbum(album, albumFolder));
                }
            }
            return infoList;
        }

        private string scanAlbum(Tuple<int, string> album, string albumFolder)
        {
            int id = album.Item1;
            string photoFolder = albumFolder + @"\a" + id.ToString("d5") + @"\DCIM\100FFIMG";
            string inifile = photoFolder + @"\.picasa.ini";
            return addProfiles(album.Item2, inifile);
        }

        private string profilesToAlbumName(IEnumerable<string> profiles)
        {
            return GetPrivateProfileString2(profiles, ".album:", "name", "");
        }

        private string profilesToAlbumDate(IEnumerable<string> profiles)
        {
            return GetPrivateProfileString2(profiles, ".album:", "date", "");
        }

        private string addProfiles(string profile, string inifile)
        {
            IEnumerable<string> lines = readLines(inifile, Encoding.UTF8);
            if ( lines != null )
            {
                string albumName = profilesToAlbumName(lines);
                string albumDate = profilesToAlbumDate(lines);
                profile += "\t"+ albumName + "\t" + albumDate;
            }
            return profile;
        }

        private void listBoxPicasaRoot_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> infoList = new List<string>();
            int index = listBoxPicasaRoot.SelectedIndex;
            // タイミングによって index が -1 になるのを回避
            if ( index >= 0 )
            {
                Folder parent = new Folder(rootFoldersPicasa[index]);
                foreach ( Folder folder in parent.SubFolders )
                {
                    string inifile = folder.FullPath + @"\.picasa.ini";
                    infoList.Add(addProfiles(folder.Name, inifile));
                }
            }
            showList(infoList);
        }

        private void showList(List<string> infoList)
        {
            string text = "";
            foreach ( string info in infoList )
            {
                text += info + "\r\n";
            }
            textBoxAlbumList.Text = text;
        }
    }
}
