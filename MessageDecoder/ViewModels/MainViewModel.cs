using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

using MessageDecoder.Models;

namespace MessageDecoder.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Constants
        #endregion

        #region Models

        public ObservableCollection<string> SpecialTypes { get; set; }

        public MenuBarItemViewModel Vm_MenuBar { get; private set; }

        public FileTextViewModel Vm_FileText { get; set; }

        public MessageStructureViewModel Vm_MessageStructure { get; set; }

        public MessageFieldViewModel Vm_MessageFields { get; set; }

        public MessageTypeCollection MessageTypes { get; set; }

        //public ObservableCollection<MessageFieldModel> MessageFieldsCollection { get; set; }

        public MessageFieldDetailsViewModel Vm_FieldDetails { get; set; }

        //public MessageTypeDetailsViewModel Vm_MessageTypeDetails { get; set; }

        public MessageTypeTreeModel SelectedType { get; set; }

        public MainModel Model;

        public BindableBase ShowView { get; set; }

        #endregion

        #region Commands

        public MyICommand<BindableBase> NavCommand { get; set; }
        public MyICommand<BindableBase> ButtonOk { get; set; }
        public MyICommand<BindableBase> ButtonCancel { get; set; }
        public MyICommand<MessageTypeTreeModel> SelectType { get; set; }

        #endregion

        #region Properties

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
                if (value)
                    CurrentView = Vm_FieldDetails;
            }
        }

        private Visibility _messageFieldsVisibility;
        public Visibility MessageFieldsVisibility
        {
            get
            {
                return _messageFieldsVisibility;
            }
            set
            {
                SetProperty(ref _messageFieldsVisibility, value);
            }
        }

        //private bool _typeSelectionChanged;
        //public bool TypeSelectionChanged
        //{
        //    get
        //    {
        //        return _typeSelectionChanged;
        //    }
        //    set
        //    {
        //        SetProperty(ref _typeSelectionChanged, value);
        //        if (value)
        //            OnTypeSelectionChanged();
        //    }
        //}

        public string dataFilePath, configFilePath;

        #endregion



        public MainViewModel()
        {
            SpecialTypes = new ObservableCollection<string>
            {
                "Header Length",
                "Message Type",
                "Message Length"
            };

            Model = new MainModel(this);

            Vm_MenuBar = new MenuBarItemViewModel
            {
                //TopMenu = Model.TopMenu
                // Initialize the top menu
                
                TopMenu = new ObservableCollection<MenuBarItemModel>
                {
                    new MenuBarItemModel
                    {
                        Header = "File",
                        ChildMenus = new ObservableCollection<MenuBarItemModel>
                        {
                            new MenuBarItemModel
                            {
                                Header = "Open Data File",
                                Command = new MyICommand<string>(OnOpenData)
                            },
                            new MenuBarItemModel
                            {
                                Header = "Load Configuration",
                                Command = new MyICommand<string>(OnOpenConfig)
                            },
                            new MenuBarItemModel
                            {
                                Header = "Save",
                                Command = new MyICommand<string>(OnSaveConfig)
                            },
                            new MenuBarItemModel
                            {
                                Header = "Save As...",
                                Command = new MyICommand<string>(OnSaveAsConfig)
                            },
                            new MenuBarItemModel
                            {
                                Header = "Export As...",
                                Command = new MyICommand<string>(OnExportAs)
                            }

                        }
                    }
                }
            };

            NavCommand = new MyICommand<BindableBase>(OnNav);
            ButtonOk = new MyICommand<BindableBase>(OnButtonOk);
            ButtonCancel = new MyICommand<BindableBase>(OnButtonCancel);
            SelectType = new MyICommand<MessageTypeTreeModel>(OnTypeSelectionChanged);

            Vm_MessageStructure = new MessageStructureViewModel();
            MessageTypes = Vm_MessageStructure.GetMessageTypes();

            //MessageFields.MessageFields = Model.SelectedMessageType.MessageFields;
            Vm_MessageFields = Vm_MessageStructure.GetMessageFields();
            //Vm_MessageFields = new MessageFieldViewModel();
            //MessageFieldsCollection = new ObservableCollection<MessageFieldModel>();

            Vm_FieldDetails = Vm_MessageStructure.GetFieldDetails();
                //new MessageFieldDetailsViewModel();
            //Vm_MessageTypeDetails = new MessageTypeDetailsViewModel();

            Vm_FileText = new FileTextViewModel();
            
            CurrentView = Vm_FileText;
            IsDetails = Model.IsDetails;
            MessageFieldsVisibility = Visibility.Collapsed;


        }


        /// <summary>
        /// When selecting a Message Type, displays a copy of all the message fields in the bottom pane.
        /// </summary>
        /// <param name="destination"></param>
        public void OnNav(BindableBase destination = null)
        {
            // Check type is MessageFieldModel
            if (destination is MessageFieldModel mfm)
            {
                //MessageFieldDetailsModel messageFieldDetails = mfm.MessageFieldDetails;
                //MessageFieldModel messageField;


                //// Set the viewModel collection to a new copy
                //Vm_MessageFields.MessageFields = CopyFieldCollection();

                //// If field is the current mfm being modified, give it priority in the ZIndex
                //messageField = Vm_MessageFields.MessageFields.FindField(
                //    messageFieldDetails.WordNum, messageFieldDetails.BitStart);
                //messageField.ZIndex++;

                // And the details to the new copied details, identifiable by the word and bit
                MessageFieldDetailsModel fieldDetails = Vm_MessageStructure.ViewMessageField(SelectedType, mfm);
                Vm_FieldDetails.SetFieldDetails(fieldDetails);

                //// Prevent multiple selections
                //Vm_MessageFields.SubtractIsEnabled = false;
                //Vm_MessageFields.TextIsEnabled = false;
                //Vm_MessageFields.AddIsEnabled = false;

                // Finally, set the view
                CurrentView = Vm_FieldDetails;
            }

            // Else if not the mfm, set view to the text...

            //switch (destination)
            //{
            //    case "details":
            //        CurrentView = Model.CurrentField;
            //        break;
            //    case "filetext":
            //        CurrentView = Vm_FileText;
            //        break;
            //    default:
            //        break;
            //}
        }


        /// <summary>
        /// Set relevant parameters for when a new type is selected.
        /// </summary>
        /// <param name="messageType"></param>
        public void OnTypeSelectionChanged(MessageTypeTreeModel messageType)
        {
            Vm_MessageStructure.DeselectAllMessageTypes();
            Vm_MessageStructure.Select(messageType);

            // Make Fields view visible if not already
            if (Visibility.Visible != MessageFieldsVisibility)
                MessageFieldsVisibility = Visibility.Visible;

            SelectedType = messageType;
        }


        /// <summary>
        /// Commit changes to message field details.
        /// </summary>
        /// <param name="destination"></param>
        public void OnButtonOk(BindableBase destination = null)
        {
            // Commit changes, discard extras
            if (destination is MessageFieldDetailsModel mfdm)
            {
                OverwriteField(mfdm);
                // Discard the copy
                Vm_MessageStructure.SetMessageFields(SelectedType.MessageFields);
                Vm_MessageStructure.SetButtons(true);

                // Reset the view
                CurrentView = Vm_FileText;
            }

        }

        private void OverwriteField(MessageFieldDetailsModel mfdm)
        {
            MessageFieldModel mfm =
                SelectedType.MessageFields.FindField(mfdm.WordNum, mfdm.BitStart);

            mfm.MessageFieldDetails = Vm_FieldDetails.FieldDetails;
            mfm.IsEnabled = true;
        }

        /// <summary>
        /// Reject changes to message field details.
        /// </summary>
        /// <param name="destination"></param>
        public void OnButtonCancel(BindableBase destination = null)
        {
            // Revert changes, discard the temp model
            if (destination is MessageFieldDetailsModel)
            {
                // Revert to the initial message fields
                Vm_MessageStructure.SetMessageFields(SelectedType.MessageFields);

                // Reset the buttons
                Vm_MessageStructure.SetButtons(true);

                // Reset the view
                CurrentView = Vm_FileText;
            }

        }



        public void OnOpenData(string str = "")
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Hex Files (*.hex)|*.hex|Text Files (*.txt)|*.txt|Data Files (*.dat)|*.dat",
            };

            if (true == openFileDialog.ShowDialog())
            {
                Vm_FileText.SetAndDisplayDataFile(openFileDialog.FileName, MessageTypes);
            }

        }


        public void OnOpenConfig(string str = "")
        {
            if ("" == configFilePath)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt"
                };

                if (true == openFileDialog.ShowDialog())
                {
                    configFilePath = openFileDialog.FileName;
                    // Clear all current types
                    MessageTypes.Clear();

                    // Read in new types
                    ReadConfig(configFilePath);
                }

            }
            else
            {
                // Prompt to save current file before opening new one
            }

        }

        public void OnSaveAsConfig(string str = "")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt"
            };

            if (true == saveFileDialog.ShowDialog())
            {
                configFilePath = saveFileDialog.FileName;
                OnSaveConfig(configFilePath);
            }

        }

        public void OnSaveConfig(string str = "")
        {
            if ("" != configFilePath)
            {
                WriteConfig(configFilePath);
            }
        }

        public void WriteConfig(string filename)
        {
            using (StreamWriter streamWriter = new StreamWriter(filename, false))
            {
                foreach (MessageTypeTreeModel mttm in MessageTypes)
                {
                    streamWriter.WriteLine(mttm.SaveConfig());
                }

                streamWriter.Close();
            }
        }

        public void ReadConfig(string filename)
        {
            string fileAsString;
            Regex typeReg = new Regex(@"<\s*Type\s+(.*)\\\s*Type\s*>", RegexOptions.Singleline);
            MatchCollection typeMatches;
            MessageTypeTreeModel messageTypeTree;

            using (StreamReader streamReader = new StreamReader(filename))
            {
                // Read from file
                fileAsString = streamReader.ReadToEnd();
                streamReader.Close();
            }

            // Check that we read something
            if ("" != fileAsString)
            {
                // Split into tokens
                if (typeReg.IsMatch(fileAsString))
                {
                    // Read each message type
                    // Create a new Type for each Type found
                    typeMatches = typeReg.Matches(fileAsString);
                    foreach (Match typeMatch in typeMatches)
                    {
                        messageTypeTree = new MessageTypeTreeModel();
                        messageTypeTree.ReadConfig(typeMatch.Groups[1].Value);

                        MessageTypes.Add(messageTypeTree);
                    }
                }
            }
        }


        public void OnExportAs(string filename)
        {

        }

    }
}
