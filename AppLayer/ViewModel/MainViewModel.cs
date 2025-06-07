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





namespace AppLayer.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private RelayCommand addImageCommand;
        private RelayCommand addFolderCommand;
        private RelayCommand removeImageCommand;
        private RelayCommand removeMetadataCommand;
        private RelayCommand removeImagesCommand;
        private RelayCommand _manipulate;
        private bool _isFolderOpen;
        private bool _isCreateZip;
        private bool _isResize;
        private bool _isCompress;
        private bool _isRemove;
        private CompressLevel _compressLevel;
        private Weight _weight;

        private readonly string[] allowedExtensions = [".jpg", ".jpeg"];
        private MyImage _selectedImage;

        public ObservableCollection<MyImage> images { get; set; } = new ObservableCollection<MyImage>();

        public bool IsRemove
        {
            get => _isRemove;
            set
            {
                _isRemove = value;
                RaisePropertyChangedEvent(nameof(IsRemove));
            }
        }
        public bool IsResize => Weight != Weight.None;
        public Weight Weight 
        { 
            get => _weight; 
            set
            {
                _weight = value;
                RaisePropertyChangedEvent(nameof(Weight));
                RaisePropertyChangedEvent(nameof(IsBestWeight));
                RaisePropertyChangedEvent(nameof(IsNormalWeight));
                RaisePropertyChangedEvent(nameof(IsExtraWeight));
            }
        
        }

        public bool IsBestWeight
        {
            get => Weight == Weight.Best;
            set
            {
                if (value)
                    Weight = Weight.Best;
                    RaisePropertyChangedEvent(nameof(IsBestWeight));
            }
        }
        public bool IsNormalWeight
        {
            get => Weight == Weight.Normal;
            set
            {
                if (value)
                    Weight = Weight.Normal;
                    RaisePropertyChangedEvent(nameof(IsNormalWeight));
            }
        }
        public bool IsExtraWeight
        {
            get => Weight == Weight.Extra;
            set
            {
                if (value)
                    Weight = Weight.Extra;
                    RaisePropertyChangedEvent(nameof(IsExtraWeight));
            }
        }
        public bool IsCompress => CompressLevel != CompressLevel.None;
        public CompressLevel CompressLevel
        {
            get => _compressLevel;
            set
            {
                _compressLevel = value; 
                RaisePropertyChangedEvent(nameof(IsCompress));
                RaisePropertyChangedEvent(nameof(CompressLevel));
                RaisePropertyChangedEvent(nameof(IsBestCompress));
                RaisePropertyChangedEvent(nameof(IsNormalCompress));
                RaisePropertyChangedEvent(nameof(IsExtraCompress));
            }

        }
        public bool IsBestCompress
        {
            get => CompressLevel == CompressLevel.Best;
            set
            {
                if (value)
                    CompressLevel = CompressLevel.Best;
                RaisePropertyChangedEvent(nameof(IsBestCompress));
            }
        }
        public bool IsNormalCompress
        {
            get => CompressLevel == CompressLevel.Normal;
            set
            {
                if (value)
                    CompressLevel = CompressLevel.Normal;
                RaisePropertyChangedEvent(nameof(IsNormalCompress));
            }
        }
        public bool IsExtraCompress
        {
            get => CompressLevel == CompressLevel.Extra;
            set
            {
                if (value)
                    CompressLevel = CompressLevel.Extra;
                RaisePropertyChangedEvent(nameof(IsExtraCompress));
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
                        MetadataRemover removeMetadata = new MetadataRemover(images);
                        
                        GetJPGs();
                        if (IsRemove)
                        {
                            removeMetadata.Remove();
                        }

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
            MetadataRemover removeMetadata = new MetadataRemover(images);
            ImageResize imageResize = new ImageResize();
            ImageCompressor imageCompressor = new ImageCompressor();
            if (IsRemove)
            {
                removeMetadata.Remove();
            }
            if (IsResize)
            {
                if (Weight == Weight.Best)
                {
                    foreach (var image in images)
                    {
                        imageResize.ResizeJPG(image.FilePath, Weight.Best);
                    }

                }
                else if (Weight == Weight.Normal)
                {
                    foreach (var image in images)
                    {
                        imageResize.ResizeJPG(image.FilePath, Weight.Normal);
                    }
                }
                else if (Weight == Weight.Extra)
                {
                    foreach (var image in images)
                    {
                        imageResize.ResizeJPG(image.FilePath, Weight.Extra);
                    }
                }
            }
            if (IsCompress)
            {
                if (CompressLevel == CompressLevel.Best)
                {
                    foreach (var image in images)
                    {
                        imageCompressor.JPGCompress(image.FilePath, CompressLevel.Best);
                    }
                }
                else if (CompressLevel == CompressLevel.Normal)
                {
                    foreach (var image in images)
                    {
                        imageCompressor.JPGCompress(image.FilePath, CompressLevel.Normal);
                    }
                }
                else if (CompressLevel == CompressLevel.Extra)
                {
                    foreach (var image in images)
                    {
                        imageCompressor.JPGCompress(image.FilePath, CompressLevel.Extra);
                    }
                }
            }
                removeMetadata.SaveImages();
        });



        internal ObservableCollection<MyImage> GetJPGs()
        {
            ImageInfoHandler imageInfoHandler = new ImageInfoHandler();
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
                        scaler.ConvertToNewSize((jpg.Width, jpg.Height), Weight.Normal);  
                        RaisePropertyChangedEvent(nameof(images));
                }

            }
            return images;
        }
        internal void GetFolder()
        {
            ImageInfoHandler imageInfoHandler = new ImageInfoHandler();

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

