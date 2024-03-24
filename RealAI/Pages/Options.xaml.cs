using Microsoft.Maui.Controls.Shapes;
using RealAI.Util;

namespace RealAI.Pages;

public partial class Options : ContentPage
{
    public static Label lb_ThinkOptions;
    public static Line ln_ThinkOptions;

	public static int ThinkSpeed;
    public static Label lb_ThinkSpeed;
    public static Slider sl_ThinkSpeed;

    public static bool CanThink = true;
    public static Label lb_CanThink;
    public static Switch sw_CanThink;

    public static bool CanLearnFromThinking;
    public static Label lb_CanLearnFromThinking;
    public static Switch sw_CanLearnFromThinking;

    public static Label lb_ConversationOptions;
    public static Line ln_ConversationOptions;

    public static int AttentionSpan;
    public static Label lb_AttentionSpan;
    public static Slider sl_AttentionSpan;

    public static bool Initiate;
    public static Label lb_Initiate;
    public static Switch sw_Initiate;

    public static bool TTS;
    public static Label lb_TTS;
    public static Switch sw_TTS;

    public static Label lb_RespondingOptions;
    public static Line ln_RespondingOptions;

    public static bool TopicResponding = true;
    public static Label lb_Topic;
    public static Switch sw_Topic;

    public static bool WholeResponding = true;
    public static Label lb_Whole;
    public static Switch sw_Whole;

    public static bool ProceduralResponding = true;
    public static Label lb_Procedural;
    public static Switch sw_Procedural;

    public Options()
	{
		InitializeComponent();
        InitControls();
    }

