using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using TextRemoveExif.Model.Entities;
using TextRemoveExif.Services.Commands;
using ExifDeleteLib;
using SixLabors.ImageSharp;
using TextRemoveExif.Services.ImageManipulation;
using Windows.Devices.Scanners;
using Windows.Storage.Search;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;





namespace TextRemoveExif.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private RelayCommand addImageCommand;
        private RelayCommand addFolderCommand;
        private RelayCommand removeImageCommand;
        private RelayCommand removeMetadataCommand;
        private RelayCommand removeImagesCommand;
        private RelayCommand createZipInCurrentFolder;
        private RelayCommand _resizeJPG;
        private bool _isFolderOpen;
        private bool _isCreateZip;
        private bool _isResize;
        private Weight _weight;
        private Weight _isBest; 

        private readonly string[] allowedExtensions = [".jpg", ".jpeg"];
        private MyImage _selectedImage;
        private string _selectedFolder;

        public ObservableCollection<MyImage> images { get; set; } = new ObservableCollection<MyImage>();

        public Weight Weight 
        { 
            get => _weight; 
            set
            {
                _weight = value;
                RaisePropertyChangedEvent(nameof(Weight));
                RaisePropertyChangedEvent(nameof(IsBest));
                RaisePropertyChangedEvent(nameof(IsNormal));
                RaisePropertyChangedEvent(nameof(IsExtra));
            }
        
        }

        public bool IsBest
        {
            get => Weight == Weight.Best;
            set
            {
                if (value)
                    Weight = Weight.Best;
                    RaisePropertyChangedEvent(nameof(IsBest));
            }
        }
        public bool IsNormal
        {
            get => Weight == Weight.Normal;
            set
            {
                if (value)
                    Weight = Weight.Normal;
                    RaisePropertyChangedEvent(nameof(IsNormal));
            }
        }
        public bool IsExtra
        {
            get => Weight == Weight.Extra;
            set
            {
                if (value)
                    Weight = Weight.Extra;
                    RaisePropertyChangedEvent(nameof(IsExtra));
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
        public bool IsResize
        {
            get => _isResize;
            set
            {
                _isResize = value;
                RaisePropertyChangedEvent(nameof(_isResize));
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
        public RelayCommand RemoveMetadataCommand => removeMetadataCommand = new RelayCommand(parameter =>
                   {
                       MetadataRemover removeMetadata = new MetadataRemover(images);

                       removeMetadata.Remove();
                       if (_isFolderOpen)
                           OpenNewJPGFolder();
                       if(_isCreateZip)
                            removeMetadata.CreateZip();
                   });

        public RelayCommand ResizeJPGCommand => _resizeJPG = new RelayCommand(parameter => 
        {
            ImageResize imageResize = new ImageResize(images);
            if (Weight == Weight.Best)
                {
                    imageResize.ResizeJPG(images, Weight.Best);

                }
            else if (Weight == Weight.Normal)
            {
                imageResize.ResizeJPG(images, Weight.Normal);

            }
            else if (Weight == Weight.Extra)
            {
                imageResize.ResizeJPG(images, Weight.Extra);

            }

            MetadataRemover removeMetadata = new MetadataRemover(images);
            removeMetadata.SaveImages();
            // Обработчик нажатия кнопки

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

