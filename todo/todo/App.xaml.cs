using System;
using todo.Services;
using todo.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using todo.Data;

namespace todo
{
    public partial class App : Application
    {
        static DatabaseSQLite database;
        static String token;

        // Create the database connection as a singleton.
        public static DatabaseSQLite Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseSQLite(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Todo.db3"));
                }
                return database;
            }
        }

        public static string Token
        {
            set
            {
                token = value;
            }

            get
            {
                if (token == null) {
                    return "Effettua il login per ottenere un token.";
                }
                return token;
            }
        }


        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new TodosPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
