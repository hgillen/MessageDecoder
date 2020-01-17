using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

using MessageDecoder.ViewModels;
namespace MessageDecoder.Models
{
    public class MainModel : BindableBase
    {

        #region Constants

        #endregion

        #region Models

        public MessageTypeCollection MessageTypes { get; set; }
        //public ObservableCollection<MessageFieldModel> MessageFields { get; set; }
        public ObservableCollection<MenuBarItemModel> TopMenu { get; private set; }
        private MainViewModel MainParent;

        #endregion

        #region Commands

        #endregion

        #region Properties

        private string configFilePath = "", dataFilePath = "";
        private byte[] importedData;

        private MessageTypeTreeModel _selectedMessageType;
        public MessageTypeTreeModel SelectedMessageType
        {
            get
            {
                return _selectedMessageType;
            }
            set
            {
                SetProperty(ref _selectedMessageType, value);
                //MessageFields = value?.MessageFields;
                //MainParent.OnTypeSelectionChanged();
            }
        }

        //private MessageTypeTreeModel _currentType;
        //public MessageTypeTreeModel CurrentType
        //{
        //    get
        //    {
        //        return _currentType;
        //    }
        //    set
        //    {
        //        SetProperty(ref _currentType, value);
        //        MessageFields = value?.MessageFields;
        //    }
        //}


        private MessageFieldDetailsModel _currentField;
        public MessageFieldDetailsModel CurrentField
        {
            get
            {
                return _currentField;
            }
            set
            {
                SetProperty(ref _currentField, value);
                //OnNav("details");
                CurrentView = value;
                IsDetails = true;
            }
        }

        private BindableBase _currentView;
        public BindableBase CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                SetProperty(ref _currentView, value);
            }
        }

        private bool _isDetails;
        public bool IsDetails
        {
            get
            {
                return _isDetails;
            }
            set
            {
                SetProperty(ref _isDetails, value);
            }
        }

        #endregion


        public MainModel(MainViewModel parent = null)
        {
            MainParent = parent;
            SelectedMessageType = null;

            //MessageTypes = new MessageTypeCollection
            //{
            //    new MessageTypeTreeModel
            //    {
            //        SubtypeName = "New Type",
            //    }
            //};
            //MessageTypes[0].Parent = MessageTypes;
            //MessageTypes[0].MainParent = this;
            //MessageTypes[0].IsSelected = true;

            //SelectedMessageType = MessageTypes[0];
            //MessageFields = SelectedMessageType.MessageFields;

            //// Initialize the top menu
            //TopMenu = new ObservableCollection<MenuBarItemModel>
            //{
            //    new MenuBarItemModel
            //    {
            //        Header = "File",
            //        ChildMenus = new ObservableCollection<MenuBarItemModel>
            //        {
            //            new MenuBarItemModel
            //            {
            //                Header = "Open Data File",
            //                Command = new MyICommand<string>(OnOpenData)
            //            },
            //            new MenuBarItemModel
            //            {
            //                Header = "Load Configuration",
            //                Command = new MyICommand<string>(OnOpenConfig)
            //            },
            //            new MenuBarItemModel
            //            {
            //                Header = "Save",
            //                Command = new MyICommand<string>(OnSaveConfig)
            //            },
            //            new MenuBarItemModel
            //            {
            //                Header = "Save As...",
            //                Command = new MyICommand<string>(OnSaveAsConfig)
            //            },
            //            new MenuBarItemModel
            //            {
            //                Header = "Export As...",
            //                Command = new MyICommand<string>(OnExportAs)
            //            }

            //        }
            //    }
            //};

        }

        //public void OnOpenData(string str = "")
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog
        //    {
        //        Filter = "Hex Files (*.hex)|*.hex|Text Files (*.txt)|*.txt|Data Files (*.dat)|*.dat",
        //    };

        //    if (true == openFileDialog.ShowDialog())
        //    {
        //        dataFilePath = openFileDialog.FileName;
        //        ReadData(dataFilePath);
        //    }

        //}

        //public void ReadData(string filename)
        //{
        //    if (File.Exists(filename))
        //    {
        //        importedData = File.ReadAllBytes(filename);
        //    }

        //}

        //public void OnOpenConfig(string str = "")
        //{
        //    if ("" == configFilePath)
        //    {
        //        OpenFileDialog openFileDialog = new OpenFileDialog
        //        {
        //            Filter = "Text files (*.txt)|*.txt"
        //        };

        //        if (true == openFileDialog.ShowDialog())
        //        {
        //            configFilePath = openFileDialog.FileName;
        //            // Clear all current types
        //            MessageTypes.Clear();

        //            // Read in new types
        //            ReadConfig(configFilePath);
        //        }

        //    }
        //    else
        //    {
        //        // Prompt to save current file before opening new one
        //    }

        //}

        //public void OnSaveAsConfig(string str = "")
        //{
        //    SaveFileDialog saveFileDialog = new SaveFileDialog()
        //    {
        //        Filter = "Text files (*.txt)|*.txt"
        //    };

        //    if (true == saveFileDialog.ShowDialog())
        //    {
        //        configFilePath = saveFileDialog.FileName;
        //        OnSaveConfig(configFilePath);
        //    }

        //}

        //public void OnSaveConfig(string str = "")
        //{
        //    if ("" != configFilePath)
        //    {
        //        WriteConfig(configFilePath);
        //    }
        //}

        //public void WriteConfig(string filename)
        //{
        //    using (StreamWriter streamWriter = new StreamWriter(filename, false))
        //    {
        //        foreach (MessageTypeTreeModel mttm in MessageTypes)
        //        {
        //            streamWriter.WriteLine(mttm.SaveConfig());
        //        }

        //        streamWriter.Close();
        //    }
        //}

        //public void ReadConfig(string filename)
        //{
        //    string fileAsString;
        //    Regex typeReg = new Regex(@"<\s*Type\s+(.*)\\\s*Type\s*>", RegexOptions.Singleline);
        //    MatchCollection typeMatches;
        //    MessageTypeTreeModel messageTypeTree;

        //    using (StreamReader streamReader = new StreamReader(filename))
        //    {
        //        // Read from file
        //        fileAsString = streamReader.ReadToEnd();
        //        streamReader.Close();
        //    }

        //    // Check that we read something
        //    if ("" != fileAsString)
        //    {
        //        // Split into tokens
        //        if (typeReg.IsMatch(fileAsString))
        //        {
        //            // Read each message type
        //            // Create a new Type for each Type found
        //            typeMatches = typeReg.Matches(fileAsString);
        //            foreach (Match typeMatch in typeMatches)
        //            {
        //                messageTypeTree = new MessageTypeTreeModel();
        //                messageTypeTree.ReadConfig(typeMatch.Groups[1].Value);

        //                MessageTypes.Add(messageTypeTree);
        //            }
        //        }
        //    }
        //}

        //public void OnParseData()
        //{

        //}


        //public void OnExportAs(string filename)
        //{

        //}




    }
}
