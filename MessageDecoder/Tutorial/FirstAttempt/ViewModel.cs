using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MessageDecoder
{
   public interface IMessageType
   {
      string FullPath { get; }
      string Label { get; }
      List<IMessageType> Subtypes { get; set; }
   }

   class MessageType : IMessageType
   {
      public string FullPath { get; set; }
      public string Label { get; set; }
      public List<IMessageType> Subtypes { get; set; }
   }


   class ViewModel : INotifyPropertyChanged
   {
      public ViewModel()
      {
         TEST = "jubba";

         m_messageTypes = new List<IMessageType>();

         //add Root items
         MessageTypes.Add(new MessageType { Label = "Dummy1", FullPath = @"C:\dummy1" });
         MessageTypes.Add(new MessageType { Label = "Dummy2", FullPath = @"C:\dummy2" });
         MessageTypes.Add(new MessageType { Label = "Dummy3", FullPath = @"C:\dummy3" });
         MessageTypes.Add(new MessageType { Label = "Dummy4", FullPath = @"C:\dummy4" });

         //add sub items
         MessageTypes[0].Subtypes = new List<IMessageType>();
         MessageTypes[0].Subtypes.Add(new MessageType { Label = "Dummy11", FullPath = @"C:\dummy11" });
         MessageTypes[0].Subtypes.Add(new MessageType { Label = "Dummy12", FullPath = @"C:\dummy12" });
         MessageTypes[0].Subtypes.Add(new MessageType { Label = "Dummy13", FullPath = @"C:\dummy13" });
         MessageTypes[0].Subtypes.Add(new MessageType { Label = "Dummy14", FullPath = @"C:\dummy14" });

      }

      public string TEST { get; set; }


      private List<IMessageType> m_messageTypes;
      public List<IMessageType> MessageTypes
      {
         get { return m_messageTypes; }
         set
         {
            m_messageTypes = value;
            NotifyPropertyChanged("MessageTypes");
         }
      }

      void NotifyPropertyChanged(string property)
      {
         if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(property));
      }

      public event PropertyChangedEventHandler PropertyChanged;




      //public void RenameTreeViewItem(object sender, RoutedEventArgs e)
      //{

      //}

   }
}
