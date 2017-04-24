using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AfekaTorrentServerConsole
{
    public interface IUnitOfWork
    {
        void Save();
    }
}
