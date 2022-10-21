using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Essentials;
using Android.Content;
using Xamarin.Forms;

namespace todo.Droid
{
    [Activity(Label = "todo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Intent foregroundLocationServiceIntent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            foregroundLocationServiceIntent = new Intent(this, typeof(ForeGroundLocationService));
            SetServiceMethods();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void SetServiceMethods()
        {
            MessagingCenter.Subscribe<StartLocationServiceMessage>(this, "ServiceStarted", message => {
                if (!IsServiceRunning(typeof(ForeGroundLocationService)))
                {
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                    {
                        StartForegroundService(foregroundLocationServiceIntent);
                    }
                    else
                    {
                        StartService(foregroundLocationServiceIntent);
                    }
                }
            });

            MessagingCenter.Subscribe<StopLocationServiceMessage>(this, "ServiceStopped", message => {
                if (IsServiceRunning(typeof(ForeGroundLocationService)))
                    StopService(foregroundLocationServiceIntent);
            });
        }

        private bool IsServiceRunning(System.Type cls)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
            /*
             * GetRunningServices deprecato per ragioni di sicurezza perche' si potevano ottenere
             * informazioni sulle altre applicazioni, pero' e' ancora funzionante per servizi della
             * stessa applicazione (come in questo caso).
             */
            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName.Equals(Java.Lang.Class.FromType(cls).CanonicalName))
                {
                    return true;
                }
            }
            return false;
        }

        /*
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == RequestCode)
            {
                if (Android.Provider.Settings.CanDrawOverlays(this))
                {

                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
        */
    }
}