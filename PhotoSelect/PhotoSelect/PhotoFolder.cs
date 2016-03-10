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
    class PhotoFolder : PhotoElement
    {
        private string name = "フォルダ";
        private ObservableCollection<AFolder> elementList = new ObservableCollection<AFolder>();
        private ObservableCollection<string> _imageFiles;
        private bool isSelected = false;
        private bool isExpanded = false;

        public override event PropertyChangedEventHandler PropertyChanged;

        public void Add(PhotoElement photoElement)
        {
            elementList.Add(photoElement);
        }

        public override XmlElement GetXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement("photofolder");
            element.SetAttribute("name", name);
            foreach ( PhotoElement pe in elementList )
            {
                element.AppendChild(pe.GetXmlElement(doc));
            }
            return element;
        }

        public override string Name
        {
            get
            {
                return name;
            }
            set
            {
                if ( value == null || value == "" )
                {
                    name = "フォルダ";
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
                }

                return this._imageFiles;
            }
        }

        public override ObservableCollection<AFolder> SubFolders
        {
            get
            {
                if ( this.elementList == null )
                {
                    this.elementList = new ObservableCollection<AFolder>(elementList);
                }

                return this.elementList;
            }
        }

        private bool containsSelected()
        {
            // SubFolders.Anyがない！ SubFolders.Any((PhotoFolder element) => element.isSelected) の代わり
            foreach ( PhotoElement element in SubFolders )
            {
                if ( element.IsSelected )
                {
                    return true;
                }
            }
            return false;
        }

        private int getSelectedIndex()
        {
            for ( int i = 0; i < SubFolders.Count; i++ )
            {
                if ( ((PhotoElement)SubFolders[i]).IsSelected )
                {
                    return i;
                }
            }
            return -1;
        }

        public override PhotoFolder GetParent()
        {
            if ( containsSelected() )
            {
                return this;
            }
            foreach ( AFolder folder in SubFolders )
            {
                PhotoFolder parent = ((PhotoElement)folder).GetParent();
                if ( parent != null )
                {
                    return parent;
                }
            }
            return null;
        }

        public override PhotoFolder GetParent(PhotoElement element)
        {
            if ( SubFolders.Contains(element) )
            {
                return this;
            }
            foreach ( AFolder folder in SubFolders )
            {
                PhotoFolder parent = ((PhotoElement)folder).GetParent(element);
                if ( parent != null )
                {
                    return parent;
                }
            }
            return null;
        }

        public void MoveUp()
        {
            int index = getSelectedIndex();
            if ( index >= 1 )
            {
                PhotoElement element = (PhotoElement)SubFolders[index];
                SubFolders.RemoveAt(index);
                SubFolders.Insert(index - 1, element);
            }
        }

        public void MoveDown()
        {
            int index = getSelectedIndex();
            if ( index >= 0 && index < SubFolders.Count - 1 )
            {
                PhotoElement element = (PhotoElement)SubFolders[index];
                SubFolders.RemoveAt(index);
                SubFolders.Insert(index + 1, element);
            }
        }

        /// <summary>
        /// 上位のフォルダのこのフォルダのすぐ上に移動
        /// </summary>
        /// <param name="parent">上位のフォルダ</param>
        public void MoveHigher(PhotoFolder parent)
        {
            int index = getSelectedIndex();
            if ( index >= 0 )
            {
                int index_of_this = parent.SubFolders.IndexOf(this);
                if ( index_of_this >= 0 )
                {
                    PhotoElement element = (PhotoElement)SubFolders[index];
                    SubFolders.RemoveAt(index);
                    parent.SubFolders.Insert(index_of_this, element);
                }
            }
        }

        /// <summary>
        /// 下位のフォルダの最初に移動
        /// </summary>
        public void MoveLower()
        {
            int index = getSelectedIndex();
            if ( index >= 0 && index < SubFolders.Count - 1 )
            {
                if ( SubFolders[index + 1] is PhotoFolder )
                {
                    // 下の要素はフォルダ
                    PhotoElement element = (PhotoElement)SubFolders[index];
                    SubFolders.RemoveAt(index);
                    PhotoFolder folder = (PhotoFolder)SubFolders[index];
                    folder.SubFolders.Insert(0, element);
                    if ( !folder.IsExpanded )
                    {
                        folder.IsExpanded = true;
                    }
                }
            }
        }

        /// <summary>
        /// 指定した要素の下にリストを追加
        /// </summary>
        /// <param name="newElement"></param>
        public void AddElement(PhotoElement newElement)
        {
            int index = getSelectedIndex();
            if ( index >= 0 )
            {
                SubFolders.Insert(index + 1, newElement);
            }
        }

        /// <summary>
        /// 指定した要素の下に空のフォルダを追加
        /// </summary>
        public void AddFolder()
        {
            int index = getSelectedIndex();
            if ( index >= 0 )
            {
                SubFolders.Insert(index + 1, new PhotoFolder());
            }
        }

        public void DeleteElement()
        {
            int index = getSelectedIndex();
            if ( index >= 0 )
            {
                SubFolders.RemoveAt(index);
            }
            if ( index < SubFolders.Count )
            {
                ((PhotoElement)SubFolders[index]).IsSelected = true;
            }
            else if ( SubFolders.Count > 0 )
            {
                // 最後の要素を削除したときは最後の要素を選択
                ((PhotoElement)SubFolders[SubFolders.Count - 1]).IsSelected = true;
            }
            else
            {
                // 要素がなくなったときは上位のフォルダを選択
                IsSelected = true;
            }
        }

        public void ReplaceElement(PhotoElement element, PhotoElement newElement)
        {
            int index = SubFolders.IndexOf(element);
            if ( index >= 0 )
            {
                SubFolders.RemoveAt(index);
                SubFolders.Insert(index, newElement);
            }
        }
    }
}
