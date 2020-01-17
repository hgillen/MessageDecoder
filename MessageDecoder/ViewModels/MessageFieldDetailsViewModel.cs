using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageDecoder.Models;

namespace MessageDecoder.ViewModels
{
    public class MessageFieldDetailsViewModel : BindableBase
    {
        #region Constants
        #endregion

        #region Models

        public MessageFieldDetailsModel FieldDetails { get; set; }

        #endregion

        #region Commands

        public MyICommand<MessageFieldDetailsModel> ButtonPress { get; set; }

        #endregion

        #region Properties
        #endregion

        public MessageFieldDetailsViewModel()
        {

        }

        public void SetFieldDetails(MessageFieldDetailsModel fieldDetails)
        {
            FieldDetails = fieldDetails;
        }





    }
}
