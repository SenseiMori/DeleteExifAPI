using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExifDeleteLib.Core
{

    public class JPGFile
    {
        private readonly HashSet<byte> _markers = new JPGMarkers().markers;

        public byte[] GetJPGWithoutAppSegments(byte [] originData) 
        {
            JPGMarkers jPGMarkers = new JPGMarkers();

            List<byte> cleanImageData = new List<byte>();
            using (MemoryStream ms = new MemoryStream(originData))
            {
                using (BinaryReader binaryReader = new BinaryReader(ms))
                {
                    byte[] buffer = new byte[2];
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        int read = binaryReader.Read(buffer, 0, buffer.Length);
                        if (buffer[0] == 0xFF && _markers.Contains(buffer[1]))
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
        public List<byte> GetMarkersAppSegment(string file)
        {
            List<byte> markers = new List<byte>();
            JPGMarkers jPGMarkers = new JPGMarkers();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (var binaryReader = new BinaryReader(fs))
                {
                    byte[] buffer = new byte[2];
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        int read = binaryReader.Read(buffer, 0, buffer.Length);
                        if (buffer[0] == 0xFF && _markers.Contains(buffer[1]))
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
