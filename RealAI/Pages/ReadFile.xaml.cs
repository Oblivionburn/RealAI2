using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using RealAI.Util;

namespace RealAI.Pages;

public partial class ReadFile : ContentPage
{
    private Label lb_ReadFile;
    private Button bt_Browse;
    private FileResult FilePickerResult;
    private Button bt_ReadFile;

    private ProgressBar pb_ProgressTime;
    private Label lb_ProgressTime;
    private Label lb_ProgressStep;
    private DateTime ProgressStart;
    private Button bt_Cancel;

    private CancellationTokenSource ReadingTokenSource;

    private List<string> words;
    private List<string> inputs;
    private List<SqliteCommand> commands;
    private StringBuilder stringBuilder;

    public ReadFile()
	{
		InitializeComponent();
        InitControls();
    }

    private void InitControls()
    {
        try
        {
            lb_ReadFile = new Label
            {
                FontSize = 16,
                Text = "(Press Browse to pick a txt file)",
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            lb_ReadFile.IsVisible = true;

            pb_ProgressTime = new ProgressBar
            {
                ProgressColor = Colors.DarkCyan,
            };
            pb_ProgressTime.ProgressTo(1, 0, Easing.Linear);
            pb_ProgressTime.IsVisible = false;

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

            bt_Browse = new Button();
            bt_Browse.Text = "Browse";
            bt_Browse.HorizontalOptions = LayoutOptions.Fill;
            bt_Browse.VerticalOptions = LayoutOptions.Fill;
            bt_Browse.IsVisible = true;
            bt_Browse.Clicked -= Bt_Browse_Clicked;
            bt_Browse.Clicked += Bt_Browse_Clicked;

            bt_ReadFile = new Button();
            bt_ReadFile.Text = "Read File";
            bt_ReadFile.HorizontalOptions = LayoutOptions.Fill;
            bt_ReadFile.VerticalOptions = LayoutOptions.Fill;
            bt_ReadFile.IsVisible = true;
            bt_ReadFile.Clicked -= Bt_ReadFile_Clicked;
            bt_ReadFile.Clicked += Bt_ReadFile_Clicked;

            bt_Cancel = new Button();
            bt_Cancel.Text = "Cancel";
            bt_Cancel.HorizontalOptions = LayoutOptions.Fill;
            bt_Cancel.VerticalOptions = LayoutOptions.Fill;
            bt_Cancel.IsVisible = false;
            bt_Cancel.Clicked -= Bt_Cancel_Clicked;
            bt_Cancel.Clicked += Bt_Cancel_Clicked;

            LoadGrid();
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.InitControls", ex.Message, ex.StackTrace);
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
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.6, GridUnitType.Star) });
            BoxView top_empty_box = new BoxView();
            top_empty_box.Color = BackgroundColor;
            grid.Add(top_empty_box, 0, row);
            Grid.SetColumnSpan(top_empty_box, 8);

            //Progress Step
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
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
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView lb_progress_box = new BoxView();
            lb_progress_box.Color = BackgroundColor;
            grid.Add(lb_progress_box, 1, row);
            Grid.SetColumnSpan(lb_progress_box, 6);
            grid.Add(lb_ProgressTime, 1, row);
            Grid.SetColumnSpan(lb_ProgressTime, 6);

            //File Picker Button
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView bt_picker_box = new BoxView();
            bt_picker_box.Color = BackgroundColor;
            grid.Add(bt_picker_box, 2, row);
            Grid.SetColumnSpan(bt_picker_box, 4);
            grid.Add(bt_Browse, 2, row);
            Grid.SetColumnSpan(bt_Browse, 4);

            //Cancel Button
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView bt_cancel_box = new BoxView();
            bt_cancel_box.Color = BackgroundColor;
            grid.Add(bt_cancel_box, 2, row);
            Grid.SetColumnSpan(bt_cancel_box, 4);
            grid.Add(bt_Cancel, 2, row);
            Grid.SetColumnSpan(bt_Cancel, 4);

