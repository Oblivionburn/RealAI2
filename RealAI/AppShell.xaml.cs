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

    private void LoadConfig()
    {
        try
        {
            AppUtil.GetBrainList();

            string lastBrain = AppUtil.GetConfig("Last Loaded Brain");
            if (!string.IsNullOrEmpty(lastBrain) &&
                SQLUtil.BrainList.Contains(lastBrain))
            {
                SQLUtil.BrainFile = lastBrain;
            }

            string thinkSpeed = AppUtil.GetConfig("ThinkSpeed");
            if (string.IsNullOrEmpty(thinkSpeed))
            {
                Options.ThinkSpeed = 1000;
                AppUtil.SetConfig("ThinkSpeed", "1000");
            }
            else
            {
                Options.ThinkSpeed = int.Parse(thinkSpeed);
            }

            string canThink = AppUtil.GetConfig("CanThink");
            if (string.IsNullOrEmpty(canThink))
            {
                AppUtil.SetConfig("CanThink", "True");
            }
            else
            {
                Options.CanThink = bool.Parse(canThink);
            }

            string canLearnFromThinking = AppUtil.GetConfig("CanLearnFromThinking");
            if (string.IsNullOrEmpty(canLearnFromThinking))
            {
                AppUtil.SetConfig("CanLearnFromThinking", "False");
            }
            else
            {
                Options.CanLearnFromThinking = bool.Parse(canLearnFromThinking);
            }

            string attentionSpan = AppUtil.GetConfig("AttentionSpan");
            if (string.IsNullOrEmpty(attentionSpan))
            {
                Options.AttentionSpan = 7;
                AppUtil.SetConfig("AttentionSpan", "7");
            }
            else
            {
                Options.AttentionSpan = int.Parse(attentionSpan);
            }

            string initiate = AppUtil.GetConfig("Initiate");
            if (string.IsNullOrEmpty(initiate))
            {
                AppUtil.SetConfig("Initiate", "False");
            }
            else
            {
                Options.Initiate = bool.Parse(initiate);
            }

            string topicResponding = AppUtil.GetConfig("TopicResponding");
            if (string.IsNullOrEmpty(topicResponding))
            {
                AppUtil.SetConfig("TopicResponding", "True");
            }
            else
            {
                Options.TopicResponding = bool.Parse(topicResponding);
            }

            string wholeResponding = AppUtil.GetConfig("WholeResponding");
            if (string.IsNullOrEmpty(wholeResponding))
            {
                AppUtil.SetConfig("WholeResponding", "True");
            }
            else
            {
                Options.WholeResponding = bool.Parse(wholeResponding);
            }

            string proceduralResponding = AppUtil.GetConfig("ProceduralResponding");
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

                string historyFolder = AppUtil.GetHistoryPath(SQLUtil.BrainFile);
                if (Directory.Exists(historyFolder))
                {
                    Directory.Delete(historyFolder, true);
                }

                if (Thinking.ThinkTimer != null)
                {
                    Thinking.ThinkTimer.Stop();
                }
                
                Talk.Clear();

                List<SqliteCommand> wipe = SQLUtil.Wipe();
                SQLUtil.BulkExecute(wipe);

                if (Thinking.ThinkTimer != null)
                {
                    Thinking.ThinkTimer.Start();
                }

                await DisplayAlert("Wipe Memory?", "The memory has been wiped.", "OK");
            }
        }
        catch (Exception ex)
        {
            Logger.AddLog("AppShell.WipeMemory", ex.Message, ex.StackTrace);
        }
    }
}