    private async void InitControls()
    {
        try
        {
            BackgroundColor = Color.FromRgb(32, 32, 32);

            lb_ThinkOptions = new Label();
            lb_ThinkOptions.FontSize = 24;
            lb_ThinkOptions.TextColor = Color.FromRgb(255, 255, 255);
            lb_ThinkOptions.BackgroundColor = BackgroundColor;
            lb_ThinkOptions.Text = "Thinking Options";
            lb_ThinkOptions.HorizontalOptions = LayoutOptions.Fill;
            lb_ThinkOptions.VerticalOptions = LayoutOptions.Start;

            ln_ThinkOptions = new Line();
            ln_ThinkOptions.HorizontalOptions = LayoutOptions.Fill;
            ln_ThinkOptions.VerticalOptions = LayoutOptions.Center;
            ln_ThinkOptions.BackgroundColor = Color.FromRgb(255, 255, 255);

            lb_ThinkSpeed = new Label();
            lb_ThinkSpeed.FontSize = 20;
            lb_ThinkSpeed.BackgroundColor = BackgroundColor;
            lb_ThinkSpeed.Text = "Think Delay (milliseconds): " + ThinkSpeed;
            lb_ThinkSpeed.HorizontalOptions = LayoutOptions.Fill;
            lb_ThinkSpeed.VerticalOptions = LayoutOptions.Start;

            sl_ThinkSpeed = new Slider();
            sl_ThinkSpeed.Minimum = 100;
            sl_ThinkSpeed.Maximum = 1000;
            sl_ThinkSpeed.Value = ThinkSpeed;
            sl_ThinkSpeed.BackgroundColor = BackgroundColor;
            sl_ThinkSpeed.MinimumTrackColor = Color.FromRgb(0, 200, 255);
            sl_ThinkSpeed.MaximumTrackColor = Color.FromRgb(64, 64, 64);
            sl_ThinkSpeed.ThumbColor = Color.FromRgb(255, 255, 255);
            sl_ThinkSpeed.HorizontalOptions = LayoutOptions.Fill;
            sl_ThinkSpeed.VerticalOptions = LayoutOptions.Center;
            sl_ThinkSpeed.ValueChanged += Sl_ThinkSpeed_ValueChanged;

            lb_CanThink = new Label();
            lb_CanThink.FontSize = 20;
            lb_CanThink.Text = "Can Think:";
            lb_CanThink.BackgroundColor = BackgroundColor;
            lb_CanThink.HorizontalOptions = LayoutOptions.Start;
            lb_CanThink.VerticalOptions = LayoutOptions.Center;

            sw_CanThink = new Switch();
            sw_CanThink.IsToggled = CanThink;
            sw_CanThink.BackgroundColor = BackgroundColor;
            sw_CanThink.HorizontalOptions = LayoutOptions.Start;
            sw_CanThink.VerticalOptions = LayoutOptions.Center;
            sw_CanThink.Toggled += Sw_CanThink_Toggled;

            lb_CanLearnFromThinking = new Label();
            lb_CanLearnFromThinking.FontSize = 20;
            lb_CanLearnFromThinking.BackgroundColor = BackgroundColor;
            lb_CanLearnFromThinking.Text = "Learn From Thinking:";
            lb_CanLearnFromThinking.HorizontalOptions = LayoutOptions.Start;
            lb_CanLearnFromThinking.VerticalOptions = LayoutOptions.Center;

            sw_CanLearnFromThinking = new Switch();
            sw_CanLearnFromThinking.IsToggled = CanLearnFromThinking;
            sw_CanLearnFromThinking.BackgroundColor = BackgroundColor;
            sw_CanLearnFromThinking.HorizontalOptions = LayoutOptions.Start;
            sw_CanLearnFromThinking.VerticalOptions = LayoutOptions.Center;
            sw_CanLearnFromThinking.Toggled += Sw_CanLearnFromThinking_Toggled;

            lb_ConversationOptions = new Label();
            lb_ConversationOptions.FontSize = 24;
            lb_ConversationOptions.TextColor = Color.FromRgb(255, 255, 255);
            lb_ConversationOptions.BackgroundColor = BackgroundColor;
            lb_ConversationOptions.Text = "Conversation Options";
            lb_ConversationOptions.HorizontalOptions = LayoutOptions.Fill;
            lb_ConversationOptions.VerticalOptions = LayoutOptions.Start;

            ln_ConversationOptions = new Line();
            ln_ConversationOptions.HorizontalOptions = LayoutOptions.Fill;
            ln_ConversationOptions.VerticalOptions = LayoutOptions.Center;
            ln_ConversationOptions.BackgroundColor = Color.FromRgb(255, 255, 255);

            lb_AttentionSpan = new Label();
            lb_AttentionSpan.FontSize = 20;
            lb_AttentionSpan.BackgroundColor = BackgroundColor;
            lb_AttentionSpan.Text = "Conversation Delay (seconds): " + AttentionSpan;
            lb_AttentionSpan.HorizontalOptions = LayoutOptions.Fill;
            lb_AttentionSpan.VerticalOptions = LayoutOptions.Start;

            sl_AttentionSpan = new Slider();
            sl_AttentionSpan.Minimum = 1;
            sl_AttentionSpan.Maximum = 60;
            sl_AttentionSpan.Value = AttentionSpan;
            sl_AttentionSpan.BackgroundColor = BackgroundColor;
            sl_AttentionSpan.MinimumTrackColor = Color.FromRgb(0, 200, 255);
            sl_AttentionSpan.MaximumTrackColor = Color.FromRgb(64, 64, 64);
            sl_AttentionSpan.ThumbColor = Color.FromRgb(255, 255, 255);
            sl_AttentionSpan.HorizontalOptions = LayoutOptions.Fill;
            sl_AttentionSpan.VerticalOptions = LayoutOptions.Center;
            sl_AttentionSpan.ValueChanged += Sl_AttentionSpan_ValueChanged;

            lb_Initiate = new Label();
            lb_Initiate.FontSize = 20;
            lb_Initiate.Text = "Initiate Conversation:";
            lb_Initiate.BackgroundColor = BackgroundColor;
            lb_Initiate.HorizontalOptions = LayoutOptions.Start;
            lb_Initiate.VerticalOptions = LayoutOptions.Center;

            sw_Initiate = new Switch();
            sw_Initiate.IsToggled = Initiate;
            sw_Initiate.BackgroundColor = BackgroundColor;
            sw_Initiate.HorizontalOptions = LayoutOptions.Start;
            sw_Initiate.VerticalOptions = LayoutOptions.Center;
            sw_Initiate.Toggled += Sw_Initiate_Toggled;

            lb_TTS = new Label();
            lb_TTS.FontSize = 20;
            lb_TTS.Text = "Text-To-Speech:";
            lb_TTS.BackgroundColor = BackgroundColor;
            lb_TTS.HorizontalOptions = LayoutOptions.Start;
            lb_TTS.VerticalOptions = LayoutOptions.Center;

            sw_TTS = new Switch();
            sw_TTS.IsToggled = TTS;
            sw_TTS.BackgroundColor = BackgroundColor;
            sw_TTS.HorizontalOptions = LayoutOptions.Start;
            sw_TTS.VerticalOptions = LayoutOptions.Center;
            sw_TTS.Toggled += Sw_TTS_Toggled;

            lb_RespondingOptions = new Label();
            lb_RespondingOptions.FontSize = 24;
            lb_RespondingOptions.TextColor = Color.FromRgb(255, 255, 255);
            lb_RespondingOptions.BackgroundColor = BackgroundColor;
            lb_RespondingOptions.Text = "Responding Options";
            lb_RespondingOptions.HorizontalOptions = LayoutOptions.Fill;
            lb_RespondingOptions.VerticalOptions = LayoutOptions.Start;

            ln_RespondingOptions = new Line();
            ln_RespondingOptions.HorizontalOptions = LayoutOptions.Fill;
            ln_RespondingOptions.VerticalOptions = LayoutOptions.Center;
            ln_RespondingOptions.BackgroundColor = Color.FromRgb(255, 255, 255);

            lb_Topic = new Label();
            lb_Topic.FontSize = 20;
            lb_Topic.Text = "Topic-based Responding:";
            lb_Topic.BackgroundColor = BackgroundColor;
            lb_Topic.HorizontalOptions = LayoutOptions.Start;
            lb_Topic.VerticalOptions = LayoutOptions.Center;

            sw_Topic = new Switch();
            sw_Topic.IsToggled = TopicResponding;
            sw_Topic.BackgroundColor = BackgroundColor;
            sw_Topic.HorizontalOptions = LayoutOptions.Start;
            sw_Topic.VerticalOptions = LayoutOptions.Center;
            sw_Topic.Toggled += Sw_Topic_Toggled;

            lb_Whole = new Label();
            lb_Whole.FontSize = 20;
            lb_Whole.Text = "Whole Input Responding:";
            lb_Whole.BackgroundColor = BackgroundColor;
            lb_Whole.HorizontalOptions = LayoutOptions.Start;
            lb_Whole.VerticalOptions = LayoutOptions.Center;

            sw_Whole = new Switch();
            sw_Whole.IsToggled = WholeResponding;
            sw_Whole.BackgroundColor = BackgroundColor;
            sw_Whole.HorizontalOptions = LayoutOptions.Start;
            sw_Whole.VerticalOptions = LayoutOptions.Center;
            sw_Whole.Toggled += Sw_Whole_Toggled;

            lb_Procedural = new Label();
            lb_Procedural.FontSize = 20;
            lb_Procedural.Text = "Procedural Responding:";
            lb_Procedural.BackgroundColor = BackgroundColor;
            lb_Procedural.HorizontalOptions = LayoutOptions.Start;
            lb_Procedural.VerticalOptions = LayoutOptions.Center;

            sw_Procedural = new Switch();
            sw_Procedural.IsToggled = ProceduralResponding;
            sw_Procedural.BackgroundColor = BackgroundColor;
            sw_Procedural.HorizontalOptions = LayoutOptions.Start;
            sw_Procedural.VerticalOptions = LayoutOptions.Center;
            sw_Procedural.Toggled += Sw_Procedural_Toggled;

            LoadGrid();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.InitControls", ex.Message, ex.StackTrace);
        }
    }

