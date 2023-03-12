using RealAI.Util;

namespace RealAI.Pages;

public partial class ImportBrain : ContentPage
{
    public static FileResult FilePickerResult;
    private Label lb_Importing;
    private ProgressBar pb_ProgressTime;
    private Label lb_ProgressTime;
    private DateTime ProgressStart;

    public ImportBrain()
	{
		InitializeComponent();
        InitControls();
    }

    private void InitControls()
    {
        try
        {
            ProgressStart = DateTime.Now;

            lb_Importing = new Label
            {
                FontSize = 24,
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            pb_ProgressTime = new ProgressBar
            {
                ProgressColor = Colors.DarkCyan
            };

            lb_ProgressTime = new Label
            {
                FontSize = 18,
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            AppUtil.InitProgress(pb_ProgressTime, lb_ProgressTime);

            LoadGrid();
        }
        catch (Exception ex)
        {
            Logger.AddLog("ImportBrain.InitControls", ex.Message, ex.StackTrace);
        }
    }

    private void LoadGrid()
    {
        try
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) });
            grid.Add(new BoxView(), 0, 0);

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            grid.Add(new BoxView(), 0, 1);
            grid.Add(lb_Importing, 0, 1);

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            grid.Add(new BoxView(), 0, 2);
            grid.Add(lb_ProgressTime, 0, 2);

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            grid.Add(new BoxView(), 0, 3);
            grid.Add(pb_ProgressTime, 0, 3);

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.Add(new BoxView(), 0, 4);

            Content = grid;
        }
        catch (Exception ex)
        {
            Logger.AddLog("ImportBrain.LoadGrid", ex.Message, ex.StackTrace);
        }
    }

    protected override void OnAppearing()
    {
        CopyFile();
    }

    public async void CopyFile()
    {
        try
        {
            string fileName = Path.GetFileNameWithoutExtension(FilePickerResult.FileName);
            lb_Importing.Text = "Importing '" + fileName + "' brain...";

            string targetFile = AppUtil.GetBrainFile(FilePickerResult.FileName);
            long totalBytes = new FileInfo(FilePickerResult.FullPath).Length;

            using (FileStream outputStream = File.OpenWrite(targetFile))
            {
                using (Stream inputStream = await FilePickerResult.OpenReadAsync())
                {
                    using (BinaryWriter writer = new BinaryWriter(outputStream))
                    {
                        using (BinaryReader reader = new BinaryReader(inputStream))
                        {
                            int bytesRead = 0;
                            int totalBytesRead = 0;
                            int bufferSize = 1024;
                            byte[] buffer = new byte[bufferSize];

                            using (inputStream)
                            {
                                do
                                {
                                    buffer = reader.ReadBytes(bufferSize);

                                    bytesRead = buffer.Length;

                                    totalBytesRead += bytesRead;
                                    double percent_completed = totalBytesRead / totalBytes;
                                    AppUtil.UpdateProgress(pb_ProgressTime, percent_completed, lb_ProgressTime, ProgressStart);

                                    writer.Write(buffer);
                                }
                                while (bytesRead > 0);
                            }
                        };
                    };
                };
            };

            AppUtil.FinishProgress(pb_ProgressTime, lb_ProgressTime, ProgressStart);

            SQLUtil.BrainFile = fileName;

            SQLUtil.BrainList.Add(fileName);
            string file = AppUtil.GetInternalPath("BrainList.txt");
            File.WriteAllLines(file, SQLUtil.BrainList);

            Brains.lb_LoadedBrain.Text = "Loaded Brain: " + SQLUtil.BrainFile;
            AppUtil.SetConfig("Last Loaded Brain", SQLUtil.BrainFile);

            Talk.Clear();

            await DisplayAlert("Import Brain", "'" + fileName + "' brain has been imported.\nTotal import time: " + lb_ProgressTime.Text, "OK");
        }
        catch (Exception ex)
        {
            Logger.AddLog("ImportBrain.CopyFile", ex.Message, ex.StackTrace);
        }

        FilePickerResult = null;
        await Shell.Current.GoToAsync("..");
    }
}