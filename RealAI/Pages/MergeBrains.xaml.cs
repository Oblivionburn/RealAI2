using Microsoft.Data.Sqlite;
using RealAI.Util;
using System.Data;

namespace RealAI.Pages;

public partial class MergeBrains : ContentPage
{
    private CancellationTokenSource TokenSource;

    public List<string> FirstBrainList = new List<string>();
    private Label lb_FirstBrain;
    private Picker pk_FirstBrain;
    private int idx_FirstBrain = -1;

    public List<string> SecondBrainList = new List<string>();
    private Label lb_SecondBrain;
    private Picker pk_SecondBrain;
    private int idx_SecondBrain = -1;

    public Entry NewBrain;

    private ProgressBar pb_ProgressTime;
    private Label lb_ProgressTime;
    private Label lb_ProgressStep;
    private DateTime ProgressStart;

    private Button bt_Merge;
    private Button bt_Cancel;

    public MergeBrains()
	{
		InitializeComponent();
		InitControls();
    }

    private void InitControls()
	{
		try
		{
            lb_FirstBrain = new Label
            {
                FontSize = 20,
                BackgroundColor = BackgroundColor,
                Text = "First Brain:",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };

            pk_FirstBrain = new Picker
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };
            pk_FirstBrain.SelectedIndexChanged -= FirstBrain_SelectedIndexChanged;
            pk_FirstBrain.SelectedIndexChanged += FirstBrain_SelectedIndexChanged;

            lb_SecondBrain = new Label
            {
                FontSize = 20,
                BackgroundColor = BackgroundColor,
                Text = "Second Brain:",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };

            pk_SecondBrain = new Picker
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };
            pk_SecondBrain.SelectedIndexChanged -= SecondBrain_SelectedIndexChanged;
            pk_SecondBrain.SelectedIndexChanged += SecondBrain_SelectedIndexChanged;

