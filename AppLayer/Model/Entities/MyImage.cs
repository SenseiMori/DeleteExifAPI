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
using ExifDeleteLib.Core;



namespace AppLayer.Model.Entities
{
    public class MyImage : ObservableObject
    {
        private List<byte> _JPGMarkers;
        public List<byte> JPGMarkers 
        {
            get
            {
                return _JPGMarkers;
            }
            set
            {
                _JPGMarkers = value;
                RaisePropertyChangedEvent(nameof(_JPGMarkers));
            }
        } 

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
            { return _filePath; }
            set
            {
                _filePath = value;
                RaisePropertyChangedEvent(nameof(_filePath));
            }
        }
        private string _shortFileName;
        public string ShortFileName
        {
            get
            { return _shortFileName; }
            set
            {
                _shortFileName = value;
                RaisePropertyChangedEvent(nameof(_shortFileName));
            }
        }
        private int _height;
        public int Height
        {
            get
            { return _height; }
            set
            {
                _height = value;
                RaisePropertyChangedEvent(nameof(_height));
            }
        }
        private int _width;
        public int Width
        {
            get
            { return _width; }
            set
            {
                _width = value;
                RaisePropertyChangedEvent(nameof(_width));
            }
        }
        private string _widthHeight;
        public string WidthHeight
        {
            get
            { return _widthHeight; }
            set
            {
                _widthHeight = value;
                RaisePropertyChangedEvent(nameof(_widthHeight));
            }
        }
        private string _size; // Размер файла
        public string Size
        {
            get
            { return _size; }
            set
            {
                _size = value;
                RaisePropertyChangedEvent(nameof(_size));
            }
        }
        private string _expectedWidthHeight;
        public string ExpectedWidthHeight
        {
            get
            { return _expectedWidthHeight; }
            set
            {
                _expectedWidthHeight = value;
                RaisePropertyChangedEvent(nameof(_expectedWidthHeight));
            }
        }
    }
}
