using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Book>();

                var books = conn.Table<Book>().ToList();
                booksListView.ItemsSource = books;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Book>();

                var books = conn.Table<Book>().ToList();
                booksListView.ItemsSource = books;
            }
        }

        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewBookPage());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var id = int.Parse(((Button)sender).BindingContext.ToString());
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                var books = conn.Table<Book>();
                books.Delete(x => x.Id == id);
                booksListView.ItemsSource = books;
            }
        }
        private void Edit(object sender, EventArgs e)
        {
            var id = int.Parse(((Button)sender).BindingContext.ToString());
            Navigation.PushAsync(new EditBookPage(id));
        }
    }
}
