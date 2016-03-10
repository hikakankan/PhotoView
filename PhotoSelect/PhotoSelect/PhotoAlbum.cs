using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PhotoSelect
{
    class PhotoAlbum
    {
        private PhotoElement photoElement;

        public PhotoAlbum()
        {
            photoElement = null;
        }

        public PhotoAlbum(PhotoFolder folder)
        {
            photoElement = folder;
        }

        public PhotoAlbum(PhotoList list)
        {
            photoElement = list;
        }

        public PhotoAlbum(string filename)
        {
            Load(filename);
        }

        public PhotoElement Element
        {
            get
            {
                return photoElement;
            }
        }

        private static XmlElement findChild(XmlDocument doc, string name)
        {
            foreach ( XmlNode node in doc.ChildNodes )
            {
                if ( node.NodeType == XmlNodeType.Element && node.Name == name)
                {
                    return (XmlElement)node;
                }
            }
            return null;
        }

        private static PhotoList getPhotoList(XmlElement element)
        {
            PhotoList list = new PhotoList();
            list.Name = element.GetAttribute("name");
            foreach ( XmlNode node in element.ChildNodes )
            {
                if ( node.NodeType == XmlNodeType.Element )
                {
                    if ( node.Name == "photo" )
                    {
                        list.Add((XmlElement)node);
                    }
                }
            }
            return list;
        }

        private static PhotoFolder getPhotoFolder(XmlElement element)
        {
            PhotoFolder folder = new PhotoFolder();
            folder.Name = element.GetAttribute("name");
            foreach ( XmlNode node in element.ChildNodes )
            {
                if ( node.NodeType == XmlNodeType.Element )
                {
                    PhotoElement pe = getPhotoElement((XmlElement)node);
                    if ( pe != null )
                    {
                        folder.Add(pe);
                    }
                }
            }
            return folder;
        }

        private static PhotoElement getPhotoElement(XmlElement element)
        {
            if ( element.Name == "photofolder" )
            {
                return getPhotoFolder(element);
            }
            else if ( element.Name == "photolist" )
            {
                return getPhotoList(element);
            }
            return null;
        }

        private void Load(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlElement albumElement = findChild(doc, "photoalbum");
            if ( albumElement == null )
            {
                return;
            }
            if ( albumElement.FirstChild != null && albumElement.FirstChild.NodeType == XmlNodeType.Element )
            {
                photoElement = getPhotoElement((XmlElement)albumElement.FirstChild);
            }
        }

        public void Save(string filename)
        {
            if ( photoElement == null )
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            XmlElement albumnode = doc.CreateElement("photoalbum");
            XmlElement child = photoElement.GetXmlElement(doc);
            albumnode.AppendChild(child);
            doc.AppendChild(albumnode);
            doc.Save(filename);
        }

        private PhotoFolder GetParent()
        {
            if ( photoElement == null )
            {
                return null;
            }
            return photoElement.GetParent();
        }

        private PhotoFolder GetParent(PhotoElement element)
        {
            if ( photoElement == null )
            {
                return null;
            }
            return photoElement.GetParent(element);
        }

        public void MoveUp()
        {
            PhotoFolder parent = GetParent();
            if ( parent == null )
            {
                return;
            }
            parent.MoveUp();
        }

        public void MoveDown()
        {
            PhotoFolder parent = GetParent();
            if ( parent == null )
            {
                return;
            }
            parent.MoveDown();
        }

        public void MoveHigher()
        {
            PhotoFolder parent = GetParent();
            if ( parent == null )
            {
                return;
            }
            PhotoFolder parent_of_parent = GetParent(parent);
            if ( parent_of_parent == null )
            {
                return;
            }
            parent.MoveHigher(parent_of_parent);
        }

        public void MoveLower()
        {
            PhotoFolder parent = GetParent();
            if ( parent == null )
            {
                return;
            }
            parent.MoveLower();
        }

        /// <summary>
        /// 選択された要素の下にリストを追加
        /// </summary>
        /// <param name="newElement"></param>
        public void AddElement(PhotoElement newElement)
        {
            PhotoFolder parent = GetParent();
            if ( parent == null )
            {
                return;
            }
            parent.AddElement(newElement);
            newElement.IsSelected = true;
        }

        /// <summary>
        /// 最上位にフォルダを追加
        /// </summary>
        public void AddFolderRoot()
        {
            PhotoFolder root = new PhotoFolder();
            root.Add(photoElement);
            photoElement = root;
        }

        public void DeleteElement()
        {
            PhotoFolder parent = GetParent();
            if ( parent == null )
            {
                return;
            }
            parent.DeleteElement();
        }
    }
}
