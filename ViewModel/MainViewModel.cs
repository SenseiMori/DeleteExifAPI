using ExifDeleteLib;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using TextRemoveExif.Model;
using TextRemoveExif.Services.Commands;




namespace TextRemoveExif.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private Image _selectedImage;
        private string _selectedFolder;
        private bool _isFolderOpen;
        private RelayCommand addImageCommand;
        private RelayCommand addImageFolderCommand;
        private RelayCommand removeImageCommand;
        private RelayCommand removeMetadataCommand;

        public ObservableCollection<Image> images { get; set; } = new ObservableCollection<Image>();
        public bool IsFolderOpen
        {
            get => _isFolderOpen;
            set
            {     
                _isFolderOpen = value;
                RaisePropertyChangedEvent(nameof(IsFolderOpen));
            }
        }
        public Image selectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                RaisePropertyChangedEvent(nameof(selectedImage));
            }
        }


        public RelayCommand AddImageCommand => addImageCommand = new RelayCommand(parameter =>
                    {
                        AddImage();
                    });

        public RelayCommand AddImageFolderCommand =>
                    addImageFolderCommand = new RelayCommand(parameter =>
                   {
                       AddImageFolder();

                   });

        
        public RelayCommand RemoveImageCommand => removeImageCommand = new RelayCommand(parameter =>
                   {
                       DeleteImage();
                   });
        
        public RelayCommand RemoveMetadataCommand => removeMetadataCommand = new RelayCommand(async parameter =>
                   {
                       RemoveMetadata removeMetadata = new RemoveMetadata(images);
                       removeMetadata.Remove();

                       //RemoveMetadata remove = new RemoveMetadata(images);
                       //await remove.RemoveAllMetadata(_selectedFolder);
                
                      if (IsFolderOpen)
                        OpenfolderWithNewImages(_selectedFolder);
                   }); 


        private void AddImageFolder()
        {
            using var folderDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = folderDialog.FileName;
                var imageFiles = Directory.EnumerateFiles(folder, "*.*")
                 .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase));
                foreach (var file in imageFiles)
                {
                    images.Add(new Image() { FileName = file, FilePath = file });
                    RaisePropertyChangedEvent(nameof(images));
                }     
                _selectedFolder = folder;
            }
        }

        private void AddImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите изображения",
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    images.Add(new Image() { FilePath = file, FileName = file });
                    RaisePropertyChangedEvent(nameof(images));
                }
            }
        }
        private void DeleteImage()
        {
            images.Remove(_selectedImage);
            RaisePropertyChangedEvent(nameof(images));
        }
        private void OpenfolderWithNewImages(string folder)
        {       
                Process.Start(new ProcessStartInfo
                {
                    FileName = folder,
                    UseShellExecute = true
                });
        }



    }
}
