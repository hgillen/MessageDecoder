using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using MessageDecoder.ViewModels;
using MessageDecoder.Models;

namespace ViewModelTests
{
    [TestClass]
    public class MainViewModelTests
    {
        private readonly MainViewModel mvm = new MainViewModel();

        [TestClass]
        public class ConstructorTests : MainViewModelTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                Assert.IsNotNull(mvm.Model);
                Assert.IsNotNull(mvm.Vm_MenuBar);
                Assert.IsNotNull(mvm.Vm_MessageStructure);
                Assert.IsNotNull(mvm.Vm_MessageStructure.Vm_MessageTypeTree);
                //Assert.IsNotNull(mvm.Vm_MessageFields);
                Assert.IsNotNull(mvm.Vm_FieldDetails);
                //Assert.IsNotNull(mvm.Vm_MessageTypeDetails);
                Assert.IsNotNull(mvm.Vm_FileText);
                Assert.IsFalse(mvm.IsDetails);
                //Assert.AreEqual(System.Windows.Visibility)
            }
        }

        [TestClass]
        public class SelectTypeTests : MainViewModelTests
        {
            [TestMethod]
            public void FirstSelectType()
            {
                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0]);

                Assert.AreEqual(true, mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].IsSelected);
            }

            [TestMethod]
            public void NominalSelectType()
            {
                ((ICommand)mvm.Vm_MessageStructure.Vm_MessageTypeTree.AddTypeCommand).Execute(null);

                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[1]);

                Assert.AreEqual(false, mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].IsSelected);
                Assert.AreEqual(true, mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[1].IsSelected);
            }

            [TestMethod]
            public void SelectSubtype()
            {
                ((ICommand)mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].AddSubtypeCommand).Execute(null);

                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].MessageSubtypes[0]);

                Assert.AreEqual(false, mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].IsSelected);
                Assert.AreEqual(true, mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].MessageSubtypes[0].IsSelected);
            }

            [TestMethod]
            public void SelectDeepSubtype()
            {
                int count = 100;
                MessageTypeTreeModel mttm = mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0];

                for (int i = 0; i < count; ++i)
                {
                    ((ICommand)mttm.AddSubtypeCommand).Execute(null);
                    mttm = mttm.MessageSubtypes[0];
                }

                ((ICommand)mvm.SelectType).Execute(mttm);

                Assert.AreEqual(true, mttm.IsSelected);
                mttm = mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0];
                for (int i = 0; i < count; ++i)
                {
                    Assert.AreEqual(false, mttm.IsSelected);
                    mttm = mttm.MessageSubtypes[0];
                }
            }

            [TestMethod]
            public void ReselectType()
            {
                ((ICommand)mvm.Vm_MessageStructure.Vm_MessageTypeTree.AddTypeCommand).Execute(null);

                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[1]);
                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0]);

                Assert.AreEqual(true, mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].IsSelected);
                Assert.AreEqual(false, mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[1].IsSelected);
            }

            [TestMethod]
            public void ReselectSubtype()
            {
                ((ICommand)mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].AddSubtypeCommand).Execute(null);

                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0]);
                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].MessageSubtypes[0]);

                Assert.AreEqual(false, mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].IsSelected);
                Assert.AreEqual(true, mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].MessageSubtypes[0].IsSelected);
            }
        }

        [TestClass]
        public class NavTests : MainViewModelTests
        {
            [TestMethod]
            public void NavToField()
            {
                MessageFieldModel mfm = new MessageFieldModel();
                mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].MessageFields =
                    new FieldCollection
                    {
                        mfm
                    };

                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0]);
                ((ICommand)mvm.NavCommand).Execute(mfm);

                //Assert.AreEqual(1, mvm.Vm_MessageFields.MessageFields.Count);
                //Assert.IsFalse(mvm.Vm_MessageFields.MessageFields[0].IsEnabled);
                //Assert.IsFalse(mvm.Vm_MessageFields.SubtractIsEnabled);
                //Assert.IsFalse(mvm.Vm_MessageFields.TextIsEnabled);
                //Assert.IsFalse(mvm.Vm_MessageFields.AddIsEnabled);
                Assert.IsNotNull(mvm.Vm_FieldDetails);
                Assert.AreEqual(mvm.Vm_FieldDetails, mvm.CurrentView);
            }
        }

        [TestClass]
        public class EditFields : MainViewModelTests
        {
            [TestMethod]
            public void EditBit()
            {
                int testWord = 1, testBit = 1, newBit = 2;
                MessageFieldModel mfm = mvm.SelectedType.MessageFields.FirstOrDefault(x =>
                    testBit == x.MessageFieldDetails.BitStart && testWord == x.MessageFieldDetails.WordNum);

                // Needs work
                Assert.AreEqual(testBit, mfm.MessageFieldDetails.BitStart);
                Assert.AreEqual(testWord, mfm.MessageFieldDetails.WordNum);
                Assert.AreEqual(newBit, mfm.MessageFieldDetails.BitStart);
                Assert.AreEqual(testBit, mfm.MessageFieldDetails.BitStart);
            }

            [TestMethod]
            public void EditWord()
            {

            }

            [TestMethod]
            public void EditSize()
            {

            }

        }

        [TestClass]
        public class ConfirmFieldEditsTests : MainViewModelTests
        {
            [TestMethod]
            public void NominalConfirm()
            {
                int newLength = 2, newStart = 3, newWord = 2;
                string newName = "NewName1234", newType = "Header Length";
                MessageFieldModel mfm = mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].MessageFields[0];
                MessageFieldDetailsModel mfdm;

                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0]);
                ((ICommand)mvm.NavCommand).Execute(mfm);
                mfdm = mvm.Vm_FieldDetails.FieldDetails;

                mfdm.BitLength = newLength;
                mfdm.FieldName = newName;
                mfdm.BitStart = newStart;
                mfdm.WordNum = newWord;
                mfdm.IsSpecialType = true;
                mfdm.SpecialType = newType;

                ((ICommand)mvm.ButtonOk).Execute(mfm.MessageFieldDetails);
                mfdm = mfm.MessageFieldDetails;

                Assert.AreEqual(newLength, mfdm.BitLength);
                Assert.AreEqual(newLength.ToString(), mfdm.BitLengthString);
                Assert.AreEqual(newName, mfdm.FieldName);
                Assert.AreEqual(newStart, mfdm.BitStart);
                Assert.AreEqual(newWord, mfdm.WordNum);
                Assert.AreEqual(true, mfdm.IsSpecialType);
                Assert.AreEqual(newType, mfdm.SpecialType);
                //Assert.IsTrue(mvm.Vm_MessageFields.MessageFields[0].IsEnabled);
                //Assert.IsTrue(mvm.Vm_MessageFields.SubtractIsEnabled);
                //Assert.IsTrue(mvm.Vm_MessageFields.TextIsEnabled);
                //Assert.IsTrue(mvm.Vm_MessageFields.AddIsEnabled);
            }

            [TestMethod]
            public void ConfirmOverMax()
            {
                int newLength = 20, newStart = 30, newWord = 5;
                string newName = "NewName1234", newType = "Header Length";
                MessageFieldModel mfm = mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].MessageFields[0];
                MessageFieldDetailsModel mfdm;

                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0]);
                ((ICommand)mvm.NavCommand).Execute(mfm);
                mfdm = mvm.Vm_FieldDetails.FieldDetails;

                mfdm.BitLength = newLength;
                mfdm.FieldName = newName;
                mfdm.BitStart = newStart;
                mfdm.WordNum = newWord;
                mfdm.IsSpecialType = true;
                mfdm.SpecialType = newType;

                ((ICommand)mvm.ButtonOk).Execute(mfm.MessageFieldDetails);
                mfdm = mfm.MessageFieldDetails;

                Assert.AreEqual(1, mfdm.BitLength);
                Assert.AreEqual("1", mfdm.BitLengthString);
                Assert.AreEqual(newName, mfdm.FieldName);
                Assert.AreEqual(15, mfdm.BitStart);
                Assert.AreEqual(newWord, mfdm.WordNum);
                Assert.AreEqual(true, mfdm.IsSpecialType);
                Assert.AreEqual(newType, mfdm.SpecialType);
                //Assert.IsTrue(mvm.Vm_MessageFields.MessageFields[0].IsEnabled);
                //Assert.IsTrue(mvm.Vm_MessageFields.SubtractIsEnabled);
                //Assert.IsTrue(mvm.Vm_MessageFields.TextIsEnabled);
                //Assert.IsTrue(mvm.Vm_MessageFields.AddIsEnabled);
            }
        }

        [TestClass]
        public class CancelFieldEditsTests : MainViewModelTests
        {
            [TestMethod]
            public void NominalCancel()
            {
                int newLength = 2, newStart = 3, newWord = 2;
                string newName = "NewName1234", newType = "Header Length";
                MessageFieldModel mfm = mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0].MessageFields[0];
                MessageFieldDetailsModel mfdm;

                ((ICommand)mvm.SelectType).Execute(mvm.Vm_MessageStructure.Vm_MessageTypeTree.MessageTypes[0]);
                ((ICommand)mvm.NavCommand).Execute(mfm);
                mfdm = mvm.Vm_FieldDetails.FieldDetails;

                mfdm.BitLength = newLength;
                mfdm.FieldName = newName;
                mfdm.BitStart = newStart;
                mfdm.WordNum = newWord;
                mfdm.IsSpecialType = true;
                mfdm.SpecialType = newType;

                ((ICommand)mvm.ButtonCancel).Execute(mfm.MessageFieldDetails);
                mfdm = mfm.MessageFieldDetails;

                Assert.AreNotEqual(newLength, mfdm.BitLength);
                Assert.AreNotEqual(newLength.ToString(), mfdm.BitLengthString);
                Assert.AreNotEqual(newName, mfdm.FieldName);
                Assert.AreNotEqual(newStart, mfdm.BitStart);
                Assert.AreNotEqual(newWord, mfdm.WordNum);
                Assert.AreNotEqual(true, mfdm.IsSpecialType);
                Assert.AreNotEqual(newType, mfdm.SpecialType);
                //Assert.IsTrue(mvm.Vm_MessageFields.MessageFields[0].IsEnabled);
                //Assert.IsTrue(mvm.Vm_MessageFields.SubtractIsEnabled);
                //Assert.IsTrue(mvm.Vm_MessageFields.TextIsEnabled);
                //Assert.IsTrue(mvm.Vm_MessageFields.AddIsEnabled);
            }

        }

        [TestClass]
        public class ReadFileTests : MainViewModelTests
        {
            [TestMethod]
            public void NominalRead()
            {
                mvm.ReadConfig("testConfig.txt");
            }
        }


    }



    [TestClass]
    public class MessageFieldDetailsViewModelTests
    {
        private readonly MessageFieldDetailsViewModel mfdvm = new MessageFieldDetailsViewModel();

        [TestClass]
        public class ConstructorTests : MessageFieldDetailsViewModelTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                Assert.IsNull(mfdvm.FieldDetails);
            }

        }

    }

    [TestClass]
    public class MessageFieldViewModelTests
    {
        private readonly MessageFieldViewModel mfvm = new MessageFieldViewModel();

        [TestClass]
        public class ConstructorTests : MessageFieldViewModelTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                Assert.IsNotNull(mfvm.MessageFields);
                Assert.AreEqual(0, mfvm.MessageFields.Count);
            }

        }

        [TestClass]
        public class AddWordTests : MessageFieldViewModelTests
        {
            // Add word should belong to MessageTypeViewModel

            [TestMethod]
            public void NominalAdd()
            {
                int numElements = mfvm.MessageFields.Count;

                ((ICommand)mfvm.AddWord).Execute(null);

                Assert.AreEqual(1, mfvm.NumWords);
                Assert.AreEqual(numElements + 16, mfvm.MessageFields.Count);
            }
        }

        [TestClass]
        public class SubtractWordTests : MessageFieldViewModelTests
        {
            [TestMethod]
            public void NominalSubtract()
            {
                ((ICommand)mfvm.AddWord).Execute(0);
                ((ICommand)mfvm.AddWord).Execute(0);
                int numElements = mfvm.MessageFields.Count;

                ((ICommand)mfvm.SubtractWord).Execute(0);

                Assert.AreEqual(1, mfvm.NumWords);
                Assert.AreEqual(numElements - 16, mfvm.MessageFields.Count);
            }

            [TestMethod]
            public void SubtractLastOne()
            {
                ((ICommand)mfvm.AddWord).Execute(0);
                int numElements = mfvm.MessageFields.Count;

                ((ICommand)mfvm.SubtractWord).Execute(0);

                Assert.AreEqual(numElements, mfvm.MessageFields.Count);
            }

            [TestMethod]
            public void SubtractNone()
            {
                int numElements = mfvm.MessageFields.Count;

                ((ICommand)mfvm.SubtractWord).Execute(0);

                Assert.AreEqual(numElements, mfvm.MessageFields.Count);
            }

        }

        [TestClass]
        public class ChangeWordsTests : MessageFieldViewModelTests
        {
            [TestMethod]
            public void ChangeZeroToOne()
            {
                int numElements = mfvm.MessageFields.Count;

                mfvm.NumWordsString = "1";

                Assert.AreEqual(numElements + 16, mfvm.MessageFields.Count);
            }

            [TestMethod]
            public void ChangeZeroToMany()
            {
                string numWords = "10";
                int numElements = mfvm.MessageFields.Count,
                    expectedWords = int.Parse(numWords);

                mfvm.NumWordsString = numWords;

                Assert.AreEqual(numElements + 16 * expectedWords, mfvm.MessageFields.Count);
            }

            [TestMethod]
            public void ChangeManyToOne()
            {
                string numWords = "10";
                int numElements = mfvm.MessageFields.Count;

                mfvm.NumWordsString = numWords;
                mfvm.NumWordsString = "1";

                Assert.AreEqual(numElements + 16, mfvm.MessageFields.Count);
            }
        }

    }

    [TestClass]
    public class MessageTypeTreeViewModelTests
    {
        private readonly MessageTypeTreeViewModel mttvm = new MessageTypeTreeViewModel();

        [TestClass]
        public class ConstructorTests : MessageTypeTreeViewModelTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                Assert.AreEqual(1, mttvm.MessageTypes.Count);
            }

        }

        [TestClass]
        public class AddTests : MessageTypeTreeViewModelTests
        {
            [TestMethod]
            public void NominalAdd()
            {
                string testName = "testType";

                ((ICommand)mttvm.AddTypeCommand).Execute(testName);

                Assert.AreEqual(2, mttvm.MessageTypes.Count);
                Assert.AreEqual(testName, mttvm.MessageTypes[1].SubtypeName);
            }
        }

        [TestClass]
        public class DeleteTests : MessageTypeTreeViewModelTests
        {
            [TestMethod]
            public void NominalDelete()
            {
                ((ICommand)mttvm.AddTypeCommand).Execute(null);
                ((ICommand)mttvm.DeleteCommand).Execute(mttvm.MessageTypes[1]);

                Assert.AreEqual(1, mttvm.MessageTypes.Count);
            }

            [TestMethod]
            public void DeleteNull()
            {
                ((ICommand)mttvm.DeleteCommand).Execute(null);

                Assert.AreEqual(1, mttvm.MessageTypes.Count);
            }

            [TestMethod]
            public void DeleteNonexistent()
            {
                MessageTypeTreeModel mttm = new MessageTypeTreeModel();
                ((ICommand)mttvm.DeleteCommand).Execute(mttm);

                Assert.AreEqual(1, mttvm.MessageTypes.Count);
            }

            [TestMethod]
            public void DeleteLast()
            {
                ((ICommand)mttvm.DeleteCommand).Execute(mttvm.MessageTypes[0]);

                Assert.AreEqual(0, mttvm.MessageTypes.Count);
            }

            [TestMethod]
            public void DeepDelete()
            {
                MessageTypeTreeModel mttm = mttvm.MessageTypes[0], child;

                for (int i = 0; i < 100; ++i)
                {
                    ((ICommand)mttm.AddSubtypeCommand).Execute(null);
                    mttm = mttm.MessageSubtypes[0];
                }

                ((ICommand)mttm.AddSubtypeCommand).Execute(null);
                child = mttm.MessageSubtypes[0];

                ((ICommand)mttvm.DeleteCommand).Execute(child);

                Assert.AreEqual(0, mttm.MessageSubtypes.Count);
            }

        }

    }

    [TestClass]
    public class MessageTypeDetailsViewModelTests
    {
        private readonly MessageTypeDetailsViewModel mtdvm = new MessageTypeDetailsViewModel();

        [TestClass]
        public class ConstructorTests : MessageTypeDetailsViewModelTests
        {

            [TestMethod]
            public void BaseConstructor()
            {
                Assert.IsNotNull(mtdvm);
            }

        }


    }

    [TestClass]
    public class MessageStructureViewModelTests
    {
        private readonly MessageStructureViewModel msvm = new MessageStructureViewModel();

        [TestClass]
        public class ConstructorTests : MessageStructureViewModelTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                Assert.IsNotNull(msvm.Vm_MessageTypeTree);
            }


        }
    }

    [TestClass]
    public class MenuBarItemViewModelTests
    {
        private readonly MenuBarItemViewModel mbivm = new MenuBarItemViewModel();

        [TestClass]
        public class ConstructorTests : MenuBarItemViewModelTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                Assert.AreEqual(null, mbivm.TopMenu);
            }


        }
    }

    [TestClass]
    public class FileTextViewModelTests
    {
        private readonly FileTextViewModel ftvm = new FileTextViewModel();

        [TestClass]
        public class ConstructorTests : FileTextViewModelTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                //Assert.AreEqual("", ftvm.FileTextstr);
            }
        }

    }



}
