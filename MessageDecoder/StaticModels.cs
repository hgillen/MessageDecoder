using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using MessageDecoder.Models;

namespace MessageDecoder
{
    public static class StaticModels
    {

        public static ObservableCollection<MessageTypeTreeModel> MessageTypes = new ObservableCollection<MessageTypeTreeModel>();

        public static MessageTypeTreeModel SelectedType = new MessageTypeTreeModel();


    }
}
