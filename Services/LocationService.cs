using Microsoft.Maui.Devices.Sensors;

public class LocationService
{
    private IGeolocation geolocation;
    private ITimer timer;
    
    public LocationService(IGeolocation geolocation)
    {
        this.geolocation = geolocation;
    }

    public async Task<Location> GetCurrentLocation()
    {
        try
        {
            var location = await geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(5)
            });

            return new Location
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately
            Debug.WriteLine($"Unable to get location: {ex.Message}");
            return null;
        }
    }
}