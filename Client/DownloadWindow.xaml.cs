using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AfekaTorrent.DownloadManager;
using System.IO;
using AfekaTorrent.DownloadManager.UserServer;
using AfekaTorrent.DownloadManager.FileServer;
using System.Xml;
using System.Windows.Threading;

namespace Client
{

    public partial class DownloadWindow : Window
    {
        FileTransferManager fileTransferManager;
        String DownloadFolderPath;
        String SharedFolderPath;
        String UserName, Password;
        long fileSize;
        Entities.User[] users;

        System.Diagnostics.Stopwatch watch;

        //
        List<Tuple<AfekaTorrent.DownloadManager.FileTransferManager.DownloadParameter, Byte[]>> data;
        //

        public delegate void ReadyToShowDelegate(object sender, EventArgs args);

        public event ReadyToShowDelegate ReadyToShow;

        private DispatcherTimer timer;

        public DownloadWindow()
        {
            FileProviderServerManager.StartFileProviderServer();

            InitializeComponent();
            TimerSettings();

        }
        public void TimerSettings()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {


            timer.Stop();

            if (ReadyToShow != null)
            {
                ReadyToShow(this, null);
            }
        }
        public void GetFoldersPath()
        {
            XmlReader reader = XmlReader.Create(System.IO.Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "MyConfig.xml"));
            reader.ReadToFollowing("User");

            if (reader.ReadToDescendant("UserName"))
            {
                this.UserName = reader.ReadElementContentAsString();
                if ((reader.ReadToFollowing("Password")))
                    this.Password = reader.ReadElementContentAsString();
                if (reader.ReadToFollowing("SharedFolder"))
                    this.SharedFolderPath = reader.ReadElementContentAsString();
                if (reader.ReadToFollowing("DownloadFolder"))
                    this.DownloadFolderPath = reader.ReadElementContentAsString();
            }


            reader.Close();

        }

        public bool checkForDuplicate(String fileName)
        {
            FilesServiceClient fsc = new FilesServiceClient();
            List<Entities.File> fileList = new List<Entities.File>();
            foreach (Entities.File serverFile in fsc.GetAllFiles())
            {
                if (serverFile.FileName == null)
                    return false;
                if (serverFile.FileName.Equals(fileName))
                    return true;


            }
            return false;
        }
        public void uploadFolderFilesToDataBase()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(this.SharedFolderPath);
            FileInfo[] info = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
            Entities.Peer peerType = new Entities.Peer();

