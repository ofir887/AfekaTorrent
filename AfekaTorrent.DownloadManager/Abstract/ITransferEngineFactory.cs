using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AfekaTorrent.DownloadManager.Abstract
{
    public  interface ITransferEngineFactory
    {
        ITransferEngine CreateTransferEngine();
    }
}
