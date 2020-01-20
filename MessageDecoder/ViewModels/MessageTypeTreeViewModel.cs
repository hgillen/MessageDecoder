using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using MessageDecoder.Models;

namespace MessageDecoder.ViewModels
{

    public class MessageTypeCollection : ObservableCollection<MessageTypeTreeModel>
    {

        public MessageTypeTreeModel SelectedType;

        public BinaryMessageCollection
            ParseDataFromBytesIntoMessages(byte[] bytes)
        {
            List<byte> subBytes = bytes.ToList();
            //int byteIndex = 0;
            int tempCount = 0;
            BinaryMessageCollection fileTextModels = new BinaryMessageCollection();
            
            while (subBytes.Count > 0)
            {
                tempCount = subBytes.Count;
                foreach (MessageTypeTreeModel messageType in this)
                {
                    // if messageType contains a STF or other constant fields
                    if (messageType.IsMatch(subBytes.ToArray()))
                    {
                        FileTextModel ftm = messageType.GetMatch(subBytes.ToArray());
                        subBytes.RemoveRange(0, ftm.Length);
                        //byteIndex += ftm.Length;
                        fileTextModels.Add(ftm);
                    }
                }
                if (tempCount == subBytes.Count)
                {
                    FileTextModel ftm = new FileTextModel()
                    {
                        RawData = subBytes.ToArray()
                    };
                    fileTextModels.Add(ftm);
                    subBytes.Clear();
                }
            }

            return fileTextModels;
        }


    }

    public class MessageTypeTreeViewModel : BindableBase
    {

        #region Constants

        public const int maxWords = 64;

        #endregion

        #region Models

        public MessageTypeCollection MessageTypes { get; set; }

        public MessageFieldViewModel Vm_MessageFields { get; set; }

        #endregion

        #region Commands

        public MyICommand<MessageTypeTreeModel> SelectedItemChangedCommand { get; set; }
        public MyICommand<string> AddTypeCommand { get; set; }
        public MyICommand<MessageTypeTreeModel> DeleteCommand { get; set; }

        #endregion

        #region Properties

        private int _numWords;
        public int NumWords
        {
            get
            {
                return _numWords;
            }
            set
            {
                SetProperty(ref _numWords, value);
            }
        }

        #endregion

        public MessageTypeTreeViewModel()
        {
            AddTypeCommand = new MyICommand<string>(OnAddType);
            DeleteCommand = new MyICommand<MessageTypeTreeModel>(OnDelete);

            MessageTypes = new MessageTypeCollection
            {
                new MessageTypeTreeModel()
            };

            Vm_MessageFields = new MessageFieldViewModel()
            {
                AddWord = new MyICommand<string>(OnAddWord),
                SubtractWord = new MyICommand<string>(OnSubtractWord)
            };

        }

        public MessageFieldDetailsViewModel GetFieldDetails()
        {
            return Vm_MessageFields.GetFieldDetails();
        }

        public MessageFieldViewModel GetMessageFields()
        {
            return Vm_MessageFields;
        }

        private MessageTypeTreeModel FindSelected(MessageTypeTreeModel node)
        {
            if (null == node)
                return null;
            if (node.IsSelected)
                return node;
            if (null == node.MessageSubtypes)
                return null;

            foreach (MessageTypeTreeModel n in node.MessageSubtypes)
            {
                var found = FindSelected(n);
                if (null != found)
                    return found;
            }

            return null;
        }


        private void OnAddType(string str = "")
        {
            MessageTypes.Add(new MessageTypeTreeModel
            {
                SubtypeName = (str == "") ? "New Type" : str
            });

            OnPropertyChanged("MessageTypes");
        }


        private void OnDelete(MessageTypeTreeModel messageType)
        {
            RemoveMessageType(messageType, MessageTypes);
        }

        private void RemoveMessageType(MessageTypeTreeModel toFind, MessageTypeCollection typeCollection)
        {
            if (typeCollection.Contains(toFind))
            {
                typeCollection.Remove(toFind);
            }
            else
            {
                foreach (MessageTypeTreeModel messageSubtype in typeCollection)
                {
                    RemoveMessageType(toFind, messageSubtype.MessageSubtypes);
                }
            }
        }



        public void OnAddWord(string a = "")
        {
            if (NumWords >= maxWords) return;

            Vm_MessageFields.AddWordFields();

            // Add to WordNumbers collection
            //WordNumbers.Add(NumWords.ToString());

            // Increment field last
            ++NumWords;
        }

        public void OnSubtractWord(string a = "")
        {
            if (NumWords <= 1) return;

            // Decrement field first
            --NumWords;
            Vm_MessageFields.SubtractWordFields();

            // Remove from WordNumbers collection
            //WordNumbers.Remove(NumWords.ToString());

        }

        public void DeselectAllMessageTypes()
        {
            DeselectMessageTypes(MessageTypes);
        }

        private void DeselectMessageTypes(MessageTypeCollection messageTypes)
        {
            foreach (MessageTypeTreeModel messageType in messageTypes)
            {
                messageType.IsSelected = false;
                DeselectMessageTypes(messageType.MessageSubtypes);
            }
        }


        public void Select(MessageTypeTreeModel messageType)
        {
            MessageTypes.SelectedType = messageType;
            messageType.ByteCount = 2 * NumWords;
            messageType.Select();
        }


    }
}
