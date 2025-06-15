using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLayer.ViewModel;
using SixLabors.ImageSharp;
using System.Drawing;

namespace AppLayer.Model.Entities
{
    public class MyImage : ObservableObject
    {
        private List<byte> _JPGMarkers;
        public List<byte> JPGMarkers 
        {
            get => _JPGMarkers;
            set
            {
                _JPGMarkers = value;
                RaisePropertyChangedEvent(nameof(JPGMarkers));
            }
        } 

        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                RaisePropertyChangedEvent(nameof(FileName));
            }
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                RaisePropertyChangedEvent(nameof(FilePath));
            }
        }
        private string _shortFileName;
        public string ShortFileName
        {
            get => _shortFileName;
            set
            {
                _shortFileName = value;
                RaisePropertyChangedEvent(nameof(ShortFileName));
            }
        }
        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                RaisePropertyChangedEvent(nameof(Height));
            }
        }
        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                RaisePropertyChangedEvent(nameof(Width));
            }
        }
        private string _widthHeight;
        public string WidthHeight
        {
            get => _widthHeight;
            set
            {
                _widthHeight = value;
                RaisePropertyChangedEvent(nameof(WidthHeight));
            }
        }
        private string _size; // Размер файла
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                RaisePropertyChangedEvent(nameof(Size));
            }
        }
        private string _expectedWidthHeight;
        public string ExpectedWidthHeight
        {
            get => _expectedWidthHeight;
            set
            {
                _expectedWidthHeight = value;
                RaisePropertyChangedEvent(nameof(ExpectedWidthHeight));
            }
        }
        private string _expectedSize;
        public string ExpectedSize
        {
            get => _expectedSize;
            set
            {
                _expectedSize = value;
                RaisePropertyChangedEvent(nameof(ExpectedSize));
            }
        }
    }
}
