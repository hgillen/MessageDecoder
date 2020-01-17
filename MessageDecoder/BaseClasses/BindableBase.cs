using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MessageDecoder
{
   public abstract class BindableBase : INotifyPropertyChanged
   {
      public event PropertyChangedEventHandler PropertyChanged;

      public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
      {
         if (EqualityComparer<T>.Default.Equals(storage, value))
            return false;
         storage = value;
         this.OnPropertyChanged(propertyName);
         return true;
      }

   }
}
