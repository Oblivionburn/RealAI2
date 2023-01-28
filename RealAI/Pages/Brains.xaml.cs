using RealAI.Util;

namespace RealAI.Pages;

public partial class Brains : ContentPage
{
    public Grid grid;
    public Label lb_LoadedBrain;
    public Button bt_NewBrain;

    public Brains()
	{
		InitializeComponent();
        LoadGrid();
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

            //Loaded Brain
            int row = 0;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Absolute) });
            BoxView lb_loadedBrain_box = new BoxView();
            grid.Add(lb_loadedBrain_box, 0, row);
            Grid.SetColumnSpan(lb_loadedBrain_box, 8);

            lb_LoadedBrain = new Label();
            lb_LoadedBrain.FontSize = 24;
            lb_LoadedBrain.TextColor = Color.FromArgb("#FFFFFF");
            lb_LoadedBrain.HorizontalTextAlignment = TextAlignment.Center;
            lb_LoadedBrain.VerticalTextAlignment = TextAlignment.Center;
            lb_LoadedBrain.HorizontalOptions = LayoutOptions.Center;
            lb_LoadedBrain.VerticalOptions = LayoutOptions.Center;
            grid.Add(lb_LoadedBrain, 0, row);
            Grid.SetColumnSpan(lb_LoadedBrain, 8);

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
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Absolute) });
            BoxView bt_newBrain_box = new BoxView();
            grid.Add(bt_newBrain_box, 1, row);
            Grid.SetColumnSpan(bt_newBrain_box, 6);

            bt_NewBrain = new Button();
            bt_NewBrain.Text = "New Brain";
            bt_NewBrain.HorizontalOptions = LayoutOptions.Fill;
            bt_NewBrain.VerticalOptions = LayoutOptions.Fill;
            bt_NewBrain.Clicked -= NewBrain_Clicked;
            bt_NewBrain.Clicked += NewBrain_Clicked;
            grid.Add(bt_NewBrain, 1, row);
            Grid.SetColumnSpan(bt_NewBrain, 6);

            if (SQLUtil.BrainList.Count > 0)
            {
                for (int i = 0; i < SQLUtil.BrainList.Count; i++)
                {
                    string brain = SQLUtil.BrainList[i];

                    string file = AppUtil.GetPath(brain + ".brain");
                    if (File.Exists(file))
                    {
                        //Empty space between Load/Delete Brain buttons
                        row++;
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) });
                        BoxView empty_box = new BoxView();
                        grid.Add(empty_box, 0, row);
                        Grid.SetColumnSpan(empty_box, 8);

                        row++;

                        //Load Brain
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) });
                        BoxView bt_loadBrain_box = new BoxView();
                        grid.Add(bt_loadBrain_box, 0, row);
                        Grid.SetColumnSpan(bt_loadBrain_box, 4);

                        Button bt_LoadBrain = new Button();
                        bt_LoadBrain.Text = "Load Brain: " + brain;
                        bt_LoadBrain.HorizontalOptions = LayoutOptions.Fill;
                        bt_LoadBrain.VerticalOptions = LayoutOptions.Fill;
                        bt_LoadBrain.Clicked += LoadBrain_Clicked;
                        grid.Add(bt_LoadBrain, 0, row);
                        Grid.SetColumnSpan(bt_LoadBrain, 4);

                        //Delete Brain
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) });
                        BoxView bt_deleteBrain_box = new BoxView();
                        grid.Add(bt_deleteBrain_box, 4, row);
                        Grid.SetColumnSpan(bt_deleteBrain_box, 4);

                        Button bt_DeleteBrain = new Button();
                        bt_DeleteBrain.Text = "Delete Brain: " + brain;
                        bt_DeleteBrain.HorizontalOptions = LayoutOptions.Fill;
                        bt_DeleteBrain.VerticalOptions = LayoutOptions.Fill;
                        bt_DeleteBrain.Clicked += DeleteBrain_Clicked;
                        grid.Add(bt_DeleteBrain, 4, row);
                        Grid.SetColumnSpan(bt_DeleteBrain, 4);
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
            else
            {
                string file = AppUtil.GetPath(SQLUtil.BrainFile + ".brain");
                if (!File.Exists(file))
                {
                    SQLUtil.BrainFile = null;
                    lb_LoadedBrain.Text = "No Brain Loaded";
                }
            }

            Content = grid;
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.LoadGrid", ex.Message, ex.StackTrace);
        }
    }

    private async void NewBrain_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (SQLUtil.BrainList.Count < 8)
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

                            LoadGrid();
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
            else
            {
                await DisplayAlert("New Brain", "No more brains can be added.", "OK");
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.NewBrain_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private async void DeleteBrain_Clicked(object sender, EventArgs e)
    {
        try
        {
            Button bt_DeleteBrain = (Button)sender;
            string[] text = bt_DeleteBrain.Text.Split(":");
            string brain = text[1].Trim();
                
            if (!string.IsNullOrEmpty(brain))
            {
                bool answer = await DisplayAlert("Delete Brain", "Are you sure you want to delete the '" + brain + "' brain?", "Yes", "No");
                if (answer)
                {
                    string result = SQLUtil.DeleteBrain(brain);
                    if (result == "SUCCESS")
                    {
                        LoadGrid();
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

    private async void LoadBrain_Clicked(object sender, EventArgs e)
    {
        try
        {
            Button button = (Button)sender;
            string[] text = button.Text.Split(":");
            string brain = text[1].Trim();

            SQLUtil.BrainFile = brain;
            AppUtil.SetConfig("Last Loaded Brain", SQLUtil.BrainFile);

            lb_LoadedBrain.Text = "Loaded Brain: " + brain;

            Talk.Clear();

            await DisplayAlert("Load Brain", "'" + brain + "' brain has been loaded.", "OK");
        }
        catch (Exception ex)
        {
            Logger.AddLog("Brains.LoadBrain_Clicked", ex.Message, ex.StackTrace);
        }
    }
}