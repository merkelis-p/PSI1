namespace WakeyWakey;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private async void OnUploadButtonClick(object sender, EventArgs e)
    {
        try
        {
            // Use the FilePicker to select a calendar file
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.MacCatalyst, new[] { "ics" } }
                })
            });


            if (fileResult != null)
            {
                // Process the selected file (e.g., save it, parse it, etc.)
                // Example: var stream = await fileResult.OpenReadAsync();
                var stream = await fileResult.OpenReadAsync();
                var fileName = fileResult.FileName;
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

                using (var fileStream = File.Create(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}


