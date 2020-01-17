using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using Microsoft.Win32;

using MessageDecoder.Models;

namespace MessageDecoder.ViewModels
{
    public class MenuBarItemViewModel : BindableBase
    {
        #region Constants
        #endregion

        #region Models

        public ObservableCollection<MenuBarItemModel> TopMenu { get; set; }

        #endregion


        #region Commands

        public MyICommand<string> OpenConfig { get; private set; }

        #endregion

        #region Properties
        #endregion




        public MenuBarItemViewModel()
        {
            OpenConfig = new MyICommand<string>(OnOpenConfig);

            //TopMenu = new ObservableCollection<MenuBarItemModel>
            //{
            //    new MenuBarItemModel
            //    {
            //        Header = "File",
            //        ChildMenus = new ObservableCollection<MenuBarItemModel>
            //        {
            //            new MenuBarItemModel
            //            {
            //                Header = "Open Data File"
            //            },
            //            new MenuBarItemModel
            //            {
            //                Header = "Load Configuration"
            //            },
            //            new MenuBarItemModel
            //            {
            //                Header = "Save Configuration"
            //            }
            //        }
            //    }
            //};

        }


        public void OnOpenConfig(string str = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "XAML files | (*.xml | *.xaml)";
            
            if (true == openFileDialog.ShowDialog())
            {

            }


        }








    }
}
