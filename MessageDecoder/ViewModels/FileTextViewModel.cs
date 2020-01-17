using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

using MessageDecoder.Models;

namespace MessageDecoder.ViewModels
{


    public class DataFileInfo
    {
        public DataFileInfo(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }

    }


    public abstract class DataFile
    {
        public byte[] bytes;
        public string filename;

        /// <summary>
        /// Pull data from the file and return the data as an array of the bytes intended.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public abstract byte[] ParseDataFromFileIntoBytes(string filename);

    }

    public interface IDataFileFactory
    {
        DataFile MakeDataFile(DataFileInfo d);

    }

    public class DataFileFactoryImpl : IDataFileFactory
    {
        public DataFile MakeDataFile(DataFileInfo d)
        {
            if (d.FileName.Contains(".txt"))
            {
                return new AsciiFile(d);
            }
            else if (d.FileName.Contains(".hex"))
            {
                return new HexFile(d);
            }
            else if (d.FileName.Contains(".dat"))
            {
                return new DatFile();
            }
            else
            {
                throw new Exception("No file type selected");
            }
        }

    }


    public class AsciiFile : DataFile
    {

        public AsciiFile(DataFileInfo d)
        {
            filename = d.FileName;
        }

        private void DetermineEncoding()
        {
            foreach (byte b in bytes)
            {
                // If any byte is less than space (ASCII 0x20),
                // it must be raw hex
                if (b < ' ')
                {
                    //return false;
                }
            }
        }

        public override byte[] ParseDataFromFileIntoBytes(string filename)
        {
            byte[] returnBytes = { };
            returnBytes = File.ReadAllBytes(filename);
            for (int i = 0; i < returnBytes.Length; i++)
            {
                if (returnBytes[i] >= 'a' && returnBytes[i] <= 'f')
                    returnBytes[i] = (byte)(returnBytes[i] - 'a' + 10);
                else if (returnBytes[i] >= 'A' && returnBytes[i] <= 'F')
                    returnBytes[i] = (byte)(returnBytes[i] - 'A' + 10);
                else
                    returnBytes[i] = (byte)(returnBytes[i] - '0');
            }
            bytes = returnBytes;
            return returnBytes;
        }

    }

    public class HexFile : DataFile
    {
        public HexFile(DataFileInfo d)
        {
            filename = d.FileName;
        }

        public override byte[] ParseDataFromFileIntoBytes(string filename)
        {
            return File.ReadAllBytes(filename);
        }

    }

    public class DatFile : DataFile
    {

        public override byte[] ParseDataFromFileIntoBytes(string filename)
        {
            throw new NotImplementedException();
        }
    }



    public class BinaryMessageCollection : ObservableCollection<FileTextModel>
    {

    }


    public class FileTextViewModel : BindableBase
    {

        #region Constants
        #endregion

        #region Models

        private BinaryMessageCollection _binaryMessages;
        public BinaryMessageCollection BinaryMessages
        {
            get
            {
                return _binaryMessages;
            }
            set
            {
                SetProperty(ref _binaryMessages, value);
            }
        }

        #endregion

        #region Commands
        #endregion

        #region Properties

        private byte[] _fileData;
        public byte[] FileData
        {
            get
            {
                return _fileData;
            }
            set
            {
                SetProperty(ref _fileData, value);
                //ParseData();
            }
        }


        DataFile MyDataFile;

        #endregion


        public FileTextViewModel()
        {

        }

        public void SetAndDisplayDataFile(string filePath, MessageTypeCollection messageTypes)
        {
            DataFileInfo dataFileInfo = new DataFileInfo(filePath);
            SetDataFile(dataFileInfo);
            MyDataFile.ParseDataFromFileIntoBytes(filePath);
            DisplayDataFile(messageTypes);
        }

        public void SetDataFile(DataFileInfo dataFileInfo)
        {
            DataFileFactoryImpl dataFileFactory = new DataFileFactoryImpl();
            MyDataFile = dataFileFactory.MakeDataFile(dataFileInfo);
        }

        public void DisplayDataFile(MessageTypeCollection messageTypes)
        {
            BinaryMessages = messageTypes.ParseDataFromBytesIntoMessages(MyDataFile.bytes);
        }

    }
}
