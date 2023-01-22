using Microsoft.Data.Sqlite;
using RealAI.Pages;
using RealAI.Util;

namespace RealAI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        LoadConfig();
    }

    private async void LoadConfig()
    {
        try
        {
            AppUtil.GetBrainList();

            string lastBrain = await AppUtil.GetConfig("Last Loaded Brain");
            if (!string.IsNullOrEmpty(lastBrain) &&
                SQLUtil.BrainList.Contains(lastBrain))
            {
                SQLUtil.BrainFile = lastBrain;
            }

            string thinkSpeed = await AppUtil.GetConfig("ThinkSpeed");
            if (string.IsNullOrEmpty(thinkSpeed))
            {
                Options.ThinkSpeed = 1000;
                AppUtil.SetConfig("ThinkSpeed", "1000");
            }
            else
            {
                Options.ThinkSpeed = int.Parse(thinkSpeed);
            }

            string canThink = await AppUtil.GetConfig("CanThink");
            if (string.IsNullOrEmpty(canThink))
            {
                AppUtil.SetConfig("CanThink", "True");
            }
            else
            {
                Options.CanThink = bool.Parse(canThink);
            }

            string canLearnFromThinking = await AppUtil.GetConfig("CanLearnFromThinking");
            if (string.IsNullOrEmpty(canLearnFromThinking))
            {
                AppUtil.SetConfig("CanLearnFromThinking", "False");
            }
            else
            {
                Options.CanLearnFromThinking = bool.Parse(canLearnFromThinking);
            }

            string attentionSpan = await AppUtil.GetConfig("AttentionSpan");
            if (string.IsNullOrEmpty(attentionSpan))
            {
                Options.AttentionSpan = 7;
                AppUtil.SetConfig("AttentionSpan", "7");
            }
            else
            {
                Options.AttentionSpan = int.Parse(attentionSpan);
            }

            string initiate = await AppUtil.GetConfig("Initiate");
            if (string.IsNullOrEmpty(initiate))
            {
                AppUtil.SetConfig("Initiate", "False");
            }
            else
            {
                Options.Initiate = bool.Parse(initiate);
            }

            string topicResponding = await AppUtil.GetConfig("TopicResponding");
            if (string.IsNullOrEmpty(topicResponding))
            {
                AppUtil.SetConfig("TopicResponding", "True");
            }
            else
            {
                Options.TopicResponding = bool.Parse(topicResponding);
            }

            string wholeResponding = await AppUtil.GetConfig("WholeResponding");
            if (string.IsNullOrEmpty(wholeResponding))
            {
                AppUtil.SetConfig("WholeResponding", "True");
            }
            else
            {
                Options.WholeResponding = bool.Parse(wholeResponding);
            }

            string proceduralResponding = await AppUtil.GetConfig("ProceduralResponding");
            if (string.IsNullOrEmpty(proceduralResponding))
            {
                AppUtil.SetConfig("ProceduralResponding", "True");
            }
            else
            {
                Options.ProceduralResponding = bool.Parse(proceduralResponding);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("AppShell.LoadConfig", ex.Message, ex.StackTrace);
        }
    }

    private async void WipeMemory(object sender, EventArgs e)
    {
        try
        {
            bool answer = await DisplayAlert("Wipe Memory?", "Are you sure you want to wipe the brain's memory?", "Yes", "No");
            if (answer)
            {
                Talk.txt_Output.Text = "";
                Talk.txt_Input.Text = "";

                string historyFolder = await AppUtil.GetHistoryPath(SQLUtil.BrainFile);
                if (Directory.Exists(historyFolder))
                {
                    Directory.Delete(historyFolder, true);
                }

                Talk.Clear();

                List<SqliteCommand> wipe = await SQLUtil.Wipe();
                await SQLUtil.BulkExecute(wipe);

                await DisplayAlert("Wipe Memory?", "The memory has been wiped.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            Logger.AddLog("AppShell.WipeMemory", ex.Message, ex.StackTrace);
        }
    }
}
