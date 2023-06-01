namespace RealAI.Util
{
    public static class AppUtil
    {
        public static string GetVersion()
        {
            return AppInfo.Current.VersionString;
        }

        public static string GetBasePath()
        {
            try
            {
                string path = FileSystem.AppDataDirectory;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetBasePath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string GetBaseExternalPath()
        {
            try
            {
                string path = @"/storage/emulated/0/Documents/RealAIv" + GetVersion() + @"/";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetBaseExternalPath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string GetInternalPath(string file)
        {
            try
            {
                string basePath = GetBasePath();
                if (!string.IsNullOrEmpty(basePath) &&
                    !string.IsNullOrEmpty(file))
                {
                    return Path.Combine(basePath, file);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetInternalPath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string GetExternalPath(string file)
        {
            try
            {
                string basePath = GetBaseExternalPath();
                if (!string.IsNullOrEmpty(basePath) && 
                    !string.IsNullOrEmpty(file))
                {
                    return Path.Combine(basePath, file);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetExternalPath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string GetBrainsBasePath()
        {
            try
            {
                string basePath = GetBasePath();
                if (!string.IsNullOrEmpty(basePath))
                {
                    string brains_base = Path.Combine(basePath, "Brains");
                    if (!string.IsNullOrEmpty(brains_base))
                    {
                        bool brains_base_exists = Directory.Exists(brains_base);
                        if (!brains_base_exists)
                        {
                            Directory.CreateDirectory(brains_base);
                        }

                        return brains_base;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetBrainsBasePath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string GetBrainFile(string brainFile)
        {
            try
            {
                string brains_base = GetBrainsBasePath();
                if (!string.IsNullOrEmpty(brainFile) &&
                    !string.IsNullOrEmpty(brains_base))
                {
                    return Path.Combine(brains_base, brainFile);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetBrainFile", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string GetHistoryBasePath()
        {
            try
            {
                string basePath = GetBasePath();
                if (!string.IsNullOrEmpty(basePath))
                {
                    string history_base = Path.Combine(basePath, "History");
                    if (!string.IsNullOrEmpty(history_base))
                    {
                        bool history_base_exists = Directory.Exists(history_base);
                        if (!history_base_exists)
                        {
                            Directory.CreateDirectory(history_base);
                        }

                        return history_base;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetHistoryBasePath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string GetHistoryPath(string brainFile)
        {
            try
            {
                string history_base = GetHistoryBasePath();
                if (!string.IsNullOrEmpty(brainFile) && 
                    !string.IsNullOrEmpty(history_base))
                {
                    string history_brain_dir = Path.Combine(history_base, brainFile);

                    bool history_brain_dir_exists = Directory.Exists(history_brain_dir);
                    if (!history_brain_dir_exists)
                    {
                        Directory.CreateDirectory(history_brain_dir);
                    }

                    return history_brain_dir;
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetHistoryPath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string GetHistoryFile(string brainFile)
        {
            try
            {
                string history_dir = GetHistoryPath(brainFile);
                if (!string.IsNullOrEmpty(history_dir))
                {
                    string date = DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString();
                    return Path.Combine(GetHistoryPath(brainFile), date + ".txt");
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetHistoryFile", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static void GetBrainList()
        {
            try
            {
                string file = GetInternalPath("BrainList.txt");
                if (!string.IsNullOrEmpty(file) &&
                    File.Exists(file))
                {
                    SQLUtil.BrainList.AddRange(File.ReadAllLines(file));
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetBrainList", ex.Message, ex.StackTrace);
            }
        }

        public static void SaveBrainList()
        {
            try
            {
                string file = GetInternalPath("BrainList.txt");
                if (!string.IsNullOrEmpty(file))
                {
                    File.WriteAllLines(file, SQLUtil.BrainList);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.SaveBrainList", ex.Message, ex.StackTrace);
            }
        }

        public static void SetConfig(string name, string value)
        {
            try
            {
                string file = GetInternalPath("Config.ini");
                if (!string.IsNullOrEmpty(file))
                {
                    List<string> lines = new List<string>();

                    if (File.Exists(file))
                    {
                        lines = File.ReadAllLines(file).ToList();
                    }

                    bool found = false;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains(name))
                        {
                            found = true;
                            lines[i] = name + "=" + value;
                            break;
                        }
                    }

                    if (!found)
                    {
                        lines.Add(name + "=" + value);
                    }

                    File.WriteAllLines(file, lines);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.Set_Config", ex.Message, ex.StackTrace);
            }
        }

        public static string GetConfig(string name)
        {
            try
            {
                string file = GetInternalPath("Config.ini");
                if (!string.IsNullOrEmpty(file) &&
                    File.Exists(file))
                {
                    string[] lines = File.ReadAllLines(file);
                    foreach (string line in lines)
                    {
                        if (line.Contains(name))
                        {
                            string[] results = line.Split('=');
                            if (results.Length > 1)
                            {
                                string value = results[1];
                                if (!string.IsNullOrEmpty(value))
                                {
                                    if (value != "Nothing")
                                    {
                                        return value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.Get_Config", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static List<string> GetHistory()
        {
            List<string> history = new List<string>();

            try
            {
                string file = GetHistoryFile(SQLUtil.BrainFile);
                if (!string.IsNullOrEmpty(file) &&
                    File.Exists(file))
                {
                    history = File.ReadAllLines(file).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetHistory", ex.Message, ex.StackTrace);
            }

            return history;
        }

        public static void SaveHistory(List<string> history)
        {
            try
            {
                string path = GetHistoryPath(SQLUtil.BrainFile);
                if (!string.IsNullOrEmpty(path))
                {
                    string file = GetHistoryFile(SQLUtil.BrainFile);
                    if (!string.IsNullOrEmpty(file))
                    {
                        File.WriteAllLines(file, history);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.SaveHistory", ex.Message, ex.StackTrace);
            }
        }

        public static string GetTime_Milliseconds(DateTime start)
        {
            string result = "";

            try
            {
                int milliseconds = (int)(DateTime.Now - start).TotalMilliseconds;

                int seconds = 0;
                for (int i = 1000; i <= milliseconds;)
                {
                    seconds++;
                    milliseconds -= 1000;
                }

                int minutes = 0;
                for (int i = 60; i <= seconds;)
                {
                    minutes++;
                    seconds -= 60;
                }

                int hours = 0;
                for (int i = 60; i <= minutes;)
                {
                    hours++;
                    minutes -= 60;
                }

                if (hours > 9)
                {
                    result += hours;
                }
                else
                {
                    result += "0" + hours;
                }

                if (minutes > 9)
                {
                    result += ":" + minutes;
                }
                else
                {
                    result += ":0" + minutes;
                }

                if (seconds > 9)
                {
                    result += ":" + seconds;
                }
                else
                {
                    result += ":0" + seconds;
                }

                if (milliseconds > 99)
                {
                    result += "." + milliseconds;
                }
                else if (milliseconds > 9)
                {
                    result += ".0" + milliseconds;
                }
                else
                {
                    result += ".00" + milliseconds;
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.GetTime_Milliseconds", ex.Message, ex.StackTrace);
            }

            return result;
        }

        public static void InitProgress(ProgressBar progressBar, Label progresLabel)
        {
            try
            {
                if (MainThread.IsMainThread)
                {
                    progressBar.ProgressTo(0, 0, Easing.Linear);
                    progresLabel.Text = GetTime_Milliseconds(DateTime.Now);
                }
                else
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        progressBar.ProgressTo(0, 0, Easing.Linear);
                        progresLabel.Text = GetTime_Milliseconds(DateTime.Now);
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.InitProgress", ex.Message, ex.StackTrace);
            }
        }

        public static void UpdateProgress(ProgressBar progressBar, double value, Label progresLabel, DateTime startDateTime)
        {
            try
            {
                if (MainThread.IsMainThread)
                {
                    progressBar.ProgressTo(value, 0, Easing.Linear);
                    progresLabel.Text = GetTime_Milliseconds(startDateTime);
                }
                else
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        progressBar.ProgressTo(value, 0, Easing.Linear);
                        progresLabel.Text = GetTime_Milliseconds(startDateTime);
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.UpdateProgress", ex.Message, ex.StackTrace);
            }
        }

        public static void FinishProgress(ProgressBar progressBar, Label progresLabel, DateTime startDateTime)
        {
            try
            {
                if (MainThread.IsMainThread)
                {
                    progressBar.ProgressTo(1, 0, Easing.Linear);
                    progresLabel.Text = GetTime_Milliseconds(startDateTime);
                }
                else
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        progressBar.ProgressTo(1, 0, Easing.Linear);
                        progresLabel.Text = GetTime_Milliseconds(startDateTime);
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.FinishProgress", ex.Message, ex.StackTrace);
            }
        }
    }
}
