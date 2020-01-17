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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MessageDecoder
{
   /// <summary>
   /// Interaction logic for StartWindow.xaml
   /// </summary>
   public partial class StartWindow : Window
   {
      public StartWindow()
      {
         InitializeComponent();
      }

      private void OpenFile(object sender, RoutedEventArgs e)
      {
         Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
         ofd.DefaultExt = ".hex";
         ofd.Filter = "Raw data files (*.hex,*.dat)|*.hex;*.dat";

         Nullable<bool> result = ofd.ShowDialog();

         if (true == result)
         {
            GlobalVars.filename = ofd.FileName;

            readFile(GlobalVars.filename);



            GlobalVars.mainWindow = new MainWindow();
            GlobalVars.mainWindow.Show();

            this.Close();
         }

      }



      void readFile(string filename)
      {
         GlobalVars.fileAsBytes = System.IO.File.ReadAllBytes(filename);
         GlobalVars.fileAsString = "";


         foreach (byte b in GlobalVars.fileAsBytes)
         {
            GlobalVars.fileAsString += String.Format("{0} ", b.ToString("X2"));
         }

      }


   }
}