            //File Picker Label
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.8, GridUnitType.Star) });
            BoxView lb_picker_box = new BoxView();
            lb_picker_box.Color = BackgroundColor;
            grid.Add(lb_picker_box, 1, row);
            Grid.SetColumnSpan(lb_picker_box, 6);
            grid.Add(lb_ReadFile, 1, row);
            Grid.SetColumnSpan(lb_ReadFile, 6);

            //Read File Button
            row++;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            BoxView bt_read_box = new BoxView();
            bt_read_box.Color = BackgroundColor;
            grid.Add(bt_read_box, 2, row);
            Grid.SetColumnSpan(bt_read_box, 4);
            grid.Add(bt_ReadFile, 2, row);
            Grid.SetColumnSpan(bt_ReadFile, 4);

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
            Logger.AddLog("ReadFile.LoadGrid", ex.Message, ex.StackTrace);
        }
    }

    private async void Bt_Browse_Clicked(object sender, EventArgs e)
    {
        try
        {
            Dictionary<DevicePlatform, IEnumerable<string>> mimeTypes = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "text/plain" } }
            };

            PickOptions options = new PickOptions()
            {
                PickerTitle = "Which txt file are you wanting to read?",
                FileTypes = new FilePickerFileType(mimeTypes),
            };

            FilePickerResult = await FilePicker.Default.PickAsync(options);
            if (FilePickerResult != null)
            {
                lb_ReadFile.Text = FilePickerResult.FullPath;
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.Bt_Browse_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private void Bt_Cancel_Clicked(object sender, EventArgs e)
    {
        try
        {
            try
            {
                ReadingTokenSource.Cancel();
                ReadingTokenSource.Dispose();
            }
            catch (ObjectDisposedException)
            {
                //Ignore ReadingTokenSource already disposed
            }

            lb_ReadFile.IsVisible = true;
            bt_Browse.IsVisible = true;
            bt_ReadFile.IsVisible = true;

            lb_ProgressTime.IsVisible = false;
            lb_ProgressStep.IsVisible = false;
            pb_ProgressTime.IsVisible = false;
            bt_Cancel.IsVisible = false;
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.Bt_Cancel_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private async void Bt_ReadFile_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists(lb_ReadFile.Text))
            {
                lb_ReadFile.IsVisible = false;
                bt_Browse.IsVisible = false;
                bt_ReadFile.IsVisible = false;

                lb_ProgressTime.IsVisible = true;
                lb_ProgressStep.IsVisible = true;
                pb_ProgressTime.IsVisible = true;
                bt_Cancel.IsVisible = true;

                ProgressStart = DateTime.Now;
                AppUtil.InitProgress(pb_ProgressTime, lb_ProgressTime);

                ReadingTokenSource = new CancellationTokenSource();
                
                words = new List<string>();
                inputs = new List<string>();
                commands = new List<SqliteCommand>();
                stringBuilder = new StringBuilder();

                await Task.Run(() => ProcessFile_ReadLines(ReadingTokenSource.Token)).WaitAsync(ReadingTokenSource.Token);
                await Task.Run(() => ProcessFile_CompileWords(ReadingTokenSource.Token)).WaitAsync(ReadingTokenSource.Token);
                await Task.Run(() => ProcessFile_IdentifySentences(ReadingTokenSource.Token)).WaitAsync(ReadingTokenSource.Token);
                await Task.Run(() => ProcessFile_PrepData(ReadingTokenSource.Token)).WaitAsync(ReadingTokenSource.Token);
                await Task.Run(() => ProcessFile_SaveData(ReadingTokenSource.Token)).WaitAsync(ReadingTokenSource.Token);
                await ReadingDone();
            }
            else
            {
                await DisplayAlert("Read File", "\"" + lb_ReadFile.Text + "\" file not found.", "OK");
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.Bt_ReadFile_Clicked", ex.Message, ex.StackTrace);
        }
    }

    private async Task ProcessFile_ReadLines(CancellationToken token)
    {
        try
        {
            double count = 0;
            double total = 0;

            string[] lines = File.ReadAllLines(lb_ReadFile.Text);
            total = lines.Length;

            for (int l = 0; l < total; l++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    lb_ProgressStep.Text = "Reading lines from file...(" + count + "/" + total + ")";
                });

                string line = lines[l];
                int line_length = line.Length;

                string new_line = line.Trim();
                int new_line_length = new_line.Length;

                if (new_line_length > 0)
                {
                    bool replaced_semicolon = false;

                    for (var i = 0; i < line_length; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        string value = line[i].ToString();
                        if (Brain.NormalCharacters.IsMatch(value) &&
                            value != " ")
                        {
                            if (replaced_semicolon)
                            {
                                replaced_semicolon = false;
                                stringBuilder.Append(value.ToUpper());
                            }
                            else
                            {
                                stringBuilder.Append(value);
                            }
                        }
                        else if (value == "." ||
                                 value == "!" ||
                                 value == "?" ||
                                 value == "," ||
                                 value == "'" ||
                                 value == "’" ||
                                 value == ":" ||
                                 value == "-" ||
                                 value == ";" ||
                                 value == " ")
                        {
                            if (value == ";")
                            {
                                replaced_semicolon = true;
                                value = ".";
                            }

                            stringBuilder.Append(value);
                        }
                    }

                    stringBuilder.Append(" ");
                }

                count++;

                AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                Thread.Sleep(10);
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.ProcessFile_ReadLines", ex.Message, ex.StackTrace);
        }
    }

    private async Task ProcessFile_CompileWords(CancellationToken token)
    {
        try
        {
            double count = 0;
            double total = 0;

            if (!token.IsCancellationRequested &&
                !string.IsNullOrEmpty(stringBuilder.ToString()) &&
                stringBuilder.ToString().Length > 2)
            {
                AppUtil.UpdateProgress(pb_ProgressTime, 0, lb_ProgressTime, ProgressStart);

                string[] gap_array = Brain.GapSpecials(stringBuilder.ToString()).Split(' ');

                count = 0;
                total = gap_array.Length;

                for (int i = 0; i < total; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        lb_ProgressStep.Text = "Compiling words...(" + count + "/" + total + ")";
                    });

                    string word = gap_array[i];

                    if (!string.IsNullOrEmpty(word))
                    {
                        words.Add(word);
                    }

                    count++;

                    AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                    Thread.Sleep(10);
                }
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.ProcessFile_CompileWords", ex.Message, ex.StackTrace);
        }
    }

    private async Task ProcessFile_IdentifySentences(CancellationToken token)
    {
        try
        {
            double count = 0;
            double total = 0;

            if (!token.IsCancellationRequested &&
                words.Count > 0)
            {
                AppUtil.UpdateProgress(pb_ProgressTime, 0, lb_ProgressTime, ProgressStart);

                string[] words_array = words.ToArray();

                count = 0;
                total = words_array.Length;

                StringBuilder input = new StringBuilder();
                for (int i = 0; i < total; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        lb_ProgressStep.Text = "Identifying sentences...(" + count + "/" + total + ")";
                    });

                    string word = words_array[i];

                    input.Append(word);
                    input.Append(" ");

                    if (word == "?" ||
                        word == "!")
                    {
                        inputs.Add(input.ToString().Trim());
                        input = new StringBuilder();
                    }
                    else if (word == ".")
                    {
                        if (i < words_array.Length - 1 &&
                            i > 0)
                        {
                            string previous_word = words_array[i - 1];
                            string next_word = words_array[i + 1];
                            if (next_word != "." &&
                                previous_word != ".")
                            {
                                inputs.Add(input.ToString().Trim());
                                input = new StringBuilder();
                            }
                        }
                    }

                    count++;

                    AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                    Thread.Sleep(10);
                }

                words.Clear();
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.ProcessFile_IdentifySentences", ex.Message, ex.StackTrace);
        }
    }

    private async Task ProcessFile_PrepData(CancellationToken token)
    {
        try
        {
            double count = 0;
            double total = 0;

            if (!token.IsCancellationRequested &&
                inputs.Count > 0)
            {
                AppUtil.UpdateProgress(pb_ProgressTime, 0, lb_ProgressTime, ProgressStart);

                count = 0;
                total = inputs.Count;

                for (int i = 0; i < total; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        lb_ProgressStep.Text = "Prepping data...(" + count + "/" + total + ")";
                    });

                    string existing = inputs[i];

                    string[] WordArray = existing.Split(' ');
                    if (WordArray.Length > 0)
                    {
                        commands.AddRange(Brain.AddInputs(Brain.RulesCheck(existing)));
                        commands.AddRange(Brain.AddWords(WordArray));
                        commands.AddRange(Brain.AddPreWords(WordArray));
                        commands.AddRange(Brain.AddProWords(WordArray));
                    }

                    count++;

                    AppUtil.UpdateProgress(pb_ProgressTime, count / total, lb_ProgressTime, ProgressStart);
                    Thread.Sleep(10);
                }

                inputs.Clear();
            }

            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.ProcessFile_PrepData", ex.Message, ex.StackTrace);
        }
    }

    private async Task ProcessFile_SaveData(CancellationToken token)
    {
        try
        {
            if (!token.IsCancellationRequested)
            {
                SQLUtil.BulkExecute(commands, pb_ProgressTime, lb_ProgressTime, lb_ProgressStep, ProgressStart, token);
            }

            commands.Clear();
            
            await Task.Delay(1000, token);
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.ProcessFile_SaveData", ex.Message, ex.StackTrace);
        }
    }

    private async Task ReadingDone()
    {
        try
        {
            AppUtil.FinishProgress(pb_ProgressTime, lb_ProgressTime, ProgressStart);

            try
            {
                ReadingTokenSource.Cancel();
                ReadingTokenSource.Dispose();
            }
            catch (ObjectDisposedException)
            {
                //Ignore ReadingTokenSource already disposed
            }

            await DisplayAlert("Read File", "\"" + lb_ReadFile.Text + "\" has been read.\nTotal Processing Time: " + AppUtil.GetTime_Milliseconds(ProgressStart), "OK");

            lb_ReadFile.IsVisible = true;
            bt_Browse.IsVisible = true;
            bt_ReadFile.IsVisible = true;

            lb_ProgressTime.IsVisible = false;
            lb_ProgressStep.IsVisible = false;
            pb_ProgressTime.IsVisible = false;
            bt_Cancel.IsVisible = false;
        }
        catch (Exception ex)
        {
            Logger.AddLog("ReadFile.ReadingDone", ex.Message, ex.StackTrace);
        }
    }
}