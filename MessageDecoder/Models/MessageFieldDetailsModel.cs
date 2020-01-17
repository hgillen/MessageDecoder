using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

using MessageDecoder.ViewModels;

namespace MessageDecoder.Models
{
    public class MessageFieldDetailsModel : BindableBase
    {

        #region Constants
        const string nameString = "Name", wordString = "Word", bitString = "Bit", lengthString = "Length";

        #endregion

        #region Models

        public MainViewModel Parent { get; set; }

        public MessageTypeTreeModel MessageTypeParent { get; set; }

        #endregion

        #region Commands

        public MyICommand<MessageFieldDetailsModel> MessageDetailsCommand { get; set; }

        #endregion

        #region Properties

        private string _fieldName;
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                SetProperty(ref _fieldName, value);
            }
        }

        private int _wordNum;
        public int WordNum
        {
            get
            {
                return _wordNum;
            }
            set
            {
                SetProperty(ref _wordNum, value);
            }
        }

        private int _maxWordStart;
        public int MaxWordStart
        {
            get
            {
                return _maxWordStart;
            }
            set
            {
                SetProperty(ref _maxWordStart, value);
            }
        }


        private int _bitStart;
        public int BitStart
        {
            get
            {
                return _bitStart;
            }
            set
            {
                if (value > MaxBitStart) value = MaxBitStart;
                if (value < 0) value = 0;

                MaxLength = MaxLength - value + BitStart;
                if (BitLength > MaxLength) BitLength = MaxLength;
                SetProperty(ref _bitStart, value);
            }
        }

        private int _bitLength;
        public int BitLength
        {
            get
            {
                return _bitLength;
            }
            set
            {
                if (value > MaxLength) value = MaxLength;
                if (value < 1) value = 1;
                SetProperty(ref _bitLength, value);
                OnPropertyChanged("BitLengthString");
            }
        }

        public string BitLengthString
        {
            get
            {
                return _bitLength.ToString();
            }
            set
            {
                if (Int32.TryParse(value, out int tempInt) && tempInt > 0)
                {
                    BitLength = tempInt;
                }
            }
        }

        private int _maxBitStart;
        public int MaxBitStart
        {
            get
            {
                return _maxBitStart;
            }
            set
            {
                SetProperty(ref _maxBitStart, value);
            }
        }

        private int _maxLength;
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                SetProperty(ref _maxLength, value);
            }
        }


        private bool _isSpecialType;
        public bool IsSpecialType
        {
            get
            {
                return _isSpecialType;
            }
            set
            {
                SetProperty(ref _isSpecialType, value);
                if (!value)
                {
                    SpecialTypeVisibility = Visibility.Collapsed;
                }
                else if (value && SpecialType != "")
                {
                    SpecialTypeVisibility = Visibility.Visible;
                }
            }
        }

        private string _specialType;
        public string SpecialType
        {
            get
            {
                return _specialType;
            }
            set
            {
                SetProperty(ref _specialType, value);
                if (Visibility.Collapsed == SpecialTypeVisibility)
                {
                    SpecialTypeVisibility = Visibility.Visible;
                }
            }
        }

        private Visibility _specialTypeVisibility;
        public Visibility SpecialTypeVisibility
        {
            get
            {
                return _specialTypeVisibility;
            }
            set
            {
                SetProperty(ref _specialTypeVisibility, value);
            }
        }

        private bool _constantValueEnabled;
        public bool ConstantValueEnabled
        {
            get
            {
                return _constantValueEnabled;
            }
            set
            {
                SetProperty(ref _constantValueEnabled, value);
            }
        }

        private int _constantValue;
        public int ConstantValue
        {
            get
            {
                return _constantValue;
            }
            set
            {
                SetProperty(ref _constantValue, value);
            }
        }

        private bool _isDuplicable;
        public bool IsDuplicable
        {
            get
            {
                return _isDuplicable;
            }
            set
            {
                SetProperty(ref _isDuplicable, value);
            }
        }



        #endregion


        public MessageFieldDetailsModel()
        {
            FieldName = "New Field";
            WordNum = 0;
            MaxWordStart = 1;
            BitStart = 0;
            BitLength = 1;
            MaxBitStart = 15;
            MaxLength = 16;

            //MessageDetailsCommand = new MyICommand<MessageFieldDetailsModel>(OnMessageDetailsCommand);

            //MessageTypeParent = parent;
        }

        public MessageFieldDetailsModel(MessageFieldDetailsModel copy)
        {
            MaxWordStart = copy.MaxWordStart;
            MaxBitStart = copy.MaxBitStart;
            MaxLength = copy.MaxLength;

            FieldName = copy.FieldName;
            WordNum = copy.WordNum;
            BitStart = copy.BitStart;
            BitLength = copy.BitLength;

            IsSpecialType = copy.IsSpecialType;
            SpecialType = copy.SpecialType;
            SpecialTypeVisibility = copy.SpecialTypeVisibility;
        }



        /// <summary>
        /// Save the current field as a string of details.
        /// </summary>
        /// <returns>XAML formatted string of field details.</returns>
        public string SaveConfig()
        {
            string str;

            str = "<Field " + nameString + "=\"" + FieldName + "\"";
            str += " " + wordString + "=\"" + WordNum.ToString() + "\"";
            str += " " + bitString + "=\"" + BitStart.ToString() + "\"";
            str += " " + lengthString + "=\"" + BitLength.ToString() + "\"";

            if (IsSpecialType)
                str += " Special=\"" + SpecialType + "\"";

            str += "\\>\n";

            return str;
        }

        /// <summary>
        /// Read in MessageFieldDetails parameters from a formatted string.
        /// </summary>
        /// <param name="contents"></param>
        public void ReadConfig(string contents)
        {
            Regex fieldDetails = new Regex(@"\s*(\w+)\s*=\s*""([\w\s]+)""");
            MatchCollection detailMatches;

            // Check each fieldMatch for the Field Details format
            if (fieldDetails.IsMatch(contents))
            {
                detailMatches = fieldDetails.Matches(contents);
                foreach (Match detailMatch in detailMatches)
                {
                    // If we didn't capture the right thing, skip it
                    if (detailMatch.Groups.Count < 3) continue;

                    if (nameString == detailMatch.Groups[1].Value)
                    {
                        FieldName = detailMatch.Groups[2].Value;
                    }
                    else if (wordString == detailMatch.Groups[1].Value)
                    {
                        WordNum = int.Parse(detailMatch.Groups[2].Value);
                    }
                    else if (bitString == detailMatch.Groups[1].Value)
                    {
                        BitStart = int.Parse(detailMatch.Groups[2].Value);
                    }
                    else if (lengthString == detailMatch.Groups[1].Value)
                    {
                        BitLength = int.Parse(detailMatch.Groups[2].Value);
                    }

                    // TODO Validate
                }
            }

        }


    }
}
