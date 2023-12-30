using System.Data;
using Microsoft.Data.Sqlite;
using RealAI.Util;

namespace RealAI.Pages;

public partial class FixSpelling : ContentPage
{
    int index = -1;
    public Label lb_Picker;
    public Picker WordPicker;
    public Entry NewWord;
    public Button Update;

    public List<string> WordList = new List<string>();

    public FixSpelling()
	{
		InitializeComponent();
        InitControls();
    }

    private void InitControls()
    {
        lb_Picker = new Label();
        lb_Picker.FontSize = 20;
        lb_Picker.BackgroundColor = BackgroundColor;
        lb_Picker.Text = "Old Word:";
        lb_Picker.HorizontalTextAlignment = TextAlignment.Center;
        lb_Picker.VerticalTextAlignment = TextAlignment.Center;
        lb_Picker.HorizontalOptions = LayoutOptions.Fill;
        lb_Picker.VerticalOptions = LayoutOptions.Center;

        WordPicker = new Picker();
        WordPicker.FontSize = 20;
        WordPicker.HorizontalTextAlignment = TextAlignment.Start;
        WordPicker.VerticalTextAlignment = TextAlignment.Center;
        WordPicker.HorizontalOptions = LayoutOptions.Fill;
        WordPicker.VerticalOptions = LayoutOptions.Center;
        WordPicker.SelectedIndexChanged += WordPicker_SelectedIndexChanged;

        NewWord = new Entry();
        NewWord.FontSize = 20;
        NewWord.Placeholder = "Type new word here";
        NewWord.HorizontalTextAlignment = TextAlignment.Center;
        NewWord.VerticalTextAlignment = TextAlignment.Center;
        NewWord.HorizontalOptions = LayoutOptions.Fill;
        NewWord.VerticalOptions = LayoutOptions.Fill;

        Update = new Button();
        Update.Text = "Update";
        Update.HorizontalOptions = LayoutOptions.Fill;
        Update.VerticalOptions = LayoutOptions.Fill;
        Update.Clicked += OnUpdateClicked;

        LoadGrid();
    }

    private void LoadGrid()
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

        //Word Picker
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
        BoxView lb_picker_box = new BoxView();
        lb_picker_box.Color = BackgroundColor;
        grid.Add(lb_picker_box, 0, row);
        Grid.SetColumnSpan(lb_picker_box, 2);
        grid.Add(lb_Picker, 0, row);
        Grid.SetColumnSpan(lb_Picker, 2);

        BoxView picker_box = new BoxView();
        picker_box.Color = BackgroundColor;
        grid.Add(picker_box, 2, row);
        Grid.SetColumnSpan(picker_box, 6);
        grid.Add(WordPicker, 2, row);
        Grid.SetColumnSpan(WordPicker, 6);

        //Empty space between Word Picker and New Word
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
        BoxView first_empty_box = new BoxView();
        first_empty_box.Color = BackgroundColor;
        grid.Add(first_empty_box, 0, row);
        Grid.SetColumnSpan(first_empty_box, 8);

        //New Word
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
        BoxView newWord_box = new BoxView();
        newWord_box.Color = BackgroundColor;
        grid.Add(newWord_box, 1, row);
        Grid.SetColumnSpan(newWord_box, 6);
        grid.Add(NewWord, 1, row);
        Grid.SetColumnSpan(NewWord, 6);

        //Empty space between New Word and Update button
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
        BoxView second_empty_box = new BoxView();
        second_empty_box.Color = BackgroundColor;
        grid.Add(second_empty_box, 0, row);
        Grid.SetColumnSpan(second_empty_box, 8);

        //Update
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
        BoxView update_box = new BoxView();
        update_box.Color = BackgroundColor;
        grid.Add(update_box, 2, row);
        Grid.SetColumnSpan(update_box, 4);
        grid.Add(Update, 2, row);
        Grid.SetColumnSpan(Update, 4);

        //Empty space underneath to push everything up
        row++;
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        BoxView bottom_empty_box = new BoxView();
        bottom_empty_box.Color = BackgroundColor;
        grid.Add(bottom_empty_box, 0, row);
        Grid.SetColumnSpan(bottom_empty_box, 8);

