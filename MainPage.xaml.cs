public partial class MainPage : ContentPage
{
    private readonly LocationService _locationService;
    private readonly LocationDatabase _database;
    private IDispatcherTimer _timer;
    private bool _isTracking;
    private readonly List<Location> _locations = new();

    public MainPage()
    {
        InitializeComponent();
        _locationService = new LocationService(Geolocation.Default);
        _database = new LocationDatabase(Path.Combine(FileSystem.AppDataDirectory, "locations.db"));
        
        InitializeTimer();
        LoadSavedLocations();
    }

    private void InitializeTimer()
    {
        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMinutes(1);
        _timer.Tick += Timer_Tick;
    }

    private async void Timer_Tick(object sender, EventArgs e)
    {
        if (_isTracking)
        {
            var location = await _locationService.GetCurrentLocation();
            if (location != null)
            {
                await _database.SaveLocationAsync(location);
                _locations.Add(location);
                UpdateHeatmap();
            }
        }
    }

    private async void LoadSavedLocations()
    {
        var locations = await _database.GetLocationsAsync();
        _locations.AddRange(locations);
        UpdateHeatmap();
    }

    private void UpdateHeatmap()
    {
        // Clear existing heatmap
        TrackingMap.MapElements.Clear();

        // Create heatmap overlay
        foreach (var location in _locations)
        {
            var circle = new Circle
            {
                Center = new Position(location.Latitude, location.Longitude),
                Radius = Distance.FromMeters(50),
                StrokeColor = Colors.Red,
                StrokeWidth = 8,
                FillColor = Color.FromArgb("#88FF0000")
            };
            TrackingMap.MapElements.Add(circle);
        }
    }

    private void OnStartTrackingClicked(object sender, EventArgs e)
    {
        _isTracking = true;
        _timer.Start();
        StartTrackingButton.IsEnabled = false;
        StopTrackingButton.IsEnabled = true;
    }

    private void OnStopTrackingClicked(object sender, EventArgs e)
    {
        _isTracking = false;
        _timer.Stop();
        StartTrackingButton.IsEnabled = true;
        StopTrackingButton.IsEnabled = false;
    }
}