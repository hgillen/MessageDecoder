using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

using MessageDecoder.Views;
using MessageDecoder.Models;
using System.Windows;

namespace MessageDecoder.ViewModels
{
    public class GridHelpers
    {
#region RowCount Property
        /// <summary>
        /// Adds the specified number of Rows to RowDefinitions. 
        /// Default Height is Star
        /// </summary>
        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.RegisterAttached(
                "RowCount", typeof(int), typeof(GridHelpers),
                new PropertyMetadata(-1, RowCountChanged));

        // Get
        public static int GetRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }

        // Set
        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }

        // Change Event - Adds the Rows
        public static void RowCountChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || (int)e.NewValue < 0)
                return;

            Grid grid = (Grid)obj;
            grid.RowDefinitions.Clear();

            for (int i = 0; i < (int)e.NewValue; i++)
                grid.RowDefinitions.Add(
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            SetStarRows(grid);
        }

#endregion

        #region ColumnCount Property

        /// <summary>
        /// Adds the specified number of Columns to ColumnDefinitions. 
        /// Default Width is Auto
        /// </summary>
        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.RegisterAttached(
                "ColumnCount", typeof(int), typeof(GridHelpers),
                new PropertyMetadata(-1, ColumnCountChanged));

        // Get
        public static int GetColumnCount(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        // Set
        public static void SetColumnCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }

        // Change Event - Add the Columns
        public static void ColumnCountChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || (int)e.NewValue < 0)
                return;

            Grid grid = (Grid)obj;
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < (int)e.NewValue; i++)
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            //GridLength.Auto });

            SetStarColumns(grid);
        }

        #endregion

        #region StarRows Property

        /// <summary>
        /// Makes the specified Row's Height equal to Star. 
        /// Can set on multiple Rows
        /// </summary>
        public static readonly DependencyProperty StarRowsProperty =
            DependencyProperty.RegisterAttached(
                "StarRows", typeof(string), typeof(GridHelpers),
                new PropertyMetadata(string.Empty, StarRowsChanged));

        // Get
        public static string GetStarRows(DependencyObject obj)
        {
            return (string)obj.GetValue(StarRowsProperty);
        }

        // Set
        public static void SetStarRows(DependencyObject obj, string value)
        {
            obj.SetValue(StarRowsProperty, value);
        }

        // Change Event - Makes specified Row's Height equal to Star
        public static void StarRowsChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || string.IsNullOrEmpty(e.NewValue.ToString()))
                return;

            SetStarRows((Grid)obj);
        }

        #endregion

        #region StarColumns Property

        /// <summary>
        /// Makes the specified Column's Width equal to Star. 
        /// Can set on multiple Columns
        /// </summary>
        public static readonly DependencyProperty StarColumnsProperty =
            DependencyProperty.RegisterAttached(
                "StarColumns", typeof(string), typeof(GridHelpers),
                new PropertyMetadata(string.Empty, StarColumnsChanged));

        // Get
        public static string GetStarColumns(DependencyObject obj)
        {
            return (string)obj.GetValue(StarColumnsProperty);
        }

        // Set
        public static void SetStarColumns(DependencyObject obj, string value)
        {
            obj.SetValue(StarColumnsProperty, value);
        }

        // Change Event - Makes specified Column's Width equal to Star
        public static void StarColumnsChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || string.IsNullOrEmpty(e.NewValue.ToString()))
                return;

            SetStarColumns((Grid)obj);
        }

        #endregion

        private static void SetStarColumns(Grid grid)
        {
            string[] starColumns =
                GetStarColumns(grid).Split(',');

            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                if (starColumns.Contains(i.ToString()))
                    grid.ColumnDefinitions[i].Width =
                        new GridLength(1, GridUnitType.Star);
            }
        }

        private static void SetStarRows(Grid grid)
        {
            string[] starRows =
                GetStarRows(grid).Split(',');

            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                if (starRows.Contains(i.ToString()))
                    grid.RowDefinitions[i].Height =
                        new GridLength(1, GridUnitType.Star);
            }
        }
    }


    public class FieldCollection : ObservableCollection<MessageFieldModel>
    {
        public MessageFieldModel FindField(int startWord, int startBit)
        {
            MessageFieldModel messageField = this.FirstOrDefault(p =>
                startWord == p.MessageFieldDetails.WordNum &&
                startBit == p.MessageFieldDetails.BitStart);
            foreach (MessageFieldModel fieldModel in this)
            {
                if (startWord == fieldModel.MessageFieldDetails.WordNum &&
                    startBit == fieldModel.MessageFieldDetails.BitStart)
                {
                    return fieldModel;
                }
            }
            if (null == messageField)
            {
                throw new Exception("Message Field not found");
            }
            return messageField;
        }

        public bool IsMatch(byte[] bytes)
        {
            // TODO not quite right, add test cases to prove
            bool atLeastOneConstantValue = false;
            foreach (MessageFieldModel messageField in this.Where(p => p.MessageFieldDetails.ConstantValueEnabled))
            {
                if (!bytes.Contains((byte)messageField.MessageFieldDetails.ConstantValue))
                    return false;
            }
            return atLeastOneConstantValue;
        }

        public FileTextModel ExtractMatch(byte[] bytes)
        {
            bytes.Take(5);
            throw new NotImplementedException();
        }

    }


    public class MessageFieldViewModel : BindableBase
    {
        #region Constants

        private const int maxWords = 32;
        public const int buttonHeight = 250;

        #endregion

        #region Models

        public MessageFieldDetailsViewModel Vm_FieldDetails { get; set; }

        public FieldCollection MessageFields { get; set; }

        public ObservableCollection<string> WordNumbers { get; set; }

        #endregion

        #region Commands

        public MyICommand<string> NavCommand { get; set; }
        public MyICommand<string> AddWord { get; set; }
        public MyICommand<string> SubtractWord { get; set; }

        #endregion

        #region Properties

        private const int _buttonWidth = 250;
        public int ButtonWidth
        {
            get
            {
                return _buttonWidth;
            }
            set
            {

            }
        }


        private int _numWords;
        /// <summary>
        /// Number of words in the specified type, ranging from 1 to maxWords.
        /// </summary>
        public int NumWords
        {
            get
            {
                return _numWords;
            }
            set
            {
                SetProperty(ref _numWords, value);
                OnPropertyChanged("NumWordsString");
                OnPropertyChanged("MessageFields");
            }
        }

        /// <summary>
        /// String representation of NumWords for use in the add/subtract words textblock.
        /// </summary>
        public string NumWordsString
        {
            get
            {
                return (_numWords).ToString();
            }
            set
            {
                if (int.TryParse(value, out int temp) && temp >= 1 && temp <= maxWords)
                {
                    if (temp > NumWords)
                    {
                        while (temp > NumWords)
                            ((ICommand)AddWord).Execute(null);
                    }
                    else
                    {
                        while (temp < NumWords)
                            ((ICommand)SubtractWord).Execute(null);
                    }
                }
                //SetProperty(ref _numWords, temp);
            }
        }


        private int _wordSize;
        /// <summary>
        /// Used in MessageFieldView for the number of columns displayed.
        /// </summary>
        public int WordSize
        {
            get
            {
                return _wordSize;
            }
            set
            {
                SetProperty(ref _wordSize, value);
            }
        }

        private bool _subtractIsEnabled;
        /// <summary>
        /// True when the subtract word button is enabled.
        /// </summary>
        public bool SubtractIsEnabled
        {
            get
            {
                return _subtractIsEnabled;
            }
            set
            {
                SetProperty(ref _subtractIsEnabled, value);
            }
        }

        private bool _textIsEnabled;
        /// <summary>
        /// True when the change word text block is enabled.
        /// </summary>
        public bool TextIsEnabled
        {
            get
            {
                return _textIsEnabled;
            }
            set
            {
                SetProperty(ref _textIsEnabled, value);
            }
        }

        private bool _addIsEnabled;
        /// <summary>
        /// True when the add word button is enabled.
        /// </summary>
        public bool AddIsEnabled
        {
            get
            {
                return _addIsEnabled;
            }
            set
            {
                SetProperty(ref _addIsEnabled, value);
            }
        }

    #endregion


        public MessageFieldViewModel()
        {
            //AddWord = new MyICommand<string>(OnAddWord);
            //SubtractWord = new MyICommand<string>(OnSubtractWord);

            Vm_FieldDetails = new MessageFieldDetailsViewModel();

            SubtractIsEnabled = true;
            TextIsEnabled = true;
            AddIsEnabled = true;

            MessageFields = new FieldCollection();
            WordNumbers = new ObservableCollection<string>();
            NumWords = 0;
            WordSize = 16;
        }


        public MessageFieldDetailsViewModel GetFieldDetails()
        {
            return Vm_FieldDetails;
        }

        //public void OnAddWord(string a = "")
        //{
        //    if (NumWords >= maxWords) return;

        //    // Create a new field to fill in the spaces of the new word
        //    for (int i = 0; i < 16; ++i)
        //    {
        //        MessageFieldModel newField = new MessageFieldModel();
        //        newField.MessageFieldDetails.WordNum = NumWords;
        //        newField.MessageFieldDetails.BitStart = i;
        //        newField.MessageFieldDetails.BitLength = 1;

        //        MessageFields.Add(newField);
        //    }

        //    // Add to WordNumbers collection
        //    WordNumbers.Add(NumWords.ToString());

        //    // Increment field last
        //    ++NumWords;
        //}

        //public void OnSubtractWord(string a = "")
        //{
        //    if (NumWords <= 1) return;

        //    // Decrement field first
        //    --NumWords;

        //    // Remove from WordNumbers collection
        //    WordNumbers.Remove(NumWords.ToString());

        //    // Modify the collection by removing unwanted elements
        //    for (int i = 0; i < MessageFields.Count; ++i)
        //    {
        //        if (NumWords == MessageFields[i].MessageFieldDetails.WordNum)
        //        {
        //            MessageFields.Remove(MessageFields[i]);
        //            --i;
        //        }
        //    }
        //}

        public void AddWordFields()
        {
            // Create a new field to fill in the spaces of the new word
            for (int i = 0; i < 16; ++i)
            {
                MessageFieldModel newField = new MessageFieldModel();
                newField.MessageFieldDetails.WordNum = NumWords;
                newField.MessageFieldDetails.BitStart = i;
                newField.MessageFieldDetails.BitLength = 1;

                MessageFields.Add(newField);
            }
            NumWords++;
            UpdateMessageFields();
        }

        public void SubtractWordFields()
        {
            NumWords--;
            UpdateMessageFields();

            // Modify the collection by removing unwanted elements
            for (int i = 0; i < MessageFields.Count; ++i)
            {
                if (NumWords == MessageFields[i].MessageFieldDetails.WordNum)
                {
                    MessageFields.Remove(MessageFields[i]);
                    --i;
                }
            }

        }

        public void SetMessageFields(FieldCollection messageFields)
        {
            MessageFields = messageFields;
            UpdateMessageFields();
        }

        public void UpdateMessageFields()
        {
            OnPropertyChanged("MessageFields");
        }

        /// <summary>
        /// Searches the current FieldCollection for the field in the same spot as messageField, then pulls it forward.
        /// </summary>
        /// <param name="messageField"></param>
        public MessageFieldModel PrioritizeField(MessageFieldModel messageField)
        {
            MessageFieldDetailsModel fieldDetails = messageField.MessageFieldDetails;
            MessageFieldModel newField = MessageFields.FindField(fieldDetails.WordNum, fieldDetails.BitStart);
            newField.ZIndex++;
            return newField;
        }
    }
}
