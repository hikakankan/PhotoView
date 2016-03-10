using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PhotoSelect
{
    class Settings
    {
        private string settings_file_name; // 設定ファイル名

        public Settings(string settings_file)
        {
            settings_file_name = settings_file;
        }

        /// <summary>
        /// XMLのセクションの対応するエレメントを取得する
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="parent">エレメントを探す親のノード</param>
        /// <param name="doc">XMLのドキュメント</param>
        /// <param name="create">取得できなかったときは作成する</param>
        /// <returns>セクションに対応するノード</returns>
        private XmlElement get_section(string section, XmlNode parent, XmlDocument doc, bool create)
        {
            if ( parent.HasChildNodes )
            {
                foreach ( XmlNode node in parent.ChildNodes )
                {
                    if ( node.NodeType == XmlNodeType.Element && node.Name == section )
                    {
                        return (XmlElement)node;
                    }
                }
            }
            if ( create )
            {
                XmlElement child = doc.CreateElement(section);
                parent.AppendChild(child);
                return child;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="key">設定のキー</param>
        /// <param name="val">設定のデフォルトの値</param>
        /// <param name="doc">XMLのドキュメント</param>
        private void get_setting_string(string section, string key, ref string val, XmlDocument doc)
        {
            try
            {
                XmlElement conf = doc.DocumentElement;
                if ( conf != null )
                {
                    XmlElement sec = get_section(section, conf, doc, false);
                    if ( sec != null )
                    {
                        val = sec.GetAttribute(key);
                    }
                }
            }
            catch ( Exception )
            {
            }
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="key">設定のキー</param>
        /// <param name="val">設定のデフォルトの値</param>
        /// <param name="doc">XMLのドキュメント</param>
        /// <returns>設定の値</returns>
        private string get_setting_string(string section, string key, string val, XmlDocument doc)
        {
            try
            {
                string setting = val;
                get_setting_string(section, key, ref setting, doc);
                return setting;
            }
            catch ( Exception )
            {
                return val;
            }
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="key">設定のキー</param>
        /// <param name="val">設定のデフォルトの値</param>
        /// <param name="doc">XMLのドキュメント</param>
        /// <returns>設定の値</returns>
        private int get_setting_integer(string section, string key, int val, XmlDocument doc)
        {
            try
            {
                string setting = "";
                get_setting_string(section, key, ref setting, doc);
                if ( setting != "" )
                {
                    return Convert.ToInt32(setting);
                }
                return val;
            }
            catch ( Exception )
            {
                return val;
            }
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="key">設定のキー</param>
        /// <param name="val">設定のデフォルトの値</param>
        /// <param name="doc">XMLのドキュメント</param>
        /// <returns>設定の値</returns>
        private bool get_setting_bool(string section, string key, bool val, XmlDocument doc)
        {
            try
            {
                string setting = "";
                get_setting_string(section, key, ref setting, doc);
                if ( setting != "" )
                {
                    return setting != "f";
                }
                return val;
            }
            catch ( Exception )
            {
                return val;
            }
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="key">設定のキー</param>
        /// <param name="val">設定のデフォルトの値</param>
        /// <param name="doc">XMLのドキュメント</param>
        /// <returns>設定の値</returns>
        private float get_setting_real(string section, string key, float val, XmlDocument doc)
        {
            try
            {
                string setting = "";
                get_setting_string(section, key, ref setting, doc);
                if ( setting != "" )
                {
                    return Convert.ToSingle(setting);
                }
                return val;
            }
            catch ( Exception )
            {
                return val;
            }
        }

        /// <summary>
        /// 設定を書き込む
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="key">設定のキー</param>
        /// <param name="val">設定の値</param>
        /// <param name="doc">XMLのドキュメント</param>
        private void set_setting_string(string section, string key, string val, XmlDocument doc)
        {
            try
            {
                XmlElement conf = doc.DocumentElement;
                if ( conf == null )
                {
                    conf = doc.CreateElement("configuration");
                    doc.AppendChild(conf);
                }
                get_section(section, conf, doc, true).SetAttribute(key, val);
            }
            catch ( Exception )
            {
            }
        }

        /// <summary>
        /// 設定を書き込む
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="key">設定のキー</param>
        /// <param name="val">設定の値</param>
        /// <param name="doc">XMLのドキュメント</param>
        private void set_setting_integer(string section, string key, int val, XmlDocument doc)
        {
            try
            {
                set_setting_string(section, key, Convert.ToString(val), doc);
            }
            catch ( Exception )
            {
            }
        }

        /// <summary>
        /// 設定を書き込む
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="key">設定のキー</param>
        /// <param name="val">設定の値</param>
        /// <param name="doc">XMLのドキュメント</param>
        private void set_setting_bool(string section, string key, bool val, XmlDocument doc)
        {
            try
            {
                if ( val )
                {
                    set_setting_string(section, key, "t", doc);
                }
                else
                {
                    set_setting_string(section, key, "f", doc);
                }
            }
            catch ( Exception )
            {
            }
        }

        /// <summary>
        /// 設定を書き込む
        /// </summary>
        /// <param name="section">設定のセクション</param>
        /// <param name="key">設定のキー</param>
        /// <param name="val">設定の値</param>
        /// <param name="doc">XMLのドキュメント</param>
        private void set_setting_real(string section, string key, float val, XmlDocument doc)
        {
            try
            {
                set_setting_string(section, key, Convert.ToString(val), doc);
            }
            catch ( Exception )
            {
            }
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        /// <param name="path">設定ファイルのパス</param>
        public void load_settings(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlTextReader reader = new XmlTextReader(path);
                doc.Load(reader);
                reader.Close();

                left = get_setting_integer("window", "left", left, doc);
                top = get_setting_integer("window", "top", top, doc);
                width = get_setting_integer("window", "width", width, doc);
                height = get_setting_integer("window", "height", height, doc);
                vertical_splitter = get_setting_integer("window", "verticalsplitter", vertical_splitter, doc);
                horizontal_splitter = get_setting_integer("window", "horizontalsplitter", horizontal_splitter, doc);

                root_folder = get_setting_string("folder", "root", root_folder, doc);
                album_path = get_setting_string("path", "album", album_path, doc);
                //photo_dest_folder = get_setting_string("folder", "dest", photo_dest_folder, doc);
            }
            catch ( Exception )
            {
            }
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        public void load_settings()
        {
            load_settings(settings_file_name);
        }

        private string file_path;

        /// <summary>
        /// ファイルダイアログでファイル名を指定して設定を読み込む
        /// </summary>
        /// <returns>読み込んだときtrue、キャンセルのときfalse</returns>
        public bool load_settings_file()
        {
            try
            {
                if ( file_path == null )
                {
                    file_path = settings_file_name;
                }
                System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                openFileDialog1.Filter = "情報ファイル (*.xci)|*.xci|すべてのファイル (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.FileName = file_path;
                openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
                openFileDialog1.RestoreDirectory = true;
                if ( openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    file_path = openFileDialog1.FileName;
                    load_settings(file_path);
                    return true;
                }
            }
            catch ( Exception )
            {
            }
            return false;
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        /// <param name="path">設定ファイルのパス</param>
        public void save_settings(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                set_setting_integer("window", "left", left, doc);
                set_setting_integer("window", "top", top, doc);
                set_setting_integer("window", "width", width, doc);
                set_setting_integer("window", "height", height, doc);
                set_setting_integer("window", "verticalsplitter", vertical_splitter, doc);
                set_setting_integer("window", "horizontalsplitter", horizontal_splitter, doc);

                set_setting_string("folder", "root", root_folder, doc);
                set_setting_string("path", "album", album_path, doc);
                //set_setting_string("folder", "dest", photo_dest_folder, doc);

                XmlTextWriter writer = new XmlTextWriter(path, null);
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);
                writer.Close();
            }
            catch ( Exception )
            {
            }
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        public void save_settings()
        {
            save_settings(settings_file_name);
        }

        /// <summary>
        /// ファイルダイアログでファイル名を指定して設定を保存する
        /// </summary>
        public void save_settings_file()
        {
            try
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.DefaultExt = ".txt";
                saveFileDialog1.OverwritePrompt = true;
                saveFileDialog1.Filter = "情報ファイル (*.xci)|*.xci|すべてのファイル (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.FileName = "";
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.InitialDirectory = Environment.CurrentDirectory;
                saveFileDialog1.RestoreDirectory = true;
                if ( saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    file_path = saveFileDialog1.FileName;
                    save_settings(file_path);
                }
            }
            catch ( Exception )
            {
            }
        }

        private int left = 80;
        private int top = 80;
        private int width = 400;
        private int height = 300;
        private int vertical_splitter = 150;
        private int horizontal_splitter = 150;

        /// <summary>
        /// ウィンドウの位置(左)
        /// </summary>
        public int Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
            }
        }

        /// <summary>
        /// ウィンドウの位置(上)
        /// </summary>
        public int Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }

        /// <summary>
        /// ウィンドウの幅
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        /// <summary>
        /// ウィンドウの高さ
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public int VerticalSplitter
        {
            get
            {
                return vertical_splitter;
            }
            set
            {
                vertical_splitter = value;
            }
        }

        public int HorizontalSplitter
        {
            get
            {
                return horizontal_splitter;
            }
            set
            {
                horizontal_splitter = value;
            }
        }

        private int frameHeight = 120;
        private int maxHeight = 960;

        public int FrameHeight
        {
            get
            {
                return frameHeight;
            }
            set
            {
                frameHeight = value;
            }
        }

        public int MaxHeight
        {
            get
            {
                return maxHeight;
            }
            set
            {
                maxHeight = value;
            }
        }

        private int gui_delay = 100;

        public int GUIDelay
        {
            get
            {
                return gui_delay;
            }
            set
            {
                gui_delay = value;
            }
        }

        private string root_folder = @"c:\furuta\picture\";
        private string album_path = "";
        private string photo_dest_folder = "";

        public string RootFolder
        {
            get
            {
                return root_folder;
            }
            set
            {
                root_folder = value;
            }
        }

        public string AlbumPath
        {
            get
            {
                return album_path;
            }
            set
            {
                album_path = value;
            }
        }

        public string PhotoDestFolder
        {
            get
            {
                return photo_dest_folder;
            }
            set
            {
                photo_dest_folder = value;
            }
        }
    }
}
