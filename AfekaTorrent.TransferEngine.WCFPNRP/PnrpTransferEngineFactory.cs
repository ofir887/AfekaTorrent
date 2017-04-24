using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AfekaTorrent.DownloadManager;
using AfekaTorrent.DownloadManager.Abstract;
using Microsoft.Win32;

namespace AfekaTorrent.TransferEngine.WCFPNRP
{
    public sealed class PnrpTransferEngineFactory : ITransferEngineFactory
    {
        public ITransferEngine CreateTransferEngine()
        {
            return new PnrpTransferEngine(Config.LocalHostyName, Config.LocalPort
                );
        }        
    }
}
