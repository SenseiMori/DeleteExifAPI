using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRemoveExif.ViewModel;



namespace TextRemoveExif.Model
{
     public class Image : ObservableObject
    {
        private string _fileName;

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                RaisePropertyChangedEvent(nameof(_fileName));    
            }

        }

        private string _filePath;
        public string FilePath
        {
            get
            { return Path.GetFullPath(_filePath); }
            set
            {
                _filePath = value;
                RaisePropertyChangedEvent(nameof(_filePath));
            }
        }

    }
}
