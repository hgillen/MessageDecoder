using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDecoder.Models
{
    public class FileTextModel : BindableBase
    {

        #region Constants
        #endregion

        #region Models

        public readonly MessageTypeTreeModel messageType;

        #endregion

        #region Commands
        #endregion

        #region Properties

        private byte[] _rawData;
        public byte[] RawData
        {
            get
            {
                //string str = "";
                //foreach (byte b in _rawData)
                //{
                //    str += " " + String.Format("{0,02x}", b.ToString());
                //}
                //str = str.Substring(1);
                //return str;
                return _rawData;
            }
            set
            {
                SetProperty(ref _rawData, value);
                OnPropertyChanged("StringData");
            }
        }

        public string StringData
        {
            get
            {
                string str = "";
                foreach (byte b in _rawData)
                {
                    str += " " + String.Format("{0,2:X2}", b);
                }
                str = str.Substring(1);
                return str;
            }
        }

        // Unused?
        private string fileText = "Hello World";
        public string FileText
        {
            get { return "Hello World"; }
            set { fileText = value; }
        }

        #endregion



        public FileTextModel()
        {
            _rawData = new byte[]{ 9, 8, 7, 6};
        }



    }
}
