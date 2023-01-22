namespace RealAI.Util
{
    public static class AppUtil
    {
        public static string GetPath(string file)
        {
            return Path.Combine(FileSystem.AppDataDirectory, file);
        }

        public static string GetHistoryPath(string brainFile)
        {
            return Path.Combine(FileSystem.AppDataDirectory, "History", brainFile);
        }

        public static string GetHistoryFile(string brainFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(GetHistoryPath(brainFile)))
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
            string file = GetPath("BrainList.txt");

            if (File.Exists(file))
            {
                SQLUtil.BrainList.AddRange(File.ReadAllLines(file));
            }
        }

        public static void SaveBrainList()
        {
            string file = GetPath("BrainList.txt");
            File.WriteAllLines(file, SQLUtil.BrainList);
        }

        public static void SetConfig(string name, string value)
        {
            try
            {
                string file = GetPath("Config.ini");

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
                if (File.Exists(file))
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
                if (File.Exists(file))
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
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string file = GetHistoryFile(SQLUtil.BrainFile);
                    File.WriteAllLines(file, history);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("AppUtil.SaveHistory", ex.Message, ex.StackTrace);
            }
        }
    }
}
