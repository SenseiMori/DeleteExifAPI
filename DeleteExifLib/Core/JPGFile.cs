using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ExifDeleteLib.Core
{
    /*
     * 1. Получить маркеры. Вернуть: есть ли маркер в изображении. Его позиция. Длина маркера.
     * 2. Вернуть эту информацию в модель как наличие/отсутствие маркеров.
     * 3. Если есть запрос на удаление — удаление
     * 
     */

    public class JPGFile
    {
        //public List<byte> cleanImageData;
        public byte[] FindMarkers(string file)
        {
            JPGMarkers jPGMarkers = new JPGMarkers();

            List<byte> cleanImageData = new List<byte>();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (var binaryReader = new BinaryReader(fs))
                {
                    byte[] buffer = new byte[2];
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        int read = binaryReader.Read(buffer, 0, buffer.Length);
                        if (buffer[0] == 0xFF && jPGMarkers.markers.Contains(buffer[1]))
                        {
                            int appLength = binaryReader.ReadUInt16();
                            int reversBytes = ShiftBytes(appLength);
                            binaryReader.BaseStream.Seek(reversBytes - 2, SeekOrigin.Current);
                        }
                        else if (read == 1)
                        {
                            cleanImageData.Add(buffer[0]);
                        }
                        else
                        {
                            cleanImageData.Add(buffer[0]);
                            cleanImageData.Add(buffer[1]);
                        }
                    }
                }
                return cleanImageData.ToArray();
            }
        }
        public List <byte> GetMarkers(string file)
        {
            List <byte> markers = new List <byte>();
            JPGMarkers jPGMarkers = new JPGMarkers();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (var binaryReader = new BinaryReader(fs))
                {
                    byte[] buffer = new byte[2];
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        int read = binaryReader.Read(buffer, 0, buffer.Length);
                        if (buffer[0] == 0xFF && jPGMarkers.markers.Contains(buffer[1]))
                        {
                            int appLength = binaryReader.ReadUInt16();
                            int reversBytes = ShiftBytes(appLength);
                            markers.Add(buffer[1]);
                        }
                    }
                    return markers;
                }
            }
        }

        public ushort ShiftBytes(int value)
        {
            byte secondByte = (byte)(value & 0xFF); // в переменную записываются последние 8 бит 1110_0001 то есть A1
            int firstByte = value >> 8; // в переменную записываются первые 8 бит 1110_1010 то есть EA
            int result = secondByte << 8 | firstByte;
            return (ushort)result;
        }
        public string WriteClearDataToNewFile(byte[] data, string pathToNewFile)
        {
            using (var fileStream = new FileStream(pathToNewFile, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                fileStream.Write(data, 0, data.Length);
            }
            return pathToNewFile;
        }
    }
}
