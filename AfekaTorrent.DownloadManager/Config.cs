using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace AfekaTorrent.DownloadManager
{
    public static class Config
    {
        const string LocalHostNameKey = "AfekaTorrent";
        const string LocalPortKey = "AfekaTorrentLocalPort";
        const string SharedFolderNameKey = "SharedFolderName";

        public static string LocalHostyName
        {
            get
            {
                var localHostName = Registry.CurrentUser.GetValue(LocalHostNameKey);
                string hostname = string.Empty;
                if (localHostName == null)
                {
                    hostname = "AfekaTorrent" + Guid.NewGuid().ToString().Replace("-", "");
                    Registry.CurrentUser.SetValue(LocalHostNameKey, hostname);
                }
                else
                {
                    hostname = localHostName.ToString();
                }
                return hostname;
            }
        }

        public static string SharedFolder
        {
            get
            {
                var sharedFolderName = Registry.CurrentUser.GetValue(SharedFolderNameKey);
                string folder = string.Empty;
                if (sharedFolderName == null)
                {
                    folder = @"C:\AfekaTorrentFilesDirectory"; ;
                    Registry.CurrentUser.SetValue(SharedFolderNameKey, folder);
                }
                else
                {
                    folder = sharedFolderName.ToString();
                }
                return folder;
            }
        }

        public static int LocalPort
        {
            get
            {
                var localPort = Registry.CurrentUser.GetValue(LocalPortKey);
                int port = 20388;
                if (localPort == null)
                {
                    Registry.CurrentUser.SetValue(LocalPortKey, port);
                }
                return port;
            }
        }
    }
}
