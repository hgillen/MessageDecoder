using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MessageDecoder.Models
{
    public class MessageFieldModel : BindableBase
    {
        #region Constants
        #endregion

        #region Models

        public MessageFieldDetailsModel CurrentField { get; set; }

        public MessageFieldDetailsModel MessageFieldDetails { get; set; }

        #endregion

        #region Commands

        public MyICommand<string> ButtonPress { get; set; }

        #endregion

        #region Properties

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }

        private int _zIndex;
        public int ZIndex
        {
            get
            {
                return _zIndex;
            }
            set
            {
                SetProperty(ref _zIndex, value);
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                SetProperty(ref _isEnabled, value);
            }
        }

        #endregion

        //public MessageFieldModel()
        //{
        //    MessageFieldDetails = new MessageFieldDetailsModel
        //    {
        //        FieldName = "New Field"
        //    };

        //    CurrentField = MessageFieldDetails;
        //}


        public MessageFieldModel()
        {
            MessageFieldDetails = new MessageFieldDetailsModel()
            {
                FieldName = "New Field"
            };

            CurrentField = MessageFieldDetails;
            ZIndex = 0;
            IsEnabled = true;
        }


        // Copy Constructor
        public MessageFieldModel(MessageFieldModel copy)
        {
            MessageFieldDetails = new MessageFieldDetailsModel(copy.MessageFieldDetails);
            CurrentField = MessageFieldDetails;
            ZIndex = copy.ZIndex;
        }

        /// <summary>
        /// Save the current field as a string of details.
        /// </summary>
        /// <returns>XAML formatted string of field details.</returns>
        public string SaveConfig()
        {
            return MessageFieldDetails.SaveConfig();
        }

        /// <summary>
        /// Read in MessageFieldDetails parameters from a formatted string.
        /// </summary>
        /// <param name="contents"></param>
        public void ReadConfig(string contents)
        {
            MessageFieldDetails.ReadConfig(contents);
        }

    }
}
