using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MessageDecoder.ViewModels;

namespace MessageDecoder
{
    /// <summary>
    /// Interaction logic for MessageFieldDetailsWindow.xaml
    /// </summary>
    public partial class MessageFieldDetailsWindow : Window
    {
        //public MessageFieldDetailsViewModel vm;
        public MessageFieldDetailsWindow()
        {
            InitializeComponent();

            //vm = new MessageFieldDetailsViewModel();
            //this.DataContext = vm;
            //if (null != vm)
            //    vm.CloseAction = new Action(this.Close);
        }
    }
}
