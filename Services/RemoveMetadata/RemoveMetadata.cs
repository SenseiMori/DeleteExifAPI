using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TextRemoveExif.Model;
using TextRemoveExif.ViewModel;
using System.Windows;

namespace TextRemoveExif
{
    class RemoveMetadata
    {
        private ObservableCollection<Image> _images { get; set; }
        public RemoveMetadata(ObservableCollection<Image> images)
        {
            _images = images;
        }
        public async Task RemoveAllMetadata(string pathToFolder)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            string nameZip = Path.GetFileName(pathToFolder) + ".zip";
            string[] pathToImages = GetPaths(_images);
            string[] newImages = await RemoveMetadataFromMultipleImages(pathToImages, pathToFolder);
            await CreateZip(newImages, nameZip, pathToFolder);

            stopwatch.Stop();
            string logLine = Path.Combine(pathToFolder, "log.txt");
            using (StreamWriter sw = new StreamWriter(logLine, append:true))
            {
                sw.WriteLine(stopwatch.ElapsedMilliseconds.ToString() + " " + DateTime.Now);
                sw.Close();
            }
        }

        public async Task Remove(string imagePath, string pathNoExif)
        {
                    var process = new Process
                    {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "exiftool.exe"),
                        Arguments = $"-all= \"{imagePath}\" -o \"{pathNoExif}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                    };
                process.ErrorDataReceived += new DataReceivedEventHandler(ETErrorHandler);
                process.Start();
                await process.WaitForExitAsync().ConfigureAwait(false); 
        }

        private void LogToFile (string pathToLogFile, string message)
        {
            try
            {
                using (StreamWriter stream = new StreamWriter(pathToLogFile, true))
                {
                    stream.WriteLine(message);
                }

            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString() + "Ошибка записи логов в файл");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "Ошибка записи логов");
            }

        }

        private void ETErrorHandler(object sendingProcess, DataReceivedEventArgs errLine)
        {
            if (!String.IsNullOrEmpty(errLine.Data))
            {
                //LogToFile(GetPaths(_images));
                
            }
        }

        public async Task<string[]> RemoveMetadataFromMultipleImages(string[] pathToImages, string outputDir)
        {
            List<string> images = new List<string>();
            var tasks = new List<Task>();

            string outDirectory = CreateDirectoryWithNewImages(outputDir);
            foreach (string path in pathToImages)
            {
                string outputFileName = Path.GetFileName(path);
                string outputPath = Path.Combine(outDirectory, outputFileName);
                tasks.Add(Remove(path, outputPath));
                images.Add(outputPath);
            }
            await Task.WhenAll(tasks).ConfigureAwait(false);
            return images.ToArray();

        }
        public string CreateDirectoryWithNewImages(string oldDirectoryName)
        {
            string oldDirectoryPath = Path.GetFullPath(oldDirectoryName);
            string newDirectoryName = Path.Combine(
                 Path.GetFullPath(oldDirectoryName),
                 Path.GetFileName(oldDirectoryPath)
                 );
            Directory.CreateDirectory(newDirectoryName);
            return newDirectoryName;
        }
        private string[] GetPaths(ObservableCollection<Image> images) => images.Select(image => image.FilePath).ToArray();
        public async Task CreateZip(string[] inputFiles, string newZipName, string resultDirectory)
        {
            {
                string zipPath = Path.Combine(resultDirectory, newZipName);
                using var outStream = new FileStream(zipPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 4096, true);
                using var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true);
                foreach (string fileName in inputFiles)
                {
                    using (var inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                    {
                        ZipArchiveEntry fileEntry = archive.CreateEntry(Path.GetFileName(fileName));
                        using var entryStream = fileEntry.Open();
                        await inputStream.CopyToAsync(entryStream);
                        
                    }
                }
            }
        }  
    }
}
