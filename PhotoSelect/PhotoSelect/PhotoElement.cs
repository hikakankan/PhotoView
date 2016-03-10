using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel;

namespace PhotoSelect
{
    abstract class PhotoElement : AFolder, INotifyPropertyChanged
    {
        public abstract XmlElement GetXmlElement(XmlDocument doc);
        public abstract PhotoFolder GetParent();
        public abstract PhotoFolder GetParent(PhotoElement element);
        public abstract bool IsSelected { get; set; }
        public abstract bool IsExpanded { get; set; }
        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}
