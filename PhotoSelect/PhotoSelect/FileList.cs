using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace PhotoSelect
{
    class FileList
    {
        private AFolder currentFolder = null;
        private int currentIndex = -1;
        private IEnumerable<string> currentFiles = null;
        private bool[] fileSelected = null;
        private bool[] splitPoints = null;
        private bool listCreated = false;

        public FileList(AFolder currentFolder)
        {
            this.currentFolder = currentFolder;
            currentFiles = currentFolder.ImageFiles;
            fileSelected = new bool[currentFiles.Count()];
            SetSelectedAll(false);
            currentIndex = -1;
            splitPoints = new bool[currentFiles.Count()];
            for ( int i = 0; i < currentFiles.Count(); i++ )
            {
                splitPoints[i] = false;
            }
            listCreated = true;
        }

        public FileList()
        {
        }

        public bool ListCreated
        {
            get
            {
                return listCreated;
            }
        }

        public IEnumerable<string> CurrentFiles
        {
            get
            {
                return currentFiles;
            }
        }

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
            set
            {
                currentIndex = value;
            }
        }

        public string CuurentFile
        {
            get
            {
                if ( !listCreated || currentIndex < 0 )
                {
                    return "";
                }
                return currentFiles.ElementAt(currentIndex);
            }
        }

        public IEnumerable<string> SelectedFiles
        {
            get
            {
                if ( listCreated )
                {
                    for ( int i = 0; i < currentFiles.Count(); i++ )
                    {
                        if ( fileSelected[i] )
                        {
                            yield return currentFiles.ElementAt(i);
                        }
                    }
                }
            }
        }

        public void SetSelectedAll(bool val)
        {
            if ( !listCreated )
            {
                return;
            }
            for ( int i = 0; i < currentFiles.Count(); i++ )
            {
                fileSelected[i] = val;
            }
        }

        public void SelectCurrentItem(bool val)
        {
            if ( !listCreated || currentIndex < 0 )
            {
                return;
            }
            fileSelected[currentIndex] = val;
        }

        public bool CurrentIsSelected()
        {
            if ( !listCreated || currentIndex < 0 )
            {
                return false;
            }
            return fileSelected[currentIndex];
        }

        public bool GoNext(int step)
        {
            if ( !listCreated || currentIndex < 0 )
            {
                return false;
            }
            if ( currentIndex + step >= 0 && currentIndex + step < currentFiles.Count() )
            {
                currentIndex += step;
                return true;
            }
            return false;
        }

        public bool GoNextSelected(int step)
        {
            if ( !listCreated || currentIndex < 0 )
            {
                return false;
            }
            for ( int i = currentIndex + step; i >= 0 && i < currentFiles.Count(); i += step )
            {
                if ( fileSelected[i] )
                {
                    currentIndex = i;
                    return true;
                }
            }
            return false;
        }

        public void SetSpiltCurrentItem(bool val)
        {
            if ( !listCreated || currentIndex < 0 )
            {
                return;
            }
            splitPoints[currentIndex] = val;
        }

        public bool GoNextSplitPoint(int step)
        {
            if ( !listCreated || currentIndex < 0 )
            {
                return false;
            }
            for ( int i = currentIndex + step; i >= 0 && i < currentFiles.Count(); i += step )
            {
                if ( splitPoints[i] )
                {
                    currentIndex = i;
                    return true;
                }
            }
            return false;
        }

        public bool CurrentIsSplitPoint()
        {
            if ( !listCreated || currentIndex < 0 )
            {
                return false;
            }
            return splitPoints[currentIndex];
        }

        public void CopySelectedToFolder(string folder)
        {
            try
            {
                foreach ( string file in SelectedFiles )
                {
                    File.Copy(file, Path.Combine(folder, Path.GetFileName(file)));
                }
            }
            catch ( Exception )
            {
                MessageBox.Show("コピーに失敗しました");
            }
        }

        public IEnumerable<List<string>> SplitParts
        {
            get
            {
                if ( listCreated )
                {
                    List<string> splitPart = new List<string>();
                    for ( int i = 0; i < currentFiles.Count(); i++ )
                    {
                        if ( splitPoints[i] )
                        {
                            yield return splitPart;
                            splitPart = new List<string>();
                        }
                        splitPart.Add(currentFiles.ElementAt(i));
                    }
                    yield return splitPart;
                }
            }
        }
    }
}
