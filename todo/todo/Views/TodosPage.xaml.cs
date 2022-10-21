using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using todo.Models;
using todo.Services;
using Xamarin.Essentials;
using Plugin.Geolocator;

namespace todo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodosPage : ContentPage
    {
        public TodosPage()
        {
            InitializeComponent();
            this.Title = "I tuoi TODOs";

            if (Device.RuntimePlatform == Device.Android)
            {
                /*
                 * registering to the location service messages
                 */
                MessagingCenter.Subscribe<LocationMessage>(this, "Location", message => {
                    Device.BeginInvokeOnMainThread(() => {
                        Console.WriteLine($"{message.Latitude}, {message.Longitude}, {DateTime.Now.ToLongTimeString()}");
                    });
                });

                MessagingCenter.Subscribe<StopLocationServiceMessage>(this, "ServiceStopped", message => {
                    Device.BeginInvokeOnMainThread(() => {
                        Console.WriteLine("Location Service has been stopped!");
                    });
                });

                MessagingCenter.Subscribe<LocationErrorMessage>(this, "LocationError", message => {
                    Device.BeginInvokeOnMainThread(() => {
                        Console.WriteLine("There was an error updating location!");
                    });
                });

                if (Preferences.Get("LocationServiceRunning", false) == true)
                {
                    StartLocationService();
                }
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Retrieve all the notes from the database, and set them as the
            // data source for the CollectionView.
            collectionView.ItemsSource = await App.Database.GetTodoItemsAsync();
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                TodoItem todoItem = (TodoItem)e.CurrentSelection.FirstOrDefault();
                await DisplayAlert(todoItem.Title, todoItem.Description, "Ok, l'ho visto!");
                //await Shell.Current.GoToAsync($"{nameof(NoteEntryPage)}?{nameof(NoteEntryPage.ItemId)}={note.ID.ToString()}");
            }
        }

        async void OnCreateButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CreateTodoItem());
        }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        {
            string email = await DisplayPromptAsync("Inseriscilo!", "Inserisci la tua email!");
            string password = await DisplayPromptAsync("Inseriscilo!", "Inserisci la tua password!");

            TodoService api = new TodoService();
            string response  = await api.Login(email, password);

            await DisplayAlert("", response, "ok");
        }

        async void OnLocationButtonClicked(object sender, EventArgs args)
        {
            var permission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationAlways>();
            if (permission == Xamarin.Essentials.PermissionStatus.Denied)
            {
                Console.WriteLine("Permessi posizione IOS mancanti");
                return;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {

                if (CrossGeolocator.Current.IsListening)
                {
                    await CrossGeolocator.Current.StopListeningAsync();
                    CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;

                    return;
                }

                await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 10, false, new Plugin.Geolocator.Abstractions.ListenerSettings
                {
                    ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                    AllowBackgroundUpdates = true,
                    DeferLocationUpdates = false,
                    DeferralDistanceMeters = 10,
                    DeferralTime = TimeSpan.FromSeconds(5),
                    ListenForSignificantChanges = true,
                    PauseLocationUpdatesAutomatically = true
                });

                CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                if (Preferences.Get("LocationServiceRunning", false) == false)
                {
                    StartLocationService();
                }
                else
                {
                    StopLocationService();
                }
            }
        }

        private void StartLocationService()
        {
            var startServiceMessage = new StartLocationServiceMessage();
            MessagingCenter.Send(startServiceMessage, "ServiceStarted");
            Preferences.Set("LocationServiceRunning", true);
            Console.WriteLine("Location Service has been started!");
        }

        private void StopLocationService()
        {
            var stopServiceMessage = new StopLocationServiceMessage();
            MessagingCenter.Send(stopServiceMessage, "ServiceStopped");
            Preferences.Set("LocationServiceRunning", false);
            Console.WriteLine("Location Service has been stopped!");
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Console.WriteLine($"{e.Position.Latitude}, {e.Position.Longitude}, {e.Position.Timestamp.TimeOfDay}");
        }
    }
}