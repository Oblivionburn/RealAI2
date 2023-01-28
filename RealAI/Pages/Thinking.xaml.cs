using RealAI.Util;
using System.Text;

namespace RealAI.Pages;

public partial class Thinking : ContentPage
{
    public static Editor ThinkBox;

    public static System.Timers.Timer ThinkTimer;
    public static List<string> Thoughts = new List<string>();

    public Thinking()
	{
		InitializeComponent();

        ThinkBox = new Editor();
        ThinkBox.IsReadOnly = true;
        ThinkBox.FontSize = 18;
        ThinkBox.HorizontalOptions = LayoutOptions.Fill;
        ThinkBox.VerticalOptions = LayoutOptions.Fill;

        LoadGrid();

        ThinkTimer = new System.Timers.Timer();
        ThinkTimer.Interval = Options.ThinkSpeed;
        ThinkTimer.Elapsed += ThinkTimer_Elapsed;
        ThinkTimer.Start();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ThinkTimer.Start();
    }

    private void ThinkTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        Think();
    }

    private void LoadGrid()
    {
        Grid grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        grid.RowDefinitions.Add(new RowDefinition());

        grid.Add(new BoxView(), 0, 0);
        grid.Add(ThinkBox, 0, 0);

        Content = grid;

        DisplayThinking();
    }

    private void SetThinkBox(string output)
    {
        if (MainThread.IsMainThread)
        {
            ThinkBox.Text = output;
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ThinkBox.Text = output;
            });
        }
    }

    private void DisplayThinking()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Thoughts.Count; i++)
            {
                sb.AppendLine(Thoughts[i]);
            }

            SetThinkBox(sb.ToString());
        }
        catch (Exception ex)
        {
            Logger.AddLog("Thinking.DisplayThinking", ex.Message, ex.StackTrace);
        }
    }

    private void Think()
    {
        try
        {
            if (!string.IsNullOrEmpty(SQLUtil.BrainFile) &&
                Options.CanThink)
            {
                string response = Brain.Think();
                if (!string.IsNullOrEmpty(response))
                {
                    Thoughts.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] AI: " + response);
                }

                if (Thoughts.Count > 200)
                {
                    Thoughts.RemoveRange(0, Thoughts.Count - 200);
                }

                DisplayThinking();
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Thinking.Thinking", ex.Message, ex.StackTrace);
        }
    }
}