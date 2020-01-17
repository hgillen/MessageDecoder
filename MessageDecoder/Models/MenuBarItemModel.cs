using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MessageDecoder.Models
{
    public class MenuBarItemModel : BindableBase
    {
        #region Constants
        #endregion

        #region Models

        public ObservableCollection<MenuBarItemModel> ChildMenus { get; set; }

        #endregion

        #region Commands

        public MyICommand<string> Command { get; set; }

        #endregion

        #region Properties

        private string _header;
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        #endregion




        public MenuBarItemModel()
        {
            //Command = new MyICommand<string>(OnCommand);
        }


        private void OnCommand(string selection)
        {
            switch (selection)
            {
                case "Open File":
                    Header = "Done";
                    break;

                default:
                    break;
            }
        }

    }
}
