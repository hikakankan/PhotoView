using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;

namespace PhotoList
{
    class Folder
    {
        private DirectoryInfo _folder;
        //private ObservableCollection<AFolder> _subFolders;
        //private ObservableCollection<FileInfo> _files;
        //private ObservableCollection<string> _imageFiles;
        //private ObservableCollection<Image> _images;

        private const int frameHeight = 240;

        private static Size adjustToFrameHight(Size originalSize, int frameHeight)
        {
            double width = originalSize.Width * frameHeight / originalSize.Height;
            return new Size(width, frameHeight);
        }

        private static BitmapImage loadImage(string filename, int height)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.DecodePixelHeight = height;
            bitmapImage.UriSource = new Uri(filename);
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        public static Task<BitmapImage> backLoadImage(string filename, int height)
        {
            return Task.Run(() =>
            {
                return loadImage(filename, height);
            });
        }

        //private async void asyncLoadImage(string filename, int height)
        //{
        //    BitmapImage bitmapImage = await backLoadImage(filename, height);
        //    Image image = new Image();
        //    image.Source = bitmapImage;
        //    image.Width = adjustToFrameHight(new Size(bitmapImage.PixelWidth, bitmapImage.PixelHeight), frameHeight).Width;
        //    image.Height = frameHeight;
        //    this._images.Add(image);
        //}

        //public Folder()
        //{
        //    this.FullPath = @"c:\";
        //}

        public Folder(string path)
        {
            this.FullPath = path;
        }

        public string Name
        {
            get
            {
                return this._folder.Name;
            }
            set
            {
            }
        }

        public string FullPath
        {
            get
            {
                return this._folder.FullName;
            }

            set
            {
                if ( Directory.Exists(value) )
                {
                    this._folder = new DirectoryInfo(value);
                }
                else
                {
                    throw new ArgumentException("must exist", "fullPath");
                }
            }
        }

        public IEnumerable<FileInfo> Files
        {
            get
            {
                return this._folder.GetFiles();
            }
        }

        private string[] imageExtensions = { ".jpg", ".JPG" };

        private bool isImageFile(FileInfo file)
        {
            return imageExtensions.Contains(file.Extension);
        }

        public IEnumerable<string> ImageFiles
        {
            get
            {
                FileInfo[] fi = this._folder.GetFiles();

                for ( int i = 0; i < fi.Length; i++ )
                {
                    if ( isImageFile(fi[i]) )
                    {
                        yield return fi[i].FullName;
                    }
                }
            }
        }

        //public ObservableCollection<Image> Images
        //{
        //    get
        //    {
        //        if ( this._images == null )
        //        {
        //            this._images = new ObservableCollection<Image>();

        //            FileInfo[] fi = this._folder.GetFiles();

        //            for ( int i = 0; i < fi.Length; i++ )
        //            {
        //                if ( isImageFile(fi[i]) )
        //                {
        //                    asyncLoadImage(fi[i].FullName, frameHeight);
        //                }
        //            }
        //        }

        //        return this._images;
        //    }
        //}

        public IEnumerable<Folder> SubFolders
        {
            get
            {
                DirectoryInfo[] di = this._folder.GetDirectories();

                for ( int i = 0; i < di.Length; i++ )
                {
                    yield return new Folder(di[i].FullName);
                }
            }
        }
    }
}
