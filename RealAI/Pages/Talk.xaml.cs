using System.Text;
using RealAI.Util;

namespace RealAI.Pages;

public partial class Talk : ContentPage
{
    public static Editor txt_Output;
    public static Entry txt_Input;
    public static Button btn_Enter;
    public static Button btn_Encourage;
    public static Button btn_Discourage;
    public static Button btn_NewSession;
    public static ProgressBar pb_ResponseTime;
    public static Label lb_ResponseTime;

    public static DateTime ResponseStart;
    public static System.Timers.Timer AttentionTimer;

    public Talk()
	{
		InitializeComponent();
        InitControls();
    }

    private void InitControls()
    {
        try
        {
            btn_NewSession = new Button();
            btn_NewSession.Text = "New Session";
            btn_NewSession.HorizontalOptions = LayoutOptions.Fill;
            btn_NewSession.VerticalOptions = LayoutOptions.Fill;
            btn_NewSession.Clicked += OnNewSessionClicked;

            txt_Output = new Editor();
            txt_Output.IsSpellCheckEnabled = false;
            txt_Output.IsReadOnly = true;
            txt_Output.FontSize = 18;
            txt_Output.HorizontalOptions = LayoutOptions.Fill;
            txt_Output.VerticalOptions = LayoutOptions.Fill;

            txt_Input = new Entry();
            txt_Input.FontSize = 20;
            txt_Input.Placeholder = "Type input here";
            txt_Input.HorizontalTextAlignment = TextAlignment.Start;
            txt_Input.VerticalTextAlignment = TextAlignment.Center;
            txt_Input.HorizontalOptions = LayoutOptions.Fill;
            txt_Input.VerticalOptions = LayoutOptions.Fill;
            txt_Input.Completed += OnInputCompleted;

            btn_Enter = new Button();
            btn_Enter.Text = "Enter";
            btn_Enter.HorizontalOptions = LayoutOptions.Fill;
            btn_Enter.VerticalOptions = LayoutOptions.Fill;
            btn_Enter.Clicked += OnEnterClicked;

            btn_Encourage = new Button();
            btn_Encourage.Text = "Encourage";
            btn_Encourage.HorizontalOptions = LayoutOptions.Fill;
            btn_Encourage.VerticalOptions = LayoutOptions.Fill;
            btn_Encourage.Clicked += OnEncourageClicked;

            btn_Discourage = new Button();
            btn_Discourage.Text = "Discourage";
            btn_Discourage.HorizontalOptions = LayoutOptions.Fill;
            btn_Discourage.VerticalOptions = LayoutOptions.Fill;
            btn_Discourage.Clicked += OnDiscourageClicked;

            pb_ResponseTime = new ProgressBar();
            pb_ResponseTime.ProgressColor = Colors.DarkCyan;

            lb_ResponseTime = new Label();
            lb_ResponseTime.FontSize = 18;
            lb_ResponseTime.TextColor = Color.FromArgb("#FFFFFF");
            lb_ResponseTime.HorizontalTextAlignment = TextAlignment.Center;
            lb_ResponseTime.VerticalTextAlignment = TextAlignment.Center;
            lb_ResponseTime.HorizontalOptions = LayoutOptions.Center;
            lb_ResponseTime.VerticalOptions = LayoutOptions.Center;

            AppUtil.InitProgress(pb_ResponseTime, lb_ResponseTime);

            LoadGrid();

            AttentionTimer = new System.Timers.Timer();
            AttentionTimer.Interval = Options.AttentionSpan * 1000;
            AttentionTimer.Elapsed += AttentionTimer_Elapsed;
            AttentionTimer.Start();
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.InitControls", ex.Message, ex.StackTrace);
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
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            //New Session
            int row = 0;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) });
            BoxView new_box = new BoxView();
            grid.Add(new_box, 0, row);
            Grid.SetColumnSpan(new_box, 8);

            grid.Add(btn_NewSession, 0, row);
            Grid.SetColumnSpan(btn_NewSession, 8);

            //ProgressBar
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView progress_box = new BoxView();
            grid.Add(progress_box, 0, row);
            Grid.SetColumnSpan(progress_box, 8);

            grid.Add(pb_ResponseTime, 0, row);
            Grid.SetColumnSpan(pb_ResponseTime, 8);

            grid.Add(lb_ResponseTime, 0, row);
            Grid.SetColumnSpan(lb_ResponseTime, 8);

            //Output
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1.5, GridUnitType.Star) });
            BoxView output_box = new BoxView();
            grid.Add(output_box, 0, row);
            Grid.SetColumnSpan(output_box, 8);

            grid.Add(txt_Output, 0, row);
            Grid.SetColumnSpan(txt_Output, 8);

            //Empty space between Output and Input/Enter
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            BoxView empty_box = new BoxView();
            grid.Add(empty_box, 0, row);
            Grid.SetColumnSpan(empty_box, 8);

            //Input and Enter
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Star) });
            BoxView input_box = new BoxView();
            grid.Add(input_box, 0, row);
            Grid.SetColumnSpan(input_box, 6);

            grid.Add(txt_Input, 0, row);
            Grid.SetColumnSpan(txt_Input, 6);

            BoxView enter_box = new BoxView();
            grid.Add(enter_box, 6, row);
            Grid.SetColumnSpan(enter_box, 2);

            grid.Add(btn_Enter, 6, row);
            Grid.SetColumnSpan(btn_Enter, 2);

            //Empty space between Input/Enter and Encourage/Discourage
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            BoxView empty_box2 = new BoxView();
            grid.Add(empty_box2, 0, row);
            Grid.SetColumnSpan(empty_box2, 8);

            //Encourage and Discourage
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) });
            BoxView encourage_box = new BoxView();
            grid.Add(encourage_box, 0, row);
            Grid.SetColumnSpan(encourage_box, 3);

            grid.Add(btn_Encourage, 0, row);
            Grid.SetColumnSpan(btn_Encourage, 3);

            BoxView discourage_box = new BoxView();
            grid.Add(discourage_box, 5, row);
            Grid.SetColumnSpan(discourage_box, 3);

            grid.Add(btn_Discourage, 5, row);
            Grid.SetColumnSpan(btn_Discourage, 3);

            //Empty space underneath to push everything up
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
            BoxView empty_box4 = new BoxView();
            grid.Add(empty_box4, 0, row);
            Grid.SetColumnSpan(empty_box4, 8);

            Content = grid;

            StartNewSession();
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.LoadGrid", ex.Message, ex.StackTrace);
        }
    }

    protected override void OnAppearing()
    {
        DisplayHistory();
    }

    public static void SetOutput(string output)
    {
        try
        {
            if (MainThread.IsMainThread)
            {
                txt_Output.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(output));
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    txt_Output.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(output));
                });
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.SetOutput", ex.Message, ex.StackTrace);
        }
    }

    public static void Clear()
    {
        try
        {
            txt_Output.Text = "";
            txt_Input.Text = "";

            Brain.LastResponse = "";
            Brain.LastThought = "";
            Brain.CleanInput = "";
            Brain.Topics = null;
            Brain.WordArray = null;
            Brain.WordArray_Thinking = null;
            Thinking.Thoughts.Clear();

            if (Thinking.ThinkBox != null)
            {
                Thinking.ThinkBox.Text = "";
            }

            DisplayHistory();
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.Clear", ex.Message, ex.StackTrace);
        }
    }

    public static void DisplayTips()
    {
        try
        {
            List<string> tips = new List<string>
                {
                    "Here are some tips for teaching the AI:",
                    "",
                    "1. The AI learns from observing how you respond to what it says... " +
                        "so, if it says \"Hello.\" and you say \"How are you?\" it will learn that \"How are you?\" " +
                        "is a possible response to \"Hello.\". If you say something it has never seen before, it will " +
                        "repeat it to see how -you- would respond to it. Learning by imitation, like a young child, " +
                        "is not the only way it learns as you will soon discover.",
                    "",
                    "2. It will generate stuff that sounds nonsensical early on... this is part of the learning process, " +
                        "similar to the way children phrase things in ways that don't quite make sense early on.",
                    "",
                    "3. Limit your response to a single sentence or question.",
                    "",
                    "4. Use complete sentences when responding. Start with a capital letter and end with a punctuation mark.",
                    "",
                    "5. Avoid contractions (use \"it is\" instead of \"it's\").",
                    "",
                    "6. The AI cannot see/hear/taste/smell/feel any things you refer to, so it can never have any contextual " +
                        "understanding of what exactly the thing is (the way you understand it). This means it'll " +
                        "never understand you trying to reference it (or yourself) directly, as it can never have a concept of " +
                        "anything external being something different from it without spatial recognition gained from sight/touch/sound.",
                    "",
                    "7. In general... keep it simple. The simpler you speak to it, the better it learns.",
                    "",
                    "8. For help, check Discord: https://discord.gg/3yJ8rce",
                    "",
                    "To get started, create a new brain or load an existing one from the Brains menu."
                };

            StringBuilder sb = new StringBuilder();
            foreach (string line in tips)
            {
                sb.AppendLine(line);
            }

            SetOutput(sb.ToString());
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.DisplayTips", ex.Message, ex.StackTrace);
        }
    }

    public static void DisplayHistory()
    {
        try
        {
            StringBuilder sb = new StringBuilder();

            List<string> lines = AppUtil.GetHistory();
            if (lines.Count > 0)
            {
                foreach (string line in lines)
                {
                    sb.AppendLine(line);
                }

                SetOutput(sb.ToString());
            }
            else
            {
                DisplayTips();
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.DisplayHistory", ex.Message, ex.StackTrace);
        }
    }

    public void AddToHistory(string message)
    {
        try
        {
            if (!string.IsNullOrEmpty(message))
            {
                List<string> history = AppUtil.GetHistory();
                history.Add(message);
                AppUtil.SaveHistory(history);

                DisplayHistory();
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.AddToHistory", ex.Message, ex.StackTrace);
        }
    }

    public bool StartNewSession()
    {
        try
        {
            bool reset = false;

            List<string> History = AppUtil.GetHistory();
            if (History.Count > 0)
            {
                if (!History[History.Count - 1].Contains("[new session]"))
                {
                    reset = true;
                }
            }

            if (reset)
            {
                AddToHistory("[" + DateTime.Now.ToString("HH:mm:ss") + "] [new session]");
                Clear();

                return true;
            }
            else
            {
                DisplayHistory();
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.StartNewSession", ex.Message, ex.StackTrace);
        }

        return false;
    }

    private async void Respond()
    {
        try
        {
            if (!Brain.GenerateAnotherResponse)
            {
                AddToHistory("[" + DateTime.Now.ToString("HH:mm:ss") + "] You: " + Brain.Input);
            }

            string response = Brain.Respond(false);
            if (!string.IsNullOrEmpty(response))
            {
                AddToHistory("[" + DateTime.Now.ToString("HH:mm:ss") + "] AI: " + response);
            }

            AppUtil.FinishProgress(pb_ResponseTime, lb_ResponseTime, ResponseStart);

            if (Options.TTS)
            {
                await TextToSpeech.Default.SpeakAsync(response);
            }

            ResetInput();
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.Respond", ex.Message, ex.StackTrace);
        }
    }

    private async void Input()
    {
        try
        {
            if (!string.IsNullOrEmpty(SQLUtil.BrainFile))
            {
                if (!string.IsNullOrEmpty(txt_Input.Text))
                {
                    Brain.Input = txt_Input.Text;
                    Brain.GenerateAnotherResponse = false;
                }
                else
                {
                    Brain.GenerateAnotherResponse = true;
                }

                ResponseStart = DateTime.Now;

                Thread thread = new Thread(Respond);
                thread.Start();
            }
            else
            {
                await DisplayAlert("Error", "No brain connected. Go to Brains in menu to create or load one.", "OK");
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.Input", ex.Message, ex.StackTrace);
        }
    }

    private async void Encourage()
    {
        try
        {
            if (!string.IsNullOrEmpty(SQLUtil.BrainFile))
            {
                bool encourage = false;
                string message = "";

                List<string> History = AppUtil.GetHistory();
                if (History.Count > 0)
                {
                    if (!History[History.Count - 1].Contains("[encouraged]"))
                    {
                        encourage = true;
                    }

                    if (encourage)
                    {
                        if (!string.IsNullOrEmpty(Brain.LastResponse) &&
                            History[History.Count - 1].Contains(Brain.LastResponse))
                        {
                            message = Brain.LastResponse;
                        }
                        else if (!string.IsNullOrEmpty(Brain.LastThought) &&
                                 History[History.Count - 1].Contains(Brain.LastThought))
                        {
                            message = Brain.LastThought;
                        }
                    }
                }

                if (encourage &&
                    !string.IsNullOrEmpty(message))
                {
                    if (Brain.Encourage(message))
                    {
                        AddToHistory("[" + DateTime.Now.ToString("HH:mm:ss") + "] [encouraged]");
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "No brain connected.", "OK");
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.Encourage", ex.Message, ex.StackTrace);
        }
    }

    private async void Discourage()
    {
        try
        {
            if (!string.IsNullOrEmpty(SQLUtil.BrainFile))
            {
                bool discourage = false;
                string message = "";

                List<string> History = AppUtil.GetHistory();
                if (History.Count > 0)
                {
                    if (!History[History.Count - 1].Contains("[discouraged]"))
                    {
                        discourage = true;
                    }

                    if (discourage)
                    {
                        if (!string.IsNullOrEmpty(Brain.LastResponse) &&
                            History[History.Count - 1].Contains(Brain.LastResponse))
                        {
                            message = Brain.LastResponse;
                        }
                        else if (!string.IsNullOrEmpty(Brain.LastThought) &&
                                 History[History.Count - 1].Contains(Brain.LastThought))
                        {
                            message = Brain.LastThought;
                        }
                    }
                }

                if (discourage &&
                    !string.IsNullOrEmpty(message))
                {
                    if (Brain.Discourage(message))
                    {
                        AddToHistory("[" + DateTime.Now.ToString("HH:mm:ss") + "] [discouraged]");
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "No brain connected.", "OK");
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.Discourage", ex.Message, ex.StackTrace);
        }
    }

    private void OnNewSessionClicked(object sender, EventArgs e)
    {
        StartNewSession();
    }

    private void OnEnterClicked(object sender, EventArgs e)
    {
        Input();
    }

    private void OnInputCompleted(object sender, EventArgs e)
    {
        Input();
    }

    private void OnEncourageClicked(object sender, EventArgs e)
    {
        Encourage();
    }

    private void OnDiscourageClicked(object sender, EventArgs e)
    {
        Discourage();
    }

    private void ResetInput()
    {
        try
        {
            if (MainThread.IsMainThread)
            {
                txt_Input.Text = "";
                txt_Input.Focus();
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    txt_Input.Text = "";
                    txt_Input.Focus();
                });
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.ResetInput", ex.Message, ex.StackTrace);
        }
    }

    private void AttentionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(SQLUtil.BrainFile) &&
                Options.Initiate)
            {
                string message;

                if (Options.CanThink &&
                    !string.IsNullOrEmpty(Brain.LastThought))
                {
                    message = Brain.LastThought;
                }
                else
                {
                    message = Brain.Respond(true);
                }

                if (!string.IsNullOrEmpty(message))
                {
                    AddToHistory("[" + DateTime.Now.ToString("HH:mm:ss") + "] AI: " + message);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("Talk.AttentionTimer_Elapsed", ex.Message, ex.StackTrace);
        }
    }
}