using RealAI.Util;

namespace RealAI.Pages;

public partial class SelectFolder : ContentPage
{
    public Label lb_SelectFolder;
    public Button bt_SelectFolder;

    public SelectFolder()
	{
		InitializeComponent();
        InitControls();
    }

    private void InitControls()
    {
        try
        {
            lb_SelectFolder = new Label
            {
                FontSize = 24,
                Text = "Select a public folder to read/write files from (this will grant this app full permissions on the selected folder):",
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            bt_SelectFolder = new Button
            {
                Text = "Select Folder",
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            bt_SelectFolder.Clicked -= SelectFolder_Clicked;
            bt_SelectFolder.Clicked += SelectFolder_Clicked;

            LoadGrid();
        }
        catch (Exception ex)
        {
            Logger.AddLog("SelectFolder.InitControls", ex.Message, ex.StackTrace);
        }
    }

    private void LoadGrid()
    {
        try
        {
            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });

            grid.Add(lb_SelectFolder, 0, 0);
            Grid.SetColumnSpan(lb_SelectFolder, 5);
            Grid.SetRowSpan(lb_SelectFolder, 3);

            grid.Add(bt_SelectFolder, 1, 2);
            Grid.SetColumnSpan(bt_SelectFolder, 3);

            Content = grid;
        }
        catch (Exception ex)
        {
            Logger.AddLog("SelectFolder.LoadGrid", ex.Message, ex.StackTrace);
        }
    }

    private void SelectFolder_Clicked(object sender, EventArgs e)
    {
        try
        {
            FolderPermissions.RequestSelectFolder();
        }
        catch (Exception ex)
        {
            Logger.AddLog("SelectFolder.SelectFolder_Clicked", ex.Message, ex.StackTrace);
        }
    }
}