            for (int i = 0; i < info.Length; i++)
            {
                List<Entities.File> fileList = new List<Entities.File>();
                peerType = new Entities.Peer();
                Entities.File FileType = new Entities.File();
                if (!checkForDuplicate(dirInfo + "\\" + info[i]))
                {
                    FileType.FileName = dirInfo + "\\" + info[i].Name;
                    FileType.FileSize = (int)info[i].Length;
                    FileType.FileType = System.IO.Path.GetExtension(info[i].Name);
                    FileType.PeerHostName = Config.LocalHostyName;
                    peerType.PeerID = FileType.PeerID = Guid.NewGuid();
                    FileType.OwnedBy = UserName;//
                    fileList.Add(FileType);
                    peerType.PeerHostName = Config.LocalHostyName;
                    fileTransferManager.AddFiles(fileList, peerType);
                }
            }

        }
        public List<Entities.File> getFileListFromServer()
        {
            FilesServiceClient fsc = new FilesServiceClient();
            UserServiceClient usc = new UserServiceClient();
            this.users = usc.GetAllUsers();
            List<string> usersONline = new List<string>();
            for (int i = 0; i < this.users.Length; i++)
            {
                if (users[i].IsActive == true)
                    usersONline.Add(users[i].UserName);
            }
            List<Entities.File> fileList = new List<Entities.File>();
            Entities.File[] files = fsc.GetAllFiles();
            for (int j = 0; j < files.Length; j++)
            {
                for (int i = 0; i < usersONline.Count(); i++)
                {
                    Entities.File currentFile = new Entities.File();
                    currentFile.FileName = files[j].FileName;
                    currentFile.FileSize = files[j].FileSize;
                    currentFile.FileType = files[j].FileType;
                    currentFile.PeerID = files[j].PeerID;
                    currentFile.PeerHostName = files[j].PeerHostName;
                    currentFile.OwnedBy = files[j].OwnedBy;///
                    if (currentFile.OwnedBy == usersONline[i])
                        fileList.Add(currentFile);




                }



            }


            return fileList;
        }
        private void DownloadWindow_Load(object sender, RoutedEventArgs e)
        {

            GetFoldersPath();
            fileTransferManager = new FileTransferManager();
            fileTransferManager.FilePartDownloaded += fileTransferManager_FilePartDownloaded;

            DirectoryInfo dirInfo = new DirectoryInfo(this.SharedFolderPath);
            FileInfo[] info = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
            uploadFolderFilesToDataBase();



        }
        void DataWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UserServiceClient usc = new UserServiceClient();
            var loginGuid = usc.Login(this.UserName, this.Password);
            usc.UpdateFolders(this.DownloadFolderPath, this.SharedFolderPath, loginGuid);
            MessageBox.Show("Closing application & loginOff " + this.UserName);



        }




        void fileTransferManager_FilePartDownloaded(object sender, DataContainerEventArg<FileTransferManager.FilePartData> e)
        {

            this.data.Add(new Tuple<AfekaTorrent.DownloadManager.FileTransferManager.DownloadParameter, Byte[]>(e.Data.DownloadParameter, e.Data.FileBytes));
            if (e.Data.DownloadParameter.AllPartsCount == e.Data.DownloadParameter.Part)
            {
                saveFile(data);
            }
        }

        private void search_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (search_TextBox.Text.Equals("*"))
            {
                dataGrid.ItemsSource = getFileListFromServer();
                dataGrid.DataContext = getFileListFromServer();
            }
            else if (string.IsNullOrWhiteSpace(search_TextBox.Text))
            {
                dataGrid.ItemsSource = null;
                dataGrid.DataContext = null;
                dataGrid.Items.Refresh();
            }






        }

        private void search_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(search_TextBox.Text))
            {
                List<Entities.File> foundFileInfoList = fileTransferManager.SearchFileByName(search_TextBox.Text);
                List<Entities.File> onlineFileList = new List<Entities.File>();

                List<string> usersOnline = new List<string>();
                for (int i = 0; i < users.Length; i++)
                {
                    if (users[i].IsActive == true)
                        usersOnline.Add(users[i].UserName);
                }
                for (int i = 0; i < foundFileInfoList.Count(); i++)
                {
                    for (int j = 0; j < usersOnline.Count(); j++)
                    {
                        if (foundFileInfoList[i].OwnedBy == usersOnline[j])
                        {
                            onlineFileList.Add(foundFileInfoList[i]);
                        }
                    }
                }

                dataGrid.ItemsSource = onlineFileList;
                dataGrid.DataContext = onlineFileList;

            }

        }



        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            dataGrid.DataContext = null;
            dataGrid.ItemsSource = getFileListFromServer();
            dataGrid.DataContext = getFileListFromServer();
            dataGrid.Items.Refresh();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            string sMessageBoxText = "Do you want to download this file?";
            string sCaption = "AfekaTorrent";
            this.data = new List<Tuple<AfekaTorrent.DownloadManager.FileTransferManager.DownloadParameter, Byte[]>>();
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    Entities.File fileSearchResult = dataGrid.SelectedItem as Entities.File;
                    this.fileTransferManager.Download(fileSearchResult);
                    watch = System.Diagnostics.Stopwatch.StartNew(); ///
                    this.fileSize = fileSearchResult.FileSize;
                    break;

                case MessageBoxResult.No:
                    /* ... */
                    break;
            }

        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {

            this.data = new List<Tuple<AfekaTorrent.DownloadManager.FileTransferManager.DownloadParameter, Byte[]>>();
            Entities.File fileSearchResult = dataGrid.SelectedItem as Entities.File;
            this.fileSize = fileSearchResult.FileSize;
            watch = System.Diagnostics.Stopwatch.StartNew(); ///
            this.fileTransferManager.Download(fileSearchResult);


        }


        private void saveFile(List<Tuple<FileTransferManager.DownloadParameter, byte[]>> data)
        {





            Dispatcher.Invoke(new Action(() =>
            {
                var lst = data.OrderBy(x => x.Item1.Part).ToList();
                var bytes = new List<byte>();
                for (int i = 0; i < lst.Count; i++)
                {
                    bytes.AddRange(lst[i].Item2);

                }
                string fileName = System.IO.Path.GetFileName(data[0].Item1.FileSearchResult.FileName);

                {
                    System.IO.File.WriteAllBytes(System.IO.Path.Combine(this.DownloadFolderPath, fileName), bytes.ToArray());

                }
                this.watch.Stop();
                var elapsedMs = watch.Elapsed.Seconds;
                var elapsedMs2 = elapsedMs;
                if (elapsedMs2 == 0)
                    elapsedMs2 = 1;
                if (this.fileSize > 999999)
                {
                    MessageBox.Show(this, "File " + fileName + "\nFile size: " + this.fileSize / 1024 / 1024 + " MB" + "\ndownloaded & saved in " + this.DownloadFolderPath + "\nTotal time took to download: " + elapsedMs + " seconds" + "\nSpeed of download: " + this.fileSize / 1024 / elapsedMs2 + " Kbps");
                }
                else
                    MessageBox.Show(this, "File " + fileName + "\nFile size: " + this.fileSize / 1024 + " Kb" + "\ndownloaded & saved in " + this.DownloadFolderPath + "\nTotal time took to download: " + elapsedMs + " seconds" + "\nSpeed of download: " + this.fileSize/1024/elapsedMs2 + " Kbps");
            }));



        }



    }
}