        Content = grid;
    }

    protected override void OnAppearing()
    {
        GetWords();
    }

    private void GetWords()
    {
        WordPicker.Title = null;
        WordPicker.SelectedIndex = -1;
        index = -1;

        NewWord.Text = "";

        WordPicker.ItemsSource = null;
        WordList.Clear();

        if (!string.IsNullOrEmpty(SQLUtil.BrainFile))
        {
            List<Word> words = SQLUtil.Get_Words(SQLUtil.BrainFile).ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (Word word in words)
            {
                WordList.Add(word.word);
            }
        }

        WordPicker.ItemsSource = WordList;
    }

    private void WordPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        index = WordPicker.SelectedIndex;
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        try
        {
            string old_word = (string)WordPicker.SelectedItem;
            string new_word = NewWord.Text.Trim();

            if (!string.IsNullOrEmpty(SQLUtil.BrainFile) &&
                !string.IsNullOrEmpty(old_word) &&
                !string.IsNullOrEmpty(new_word))
            {
                bool exists = false;
                for (int i = 0; i < WordList.Count; i++)
                {
                    if (new_word == WordList[i].ToString() &&
                        i != index)
                    {
                        exists = true;
                        break;
                    }
                }

                List<SqliteCommand> commands = new List<SqliteCommand>();

                SqliteParameter old_word_parm = new SqliteParameter("@old_word", old_word);
                old_word_parm.DbType = DbType.String;

                SqliteParameter new_word_parm = new SqliteParameter("@new_word", new_word);
                new_word_parm.DbType = DbType.String;

                string sql;
                SqliteCommand cmd;

                if (exists)
                {
                    int old_priority = SQLUtil.Get_Word_Priority(old_word);

                    SqliteParameter old_priority_parm = new SqliteParameter("@priority", old_priority);
                    old_priority_parm.DbType = DbType.Int32;

                    sql = @"UPDATE Words SET Priority = Priority + @priority WHERE Word = @new_word";
                    cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(old_priority_parm);
                    cmd.Parameters.Add(new_word_parm);
                    commands.Add(cmd);

                    sql = @"DELETE FROM Words WHERE Word = @old_word";
                    cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(old_word_parm);
                    commands.Add(cmd);
                }

                sql = @"UPDATE Inputs SET Input = REPLACE(Input, ' ' + @old_word + ' ', @new_word) WHERE INSTR(Input, ' ' + @old_word + ' ') > 0";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                sql = @"UPDATE Outputs SET Input = REPLACE(Input, ' ' + @old_word + ' ', @new_word) WHERE INSTR(Input, ' ' + @old_word + ' ') > 0";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                sql = @"UPDATE Outputs SET Output = REPLACE(Output, ' ' + @old_word + ' ', @new_word) WHERE INSTR(Output, ' ' + @old_word + ' ') > 0";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                sql = @"UPDATE Topics SET Input = REPLACE(Input, ' ' + @old_word + ' ', @new_word) WHERE INSTR(Input, ' ' + @old_word + ' ') > 0";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                sql = @"UPDATE Words SET Word = @new_word WHERE Word = @old_word";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                sql = @"UPDATE Topics SET Topic = @new_word WHERE Topic = @old_word";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                sql = @"UPDATE PreWords SET Word = @new_word WHERE Word = @old_word";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                sql = @"UPDATE PreWords SET PreWord = @new_word WHERE PreWord = @old_word";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                sql = @"UPDATE ProWords SET Word = @new_word WHERE Word = @old_word";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                sql = @"UPDATE ProWords SET ProWord = @new_word WHERE ProWord = @old_word";
                cmd = new SqliteCommand(sql);
                cmd.Parameters.Add(new_word_parm);
                cmd.Parameters.Add(old_word_parm);
                commands.Add(cmd);

                SQLUtil.BulkExecute(commands);

                List<string> history = AppUtil.GetHistory();
                for (int i = 0; i < history.Count; i++)
                {
                    string line = history[i];
                    while (line.Contains(old_word))
                    {
                        line = line.Replace(old_word, new_word);
                    }

                    history[i] = line;
                }
                AppUtil.SaveHistory(history);
                
                GetWords();

                await DisplayAlert("Fix Spelling", "'" + old_word + "' has been changed to '" + new_word + "'.", "OK");
            }
            else if (string.IsNullOrEmpty(SQLUtil.BrainFile))
            {
                await DisplayAlert("Fix Spelling", "No brain connected to fix spelling in.", "OK");
            }
            else if (string.IsNullOrEmpty(old_word))
            {
                await DisplayAlert("Fix Spelling", "You still need to pick a word to fix.", "OK");
            }
            else if (string.IsNullOrEmpty(new_word))
            {
                await DisplayAlert("Fix Spelling", "You still need to type a new word.", "OK");
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("FixSpelling.OnUpdateClicked", ex.Message, ex.StackTrace);
        }
    }
}