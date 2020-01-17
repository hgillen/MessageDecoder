using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MessageDecoder.Models;

namespace MessageDecoder.ViewModels
{
    public class MessageStructureViewModel : BindableBase
    {
        #region Constants
        #endregion

        #region Models

        public MessageTypeTreeViewModel Vm_MessageTypeTree { get; set; }

        public MessageFieldViewModel Vm_MessageFields { get; set; }


        #endregion

        #region Commands
        #endregion

        #region Properties

        private MessageTypeTreeModel _selectedItem;
        public MessageTypeTreeModel SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        #endregion

        public MessageStructureViewModel()
        {
            Vm_MessageTypeTree = new MessageTypeTreeViewModel();

            Vm_MessageFields = Vm_MessageTypeTree.GetMessageFields();
        }

        public MessageFieldDetailsViewModel GetFieldDetails()
        {
            return Vm_MessageFields.GetFieldDetails();
        }

        public void DeselectAllMessageTypes()
        {
            Vm_MessageTypeTree.DeselectAllMessageTypes();
        }

        public void Select(MessageTypeTreeModel messageType)
        {
            Vm_MessageTypeTree.Select(messageType);
            Vm_MessageFields.AddWord = new MyICommand<string>(Vm_MessageTypeTree.OnAddWord);
            Vm_MessageFields.SubtractWord = new MyICommand<string>(Vm_MessageTypeTree.OnSubtractWord);

            SetMessageFields(messageType.MessageFields);
        }

        public MessageFieldDetailsModel ViewMessageField(MessageTypeTreeModel selectedType, MessageFieldModel mfm)
        {
            MessageFieldModel messageField;
            FieldCollection messageFields;

            messageFields = CopyFieldCollection(selectedType);
            SetMessageFields(messageFields);
            messageField = Vm_MessageFields.PrioritizeField(mfm);
            // Prevent multiple selections
            SetButtons(false);

            return messageField.MessageFieldDetails;
        }

        /// <summary>
        /// Copies the fields of selectedType into a returned collection, with each button disabled.
        /// </summary>
        /// <param name="selectedType"></param>
        /// <returns></returns>
        private FieldCollection CopyFieldCollection(MessageTypeTreeModel selectedType)
        {
            FieldCollection mfms = new FieldCollection();
            foreach (MessageFieldModel field in selectedType.MessageFields)
            {
                mfms.Add(new MessageFieldModel(field)
                {
                    IsEnabled = false
                });
            }

            return mfms;
        }

        public void SetMessageFields(FieldCollection messageFields)
        {
            MessageFieldModel messageFieldModel;
            // Get the number of words from the last field in the list
            Vm_MessageTypeTree.NumWords = 0;
            messageFieldModel = Vm_MessageTypeTree.MessageTypes.SelectedType.MessageFields[Vm_MessageTypeTree.MessageTypes.SelectedType.MessageFields.Count - 1];
            //Vm_MessageFields.MessageFields.Clear();
            Vm_MessageTypeTree.NumWords = Vm_MessageFields.NumWords =
                messageFieldModel.MessageFieldDetails.WordNum + 1;
            // WordNum will be 1 less than NumWords due to 0- / 1-indexing
            //while (messageFieldModel.MessageFieldDetails.WordNum >= Vm_MessageTypeTree.NumWords)
            //{
            //    ((ICommand)Vm_MessageFields.AddWord).Execute(null);
            //}
            // TODO Always Add?

            Vm_MessageFields.SetMessageFields(messageFields);
        }

        public void SetButtons(bool value)
        {
            Vm_MessageFields.SubtractIsEnabled = value;
            Vm_MessageFields.TextIsEnabled = value;
            Vm_MessageFields.AddIsEnabled = value;
        }

        public MessageFieldViewModel GetMessageFields()
        {
            return Vm_MessageFields;
        }

        public MessageTypeCollection GetMessageTypes()
        {
            return Vm_MessageTypeTree.MessageTypes;
        }
    }
}
