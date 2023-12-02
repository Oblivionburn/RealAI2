using Android.OS;
using Java.IO;
using RealAI.Util;

namespace RealAI.Pages;

public partial class ExportBrain : ContentPage
{
    public static string SourceFile;
    public static string TargetFile;
    private Label lb_Exporting;
    private ProgressBar pb_ProgressTime;
    private Label lb_ProgressTime;
    private DateTime ProgressStart;

    public ExportBrain()
    {
        InitializeComponent();
        InitControls();
    }

    private void InitControls()
    {
        try
        {
            ProgressStart = DateTime.Now;

            lb_Exporting = new Label
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
            grid.Add(lb_Exporting, 0, 1);

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
            string fileName = Path.GetFileNameWithoutExtension(SourceFile);
            lb_Exporting.Text = "Exporting '" + fileName + "' brain...";

            long totalBytes = new FileInfo(SourceFile).Length;

            //Get file using persisted permissions
            using (ParcelFileDescriptor fileDescriptor = FolderPermissions.GetFileDescriptor(Path.GetFileName(TargetFile), Path.GetExtension(TargetFile)))
            {
                using (FileOutputStream outputStream = new FileOutputStream(fileDescriptor.FileDescriptor))
                {
                    using (FileStream inputStream = System.IO.File.OpenRead(SourceFile))
                    {
                        using (BinaryReader reader = new BinaryReader(inputStream))
                        {
                            int bytesRead = 0;
                            int totalBytesRead = 0;
                            int bufferSize = 1024;
                            byte[] buffer = new byte[bufferSize];

                            using (outputStream)
                            {
                                do
                                {
                                    buffer = reader.ReadBytes(bufferSize);

                                    bytesRead = buffer.Count();

                                    totalBytesRead += bytesRead;
                                    double percent_completed = totalBytesRead / totalBytes;
                                    AppUtil.UpdateProgress(pb_ProgressTime, percent_completed, lb_ProgressTime, ProgressStart);

                                    outputStream.Write(buffer);
                                }
                                while (bytesRead > 0);
                            }
                        };
                    };
                };
            }

            AppUtil.FinishProgress(pb_ProgressTime, lb_ProgressTime, ProgressStart);

            await DisplayAlert("Export Brain", "'" + fileName + "' has been exported to " + AppUtil.SelectedFolder + " folder.\n\nTotal export time: " + lb_ProgressTime.Text, "OK");
        }
        catch (Exception ex)
        {
            Logger.AddLog("ExportBrain.CopyFile", ex.Message, ex.StackTrace);
        }

        SourceFile = null;
        TargetFile = null;
        await Shell.Current.GoToAsync("..");
    }
}