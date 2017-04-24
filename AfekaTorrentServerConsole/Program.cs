using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Entities;

namespace AfekaTorrentServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            #region tests
            
            #endregion
            AfekaTorrentServerConsole.WCFServices.ServiceInitializer si = new WCFServices.ServiceInitializer();
            si.InitializeServiceHost();
            Console.Write("AfekaTorrent server has started");
            Console.ReadKey();
            
        }
    }
}
