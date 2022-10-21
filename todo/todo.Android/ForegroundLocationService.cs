using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using todo.Services;
using Xamarin.Forms;

namespace todo.Droid
{
	[Service]
	public class ForeGroundLocationService : Service
	{
		CancellationTokenSource _cts;
		public const int SERVICE_RUNNING_NOTIFICATION_ID = 10001;

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			_cts = new CancellationTokenSource();

			Notification notification = new NotificationHelper().GetForegroundLocationServiceStartedNotification();
			StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);

			Task.Run(() => {
				try
				{
					var locShared = new LocationService();
					locShared.Run(_cts.Token).Wait();
				}
				catch (Android.OS.OperationCanceledException)
				{
				}
				finally
				{
					if (_cts.IsCancellationRequested)
					{
						var message = new StopLocationServiceMessage();
						Device.BeginInvokeOnMainThread(
							() => MessagingCenter.Send(message, "ServiceStopped")
						);
					}
				}
			}, _cts.Token);

			return StartCommandResult.Sticky;
		}

		public override void OnDestroy()
		{
			if (_cts != null)
			{
				_cts.Token.ThrowIfCancellationRequested();
				_cts.Cancel();
			}
			base.OnDestroy();
		}

		public override void OnTaskRemoved(Intent rootIntent)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
			{
				StopForeground(StopForegroundFlags.Remove);
			}
			else
			{
				StopForeground(true);
			}

			Console.WriteLine("Location Service has been stopped!");

			StopSelf();

			base.OnDestroy();
			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}
	}
}