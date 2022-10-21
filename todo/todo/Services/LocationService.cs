using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace todo.Services
{
    public class LocationService
    {
        public async Task Run(CancellationToken token) 
        {
			await Task.Run(async () => {
				while (true)
				{
					token.ThrowIfCancellationRequested();
					try
					{
						await Task.Delay(2000);

						var request = new GeolocationRequest(GeolocationAccuracy.High);
						var location = await Geolocation.GetLocationAsync(request);
						if (location != null)
						{
							var message = new LocationMessage
							{
								Latitude = location.Latitude,
								Longitude = location.Longitude
							};

							Device.BeginInvokeOnMainThread(() =>
							{
								MessagingCenter.Send(message, "Location");
							});
						}
					}
					catch (Exception e)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							var errormessage = new LocationErrorMessage();
							MessagingCenter.Send(errormessage, "LocationError");
						});
					}
				}
			}, token);
		}
	}
}