    private async void LoadGrid()
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

            //Think Options Header
            int row = 0;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_thinkOptions_box = new BoxView();
            lb_thinkOptions_box.Color = BackgroundColor;
            grid.Add(lb_thinkOptions_box, 0, row);
            Grid.SetColumnSpan(lb_thinkOptions_box, 8);

            grid.Add(lb_ThinkOptions, 0, row);
            Grid.SetColumnSpan(lb_ThinkOptions, 8);

            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.01, GridUnitType.Star) });
            BoxView ln_thinkOptions_box = new BoxView();
            ln_thinkOptions_box.Color = BackgroundColor;
            grid.Add(ln_thinkOptions_box, 0, row);
            Grid.SetColumnSpan(ln_thinkOptions_box, 8);

            grid.Add(ln_ThinkOptions, 0, row);
            Grid.SetColumnSpan(ln_ThinkOptions, 8);

            //Think Speed
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_thinkSpeed_box = new BoxView();
            lb_thinkSpeed_box.Color = BackgroundColor;
            grid.Add(lb_thinkSpeed_box, 0, row);
            Grid.SetColumnSpan(lb_thinkSpeed_box, 8);

            grid.Add(lb_ThinkSpeed, 0, row);
            Grid.SetColumnSpan(lb_ThinkSpeed, 8);

            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });
            BoxView sl_thinkSpeed_box = new BoxView();
            sl_thinkSpeed_box.Color = BackgroundColor;
            grid.Add(sl_thinkSpeed_box, 0, row);
            Grid.SetColumnSpan(sl_thinkSpeed_box, 8);

            grid.Add(sl_ThinkSpeed, 0, row);
            Grid.SetColumnSpan(sl_ThinkSpeed, 8);

            //Can Think
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_canThink_box = new BoxView();
            lb_canThink_box.Color = BackgroundColor;
            grid.Add(lb_canThink_box, 0, row);
            Grid.SetColumnSpan(lb_canThink_box, 7);

            grid.Add(lb_CanThink, 0, row);
            Grid.SetColumnSpan(lb_CanThink, 7);

            BoxView sw_canThink_box = new BoxView();
            sw_canThink_box.Color = BackgroundColor;
            grid.Add(sw_canThink_box, 6, row);
            grid.Add(sw_CanThink, 6, row);

            //Can Learn From Thinking
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_canLearn_box = new BoxView();
            lb_canLearn_box.Color = BackgroundColor;
            grid.Add(lb_canLearn_box, 0, row);
            Grid.SetColumnSpan(lb_canLearn_box, 7);

            grid.Add(lb_CanLearnFromThinking, 0, row);
            Grid.SetColumnSpan(lb_CanLearnFromThinking, 7);

            BoxView sw_canLearn_box = new BoxView();
            sw_canLearn_box.Color = BackgroundColor;
            grid.Add(sw_canLearn_box, 6, row);
            grid.Add(sw_CanLearnFromThinking, 6, row);

            //Empty space between Thinking and Attention options
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView first_empty_box = new BoxView();
            first_empty_box.Color = BackgroundColor;
            grid.Add(first_empty_box, 0, row);
            Grid.SetColumnSpan(first_empty_box, 8);

            //Conversation Options Header
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_conversationOptions_box = new BoxView();
            lb_conversationOptions_box.Color = BackgroundColor;
            grid.Add(lb_conversationOptions_box, 0, row);
            Grid.SetColumnSpan(lb_conversationOptions_box, 8);

            grid.Add(lb_ConversationOptions, 0, row);
            Grid.SetColumnSpan(lb_ConversationOptions, 8);

            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.01, GridUnitType.Star) });
            BoxView ln_conversationOptions_box = new BoxView();
            ln_conversationOptions_box.Color = BackgroundColor;
            grid.Add(ln_conversationOptions_box, 0, row);
            Grid.SetColumnSpan(ln_conversationOptions_box, 8);

            grid.Add(ln_ConversationOptions, 0, row);
            Grid.SetColumnSpan(ln_ConversationOptions, 8);

            //Attention Span
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_attentionSpan_box = new BoxView();
            lb_attentionSpan_box.Color = BackgroundColor;
            grid.Add(lb_attentionSpan_box, 0, row);
            Grid.SetColumnSpan(lb_attentionSpan_box, 8);

            grid.Add(lb_AttentionSpan, 0, row);
            Grid.SetColumnSpan(lb_AttentionSpan, 8);

            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });
            BoxView sl_attentionSpan_box = new BoxView();
            sl_attentionSpan_box.Color = BackgroundColor;
            grid.Add(sl_attentionSpan_box, 0, row);
            Grid.SetColumnSpan(sl_attentionSpan_box, 8);

            grid.Add(sl_AttentionSpan, 0, row);
            Grid.SetColumnSpan(sl_AttentionSpan, 8);

            //Initiate
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_initiate_box = new BoxView();
            lb_initiate_box.Color = BackgroundColor;
            grid.Add(lb_initiate_box, 0, row);
            Grid.SetColumnSpan(lb_initiate_box, 7);

            grid.Add(lb_Initiate, 0, row);
            Grid.SetColumnSpan(lb_Initiate, 7);

            BoxView sw_initiate_box = new BoxView();
            sw_initiate_box.Color = BackgroundColor;
            grid.Add(sw_initiate_box, 6, row);
            grid.Add(sw_Initiate, 6, row);

            //Empty space between Initiate and TTS
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView second_empty_box = new BoxView();
            second_empty_box.Color = BackgroundColor;
            grid.Add(second_empty_box, 0, row);
            Grid.SetColumnSpan(second_empty_box, 8);

            //TTS
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_tts_box = new BoxView();
            lb_tts_box.Color = BackgroundColor;
            grid.Add(lb_tts_box, 0, row);
            Grid.SetColumnSpan(lb_tts_box, 7);

            grid.Add(lb_TTS, 0, row);
            Grid.SetColumnSpan(lb_TTS, 7);

            BoxView sw_tts_box = new BoxView();
            sw_tts_box.Color = BackgroundColor;
            grid.Add(sw_tts_box, 6, row);
            grid.Add(sw_TTS, 6, row);

            //Empty space between TTS and Responding options
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView third_empty_box = new BoxView();
            third_empty_box.Color = BackgroundColor;
            grid.Add(third_empty_box, 0, row);
            Grid.SetColumnSpan(third_empty_box, 8);

            //Responding Options Header
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_respondingOptions_box = new BoxView();
            lb_respondingOptions_box.Color = BackgroundColor;
            grid.Add(lb_respondingOptions_box, 0, row);
            Grid.SetColumnSpan(lb_respondingOptions_box, 8);

            grid.Add(lb_RespondingOptions, 0, row);
            Grid.SetColumnSpan(lb_RespondingOptions, 8);

            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.01, GridUnitType.Star) });
            BoxView ln_respondingOptions_box = new BoxView();
            ln_respondingOptions_box.Color = BackgroundColor;
            grid.Add(ln_respondingOptions_box, 0, row);
            Grid.SetColumnSpan(ln_respondingOptions_box, 8);

            grid.Add(ln_RespondingOptions, 0, row);
            Grid.SetColumnSpan(ln_RespondingOptions, 8);

            //Topic Responding
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_topic_box = new BoxView();
            lb_topic_box.Color = BackgroundColor;
            grid.Add(lb_topic_box, 0, row);
            Grid.SetColumnSpan(lb_topic_box, 7);

            grid.Add(lb_Topic, 0, row);
            Grid.SetColumnSpan(lb_Topic, 7);

            BoxView sw_topic_box = new BoxView();
            sw_topic_box.Color = BackgroundColor;
            grid.Add(sw_topic_box, 6, row);
            grid.Add(sw_Topic, 6, row);

            //Whole Responding
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_whole_box = new BoxView();
            lb_whole_box.Color = BackgroundColor;
            grid.Add(lb_whole_box, 0, row);
            Grid.SetColumnSpan(lb_whole_box, 7);

            grid.Add(lb_Whole, 0, row);
            Grid.SetColumnSpan(lb_Whole, 7);

            BoxView sw_whole_box = new BoxView();
            sw_whole_box.Color = BackgroundColor;
            grid.Add(sw_whole_box, 6, row);
            grid.Add(sw_Whole, 6, row);

            //Procedural Responding
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_procedural_box = new BoxView();
            lb_procedural_box.Color = BackgroundColor;
            grid.Add(lb_procedural_box, 0, row);
            Grid.SetColumnSpan(lb_procedural_box, 7);

            grid.Add(lb_Procedural, 0, row);
            Grid.SetColumnSpan(lb_Procedural, 7);

            BoxView sw_procedural_box = new BoxView();
            sw_procedural_box.Color = BackgroundColor;
            grid.Add(sw_procedural_box, 6, row);
            grid.Add(sw_Procedural, 6, row);

            //Empty space underneath to push everything up
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            BoxView bottom_empty_box = new BoxView();
            bottom_empty_box.Color = BackgroundColor;
            grid.Add(bottom_empty_box, 0, row);
            Grid.SetColumnSpan(bottom_empty_box, 8);

            Content = grid;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.LoadGrid", ex.Message, ex.StackTrace);
        }
    }

    private async void Sw_CanThink_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            CanThink = sw_CanThink.IsToggled;
            AppUtil.SetConfig("CanThink", CanThink.ToString());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.Sw_CanThink_Toggled", ex.Message, ex.StackTrace);
        }
    }

    private async void Sw_CanLearnFromThinking_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            CanLearnFromThinking = sw_CanLearnFromThinking.IsToggled;
            AppUtil.SetConfig("CanLearnFromThinking", CanLearnFromThinking.ToString());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.Sw_CanLearnFromThinking_Toggled", ex.Message, ex.StackTrace);
        }
    }

    private async void Sl_ThinkSpeed_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        try
        {
            ThinkSpeed = (int)sl_ThinkSpeed.Value;
            AppUtil.SetConfig("ThinkSpeed", ThinkSpeed.ToString());

            lb_ThinkSpeed.Text = "Think Delay (milliseconds): " + ThinkSpeed;

            if (Thinking.ThinkTimer != null)
            {
                Thinking.ThinkTimer.Interval = ThinkSpeed;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.Sl_ThinkSpeed_ValueChanged", ex.Message, ex.StackTrace);
        }
    }

    private async void Sl_AttentionSpan_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        try
        {
            AttentionSpan = (int)sl_AttentionSpan.Value;
            AppUtil.SetConfig("AttentionSpan", AttentionSpan.ToString());

            lb_AttentionSpan.Text = "Conversation Delay (seconds): " + AttentionSpan;

            if (Talk.AttentionTimer != null)
            {
                Talk.AttentionTimer.Interval = AttentionSpan * 1000;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.Sl_AttentionSpan_ValueChanged", ex.Message, ex.StackTrace);
        }
    }

    private async void Sw_Initiate_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            Initiate = sw_Initiate.IsToggled;
            AppUtil.SetConfig("Initiate", Initiate.ToString());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.Sw_Initiate_Toggled", ex.Message, ex.StackTrace);
        }
    }

    private async void Sw_TTS_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            TTS = sw_TTS.IsToggled;
            AppUtil.SetConfig("TTS", TTS.ToString());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.Sw_TTS_Toggled", ex.Message, ex.StackTrace);
        }
    }

    private async void Sw_Topic_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            TopicResponding = sw_Topic.IsToggled;
            AppUtil.SetConfig("TopicResponding", TopicResponding.ToString());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.Sw_Topic_Toggled", ex.Message, ex.StackTrace);
        }
    }

    private async void Sw_Whole_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            WholeResponding = sw_Whole.IsToggled;
            AppUtil.SetConfig("WholeResponding", WholeResponding.ToString());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.Sw_Whole_Toggled", ex.Message, ex.StackTrace);
        }
    }

    private async void Sw_Procedural_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            ProceduralResponding = sw_Procedural.IsToggled;
            AppUtil.SetConfig("ProceduralResponding", ProceduralResponding.ToString());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("Options.Sw_Procedural_Toggled", ex.Message, ex.StackTrace);
        }
    }
}