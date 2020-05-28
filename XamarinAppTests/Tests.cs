using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XamarinAppTests
{
    [TestFixture(Platform.Android)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void NavigateToAddBookPage()
        {
            app.WaitForElement(c => c.Marked("Add"));
            app.Tap(c => c.Marked("Add"));
            app.WaitForElement(c => c.Marked("Add Book Page"));
            var element = app.Query(c => c.Marked("Add Book Page")).First();
            Assert.AreEqual("Add Book Page", element.Text);

        }
        [Test]
        [TestCase("Harry Potter", "J.K. Rowling")]
        [TestCase("Władca Pierścieni", "J.R.R Tolkien")]
        [TestCase("Język UML 2.0 w modelowaniu systemów informatycznych", "B. Marcinkowski")]
        public void AddBookNameAndAuthor(String name = "Harry Potter", String author = "J.K. Rowling")
        {
            NavigateToAddBookPage();
            app.EnterText(c => c.Marked("Book name"), name);
            app.EnterText(c => c.Marked("Author"), author);
            Assert.AreEqual(name, app.Query(c => c.Marked(name)).First().Text);
            Assert.AreEqual(author, app.Query(c => c.Marked(author)).First().Text);

        }
        [Test]
        public void AddBookToList()
        {
            AddBookNameAndAuthor("Władca Pierścieni", "J.R.R Tolkien");
            app.Tap(c => c.Marked("Save"));
            app.WaitForElement(c => c.Marked("Book saved!"));
            Assert.AreEqual("Book saved!", app.Query(c => c.Marked("Book saved!")).First().Text);
        }
        [Test]
        public void AddMultipleBooksToList()
        {
            for(int i = 0; i < 5;  i++) {
                AddBookToList();
                app.Back();
                app.Back();
                app.WaitForElement(c => c.Marked("Delete"));
            }
            var ElementsCount = app.Query(x => x.Marked("Władca Pierścieni")).Length;
            Assert.IsTrue(ElementsCount == 5);
        }

        [Test]
        public void RemoveBookFromList()
        {
            AddBookToList();
            app.Back();
            app.Back();
            app.WaitForElement(c => c.Marked("Delete"));
            var ElementsCount = app.Query(x => x.Marked("ListView").Child()).Length;
            app.Tap(c => c.Marked("Delete"));
            Assert.IsEmpty(app.Query(x => x.Marked("ListView").Child()));
        }

        [Test]
        public void NavigateToEditPage()
        {
            AddBookToList();
            app.Back();
            app.Back();
            app.WaitForElement(c => c.Marked("Edit"));
            app.Tap(c => c.Marked("Edit"));
            var element = app.Query(c => c.Marked("Edit Book Page")).First();
            Assert.AreEqual("Edit Book Page", element.Text);
        }
        [Test]
        public void EnterNewNameAndAuthor()
        {
            NavigateToEditPage();
            app.ClearText(c => c.Marked("Władca Pierścieni"));
            app.ClearText(c => c.Marked("J.R.R Tolkien"));
            app.EnterText(c => c.Marked("Book name"), "Pieśń lodu i ognia");
            app.EnterText(c => c.Marked("Author"), "George R.R Martin");
            Assert.AreEqual("Pieśń lodu i ognia", app.Query(c => c.Marked("Pieśń lodu i ognia")).First().Text);
            Assert.AreEqual("George R.R Martin", app.Query(c => c.Marked("George R.R Martin")).First().Text);
        }
        [Test]
        public void EditExistingElement()
        {
            EnterNewNameAndAuthor();
            app.Tap(c => c.Marked("Save"));
            app.WaitForElement(c => c.Marked("Edit"));
            Assert.AreEqual("Pieśń lodu i ognia", app.Query(c => c.Marked("Pieśń lodu i ognia")).First().Text);
            Assert.AreEqual("George R.R Martin", app.Query(c => c.Marked("George R.R Martin")).First().Text);
        }
    }
}
