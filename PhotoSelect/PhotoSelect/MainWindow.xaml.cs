using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;

namespace PhotoSelect
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            settings = new Settings(settings_file_name);
            settings.load_settings();
        }

        private const string settings_file_name = "PhotoSelect.psi";
        private Settings settings;

        private FileList fileList = new FileList();

        private static Size adjustToFrameHight(Size originalSize, int frameHeight)
        {
            double width = originalSize.Width * frameHeight / originalSize.Height;
            return new Size(width, frameHeight);
        }

        private static Tuple<TransformedBitmap, Size> loadImage(string filename, int height)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.DecodePixelHeight = height;
            bitmapImage.UriSource = new Uri(filename);
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return getTransformedBitmap(filename, bitmapImage);
        }

        public static Task<Tuple<TransformedBitmap, Size>> backLoadImage(string filename, int height)
        {
            return Task.Run(() =>
            {
                return loadImage(filename, height);
            });
        }

        private static Tuple<TransformedBitmap, Size> getTransformedBitmap(string currentPath, BitmapImage bitmapImage)
        {
            BitmapFrame bitmapFrame = BitmapFrame.Create(new Uri(currentPath));
            BitmapMetadata metaData = (BitmapMetadata)bitmapFrame.Metadata;
            //uint width = (uint)metaData.GetQuery(@"/app1/ifd/exif/subifd:{uint=40962}");
            //uint height = (uint)metaData.GetQuery(@"/app1/ifd/exif/subifd:{uint=40963}");
            //string date = (string)metaData.GetQuery(@"/app1/ifd/exif/subifd:{uint=36867}");

            ushort orientation = 1;
            object o = metaData.GetQuery(@"/app1/ifd/exif:{uint=274}");
            if ( o != null )
            {
                orientation = (ushort)o;
            }

            TransformedBitmap transformedBitmap = new TransformedBitmap();
            transformedBitmap.BeginInit();
            transformedBitmap.Source = bitmapImage;

            Size size;
            switch ( orientation )
            {
                case 1:
                    size = new Size(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
                    break;
                case 2:
                    size = new Size(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
                    break;
                case 3:
                    transformedBitmap.Transform = new RotateTransform(180);
                    size = new Size(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
                    break;
                case 4:
                    size = new Size(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
                    break;
                case 5:
                    size = new Size(bitmapImage.PixelHeight, bitmapImage.PixelWidth);
                    break;
                case 6:
                    transformedBitmap.Transform = new RotateTransform(90);
                    size = new Size(bitmapImage.PixelHeight, bitmapImage.PixelWidth);
                    break;
                case 7:
                    size = new Size(bitmapImage.PixelHeight, bitmapImage.PixelWidth);
                    break;
                case 8:
                    transformedBitmap.Transform = new RotateTransform(270);
                    size = new Size(bitmapImage.PixelHeight, bitmapImage.PixelWidth);
                    break;
                default:
                    size = new Size(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
                    break;
            }

            transformedBitmap.EndInit();
            transformedBitmap.Freeze();

            return new Tuple<TransformedBitmap, Size>(transformedBitmap, size);
        }

        private void addImageToList(Tuple<TransformedBitmap, Size> bitmapAndSize)
        {
            Image image = new Image();
            image.Width = adjustToFrameHight(bitmapAndSize.Item2, settings.FrameHeight).Width;
            image.Height = settings.FrameHeight;
            //image.Width = bitmapAndSize.Item2.Width;
            //image.Height = bitmapAndSize.Item2.Height;
            image.Source = bitmapAndSize.Item1;
            listView.Items.Add(image);

        }

        private async void loadImages(IEnumerable<string> currentFiles, IProgress<Tuple<TransformedBitmap, Size>> pAddImageToList, BackgroundLoader loader)
        {
            foreach ( string file in currentFiles )
            {
                if ( loader.Cancel )
                {
                    break;
                }
                Tuple<TransformedBitmap, Size> bitmapAndSize = loadImage(file, settings.FrameHeight);
                pAddImageToList.Report(bitmapAndSize);
                await Task.Delay(settings.GUIDelay); // GUIの操作ができるようにするためにGUI更新の時間間隔を空ける
            }
        }

        private BackgroundLoader loader = new BackgroundLoader();

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tv = (TreeView)e.Source;
            if ( tv.SelectedItem != null && tv.SelectedItem is AFolder )
            {
                fileList = new FileList((AFolder)tv.SelectedItem);

                listView.Items.Clear();

                IProgress<Tuple<TransformedBitmap, Size>> pAddImageToList = new Progress<Tuple<TransformedBitmap, Size>>(addImageToList);

                loader.Cancel = true; // 前回の画像ロードをキャンセル(タイミングによってキャンセル後もロードされることがあるが処理が重くなってはいけないのでこのままにする)
                loader = new BackgroundLoader();

                Task.Run(() => loadImages(fileList.CurrentFiles, pAddImageToList, loader));

                //CancellationTokenSource tokenSource = new CancellationTokenSource();
                //Task.Run(() => loadImages(currentFiles, pAddImageToList), tokenSource.Token);
            }
        }

        private void buttonStopLoading_Click(object sender, RoutedEventArgs e)
        {
            loader.Cancel = true; // 前回の画像ロードをキャンセル(タイミングによってキャンセル後もロードされることがあるが処理が重くなってはいけないのでこのままにする)
        }

        private void checkSelected()
        {
            checkBoxSelected.IsChecked = fileList.CurrentIsSelected();
        }

        private void checkSplitPoint()
        {
            checkBoxSplitPoint.IsChecked = fileList.CurrentIsSplitPoint();
        }

        private void showImage()
        {
            string currentPath = fileList.CuurentFile;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.DecodePixelHeight = settings.MaxHeight;
            bitmapImage.UriSource = new Uri(currentPath);
            bitmapImage.EndInit();

            image.Source = getTransformedBitmap(currentPath, bitmapImage).Item1;

            checkSelected();
            checkSplitPoint();
        }

        private void showNextImage(int step)
        {
            if ( fileList.GoNext(step) )
            {
                showImage();
            }
        }

        private void showNextSelectedImage(int step)
        {
            if ( fileList.GoNextSelected(step) )
            {
                showImage();
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)e.Source;
            if ( lv.SelectedItem != null )
            {
                fileList.CurrentIndex = lv.SelectedIndex;
                showNextImage(0);
            }
        }

        private void buttonPrev_Click(object sender, RoutedEventArgs e)
        {
            showNextImage(-1);
        }

        private void buttonPrevSelect_Click(object sender, RoutedEventArgs e)
        {
            fileList.SelectCurrentItem(true);
            showNextImage(-1);
        }

        private void buttonPrevUnselect_Click(object sender, RoutedEventArgs e)
        {
            fileList.SelectCurrentItem(false);
            showNextImage(-1);
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            showNextImage(1);
        }

        private void buttonNextSelect_Click(object sender, RoutedEventArgs e)
        {
            fileList.SelectCurrentItem(true);
            showNextImage(1);
        }

        private void buttonNextUnselect_Click(object sender, RoutedEventArgs e)
        {
            fileList.SelectCurrentItem(false);
            showNextImage(1);
        }

        private void buttonPrevSelected_Click(object sender, RoutedEventArgs e)
        {
            showNextSelectedImage(-1);
        }

        private void buttonNextSelected_Click(object sender, RoutedEventArgs e)
        {
            showNextSelectedImage(1);
        }

        private void checkBoxSelected_Checked(object sender, RoutedEventArgs e)
        {
            fileList.SelectCurrentItem(true);
        }

        private void checkBoxSelected_Unchecked(object sender, RoutedEventArgs e)
        {
            fileList.SelectCurrentItem(false);
        }

        private void buttonSelectAll_Click(object sender, RoutedEventArgs e)
        {
            fileList.SetSelectedAll(true);
            checkSelected();
        }

        private void buttonClearSelectAll_Click(object sender, RoutedEventArgs e)
        {
            fileList.SetSelectedAll(false);
            checkSelected();
        }

        private void buttonPrevSplit_Click(object sender, RoutedEventArgs e)
        {
            fileList.GoNextSplitPoint(-1);
        }

        private void buttonNextSplit_Click(object sender, RoutedEventArgs e)
        {
            fileList.GoNextSplitPoint(1);
        }

        private void checkBoxSplitPoint_Checked(object sender, RoutedEventArgs e)
        {
            fileList.SetSpiltCurrentItem(true);
        }

        private void checkBoxSplitPoint_Unchecked(object sender, RoutedEventArgs e)
        {
            fileList.SetSpiltCurrentItem(false);
        }

        private void buttonSplit_Click(object sender, RoutedEventArgs e)
        {
            if ( fileList.ListCreated )
            {
                PhotoFolder newFolder = new PhotoFolder();
                int count = 1;
                foreach ( List<string> part in fileList.SplitParts )
                {
                    PhotoList photoList = new PhotoList();
                    foreach ( string file in part )
                    {
                        photoList.Add(file);
                    }
                    photoList.Name = photoList.Name + "(" + count.ToString() + ")";
                    count++;
                    newFolder.SubFolders.Add(photoList);
                }
                newFolder.Name = ((PhotoElement)treeView.SelectedItem).Name + "分割";
                photoAlbum.AddElement(newFolder);
                newFolder.IsSelected = true;
            }
        }

        private ObservableCollection<AFolder> rootFolders;

        private void setRootFolder()
        {
            Folder folder = new Folder();
            folder.FullPath = settings.RootFolder;
            rootFolders = new ObservableCollection<AFolder>() { folder };
            treeView.Items.Clear();
            treeView.ItemsSource = rootFolders;
        }

        private void replaceRootFolder(string folderpath)
        {
            try
            {
                Folder folder = new Folder();
                folder.FullPath = folderpath;
                rootFolders[0] = folder;
                settings.RootFolder = folderpath;
            }
            catch ( Exception )
            {
                // フォルダ読み込み失敗
                MessageBox.Show("フォルダの読み込みが失敗しました");
            }
        }

        private PhotoAlbum photoAlbum = new PhotoAlbum();

        private void buttonOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if ( result == System.Windows.Forms.DialogResult.OK )
            {
                string folderpath = dlg.SelectedPath;
                replaceRootFolder(folderpath);
            }
        }

        private PhotoAlbum readAlbum()
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".psa";
                dlg.Filter = "アルバムファイル (.psa)|*.psa";
                dlg.ValidateNames = true;
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(settings.AlbumPath);
                dlg.FileName = System.IO.Path.GetFileName(settings.AlbumPath);
                Nullable<bool> result = dlg.ShowDialog();
                if ( result.HasValue && result == true )
                {
                    string albumpath = dlg.FileName;
                    PhotoAlbum album = new PhotoAlbum(albumpath);
                    settings.AlbumPath = albumpath;
                    return album;
                }
            }
            catch ( Exception )
            {
                // アルバム読み込み失敗
                MessageBox.Show("アルバムの読み込みが失敗しました");
            }
            return null;
        }

        private void buttonOpenAlbum_Click(object sender, RoutedEventArgs e)
        {
            PhotoAlbum album = readAlbum();
            if ( album != null)
            {
                photoAlbum = album;
                rootFolders[0] = album.Element;
            }
        }

        private void buttonAddAlbum_Click(object sender, RoutedEventArgs e)
        {
            PhotoAlbum album = readAlbum();
            if ( album != null )
            {
                photoAlbum.AddElement(album.Element);
            }
        }

        private PhotoList getSelectedList()
        {
            if ( !fileList.ListCreated )
            {
                return null;
            }
            PhotoList photoList = new PhotoList();
            foreach ( string file in fileList.SelectedFiles )
            {
                photoList.Add(file);
            }
            photoList.Name = ""; // 既定の名前を設定
            return photoList;
        }

        private void addSelectedList()
        {
            PhotoList photoList = getSelectedList();
            if ( photoList == null )
            {
                return;
            }
            photoAlbum.AddElement(photoList);
        }

        private void savePhotoAlbum(PhotoAlbum album, string name)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = name;
            dlg.DefaultExt = ".psa";
            dlg.Filter = "アルバムファイル (.psa)|*.psa";
            dlg.OverwritePrompt = true;
            dlg.ValidateNames = true;
            if ( settings.AlbumPath != "" )
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(settings.AlbumPath);
            }
            Nullable<bool> result = dlg.ShowDialog();
            if ( result.HasValue && result == true )
            {
                album.Save(dlg.FileName);
                settings.AlbumPath = dlg.FileName;
            }
        }

        private void saveSelectedList()
        {
            PhotoList photoList = getSelectedList();
            if ( photoList == null )
            {
                return;
            }
            PhotoAlbum album = new PhotoAlbum(photoList);
            savePhotoAlbum(album, photoList.Name);
        }

        private void buttonAddList_Click(object sender, RoutedEventArgs e)
        {
            addSelectedList();
        }

        private void buttonSaveList_Click(object sender, RoutedEventArgs e)
        {
            saveSelectedList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("現在のリストを保存しますか？", "保存の確認", MessageBoxButton.YesNoCancel);
            if (res == MessageBoxResult.Yes )
            {
                // 現在のリストを保存
                saveSelectedList();
            }
            else if ( res == MessageBoxResult.Cancel )
            {
                // キャンセル
                e.Cancel = true;
                return;
            }
            if ( photoAlbum.Element != null )
            {
                MessageBoxResult res2 = MessageBox.Show("アルバムを保存しますか？", "保存の確認", MessageBoxButton.YesNoCancel);
                if ( res2 == MessageBoxResult.Yes )
                {
                    // 現在のアルバムを保存
                    savePhotoAlbum(photoAlbum, System.IO.Path.GetFileName(settings.AlbumPath));
                }
                else if ( res2 == MessageBoxResult.Cancel )
                {
                    // キャンセル
                    e.Cancel = true;
                    return;
                }
            }
            settings.Left = (int)Left;
            settings.Top = (int)Top;
            settings.Width = (int)Width;
            settings.Height = (int)Height;
            settings.VerticalSplitter = (int)mainGrid.ColumnDefinitions[0].Width.Value;
            settings.HorizontalSplitter = (int)mainGrid.RowDefinitions[0].Height.Value;
            settings.save_settings();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonCopyToFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if ( result == System.Windows.Forms.DialogResult.OK )
            {
                string destpath = dlg.SelectedPath;
                fileList.CopySelectedToFolder(destpath);
            }
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingDialog dlg = new SettingDialog();
            bool? res = dlg.ShowDialog();
            if ( res.HasValue )
            {
                if ( res.Value )
                {
                }
                else
                {
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Left = settings.Left;
            Top = settings.Top;
            Width = settings.Width;
            Height = settings.Height;
            mainGrid.ColumnDefinitions[0].Width = new GridLength(settings.VerticalSplitter);
            mainGrid.RowDefinitions[0].Height = new GridLength(settings.HorizontalSplitter);

            setRootFolder();
        }

        private void buttonTreeLeft_Click(object sender, RoutedEventArgs e)
        {
            photoAlbum.MoveHigher();
        }

        private void buttonTreeRight_Click(object sender, RoutedEventArgs e)
        {
            photoAlbum.MoveLower();
        }

        private void buttonTreeUp_Click(object sender, RoutedEventArgs e)
        {
            photoAlbum.MoveUp();
        }

        private void buttonTreeDown_Click(object sender, RoutedEventArgs e)
        {
            photoAlbum.MoveDown();
        }

        private void buttonAddFolderRoot_Click(object sender, RoutedEventArgs e)
        {
            photoAlbum.AddFolderRoot();
            rootFolders[0] = photoAlbum.Element;
        }

        private void buttonAddFolderUnder_Click(object sender, RoutedEventArgs e)
        {
            PhotoFolder newFolder = new PhotoFolder();
            photoAlbum.AddElement(newFolder);
            newFolder.IsSelected = true;
        }

        private void buttonTreeDelete_Click(object sender, RoutedEventArgs e)
        {
            string name = ((PhotoElement)treeView.SelectedItem).Name;
            MessageBoxResult res = MessageBox.Show("「" + name + "」を削除しますか？", "ツリー削除", MessageBoxButton.YesNo);
            if ( res == MessageBoxResult.Yes )
            {
                photoAlbum.DeleteElement();
            }
        }

        private void buttonTreeRename_Click(object sender, RoutedEventArgs e)
        {
            TextDialog dlg = new TextDialog();
            dlg.Text = ((PhotoElement)treeView.SelectedItem).Name;
            bool? res = dlg.ShowDialog();
            if ( res.HasValue && res.Value )
            {
                PhotoElement item = (PhotoElement)treeView.SelectedItem;
                item.Name = dlg.Text;
            }
        }
    }
}
