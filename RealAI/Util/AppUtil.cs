namespace RealAI.Util
{
    public static class AppUtil
    {
        public static string GetBasePath()
        {
            try
            {
                string path = @"/storage/emulated/0/Documents/RealAI2/";

                bool path_exits = Directory.Exists(path);
                if (!path_exits)
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

        public static string GetPath(string file)
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
                Logger.AddLog("AppUtil.GetPath", ex.Message, ex.StackTrace);
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
                string file = GetPath("BrainList.txt");
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
                string file = GetPath("BrainList.txt");
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
                string file = GetPath("Config.ini");
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
                string file = GetPath("Config.ini");
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
    }
}
