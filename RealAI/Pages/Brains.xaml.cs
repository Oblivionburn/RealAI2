using RealAI.Util;

namespace RealAI.Pages;

public partial class Brains : ContentPage
{
    private Grid grid;
    public bool gridLoaded;
    public static Label lb_LoadedBrain;

    private Button bt_NewBrain;
    private Button bt_ImportBrain;
    private Button bt_LoadBrain;
    private Button bt_ExportBrain;
    private Button bt_DeleteBrain;

    private Label lb_Picker;
    private Picker pk_Brain;
    private int idx_Brain = -1;
    private List<string> BrainList = new List<string>();

    public Brains()
	{
		InitializeComponent();
        InitControls();
    }

    private void InitControls()
    {
        try
        {
            lb_LoadedBrain = new Label
            {
                FontSize = 24,
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            bt_NewBrain = new Button
            {
                Text = "New Brain",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            bt_NewBrain.Clicked -= NewBrain_Clicked;
            bt_NewBrain.Clicked += NewBrain_Clicked;

            bt_ImportBrain = new Button
            {
                Text = "Import Brain",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            bt_ImportBrain.Clicked -= ImportBrain_Clicked;
            bt_ImportBrain.Clicked += ImportBrain_Clicked;

            lb_Picker = new Label
            {
                FontSize = 20,
                BackgroundColor = BackgroundColor,
                Text = "Select Brain:",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };

            pk_Brain = new Picker
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };
            pk_Brain.SelectedIndexChanged -= Brain_SelectedIndexChanged;
            pk_Brain.SelectedIndexChanged += Brain_SelectedIndexChanged;

            bt_ExportBrain = new Button
            {
                Text = "Export",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            bt_ExportBrain.Clicked -= ExportBrain_Clicked;
            bt_ExportBrain.Clicked += ExportBrain_Clicked;

            bt_LoadBrain = new Button
            {
                Text = "Load",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            bt_LoadBrain.Clicked -= LoadBrain_Clicked;
            bt_LoadBrain.Clicked += LoadBrain_Clicked;

            bt_DeleteBrain = new Button
            {
                Text = "Delete",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            bt_DeleteBrain.Clicked -= DeleteBrain_Clicked;
            bt_DeleteBrain.Clicked += DeleteBrain_Clicked;

            LoadPage();
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.InitControls", ex.Message, ex.StackTrace);
        }
    }

    protected override void OnAppearing()
    {
        LoadPage();
    }

    public void LoadPage()
    {
        if (FolderPermissions.HasPermissions())
        {
            GetBrains();

            if (!gridLoaded)
            {
                LoadGrid();
            }
        }
        else
        {
            Shell.Current.GoToAsync("SelectFolder");
        }
    }

    private void LoadGrid()
    {
        try
        {
            grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            //Empty space at top to push everything down
            int row = 0;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.8, GridUnitType.Star) });
            BoxView top_empty_box = new BoxView();
            top_empty_box.Color = BackgroundColor;
            grid.Add(top_empty_box, 0, row);
            Grid.SetColumnSpan(top_empty_box, 9);

            //Loaded Brain
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_loadedBrain_box = new BoxView();
            grid.Add(lb_loadedBrain_box, 0, row);
            Grid.SetColumnSpan(lb_loadedBrain_box, 9);
            grid.Add(lb_LoadedBrain, 0, row);
            Grid.SetColumnSpan(lb_LoadedBrain, 9);

            if (!string.IsNullOrEmpty(SQLUtil.BrainFile))
            {
                lb_LoadedBrain.Text = "Loaded Brain: " + SQLUtil.BrainFile;
            }
            else
            {
                lb_LoadedBrain.Text = "No Brain Loaded";
            }

            //New Brain
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView bt_newBrain_box = new BoxView();
            grid.Add(bt_newBrain_box, 0, row);
            Grid.SetColumnSpan(bt_newBrain_box, 4);
            grid.Add(bt_NewBrain, 0, row);
            Grid.SetColumnSpan(bt_NewBrain, 4);

            //Import Brain
            BoxView bt_importBrain_box = new BoxView();
            grid.Add(bt_importBrain_box, 5, row);
            Grid.SetColumnSpan(bt_importBrain_box, 4);
            grid.Add(bt_ImportBrain, 5, row);
            Grid.SetColumnSpan(bt_ImportBrain, 4);

            //Empty space
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView first_empty_box = new BoxView();
            first_empty_box.Color = BackgroundColor;
            grid.Add(first_empty_box, 0, row);
            Grid.SetColumnSpan(first_empty_box, 9);

            //Brain Picker
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_picker_box = new BoxView();
            lb_picker_box.Color = BackgroundColor;
            grid.Add(lb_picker_box, 0, row);
            Grid.SetColumnSpan(lb_picker_box, 3);
            grid.Add(lb_Picker, 0, row);
            Grid.SetColumnSpan(lb_Picker, 3);

            BoxView picker_box = new BoxView();
            picker_box.Color = BackgroundColor;
            grid.Add(picker_box, 3, row);
            Grid.SetColumnSpan(picker_box, 6);
            grid.Add(pk_Brain, 3, row);
            Grid.SetColumnSpan(pk_Brain, 6);

            //Empty space
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView second_empty_box = new BoxView();
            second_empty_box.Color = BackgroundColor;
            grid.Add(second_empty_box, 0, row);
            Grid.SetColumnSpan(second_empty_box, 9);

            //Load Brain Button
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView bt_loadBrain_box = new BoxView();
            grid.Add(bt_loadBrain_box, 0, row);
            Grid.SetColumnSpan(bt_loadBrain_box, 3);
            grid.Add(bt_LoadBrain, 0, row);
            Grid.SetColumnSpan(bt_LoadBrain, 3);

            //Export Brain Button
            BoxView bt_exportBrain_box = new BoxView();
            grid.Add(bt_exportBrain_box, 3, row);
            Grid.SetColumnSpan(bt_exportBrain_box, 3);
            grid.Add(bt_ExportBrain, 3, row);
            Grid.SetColumnSpan(bt_ExportBrain, 3);

            //Delete Brain Button
            BoxView bt_deleteBrain_box = new BoxView();
            grid.Add(bt_deleteBrain_box, 6, row);
            Grid.SetColumnSpan(bt_deleteBrain_box, 3);
            grid.Add(bt_DeleteBrain, 6, row);
            Grid.SetColumnSpan(bt_DeleteBrain, 3);

            //Empty space underneath to push everything up
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            BoxView bottom_empty_box = new BoxView();
            bottom_empty_box.Color = BackgroundColor;
            grid.Add(bottom_empty_box, 0, row);
            Grid.SetColumnSpan(bottom_empty_box, 9);

            Content = grid;
            gridLoaded = true;
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.LoadGrid", ex.Message, ex.StackTrace);
        }
    }

    private void GetBrains()
    {
        try
        {
            idx_Brain = -1;
            BrainList.Clear();

            pk_Brain.Title = null;
            pk_Brain.SelectedIndex = -1;
            pk_Brain.ItemsSource = null;

            if (SQLUtil.BrainList.Count > 0)
            {
                for (int i = 0; i < SQLUtil.BrainList.Count; i++)
                {
                    string brain = SQLUtil.BrainList[i];

                    string file = AppUtil.GetBrainFile(brain + ".brain");
                    if (File.Exists(file))
                    {
                        BrainList.Add(brain);
                    }
                    else
                    {
                        SQLUtil.BrainList.Remove(brain);
                        AppUtil.SaveBrainList();

                        if (SQLUtil.BrainFile == brain)
                        {
                            SQLUtil.BrainFile = null;
                            lb_LoadedBrain.Text = "No Brain Loaded";
                        }

                        i--;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(SQLUtil.BrainFile))
            {
                string file = AppUtil.GetBrainFile(SQLUtil.BrainFile + ".brain");
                if (!File.Exists(file))
                {
                    SQLUtil.BrainFile = null;
                    lb_LoadedBrain.Text = "No Brain Loaded";
                }
            }
            else if (!string.IsNullOrEmpty(lb_LoadedBrain.Text))
            {
                lb_LoadedBrain.Text = "No Brain Loaded";
            }

            pk_Brain.ItemsSource = BrainList;
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.GetBrains", ex.Message, ex.StackTrace);
        }
    }

    private async void NewBrain_Clicked(object sender, EventArgs e)
    {
        try
        {
            string name = await DisplayPromptAsync("New Brain", "What's the name of this new brain?");
            if (!string.IsNullOrEmpty(name))
            {
                if (!SQLUtil.BrainList.Contains(name))
                {
                    string result = SQLUtil.NewBrain(name);
                    if (result == "SUCCESS")
                    {
                        lb_LoadedBrain.Text = "Loaded Brain: " + SQLUtil.BrainFile;

                        AppUtil.SetConfig("Last Loaded Brain", SQLUtil.BrainFile);

                        GetBrains();
                        Talk.Clear();

                        await DisplayAlert("New Brain", "'" + name + "' brain has been created and loaded.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", result, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("New Brain", "'" + name + "' brain already exists.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.NewBrain_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private async void ImportBrain_Clicked(object sender, EventArgs e)
    {
        try
        {
            Dictionary<DevicePlatform, IEnumerable<string>> mimeTypes = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "application/octet-stream" } }
            };

            PickOptions options = new PickOptions()
            {
                PickerTitle = "Which brain file are you wanting to import?",
                FileTypes = new FilePickerFileType(mimeTypes),
            };

            ImportBrain.FilePickerResult = await FilePicker.Default.PickAsync(options);
            if (ImportBrain.FilePickerResult != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(ImportBrain.FilePickerResult.FileName);
                if (!SQLUtil.BrainList.Contains(fileName))
                {
                    await Shell.Current.GoToAsync("ImportBrain");
                }
                else
                {
                    await DisplayAlert("Import Brain", "'" + fileName + "' brain already exists in list.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.ImportBrain_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private async void LoadBrain_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (idx_Brain == -1)
            {
                await DisplayAlert("Load Brain", "A brain to load must be selected from the list.", "OK");
                return;
            }

            string brain = pk_Brain.SelectedItem as string;
            if (!string.IsNullOrEmpty(brain))
            {
                SQLUtil.BrainFile = brain;
                AppUtil.SetConfig("Last Loaded Brain", SQLUtil.BrainFile);

                lb_LoadedBrain.Text = "Loaded Brain: " + brain;

                Talk.Clear();

                await DisplayAlert("Load Brain", "'" + brain + "' brain has been loaded.", "OK");
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.LoadBrain_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private async void ExportBrain_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (idx_Brain == -1)
            {
                await DisplayAlert("Export Brain", "A brain to export must be selected from the list.", "OK");
                return;
            }

            string brain = pk_Brain.SelectedItem as string;
            if (!string.IsNullOrEmpty(brain))
            {
                ExportBrain.SourceFile = AppUtil.GetBrainFile(brain + ".brain");
                ExportBrain.TargetFile = AppUtil.GetExternalPath(brain + ".brain");

                if (!string.IsNullOrEmpty(ExportBrain.SourceFile) &&
                    !string.IsNullOrEmpty(ExportBrain.TargetFile))
                {
                    await Shell.Current.GoToAsync("ExportBrain");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.ExportBrain_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private async void DeleteBrain_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (idx_Brain == -1)
            {
                await DisplayAlert("Delete Brain", "A brain to delete must be selected from the list.", "OK");
                return;
            }

            string brain = pk_Brain.SelectedItem as string;
            if (!string.IsNullOrEmpty(brain))
            {
                bool answer = await DisplayAlert("Delete Brain", "Are you sure you want to delete the '" + brain + "' brain?", "Yes", "No");
                if (answer)
                {
                    string result = SQLUtil.DeleteBrain(brain);
                    if (result == "SUCCESS")
                    {
                        GetBrains();
                        await DisplayAlert("Delete Brain", "'" + brain + "' brain has been deleted.", "OK");
                    }
                    else if (result == "NOT FOUND")
                    {
                        await DisplayAlert("Delete Brain", "'" + brain + "' brain was not found.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", result, "OK");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.DeleteBrain_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private void Brain_SelectedIndexChanged(object sender, EventArgs e)
    {
        idx_Brain = pk_Brain.SelectedIndex;
    }
}