using RealAI.Util;

namespace RealAI.Pages;

public partial class Brains : ContentPage
{
    public Grid grid;
    public Label LoadedBrain;

	public Brains()
	{
		InitializeComponent();
        LoadGrid();
    }

    private void LoadGrid()
    {
        grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Absolute) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Absolute) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Absolute) });

        grid.Add(new BoxView(), 0, 0);
        LoadedBrain = new Label
        {
            FontSize = 24,
            TextColor = Color.FromArgb("#FFFFFF"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        grid.Add(LoadedBrain, 0, 0);
        if (!string.IsNullOrEmpty(SQLUtil.BrainFile))
        {
            LoadedBrain.Text = "Loaded Brain: " + SQLUtil.BrainFile;
        }
        else
        {
            LoadedBrain.Text = "No Brain Loaded";
        }

        grid.Add(new BoxView(), 0, 1);
        Button newBrain = new Button
        {
            Text = "New Brain",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 400
        };
        newBrain.Clicked += NewBrain_Clicked;
        grid.Add(newBrain, 0, 1);

        grid.Add(new BoxView(), 0, 2);
        Button deleteBrain = new Button
        {
            Text = "Delete Brain",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 400
        };
        deleteBrain.Clicked += DeleteBrain_Clicked;
        grid.Add(deleteBrain, 0, 2);

        if (SQLUtil.BrainList.Count > 0)
        {
            for (int i = 0; i < SQLUtil.BrainList.Count; i++)
            {
                string brain = SQLUtil.BrainList[i];

                string file = AppUtil.GetPath(brain + ".brain");
                if (File.Exists(file))
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Absolute) });
                    grid.Add(new BoxView(), 0, 3 + i);
                    Button loadBrain = new Button
                    {
                        Text = "Load Brain: " + brain,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 400
                    };
                    loadBrain.Clicked += LoadBrain_Clicked;
                    grid.Add(loadBrain, 0, 3 + i);
                }
                else
                {
                    SQLUtil.BrainList.Remove(brain);
                    AppUtil.SaveBrainList();

                    if (SQLUtil.BrainFile == brain)
                    {
                        SQLUtil.BrainFile = null;
                        LoadedBrain.Text = "No Brain Loaded";
                    }

                    i--;
                }
            }
        }
        else if (!File.Exists(AppUtil.GetPath(SQLUtil.BrainFile + ".brain")))
        {
            SQLUtil.BrainFile = null;
            LoadedBrain.Text = "No Brain Loaded";
        }

        Content = grid;
    }

    private async void NewBrain_Clicked(object sender, EventArgs e)
    {
        string name = await DisplayPromptAsync("New Brain", "What's the name of this new brain?");
        if (!string.IsNullOrEmpty(name))
        {
            if (!SQLUtil.BrainList.Contains(name))
            {
                string result = SQLUtil.NewBrain(name);
                if (result == "SUCCESS")
                {
                    LoadedBrain.Text = "Loaded Brain: " + SQLUtil.BrainFile;

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

    private async void DeleteBrain_Clicked(object sender, EventArgs e)
    {
        string name = await DisplayPromptAsync("Delete Brain", "Delete which brain?");
        if (!string.IsNullOrEmpty(name))
        {
            string result = SQLUtil.DeleteBrain(name);

            if (result == "SUCCESS")
            {
                LoadGrid();
                await DisplayAlert("Delete Brain", "'" + name + "' brain has been deleted.", "OK");
            }
            else if (result == "NOT FOUND")
            {
                await DisplayAlert("Delete Brain", "'" + name + "' brain was not found.", "OK");
            }
            else
            {
                await DisplayAlert("Error", result, "OK");
            }
        }
    }

    private async void LoadBrain_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string[] text = button.Text.Split(":");
        string brain = text[1].Trim();

        SQLUtil.BrainFile = brain;
        LoadedBrain.Text = "Loaded Brain: " + brain;

        Talk.Clear();

        await DisplayAlert("Load Brain", "'" + brain + "' brain has been loaded.", "OK");
    }
}