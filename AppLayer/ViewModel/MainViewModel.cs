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
using SixLabors.ImageSharp;
using ModifierCore.Core.ImageManipulation;
using ModifierCore.Core.Const;
using AppLayer.Services.Handlers;
using AppLayer.Model.Interfaces;
using AppLayer.Services.Handlers.ModifierHandlers;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AppLayer.ViewModel
{
    public class MainViewModel : ObservableObject, IMainViewModel
    {
        public ObservableCollection<MyImage> images { get; set; } = new ();
        private MyImage _selectedImage;

        private RelayCommand addImageCommand;
        private RelayCommand addFolderCommand;
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
        
        #region Properties
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
        #endregion Properties
        #region RelayCommand
        public RelayCommand AddImageCommand => addImageCommand = new RelayCommand(async parameter =>
                    {
                       await GetJPGs();
                    });
        public RelayCommand AddFolderCommand =>
                    addFolderCommand = new RelayCommand(async parameter =>
                   {
                       await GetFolder();
                      
                   });       

        public RelayCommand RemoveImagesCommand => removeImagesCommand = new RelayCommand(parameter =>
                   {
                       DeleteJPGs();
                   });
        public RelayCommand ManipulateCommand => _manipulate = new RelayCommand(async parameter =>
        {
            MainHandler mainHandler = new MainHandler(images, this);
            if (!IsResize && !IsCompress && !IsRemove)
            {
                MessageBox.Show("Ни одна опция не выбрана!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            foreach (MyImage image in images)
            {
                await mainHandler.Processing(image.FilePath);
            }

        });
        public RelayCommand GetExpectedDataCommand => _getExpectedData = new RelayCommand( async parameter =>
        {
            ImageInfoHandler imageInfoHandler = new ImageInfoHandler(this);
            await imageInfoHandler.GetExpectedData(images, Resolution, CompressLevel);
        });
        #endregion RelayCommand
        #region Dialog
        internal async Task <ObservableCollection<MyImage>> GetJPGs()
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
                        var jpg = await imageInfoHandler.GetInfo(file);
                        images.Add(jpg);
                        scaler.ConvertToNewSize((jpg.Width, jpg.Height), SizeScale.Normal);  
                        RaisePropertyChangedEvent(nameof(images));
                }

            }
            return images;
        }
        internal async System.Threading.Tasks.Task GetFolder()
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
                                                        file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                                          .OrderBy(file => GetNaturalSortKey(Path.GetFileNameWithoutExtension(file)));
                ;
                foreach (string file in files)
                    {
                        var jpg = imageInfoHandler.GetInfo(file);

                        images.Add(await jpg);
                        RaisePropertyChangedEvent(nameof(images));
                    }
                }
                
        }
        #endregion Dialog
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
        private static string GetNaturalSortKey(string input)
        {
            return Regex.Replace(input, @"\d+", match => match.Value.PadLeft(10, '0'));
        }
    }

}


