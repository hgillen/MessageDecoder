using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace MessageDecoder
{
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs>
            ErrorsChanged = delegate { };

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                return _errors[propertyName];
            }
            else
            {
                return null;
            }
        }

        public bool HasErrors
        {
            get
            {
                return _errors.Count > 0;
            }
        }

        protected override bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (base.SetProperty(ref member, val, propertyName))
            {
                ValidateProperty(propertyName, val);
                return true;
            }
            return false;
        }

        private void ValidateProperty<T>(string propertyName, T value)
        {
            var result = new List<ValidationResult>();

            if (result.Any())
            {

            }
            else
            {
                _errors.Remove(propertyName);
            }

            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }


    }
}
