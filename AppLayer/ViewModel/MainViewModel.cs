using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using AppLayer.Model.Entities;
using AppLayer.Services.Commands;
using ExifDeleteLib;
using SixLabors.ImageSharp;
using ModifierCore.Core.ImageManipulation;
using ModifierCore.Core.Const;
using AppLayer.Services.Handlers;
using AppLayer.Model.Interfaces;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using AppLayer.Services.Handlers.ModifierHandlers;

namespace AppLayer.ViewModel
{
    public class MainViewModel : ObservableObject, IMainViewModel
    {
        public ObservableCollection<MyImage> images { get; set; } = new ObservableCollection<MyImage>();
        private MyImage _selectedImage;

        private RelayCommand addImageCommand;
        private RelayCommand addFolderCommand;
        private RelayCommand removeImageCommand;
        private RelayCommand removeImagesCommand;
        private RelayCommand _manipulate;
        private RelayCommand _resetOptions;
        private RelayCommand _getExpectedData;

        private bool _isFolderOpen;
        private bool _isCreateZip;
        private bool _isRemove;

        private CompressLevel _compressLevel = CompressLevel.None;
        private SizeScale _resolution = SizeScale.None;

        private readonly string[] allowedExtensions = [".jpg", ".jpeg"];

        public bool IsRemove
        {
            get => _isRemove;
            set
            {
                _isRemove = value;
                RaisePropertyChangedEvent(nameof(IsRemove));
            }
        }
        public bool IsResize => Resolution != SizeScale.None;
        public SizeScale Resolution 
        { 
            get => _resolution; 
            set
            {
                _resolution = value;
                RaisePropertyChangedEvent(nameof(Resolution));
                RaisePropertyChangedEvent(nameof(IsBestResolution));
                RaisePropertyChangedEvent(nameof(IsNormalResolution));
                RaisePropertyChangedEvent(nameof(IsExtraResolution));
                RaisePropertyChangedEvent(nameof(IsNotChangeResolution));
            }

        }

        public bool IsBestResolution
        {
            get => Resolution == SizeScale.Best;
            set
            {
                if (value)
                    Resolution = SizeScale.Best;
            }
        }
        public bool IsNormalResolution
        {
            get => Resolution == SizeScale.Normal;
            set
            {
                if (value)
                    Resolution = SizeScale.Normal;
            }
        }
        public bool IsExtraResolution
        {
            get => Resolution == SizeScale.Extra;
            set
            {
                if (value)
                    Resolution = SizeScale.Extra;
            }
        }
        public bool IsNotChangeResolution
        {
            get => Resolution == SizeScale.None;
            set
            {
                if (value)
                    Resolution = SizeScale.None;
            }
        }
        public bool IsCompress => CompressLevel != CompressLevel.None;

        public CompressLevel CompressLevel
        {
            get => _compressLevel;
            set
            {
                if (CompressLevel != value)
                    _compressLevel = value;
                    RaisePropertyChangedEvent(nameof(CompressLevel));
                    RaisePropertyChangedEvent(nameof(IsBestCompress));
                    RaisePropertyChangedEvent(nameof(IsNormalCompress));
                    RaisePropertyChangedEvent(nameof(IsExtraCompress));
                    RaisePropertyChangedEvent(nameof(IsCompressNone));
                    RaisePropertyChangedEvent(nameof(IsCompress));
            }

        }
        public bool IsBestCompress
        {
            get => CompressLevel == CompressLevel.Best;
            set
            {
                if (value)
                    CompressLevel = CompressLevel.Best;
            }
        }
        public bool IsNormalCompress
        {
            get => CompressLevel == CompressLevel.Normal;
            set
            {
                if (value)
                    CompressLevel = CompressLevel.Normal;
            }
        }
        public bool IsExtraCompress
        {
            get => CompressLevel == CompressLevel.Extra;
            set
            {
                if (value)
                    CompressLevel = CompressLevel.Extra;
            }
        }
        public bool IsCompressNone
        {
            get => CompressLevel == CompressLevel.None;
            set
            {
                if (value)
                    CompressLevel = CompressLevel.None;
            }
        }
        public MyImage selectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                RaisePropertyChangedEvent(nameof(selectedImage));
            }
        }

        public bool IsFolderOpen
        {
            get => _isFolderOpen;
            set
            {
                _isFolderOpen = value;
                RaisePropertyChangedEvent(nameof(IsFolderOpen));
            }
        }
        public bool IsCreateZip
        {
            get => _isCreateZip;
            set
            {
                _isCreateZip = value;
                RaisePropertyChangedEvent(nameof(IsCreateZip));
            }
        }
        public RelayCommand AddImageCommand => addImageCommand = new RelayCommand(parameter =>
                    {
                       GetJPGs();
                    });
        public RelayCommand AddFolderCommand =>
                    addFolderCommand = new RelayCommand(parameter =>
                   {
                       GetFolder();
                      
                   });       
        public RelayCommand RemoveImageCommand => removeImageCommand = new RelayCommand(parameter =>
                   {
                       DeleteJPG();
                   });
        public RelayCommand RemoveImagesCommand => removeImagesCommand = new RelayCommand(parameter =>
                   {
                       DeleteJPGs();
                   });
        public RelayCommand ManipulateCommand => _manipulate = new RelayCommand(parameter =>
        {
            MainHandler mainHandler = new MainHandler(images, this);

            foreach (MyImage image in images)
            {
                mainHandler.Processing(image.FilePath);
            }

        });

        public RelayCommand GetExpectedDataCommand => _getExpectedData = new RelayCommand(parameter =>
        {
            ImageInfoHandler imageInfoHandler = new ImageInfoHandler(this);
            imageInfoHandler.GetExpectedData(images, Resolution, CompressLevel);
        });

        internal ObservableCollection<MyImage> GetJPGs()
        {
            ImageInfoHandler imageInfoHandler = new ImageInfoHandler(this);
            Scaler scaler = new Scaler();
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = true,
                Title = "Выберите изображения"
                

            };
            if (openFileDialog.ShowDialog() == true)
                {
                foreach (string file in openFileDialog.FileNames)
                    {
                        var jpg = imageInfoHandler.GetInfo(file);
                        images.Add(jpg);
                        scaler.ConvertToNewSize((jpg.Width, jpg.Height), SizeScale.Normal);  
                        RaisePropertyChangedEvent(nameof(images));
                }

            }
            return images;
        }
        internal void GetFolder()
        {
            ImageInfoHandler imageInfoHandler = new ImageInfoHandler(this);

            using var folderDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Выберите папку"
            };

            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var folder = folderDialog.FileName;
                    var files = Directory.GetFiles(folder, ".").
                                          Where(file => file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || 
                                                        file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase));
                foreach (string file in files)
                    {
                        var jpg = imageInfoHandler.GetInfo(file);

                        images.Add(jpg);
                        RaisePropertyChangedEvent(nameof(images));
                    }

                }
        }
        internal void DeleteJPG()
        {
            if (_selectedImage !=null)
                images.Remove(_selectedImage);
            RaisePropertyChangedEvent(nameof(images));
        }
        internal void DeleteJPGs()
        {
            images.Clear();
            RaisePropertyChangedEvent(nameof(images));
        }
        internal void OpenNewJPGFolder()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Path.GetDirectoryName(images[0].FileName),
                UseShellExecute = true
            });
        }
    }

}


