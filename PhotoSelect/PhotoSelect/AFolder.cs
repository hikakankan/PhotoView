using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace PhotoSelect
{
    abstract class AFolder
    {
        public abstract string Name { get; set; }
        public abstract ObservableCollection<string> ImageFiles { get; }
        public abstract ObservableCollection<AFolder> SubFolders { get; }
    }
}
