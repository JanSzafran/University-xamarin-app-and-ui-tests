using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBookPage : ContentPage
    {
        Book book = new Book();
        public EditBookPage()
        {
            
        }
        public EditBookPage(int id)
        {
            InitializeComponent();
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                var books = conn.Table<Book>();
                book = books.First(x => x.Id == id);
                nameEntry.Text = book.Name;
                authorEntry.Text = book.Author;
            }
        }

        private void EditBookButton(object sender, EventArgs e)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                var books = conn.Table<Book>();
                var book = books.First(x => x.Id == this.book.Id);
                book.Name = nameEntry.Text;
                book.Author = authorEntry.Text;
                conn.RunInTransaction(() =>
                {
                    conn.Update(book);
                });
                Navigation.PushAsync(new MainPage());
            }
        }
    }
}