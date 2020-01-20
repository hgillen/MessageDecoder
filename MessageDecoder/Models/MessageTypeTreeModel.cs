using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

using MessageDecoder.ViewModels;

namespace MessageDecoder.Models
{
    public class MessageTypeTreeModel : BindableBase
    {

        #region Constants
        const string nameString = "Name";
        #endregion

        #region Models

        public MessageTypeCollection MessageSubtypes { get; set; }

        public FieldCollection MessageFields { get; set; }

        #endregion

        #region Commands

        public MyICommand<string> RenameCommand { get; set; }


        //public MyICommand<MessageTypeTreeModel> DeleteCommand { get; set; }
        public MyICommand<MessageTypeTreeModel> AddSubtypeCommand { get; set; }

        #endregion

        #region Properties

        private MessageTypeTreeViewModel _treeRoot;
        public MessageTypeTreeViewModel TreeRoot
        {
            get
            {
                return _treeRoot;
            }
            set
            {
                SetProperty(ref _treeRoot, value);
            }
        }

        private string _subtypeName;
        public string SubtypeName
        {
            get
            {
                return _subtypeName;
            }
            set
            {
                if (UseEditTemplate) UseEditTemplate = false;
                SetProperty(ref _subtypeName, value);
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                //StaticModels.SelectedType = this;
                return _isSelected;
            }
            set
            {
                if (value)
                {
                    //MainParent.SelectedMessageType = this;
                    //MainParent.CurrentType = this;
                    //MainParent.MessageFields = MessageFields;
                }
                SetProperty(ref _isSelected, value);
                //TreeRoot.SelectedMessageType = this;
            }
        }

        private bool _useEditTemplate;
        public bool UseEditTemplate
        {
            get
            {
                return _useEditTemplate;
            }
            set
            {
                SetProperty(ref _useEditTemplate, value);
            }
        }

        private int _byteCount;
        public int ByteCount
        {
            get
            {
                return _byteCount;
            }
            set
            {
                SetProperty(ref _byteCount, value);
            }
        }


        #endregion


        public MessageTypeTreeModel()
        {
            RenameCommand = new MyICommand<string>(OnRename);
            //DeleteCommand = new MyICommand<MessageTypeTreeModel>(OnDelete);
            AddSubtypeCommand = new MyICommand<MessageTypeTreeModel>(OnAddSubtype);
            SubtypeName = "New Type";

            MessageFields = new FieldCollection
            {
                new MessageFieldModel()
            };

            while (MessageFields.Count < 32)
            {
                MessageFields.Add(new MessageFieldModel());
                MessageFields[MessageFields.Count - 1].MessageFieldDetails.BitStart = (MessageFields.Count - 1) % 16;
                MessageFields[MessageFields.Count - 1].MessageFieldDetails.WordNum = (MessageFields.Count - 1) / 16;
            }
            ByteCount = 4;

            MessageSubtypes = new MessageTypeCollection();
        }


        private void OnRename(string newName)
        {
            UseEditTemplate = true;

        }

        private void OnAddSubtype(MessageTypeTreeModel mttm = null)
        {
            if (null == MessageSubtypes)
            {
                MessageSubtypes = new MessageTypeCollection();
            }

            MessageSubtypes.Add(new MessageTypeTreeModel
            {
                SubtypeName = "New Subtype",
            });

            OnPropertyChanged("MessageSubtypes");
        }


        /// <summary>
        /// Save the current type as a string of field details.
        /// </summary>
        /// <returns>XAML formatted string of field details.</returns>
        public string SaveConfig()
        {
            string str;

            str = "<Type " + nameString + "=\"" + SubtypeName + "\">\n";

            // Save Fields
            foreach (MessageFieldModel messageFieldModel in MessageFields)
            {
                str += "\t" + messageFieldModel.SaveConfig();
            }

            // Save Subtypes
            if (MessageSubtypes?.Count > 0)
            {
                foreach (MessageTypeTreeModel messageTypeTreeModel in MessageSubtypes)
                {
                    str += "\t" + messageTypeTreeModel.SaveConfig();
                }
            }

            str += "<\\Type>\n";

            return str;
        }

        /// <summary>
        /// Read XAML formatted text into message types.
        /// </summary>
        /// <param name="contents"></param>
        public void ReadConfig(string contents)
        {
            Regex typeDetails = new Regex(@"(\w+)\s*=\s*""([\w\s]+)"""),
                fieldReg = new Regex(@"<\s*Field\s+(.*?)\\\s*(Field\s*)?>", RegexOptions.Singleline);
            MatchCollection detailMatches, fieldMatches;
            MessageFieldModel messageFieldModel;

            // Return if invalid
            if (null == contents) return;

            // Set general Type details
            if (typeDetails.IsMatch(contents))
            {
                detailMatches = typeDetails.Matches(contents);
                ReadProperties(detailMatches);
            }

            // Define fields for each field found
            if (fieldReg.IsMatch(contents))
            {
                fieldMatches = fieldReg.Matches(contents);
                foreach (Match fieldMatch in fieldMatches)
                {
                    messageFieldModel = new MessageFieldModel();
                    messageFieldModel.ReadConfig(fieldMatch.Groups[1].Value);

                    MessageFields.Add(messageFieldModel);
                }
            }

        }

        private void ReadProperties(MatchCollection detailMatches)
        {
            foreach (Match detailMatch in detailMatches)
            {
                if (detailMatch.Groups.Count < 3) continue;

                if (nameString == detailMatch.Groups[1].Value)
                {
                    SubtypeName = ("" != detailMatch.Groups[2].Value) ?
                        detailMatch.Groups[2].Value : "New Type";
                }
            }
        }


        public bool IsMatch(byte[] bytes)
        {
            return ByteCount <= bytes.Length;
            //return MessageFields.IsMatch(bytes);
        }



        public FileTextModel GetMatch(byte[] bytes)
        {
            FileTextModel fileTextModel = new FileTextModel();
            ByteCount = 32;
            byte[] byteSubset = new byte[ByteCount];
            int startIndex = 0;

            //if (this.containsStf())
            //{
            //    // find stf in bytes
            //    for (int i = 0; i < bytes.Length; ++i)
            //    {
            //        if (this.STF == bytes[i])
            //        {
            //            startIndex = i;
            //            break;
            //        }
            //    }
            //}

            //for (int i = 0; i < ByteCount; i++)
            //{
            //    byteSubset[i] = bytes[i];
            //}

            Array.Copy(bytes, 0, byteSubset, 0, ByteCount);
            fileTextModel.RawData = byteSubset;
            return fileTextModel;
        }



        public void Select()
        {
            IsSelected = true;
        }




    }
}
