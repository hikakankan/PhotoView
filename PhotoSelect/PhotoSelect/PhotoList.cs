using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PhotoSelect
{
    class PhotoList : PhotoElement
    {
        private List<PhotoInfo> photoList;
        private ObservableCollection<AFolder> _subFolders;
        private ObservableCollection<string> _imageFiles;
        private string name = "";
        private bool isSelected = false;
        private bool isExpanded = false;

        public override event PropertyChangedEventHandler PropertyChanged;

        public PhotoList()
        {
            photoList = new List<PhotoInfo>();
        }

        public void Add(string path)
        {
            photoList.Add(new PhotoInfo(path));
        }

        public void Add(XmlElement element)
        {
            photoList.Add(new PhotoInfo(element));
        }

        private Nullable<DateTime> minDate()
        {
            Nullable<DateTime> min = null;
            foreach ( PhotoInfo photo in photoList )
            {
                if ( min == null || photo.Date != null && photo.Date < min )
                {
                    min = photo.Date;
                }
            }
            return min;
        }

        private Nullable<DateTime> maxDate()
        {
            Nullable<DateTime> max = null;
            foreach ( PhotoInfo photo in photoList )
            {
                if ( max == null || photo.Date != null && photo.Date > max )
                {
                    max = photo.Date;
                }
            }
            return max;
        }

        private string DateString
        {
            get
            {
                Nullable<DateTime> min = minDate();
                Nullable<DateTime> max = maxDate();
                string format = "yyyy年M月d日";
                if ( min == null || max == null )
                {
                    // 日付が入っていない
                    return DateTime.Now.ToString(format);
                }
                else
                {
                    string smin = min.Value.ToString(format);
                    string smax = max.Value.ToString(format);
                    if ( smin == smax )
                    {
                        // 日付が全部同じ
                        return smin;
                    }
                    else
                    {
                        // 日付が複数
                        return smin + " - " + smax;
                    }
                }
            }
        }

        public override XmlElement GetXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement("photolist");
            element.SetAttribute("name", name);
            foreach ( PhotoInfo photo in photoList )
            {
                element.AppendChild(photo.GetXmlElement(doc));
            }
            return element;
        }

        public override string Name
        {
            get
            {
                if ( name == "" )
                {
                    return DateString;
                }
                else
                {
                    return name;
                }
            }
            set
            {
                if ( value == null || value == "" )
                {
                    name = DateString;
                }
                else
                {
                    name = value;
                }
                if ( PropertyChanged != null )
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public override bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                if ( PropertyChanged != null )
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public override bool IsExpanded
        {
            get
            {
                return isExpanded;
            }

            set
            {
                isExpanded = value;
                if ( PropertyChanged != null )
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
                }
            }
        }

        public override ObservableCollection<string> ImageFiles
        {
            get
            {
                if ( this._imageFiles == null )
                {
                    this._imageFiles = new ObservableCollection<string>();

                    foreach ( PhotoInfo photo in photoList )
                    {
                        _imageFiles.Add(photo.Path);
                    }
                }

                return this._imageFiles;
            }
        }

        public override ObservableCollection<AFolder> SubFolders
        {
            get
            {
                if ( this._subFolders == null )
                {
                    this._subFolders = new ObservableCollection<AFolder>();
                }

                return this._subFolders;
            }
        }

        public override PhotoFolder GetParent()
        {
            return null;
        }

        public override PhotoFolder GetParent(PhotoElement element)
        {
            return null;
        }
    }
}
