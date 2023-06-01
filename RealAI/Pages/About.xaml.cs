using RealAI.Util;

namespace RealAI.Pages;

public partial class About : ContentPage
{
    public Label lb_Version;
	public Label lb_DevelopedBy;

    public About()
	{
		InitializeComponent();

        lb_Version = new Label();
        lb_Version.FontSize = 24;
        lb_Version.BackgroundColor = BackgroundColor;
        lb_Version.Text = "Real AI v" + AppUtil.GetVersion();
        lb_Version.HorizontalTextAlignment = TextAlignment.Center;
        lb_Version.VerticalTextAlignment = TextAlignment.Center;
        lb_Version.HorizontalOptions = LayoutOptions.Fill;
        lb_Version.VerticalOptions = LayoutOptions.Center;

        lb_DevelopedBy = new Label();
        lb_DevelopedBy.FontSize = 24;
        lb_DevelopedBy.BackgroundColor = BackgroundColor;
        lb_DevelopedBy.Text = "Developed By:\nOblivionburn Productions";
        lb_DevelopedBy.HorizontalTextAlignment = TextAlignment.Center;
        lb_DevelopedBy.VerticalTextAlignment = TextAlignment.Center;
        lb_DevelopedBy.HorizontalOptions = LayoutOptions.Fill;
        lb_DevelopedBy.VerticalOptions = LayoutOptions.Center;

        LoadGrid();
    }

    private void LoadGrid()
    {
        Grid grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition());

        //Empty space at top to push everything down
        int row = 0;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.4, GridUnitType.Star) });
        BoxView top_empty_box = new BoxView();
        top_empty_box.Color = BackgroundColor;
        grid.Add(top_empty_box, 0, row);

        //Version
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
        BoxView lb_version_box = new BoxView();
        lb_version_box.Color = BackgroundColor;
        grid.Add(lb_version_box, 0, row);
        grid.Add(lb_Version, 0, row);

        //Empty space between
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
        BoxView middle_empty_box = new BoxView();
        middle_empty_box.Color = BackgroundColor;
        grid.Add(middle_empty_box, 0, row);

        //DevelopedBy
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.6, GridUnitType.Star) });
        BoxView lb_developed_box = new BoxView();
        lb_developed_box.Color = BackgroundColor;
        grid.Add(lb_developed_box, 0, row);
        grid.Add(lb_DevelopedBy, 0, row);

        //Empty space underneath to push everything up
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        BoxView bottom_empty_box = new BoxView();
        bottom_empty_box.Color = BackgroundColor;
        grid.Add(bottom_empty_box, 0, row);

        Content = grid;
    }
}