            NewBrain = new Entry
            {
                FontSize = 20,
                Placeholder = "Type new brain name here",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            bt_Merge = new Button
            {
                Text = "Merge",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };
            bt_Merge.Clicked -= Bt_Merge_Clicked;
            bt_Merge.Clicked += Bt_Merge_Clicked;

            pb_ProgressTime = new ProgressBar
            {
                ProgressColor = Colors.DarkCyan,
                IsVisible = false
            };
            pb_ProgressTime.ProgressTo(1, 0, Easing.Linear);

            lb_ProgressTime = new Label
            {
                FontSize = 18,
                Text = AppUtil.GetTime_Milliseconds(DateTime.Now),
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsVisible = false
            };

            lb_ProgressStep = new Label
            {
                FontSize = 16,
                Text = "",
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsVisible = false
            };

            bt_Cancel = new Button
            {
                Text = "Cancel",
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                IsVisible = false
            };
            bt_Cancel.Clicked -= Bt_Cancel_Clicked;
            bt_Cancel.Clicked += Bt_Cancel_Clicked;

            LoadGrid();
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.InitControls", ex.Message, ex.StackTrace);
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

            //Empty space at top to push everything down
            int row = 0;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.8, GridUnitType.Star) });
            BoxView top_empty_box = new BoxView();
            top_empty_box.Color = BackgroundColor;
            grid.Add(top_empty_box, 0, row);
            Grid.SetColumnSpan(top_empty_box, 8);

            //First Brain Picker
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) });
            BoxView lb_picker_box = new BoxView();
            lb_picker_box.Color = BackgroundColor;
            grid.Add(lb_picker_box, 0, row);
            Grid.SetColumnSpan(lb_picker_box, 3);
            grid.Add(lb_FirstBrain, 0, row);
            Grid.SetColumnSpan(lb_FirstBrain, 3);

            BoxView picker_box = new BoxView();
            picker_box.Color = BackgroundColor;
            grid.Add(picker_box, 4, row);
            Grid.SetColumnSpan(picker_box, 4);
            grid.Add(pk_FirstBrain, 4, row);
            Grid.SetColumnSpan(pk_FirstBrain, 4);

            //Empty space between First Brain Picker and Second Brain Picker
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            BoxView first_empty_box = new BoxView();
            first_empty_box.Color = BackgroundColor;
            grid.Add(first_empty_box, 0, row);
            Grid.SetColumnSpan(first_empty_box, 8);

            //Second Brain Picker
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) });
            BoxView lb_picker_box2 = new BoxView();
            lb_picker_box2.Color = BackgroundColor;
            grid.Add(lb_picker_box2, 0, row);
            Grid.SetColumnSpan(lb_picker_box2, 3);
            grid.Add(lb_SecondBrain, 0, row);
            Grid.SetColumnSpan(lb_SecondBrain, 3);

            BoxView picker_box2 = new BoxView();
            picker_box2.Color = BackgroundColor;
            grid.Add(picker_box2, 4, row);
            Grid.SetColumnSpan(picker_box2, 4);
            grid.Add(pk_SecondBrain, 4, row);
            Grid.SetColumnSpan(pk_SecondBrain, 4);

            //Empty space between Second Brain Picker and NewBrain Entry
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView second_empty_box = new BoxView();
            second_empty_box.Color = BackgroundColor;
            grid.Add(second_empty_box, 0, row);
            Grid.SetColumnSpan(second_empty_box, 8);

            //New Brain Name
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Star) });
            BoxView newBrain_box = new BoxView();
            newBrain_box.Color = BackgroundColor;
            grid.Add(newBrain_box, 1, row);
            Grid.SetColumnSpan(newBrain_box, 6);
            grid.Add(NewBrain, 1, row);
            Grid.SetColumnSpan(NewBrain, 6);

            //Empty space between NewBrain Entry and Merge Button
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView third_empty_box = new BoxView();
            third_empty_box.Color = BackgroundColor;
            grid.Add(third_empty_box, 0, row);
            Grid.SetColumnSpan(third_empty_box, 8);

            //Merge Button
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) });
            BoxView bt_merge_box = new BoxView();
            bt_merge_box.Color = BackgroundColor;
            grid.Add(bt_merge_box, 2, row);
            Grid.SetColumnSpan(bt_merge_box, 4);
            grid.Add(bt_Merge, 2, row);
            Grid.SetColumnSpan(bt_Merge, 4);

            //Empty space between Merge Button and Progress Bar
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView fourth_empty_box = new BoxView();
            fourth_empty_box.Color = BackgroundColor;
            grid.Add(fourth_empty_box, 0, row);
            Grid.SetColumnSpan(fourth_empty_box, 8);

            //Progress Step
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) });
            BoxView lb_step_box = new BoxView();
            lb_step_box.Color = BackgroundColor;
            grid.Add(lb_step_box, 0, row);
            Grid.SetColumnSpan(lb_step_box, 8);
            grid.Add(lb_ProgressStep, 0, row);
            Grid.SetColumnSpan(lb_ProgressStep, 8);

            //Progress Bar
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView pb_progress_box = new BoxView();
            pb_progress_box.Color = BackgroundColor;
            grid.Add(pb_progress_box, 1, row);
            Grid.SetColumnSpan(pb_progress_box, 6);
            grid.Add(pb_ProgressTime, 1, row);
            Grid.SetColumnSpan(pb_ProgressTime, 6);

            //Progress Label
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) });
            BoxView lb_progress_box = new BoxView();
            lb_progress_box.Color = BackgroundColor;
            grid.Add(lb_progress_box, 1, row);
            Grid.SetColumnSpan(lb_progress_box, 6);
            grid.Add(lb_ProgressTime, 1, row);
            Grid.SetColumnSpan(lb_ProgressTime, 6);

            //Empty space between Progress Bar and Cancel Button
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView fifth_empty_box = new BoxView();
            fifth_empty_box.Color = BackgroundColor;
            grid.Add(fifth_empty_box, 0, row);
            Grid.SetColumnSpan(fifth_empty_box, 8);

            //Cancel Button
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.25, GridUnitType.Star) });
            BoxView bt_cancel_box = new BoxView();
            bt_cancel_box.Color = BackgroundColor;
            grid.Add(bt_cancel_box, 2, row);
            Grid.SetColumnSpan(bt_cancel_box, 4);
            grid.Add(bt_Cancel, 2, row);
            Grid.SetColumnSpan(bt_Cancel, 4);

            //Empty space underneath to push everything up
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.8, GridUnitType.Star) });
            BoxView bottom_empty_box = new BoxView();
            bottom_empty_box.Color = BackgroundColor;
            grid.Add(bottom_empty_box, 0, row);
            Grid.SetColumnSpan(bottom_empty_box, 8);

            Content = grid;
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.LoadGrid", ex.Message, ex.StackTrace);
        }
    }

    protected override void OnAppearing()
    {
        GetBrains();
    }

    private void GetBrains()
    {
        pk_FirstBrain.Title = null;
        pk_FirstBrain.SelectedIndex = -1;
        idx_FirstBrain = -1;

        pk_SecondBrain.Title = null;
        pk_SecondBrain.SelectedIndex = -1;
        idx_SecondBrain = -1;

        pk_FirstBrain.ItemsSource = null;
        FirstBrainList.Clear();

        pk_SecondBrain.ItemsSource = null;
        SecondBrainList.Clear();

        if (SQLUtil.BrainList.Count > 0)
        {
            for (int i = 0; i < SQLUtil.BrainList.Count; i++)
            {
                string brain = SQLUtil.BrainList[i];

                string file = AppUtil.GetBrainFile(brain + ".brain");
                if (File.Exists(file))
                {
                    FirstBrainList.Add(brain);
                    SecondBrainList.Add(brain);
                }
                else
                {
                    SQLUtil.BrainList.Remove(brain);
                    AppUtil.SaveBrainList();
                    i--;
                }
            }
        }

        pk_FirstBrain.ItemsSource = FirstBrainList;
        pk_SecondBrain.ItemsSource = SecondBrainList;
    }

    private void FirstBrain_SelectedIndexChanged(object sender, EventArgs e)
    {
        idx_FirstBrain = pk_FirstBrain.SelectedIndex;
    }

    private void SecondBrain_SelectedIndexChanged(object sender, EventArgs e)
    {
        idx_SecondBrain = pk_SecondBrain.SelectedIndex;
    }

    private void Bt_Cancel_Clicked(object sender, EventArgs e)
    {
        try
        {
            try
            {
                TokenSource.Cancel();
                TokenSource.Dispose();
            }
            catch (ObjectDisposedException)
            {
                //Ignore ReadingTokenSource already disposed
            }

            SQLUtil.DeleteBrain(NewBrain.Text);

            lb_FirstBrain.IsVisible = true;
            pk_FirstBrain.IsVisible = true;
            lb_SecondBrain.IsVisible = true;
            pk_SecondBrain.IsVisible = true;
            NewBrain.IsVisible = true;
            bt_Merge.IsVisible = true;

            pb_ProgressTime.IsVisible = false;
            lb_ProgressTime.IsVisible = false;
            lb_ProgressStep.IsVisible = false;
            bt_Cancel.IsVisible = false;
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.Bt_Cancel_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private async void Bt_Merge_Clicked(object sender, EventArgs e)
    {
        try
        {
            string newBrain = NewBrain.Text;
            if (string.IsNullOrEmpty(newBrain))
            {
                await DisplayAlert("Error", "New brain name is required.", "OK");
                return;
            }

            if (idx_FirstBrain == -1)
            {
                await DisplayAlert("Error", "A first brain is required to be selected.", "OK");
                return;
            }

            if (idx_SecondBrain == -1)
            {
                await DisplayAlert("Error", "A second brain is required to be selected.", "OK");
                return;
            }

            if (idx_FirstBrain == idx_SecondBrain)
            {
                await DisplayAlert("Error", "Second brain selected cannot match first brain selected.", "OK");
                return;
            }

            string firstBrain = (string)pk_FirstBrain.SelectedItem;
            string firstBrainFile = AppUtil.GetBrainFile(firstBrain + ".brain");
            if (!File.Exists(firstBrainFile))
            {
                await DisplayAlert("Error", "First brain selected does not exist.", "OK");
                return;
            }

            string secondBrain = (string)pk_SecondBrain.SelectedItem;
            string secondBrainFile = AppUtil.GetBrainFile(secondBrain + ".brain");
            if (!File.Exists(secondBrainFile))
            {
                await DisplayAlert("Error", "Second brain selected does not exist.", "OK");
                return;
            }

            string result = SQLUtil.NewBrain(newBrain);
            if (result != "SUCCESS")
            {
                await DisplayAlert("Error", result, "OK");
                return;
            }

            lb_FirstBrain.IsVisible = false;
            pk_FirstBrain.IsVisible = false;
            lb_SecondBrain.IsVisible = false;
            pk_SecondBrain.IsVisible = false;
            NewBrain.IsVisible = false;
            bt_Merge.IsVisible = false;

            pb_ProgressTime.IsVisible = true;
            lb_ProgressTime.IsVisible = true;
            lb_ProgressStep.IsVisible = true;
            bt_Cancel.IsVisible = true;

            ProgressStart = DateTime.Now;
            AppUtil.InitProgress(pb_ProgressTime, lb_ProgressTime);
            TokenSource = new CancellationTokenSource();
            Logger.Error = false;

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                lb_ProgressStep.Text = "Ready!";
            });

            await Task.Run(() => MergeInputs(firstBrain, secondBrain, newBrain, TokenSource.Token)).WaitAsync(TokenSource.Token);
            await Task.Run(() => MergeWords(firstBrain, secondBrain, newBrain, TokenSource.Token)).WaitAsync(TokenSource.Token);
            await Task.Run(() => MergeOutputs(firstBrain, secondBrain, newBrain, TokenSource.Token)).WaitAsync(TokenSource.Token);
            await Task.Run(() => MergeTopics(firstBrain, secondBrain, newBrain, TokenSource.Token)).WaitAsync(TokenSource.Token);
            await Task.Run(() => MergePreWords(firstBrain, secondBrain, newBrain, TokenSource.Token)).WaitAsync(TokenSource.Token);
            await Task.Run(() => MergeProWords(firstBrain, secondBrain, newBrain, TokenSource.Token)).WaitAsync(TokenSource.Token);

            if (!Logger.Error)
            {
                await MergingDone(firstBrain, secondBrain, newBrain);
                GetBrains();
            }
            else
            {
                Bt_Cancel_Clicked(null, null);
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.Bt_Merge_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private async Task MergeInputs(string firstBrain, string secondBrain, string newBrain, CancellationToken token)
    {
        try
        {
            if (!token.IsCancellationRequested &&
                !Logger.Error)
            {
                AppUtil.UpdateProgress(pb_ProgressTime, 0, lb_ProgressTime, ProgressStart);

                List<Input> firstInputs = await SQLUtil.Get_Inputs(firstBrain);
                if (Logger.Error)
                {
                    return;
                }

                List<Input> secondInputs = await SQLUtil.Get_Inputs(secondBrain);
                if (Logger.Error)
                {
                    return;
                }

                if (firstInputs.Any() &&
                    secondInputs.Any())
                {
                    foreach (Input first in firstInputs)
                    {
                        foreach (Input second in secondInputs)
                        {
                            if (second.input == first.input &&
                                second.priority > first.priority)
                            {
                                first.priority = second.priority;
                                break;
                            }
                        }
                    }

                    foreach (Input second in secondInputs)
                    {
                        bool found = false;
                        foreach (Input first in firstInputs)
                        {
                            if (second.input == first.input)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            firstInputs.Add(second);
                        }
                    }

                    double count = 1;
                    double total = firstInputs.Count;

                    for (int i = 0; i < total; i++)
                    {
                        if (token.IsCancellationRequested ||
                            Logger.Error)
                        {
                            break;
                        }

                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            lb_ProgressStep.Text = "Merging Inputs (" + count + "/" + total + ")";
                        });

                        SqliteCommand cmd = new SqliteCommand("INSERT INTO Inputs (Input, Priority) VALUES (@input, @priority)");

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "@input",
                            Value = firstInputs[i].input,
                            DbType = DbType.String
                        });

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "@priority",
                            Value = firstInputs[i].priority,
                            DbType = DbType.Int32
                        });

                        SQLUtil.Execute(cmd, newBrain);

                        count++;

                        AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                        Thread.Sleep(10);
                    }
                }
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.MergeInputs", ex.Message, ex.StackTrace);
        }
    }

    private async Task MergeWords(string firstBrain, string secondBrain, string newBrain, CancellationToken token)
    {
        try
        {
            if (!token.IsCancellationRequested &&
                !Logger.Error)
            {
                AppUtil.UpdateProgress(pb_ProgressTime, 0, lb_ProgressTime, ProgressStart);

                List<Word> firstWords = await SQLUtil.Get_Words(firstBrain);
                if (Logger.Error)
                {
                    return;
                }

                List<Word> secondWords = await SQLUtil.Get_Words(secondBrain);
                if (Logger.Error)
                {
                    return;
                }

                if (secondWords.Any())
                {
                    foreach (Word first in firstWords)
                    {
                        foreach (Word second in secondWords)
                        {
                            if (second.word == first.word &&
                                second.priority > first.priority)
                            {
                                first.priority = second.priority;
                                break;
                            }
                        }
                    }

                    foreach (Word second in secondWords)
                    {
                        bool found = false;
                        foreach (Word first in firstWords)
                        {
                            if (second.word == first.word)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            firstWords.Add(second);
                        }
                    }
                }

                double count = 1;
                double total = firstWords.Count;

                for (int i = 0; i < total; i++)
                {
                    if (token.IsCancellationRequested ||
                        Logger.Error)
                    {
                        break;
                    }

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        lb_ProgressStep.Text = "Merging Words (" + count + "/" + total + ")";
                    });

                    SqliteCommand cmd = new SqliteCommand("INSERT INTO Words (Word, Priority) VALUES (@word, @priority)");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = firstWords[i].word,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@priority",
                        Value = firstWords[i].priority,
                        DbType = DbType.Int32
                    });

                    SQLUtil.Execute(cmd, newBrain);

                    count++;

                    AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                    Thread.Sleep(10);
                }
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.MergeWords", ex.Message, ex.StackTrace);
        }
    }

    private async Task MergeOutputs(string firstBrain, string secondBrain, string newBrain, CancellationToken token)
    {
        try
        {
            if (!token.IsCancellationRequested &&
                !Logger.Error)
            {
                AppUtil.UpdateProgress(pb_ProgressTime, 0, lb_ProgressTime, ProgressStart);

                List<Output> firstOutputs = await SQLUtil.Get_Outputs(firstBrain);
                if (Logger.Error)
                {
                    return;
                }

                List<Output> secondOutputs = await SQLUtil.Get_Outputs(secondBrain);
                if (Logger.Error)
                {
                    return;
                }

                if (secondOutputs.Any())
                {
                    foreach (Output first in firstOutputs)
                    {
                        foreach (Output second in secondOutputs)
                        {
                            if (second.input == first.input &&
                                second.output == first.output &&
                                second.priority > first.priority)
                            {
                                first.priority = second.priority;
                                break;
                            }
                        }
                    }

                    foreach (Output second in secondOutputs)
                    {
                        bool found = false;
                        foreach (Output first in firstOutputs)
                        {
                            if (second.input == first.input &&
                                second.output == first.output)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            firstOutputs.Add(second);
                        }
                    }
                }

                double count = 1;
                double total = firstOutputs.Count;

                for (int i = 0; i < total; i++)
                {
                    if (token.IsCancellationRequested ||
                        Logger.Error)
                    {
                        break;
                    }

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        lb_ProgressStep.Text = "Merging Outputs (" + count + "/" + total + ")";
                    });

                    SqliteCommand cmd = new SqliteCommand("INSERT INTO Outputs (Input, Output, Priority) VALUES (@input, @output, @priority)");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@input",
                        Value = firstOutputs[i].input,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@output",
                        Value = firstOutputs[i].output,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@priority",
                        Value = firstOutputs[i].priority,
                        DbType = DbType.Int32
                    });

                    SQLUtil.Execute(cmd, newBrain);

                    count++;

                    AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                    Thread.Sleep(10);
                }
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.MergeOutputs", ex.Message, ex.StackTrace);
        }
    }

    private async Task MergeTopics(string firstBrain, string secondBrain, string newBrain, CancellationToken token)
    {
        try
        {
            if (!token.IsCancellationRequested &&
                !Logger.Error)
            {
                AppUtil.UpdateProgress(pb_ProgressTime, 0, lb_ProgressTime, ProgressStart);

                List<Topic> firstTopics = await SQLUtil.Get_Topics(firstBrain);
                if (Logger.Error)
                {
                    return;
                }

                List<Topic> secondTopics = await SQLUtil.Get_Topics(secondBrain);
                if (Logger.Error)
                {
                    return;
                }

                if (secondTopics.Any())
                {
                    foreach (Topic first in firstTopics)
                    {
                        foreach (Topic second in secondTopics)
                        {
                            if (second.input == first.input &&
                                second.topic == first.topic &&
                                second.priority > first.priority)
                            {
                                first.priority = second.priority;
                                break;
                            }
                        }
                    }

                    foreach (Topic second in secondTopics)
                    {
                        bool found = false;
                        foreach (Topic first in firstTopics)
                        {
                            if (second.input == first.input &&
                                second.topic == first.topic)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            firstTopics.Add(second);
                        }
                    }
                }

                double count = 1;
                double total = firstTopics.Count;

                for (int i = 0; i < total; i++)
                {
                    if (token.IsCancellationRequested ||
                        Logger.Error)
                    {
                        break;
                    }

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        lb_ProgressStep.Text = "Merging Topics (" + count + "/" + total + ")";
                    });

                    SqliteCommand cmd = new SqliteCommand("INSERT INTO Topics (Input, Topic, Priority) VALUES (@input, @topic, @priority)");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@input",
                        Value = firstTopics[i].input,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@topic",
                        Value = firstTopics[i].topic,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@priority",
                        Value = firstTopics[i].priority,
                        DbType = DbType.Int32
                    });

                    SQLUtil.Execute(cmd, newBrain);

                    count++;

                    AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                    Thread.Sleep(10);
                }
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.MergeTopics", ex.Message, ex.StackTrace);
        }
    }

    private async Task MergePreWords(string firstBrain, string secondBrain, string newBrain, CancellationToken token)
    {
        try
        {
            if (!token.IsCancellationRequested &&
                !Logger.Error)
            {
                AppUtil.UpdateProgress(pb_ProgressTime, 0, lb_ProgressTime, ProgressStart);

                List<PreWord> firstPreWords = await SQLUtil.Get_PreWords(firstBrain);
                if (Logger.Error)
                {
                    return;
                }

                List<PreWord> secondPreWords = await SQLUtil.Get_PreWords(secondBrain);
                if (Logger.Error)
                {
                    return;
                }

                if (secondPreWords.Any())
                {
                    foreach (PreWord first in firstPreWords)
                    {
                        foreach (PreWord second in secondPreWords)
                        {
                            if (second.word == first.word &&
                                second.preWord == first.preWord &&
                                second.distance == first.distance &&
                                second.priority > first.priority)
                            {
                                first.priority = second.priority;
                                break;
                            }
                        }
                    }

                    foreach (PreWord second in secondPreWords)
                    {
                        bool found = false;
                        foreach (PreWord first in firstPreWords)
                        {
                            if (second.word == first.word &&
                                second.preWord == first.preWord &&
                                second.distance == first.distance)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            firstPreWords.Add(second);
                        }
                    }
                }

                double count = 1;
                double total = firstPreWords.Count;

                for (int i = 0; i < total; i++)
                {
                    if (token.IsCancellationRequested ||
                        Logger.Error)
                    {
                        break;
                    }

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        lb_ProgressStep.Text = "Merging PreWords (" + count + "/" + total + ")";
                    });

                    SqliteCommand cmd = new SqliteCommand("INSERT INTO PreWords (Word, PreWord, Priority, Distance) VALUES (@word, @preWord, @priority, @distance)");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = firstPreWords[i].word,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@preWord",
                        Value = firstPreWords[i].preWord,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@priority",
                        Value = firstPreWords[i].priority,
                        DbType = DbType.Int32
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@distance",
                        Value = firstPreWords[i].distance,
                        DbType = DbType.Int32
                    });

                    SQLUtil.Execute(cmd, newBrain);

                    count++;

                    AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                    Thread.Sleep(10);
                }
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.MergePreWords", ex.Message, ex.StackTrace);
        }
    }

    private async Task MergeProWords(string firstBrain, string secondBrain, string newBrain, CancellationToken token)
    {
        try
        {
            if (!token.IsCancellationRequested &&
                !Logger.Error)
            {
                AppUtil.UpdateProgress(pb_ProgressTime, 0, lb_ProgressTime, ProgressStart);

                List<ProWord> firstProWords = await SQLUtil.Get_ProWords(firstBrain);
                if (Logger.Error)
                {
                    return;
                }

                List<ProWord> secondProWords = await SQLUtil.Get_ProWords(secondBrain);
                if (Logger.Error)
                {
                    return;
                }

                if (secondProWords.Any())
                {
                    foreach (ProWord first in firstProWords)
                    {
                        foreach (ProWord second in secondProWords)
                        {
                            if (second.word == first.word &&
                                second.proWord == first.proWord &&
                                second.distance == first.distance &&
                                second.priority > first.priority)
                            {
                                first.priority = second.priority;
                                break;
                            }
                        }
                    }

                    foreach (ProWord second in secondProWords)
                    {
                        bool found = false;
                        foreach (ProWord first in firstProWords)
                        {
                            if (second.word == first.word &&
                                second.proWord == first.proWord &&
                                second.distance == first.distance)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            firstProWords.Add(second);
                        }
                    }
                }

                double count = 1;
                double total = firstProWords.Count;

                for (int i = 0; i < total; i++)
                {
                    if (token.IsCancellationRequested ||
                        Logger.Error)
                    {
                        break;
                    }

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        lb_ProgressStep.Text = "Merging ProWords (" + count + "/" + total + ")";
                    });

                    SqliteCommand cmd = new SqliteCommand("INSERT INTO ProWords (Word, ProWord, Priority, Distance) VALUES (@word, @proWord, @priority, @distance)");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = firstProWords[i].word,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@proWord",
                        Value = firstProWords[i].proWord,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@priority",
                        Value = firstProWords[i].priority,
                        DbType = DbType.Int32
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@distance",
                        Value = firstProWords[i].distance,
                        DbType = DbType.Int32
                    });

                    SQLUtil.Execute(cmd, newBrain);

                    count++;

                    AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                    Thread.Sleep(10);
                }
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.MergeProWords", ex.Message, ex.StackTrace);
        }
    }

    private async Task MergingDone(string firstBrain, string secondBrain, string newBrain)
    {
        try
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                lb_ProgressStep.Text = "Done!";
            });

            AppUtil.FinishProgress(pb_ProgressTime, lb_ProgressTime, ProgressStart);

            try
            {
                TokenSource.Cancel();
                TokenSource.Dispose();
            }
            catch (ObjectDisposedException)
            {
                //Ignore TokenSource already disposed
            }

            await DisplayAlert("Merge Brains", "'" + firstBrain + "' and '" + secondBrain + "' brains have been merged into new '" + newBrain + "' brain.\n\nTotal Processing Time: " + AppUtil.GetTime_Milliseconds(ProgressStart), "OK");

            NewBrain.Text = "";

            lb_FirstBrain.IsVisible = true;
            pk_FirstBrain.IsVisible = true;
            lb_SecondBrain.IsVisible = true;
            pk_SecondBrain.IsVisible = true;
            NewBrain.IsVisible = true;
            bt_Merge.IsVisible = true;

            pb_ProgressTime.IsVisible = false;
            lb_ProgressTime.IsVisible = false;
            lb_ProgressStep.IsVisible = false;
            bt_Cancel.IsVisible = false;
        }
        catch (Exception ex)
        {
            Logger.AddLog("MergeBrains.MergingDone", ex.Message, ex.StackTrace);
        }
    }
}