using System;
using System.Collections.Generic;
using todo.Views;
using Xamarin.Forms;

namespace todo
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("listTodos", typeof(TodosPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("listTodos");
        }
    }
}
