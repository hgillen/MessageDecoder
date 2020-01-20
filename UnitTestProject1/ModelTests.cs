using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;
using MessageDecoder.Models;

namespace ModelTests
{

    [TestClass]
    public class MainModelTests
    {
        private readonly MainModel mm = new MainModel();

        [TestClass]
        public class ConstructorTests : MainModelTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                Assert.IsNotNull(mm.Configuration);
                Assert.AreEqual(1, mm.Configuration.Count);
                //Assert.IsNotNull(mm.MessageFields);
                //Assert.AreEqual(mm.IsDetails, false);
                //Assert.AreEqual(mm.CurrentField, null);
                //Assert.AreEqual(mm.CurrentView, null);
            }

        }

        [TestClass]
        public class SaveFileTests : MainModelTests
        {
            [TestMethod]
            public void NominalSave()
            {

            }
        }

    }


    [TestClass]
    public class MessageFieldDetailsModelTests
    {
        [TestClass]
        public class ConstructorTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                MessageFieldDetailsModel mfdm = new MessageFieldDetailsModel(null);
                Assert.AreEqual(mfdm.FieldName, "New Field");
                Assert.AreEqual(mfdm.BitStart, 0);
                Assert.AreEqual(mfdm.BitLength, 1);
                Assert.AreEqual(mfdm.BitLengthString, "1");
                Assert.AreEqual(mfdm.IsSpecialType, false);
            }
        }
    }

    [TestClass]
    public class MessageFieldModelTests
    {
        [TestClass]
        public class ConstructorTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                MessageFieldModel mfm = new MessageFieldModel();
                Assert.AreEqual(mfm.MessageFieldDetails.FieldName, "New Field");
                Assert.AreEqual(mfm.MessageFieldDetails.BitStart, 0);
                Assert.AreEqual(mfm.MessageFieldDetails.BitLength, 1);
                Assert.AreEqual(mfm.MessageFieldDetails.BitLengthString, "1");
                Assert.AreEqual(mfm.MessageFieldDetails.IsSpecialType, false);
            }
        }
    }


    [TestClass]
    public class MessageTypeTreeTests
    {
        private readonly MessageTypeTreeModel mttm = new MessageTypeTreeModel();

        [TestClass]
        public class ConstructorTests : MessageTypeTreeTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                //MessageTypeTreeModel mttm = new MessageTypeTreeModel();
                const int numFields = 32;
                Assert.AreEqual(numFields, mttm.MessageFields.Count);
                Assert.IsNull(mttm.MessageSubtypes);
                Assert.IsFalse(mttm.IsSelected);
                Assert.AreEqual("New Type", mttm.SubtypeName);
                Assert.IsFalse(mttm.UseEditTemplate);
            }
        }

        [TestClass]
        public class RenameTests : MessageTypeTreeTests
        {
            [TestMethod]
            public void NominalRename()
            {
                ((ICommand)mttm.RenameCommand).Execute(null);
                Assert.AreEqual(mttm.UseEditTemplate, true);
            }
        }

        [TestClass]
        public class AddSubtypeTests : MessageTypeTreeTests
        {
            [TestMethod]
            public void NominalAdd()
            {
                ((ICommand)mttm.AddSubtypeCommand).Execute(null);
                Assert.AreEqual(mttm.MessageSubtypes.Count, 1);
                Assert.AreEqual(mttm.MessageSubtypes[0].SubtypeName, "New Subtype");
            }
        }

        //[TestClass]
        //public class DeleteSubtypeTests : MessageTypeTreeTests
        //{
        //    [TestMethod]
        //    public void NominalDelete()
        //    {
        //        ((ICommand)mttm.AddSubtypeCommand).Execute(null);
        //        ((ICommand)mttm.DeleteCommand).Execute(mttm.MessageSubtypes[0]);
        //        Assert.AreEqual(mttm.MessageSubtypes.Count, 0);
        //    }
        //}
    }


    [TestClass]
    public class MessageTypeDetailsTests
    {
        [TestClass]
        public class ConstructorTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                MessageTypeDetailsModel mtdm = new MessageTypeDetailsModel();
                //Assert.AreEqual(mtdm.MessageFieldDetails.FieldName, "New Field");
                //Assert.AreEqual(mtdm.MessageFieldDetails.BitStart, 0);
                //Assert.AreEqual(mtdm.MessageFieldDetails.BitLength, 1);
                //Assert.AreEqual(mtdm.MessageFieldDetails.BitLengthString, "1");
                //Assert.AreEqual(mtdm.MessageFieldDetails.IsSpecialType, false);
            }
        }
    }

    [TestClass]
    public class MessageBarItemModelTests
    {
        [TestClass]
        public class ConstructorTests
        {
            [TestMethod]
            public void BaseConstructor()
            {
                MenuBarItemModel mbim = new MenuBarItemModel();
                //Assert.AreEqual(mbim.Header, "");
                //Assert.AreEqual(mbim.Header, "File");
                //Assert.AreEqual(mbim.ChildMenus.Count, 3);
                //Assert.AreEqual(mbim.ChildMenus[0].Header, "Open Config");
                //Assert.AreEqual(mbim.ChildMenus[0].ChildMenus.Count, 0);
                //Assert.AreEqual(mbim.ChildMenus[1].Header, "Open Data");
                //Assert.AreEqual(mbim.ChildMenus[1].ChildMenus.Count, 0);
                //Assert.AreEqual(mbim.ChildMenus[2].Header, "Save Config");
                //Assert.AreEqual(mbim.ChildMenus[2].ChildMenus.Count, 0);
            }
        }
    }

}
