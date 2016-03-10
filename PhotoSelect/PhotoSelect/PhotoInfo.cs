using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Xml;

namespace PhotoSelect
{
    class PhotoInfo
    {
        private string path;
        private Size size;
        private Nullable<DateTime> dateTime;
        private int orientation;

        public PhotoInfo(string path)
        {
            this.path = path;
            BitmapFrame bitmapFrame = BitmapFrame.Create(new Uri(path));
            BitmapMetadata metaData = (BitmapMetadata)bitmapFrame.Metadata;

            uint width = 0;
            object w = metaData.GetQuery(@"/app1/ifd/exif/subifd:{uint=40962}");
            if ( w != null )
            {
                width = (uint)w;
            }

            uint height = 0;
            object h = metaData.GetQuery(@"/app1/ifd/exif/subifd:{uint=40963}");
            if ( h != null )
            {
                height = (uint)h;
            }

            size = new Size(width, height);

            orientation = 1;
            object o = metaData.GetQuery(@"/app1/ifd/exif:{uint=274}");
            if ( o != null )
            {
                orientation = (ushort)o;
            }

            object d = metaData.GetQuery(@"/app1/ifd/exif/subifd:{uint=36867}");
            if ( d == null )
            {
                dateTime = null;
            }
            else
            {
                string dateString = (string)d;
                if ( Regex.IsMatch(dateString, @"^\d+\:\d+\:\d+\s+\d+\:\d+\:\d+$") )
                {
                    Match m = Regex.Match(dateString, @"^(\d+)\:(\d+)\:(\d+)\s+(\d+)\:(\d+)\:(\d+)$");
                    dateTime = new DateTime(Int32.Parse(m.Groups[1].Value), Int32.Parse(m.Groups[2].Value), Int32.Parse(m.Groups[3].Value), Int32.Parse(m.Groups[4].Value), Int32.Parse(m.Groups[5].Value), Int32.Parse(m.Groups[6].Value));
                }
                else
                {
                    dateTime = null;
                }

            }
        }

        public PhotoInfo(XmlElement element)
        {
            path = getElementText(element, "path");
            int width = getElementInt(element, "width");
            int height = getElementInt(element, "height");
            size = new Size(width, height);
            orientation = getElementInt(element, "orientation");
            dateTime = getElementDate(element, "date");
        }

        public string Path
        {
            get
            {
                return path;
            }
        }

        public Size Size
        {
            get
            {
                return size;
            }
        }

        public Nullable<DateTime> Date
        {
            get
            {
                return dateTime;
            }
        }

        public int Orientation
        {
            get
            {
                return orientation;
            }
        }

        private void AddChild(XmlDocument doc, XmlElement parent, string name, string val)
        {
            XmlElement child = doc.CreateElement(name);
            XmlText text = doc.CreateTextNode(val);
            child.AppendChild(text);
            parent.AppendChild(child);
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement("photo");
            AddChild(doc, element, "path", path);
            AddChild(doc, element, "width", size.Width.ToString());
            AddChild(doc, element, "height", size.Height.ToString());
            AddChild(doc, element, "orientation", orientation.ToString());
            if ( dateTime != null )
            {
                AddChild(doc, element, "date", dateTime.Value.ToString("yyyy年M月d日 H時m分s秒"));
            }
            return element;
        }

        private string getElementText(XmlElement element, string name)
        {
            foreach ( XmlNode node in element.ChildNodes )
            {
                if ( node.NodeType == XmlNodeType.Element && node.Name == name )
                {
                    return node.InnerText;
                }
            }
            return "";
        }

        private int getElementInt(XmlElement element, string name)
        {
            string text = getElementText(element, name);
            try
            {
                return Int32.Parse(text);
            }
            catch ( Exception )
            {
                return 0;
            }
        }

        private DateTime getElementDate(XmlElement element, string name)
        {
            string text = getElementText(element, name);
            if ( Regex.IsMatch(text, @"^(\d+)年(\d+)月(\d+)日\s+(\d+)時(\d+)分(\d+)秒$") )
            {
                Match m = Regex.Match(text, @"^(\d+)年(\d+)月(\d+)日\s+(\d+)時(\d+)分(\d+)秒$");
                return new DateTime(Int32.Parse(m.Groups[1].Value), Int32.Parse(m.Groups[2].Value), Int32.Parse(m.Groups[3].Value), Int32.Parse(m.Groups[4].Value), Int32.Parse(m.Groups[5].Value), Int32.Parse(m.Groups[6].Value));
            }
            else
            {
                return DateTime.Now;
            }
        }
    }
}
