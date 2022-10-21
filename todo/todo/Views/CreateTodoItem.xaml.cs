using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using todo.Models;
using todo.Services;

namespace todo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateTodoItem : ContentPage
    {
        public CreateTodoItem()
        {
            InitializeComponent();
            this.Title = "Crealo!";
        }

        async void OnSubmit(object sender, EventArgs args) 
        {
            TodoItem newTodoItem = new TodoItem();
            newTodoItem.Title = Entries_title.Text;
            newTodoItem.Description = Entries_description.Text;
            await App.Database.SaveTodoItemAsync(newTodoItem);
            await Navigation.PopAsync();
        }
        async void OnRandomClicked(object sender, EventArgs args)
        {
            toggleApiButtonLoading(true);

            TodoService api = new TodoService();
            TodoItem generated = await api.GetRandomTodoItem();

            toggleApiButtonLoading(false);

            Entries_title.Text = generated.Title;
            Entries_description.Text = generated.Description;
        }

        void toggleApiButtonLoading(bool isLoading)
        {
            if (isLoading)
            {
                Api_button.IsVisible = false;
                Api_loading.IsVisible = true;
            }
            else 
            {
                Api_loading.IsVisible = false;
                Api_button.IsVisible = true;
            }
        }
    }
}