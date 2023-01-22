namespace RealAI.Util
{
    public static class AppUtil
    {
        public static async Task<string> GetBasePath()
        {
            try
            {
                //string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string path = @"/storage/emulated/0/Documents/RealAI2/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Source + "\n" + ex.Message + "\n" + ex.StackTrace, "OK");
                Logger.AddLog("AppUtil.GetBasePath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static async Task<string> GetPath(string file)
        {
            try
            {
                string basePath = await GetBasePath();
                if (!string.IsNullOrEmpty(basePath) &&
                    !string.IsNullOrEmpty(file))
                {
                    return Path.Combine(basePath, file);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Source + "\n" + ex.Message + "\n" + ex.StackTrace, "OK");
                Logger.AddLog("AppUtil.GetPath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static async Task<string> GetHistoryPath(string brainFile)
        {
            try
            {
                string basePath = await GetBasePath();
                if (!string.IsNullOrEmpty(basePath))
                {
                    string history_dir = Path.Combine(basePath, "History");
                    if (!Directory.Exists(history_dir))
                    {
                        Directory.CreateDirectory(history_dir);
                    }

                    if (!string.IsNullOrEmpty(brainFile))
                    {
                        string history_brain_dir = Path.Combine(history_dir, brainFile);
                        if (!Directory.Exists(history_brain_dir))
                        {
                            Directory.CreateDirectory(history_brain_dir);
                        }

                        return history_brain_dir;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Source + "\n" + ex.Message + "\n" + ex.StackTrace, "OK");
                Logger.AddLog("AppUtil.GetHistoryPath", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static async Task<string> GetHistoryFile(string brainFile)
        {
            try
            {
                string history_dir = await GetHistoryPath(brainFile);
                if (!string.IsNullOrEmpty(history_dir))
                {
                    string date = DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString();
                    return Path.Combine(await GetHistoryPath(brainFile), date + ".txt");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("AppUtil.GetHistoryFile", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static async void GetBrainList()
        {
            try
            {
                string file = await GetPath("BrainList.txt");
                if (!string.IsNullOrEmpty(file) &&
                    File.Exists(file))
                {
                    SQLUtil.BrainList.AddRange(File.ReadAllLines(file));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("AppUtil.GetBrainList", ex.Message, ex.StackTrace);
            }
        }

        public static async void SaveBrainList()
        {
            try
            {
                string file = await GetPath("BrainList.txt");
                if (!string.IsNullOrEmpty(file))
                {
                    File.WriteAllLines(file, SQLUtil.BrainList);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("AppUtil.SaveBrainList", ex.Message, ex.StackTrace);
            }
        }

        public static async void SetConfig(string name, string value)
        {
            try
            {
                string file = await GetPath("Config.ini");
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
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("AppUtil.Set_Config", ex.Message, ex.StackTrace);
            }
        }

        public static async Task<string> GetConfig(string name)
        {
            try
            {
                string file = await GetPath("Config.ini");
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
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("AppUtil.Get_Config", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static async Task<List<string>> GetHistory()
        {
            List<string> history = new List<string>();

            try
            {
                string file = await GetHistoryFile(SQLUtil.BrainFile);
                if (!string.IsNullOrEmpty(file) &&
                    File.Exists(file))
                {
                    history = File.ReadAllLines(file).ToList();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("AppUtil.GetHistory", ex.Message, ex.StackTrace);
            }

            return history;
        }

        public static async void SaveHistory(List<string> history)
        {
            try
            {
                string path = await GetHistoryPath(SQLUtil.BrainFile);
                if (!string.IsNullOrEmpty(path))
                {
                    string file = await GetHistoryFile(SQLUtil.BrainFile);
                    if (!string.IsNullOrEmpty(file))
                    {
                        File.WriteAllLines(file, history);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("AppUtil.SaveHistory", ex.Message, ex.StackTrace);
            }
        }
    }
}
