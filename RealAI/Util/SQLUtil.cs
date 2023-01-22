using System.Data;
using Microsoft.Data.Sqlite;
using RealAI.Pages;

namespace RealAI.Util
{
    public static class SQLUtil
    {
        public static string BrainFile;
        public static List<string> BrainList = new List<string>();

        public static async Task<SqliteConnection> GetConnection(string fileName)
        {
            try
            {
                string path = await AppUtil.GetPath(fileName + ".brain");
                return new SqliteConnection("Data Source=" + path);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.GetSqlConnection", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static async Task<string> NewBrain(string fileName)
        {
            try
            {
                List<SqliteCommand> commands = new List<SqliteCommand>();

                string sql = @"
                CREATE TABLE Inputs
                (
                    ID INTEGER PRIMARY KEY,
                    Input TEXT,
                    Priority INTEGER DEFAULT 1
                )";
                SqliteCommand cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"CREATE INDEX idx_Inputs ON Inputs (input)";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"
                CREATE TABLE Words
                (
                    ID INTEGER PRIMARY KEY,
                    Word TEXT,
                    Priority INTEGER DEFAULT 1
                )";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"CREATE INDEX idx_Words ON Words (Word)";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"
                CREATE TABLE Outputs
                (
                    ID INTEGER PRIMARY KEY,
                    Input TEXT,
                    Output TEXT,
                    Priority INTEGER DEFAULT 1
                )";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"CREATE INDEX idx_Outputs ON Outputs (Input, Output)";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"
                CREATE TABLE Topics
                (
                    ID INTEGER PRIMARY KEY,
                    Input TEXT,
                    Topic TEXT,
                    Priority INTEGER DEFAULT 1
                )";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"CREATE INDEX idx_Topics ON Topics (Input, Topic)";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"
                CREATE TABLE PreWords
                (
                    ID INTEGER PRIMARY KEY,
                    Word TEXT,
                    PreWord TEXT,
                    Priority INTEGER DEFAULT 1,
                    Distance INTEGER
                )";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"CREATE INDEX idx_PreWords ON PreWords (Word, PreWord, Distance)";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"
                CREATE TABLE ProWords
                (
                    ID INTEGER PRIMARY KEY,
                    Word TEXT,
                    ProWord TEXT,
                    Priority INTEGER DEFAULT 1,
                    Distance INTEGER
                )";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                sql = @"CREATE INDEX idx_ProWords ON ProWords (Word, ProWord, Distance)";
                cmd = new SqliteCommand(sql);
                commands.Add(cmd);

                BrainFile = fileName;

                await BulkExecute(commands);

                BrainList.Add(fileName);
                string file = await AppUtil.GetPath("BrainList.txt");
                File.WriteAllLines(file, BrainList);

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SQLUtil.NewBrain", ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public static async Task<string> DeleteBrain(string fileName)
        {
            try
            {
                string file = await AppUtil.GetPath(fileName + ".brain");
                if (File.Exists(file))
                {
                    File.Delete(file);
                    BrainList.Remove(fileName);

                    string historyFolder = await AppUtil.GetHistoryPath(fileName);
                    if (Directory.Exists(historyFolder))
                    {
                        Directory.Delete(historyFolder, true);
                    }

                    if (BrainFile == fileName)
                    {
                        BrainFile = null;
                        Talk.Clear();
                    }

                    return "SUCCESS";
                }
                else
                {
                    return "NOT FOUND";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SQLUtil.NewBrain", ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public static async Task<bool> BulkExecute(List<SqliteCommand> commands)
        {
            try
            {
                if (!string.IsNullOrEmpty(BrainFile))
                {
                    SqliteConnection connection = await GetConnection(BrainFile);
                    using (SqliteConnection con = new SqliteConnection(connection.ConnectionString))
                    {
                        con.Open();

                        using (SqliteTransaction transaction = con.BeginTransaction())
                        {
                            foreach (SqliteCommand command in commands)
                            {
                                using (SqliteCommand cmd = new SqliteCommand(command.CommandText, con, transaction))
                                {
                                    foreach (SqliteParameter existing in command.Parameters)
                                    {
                                        SqliteParameter parm = new SqliteParameter();
                                        parm.ParameterName = existing.ParameterName;
                                        parm.Value = existing.Value;
                                        parm.DbType = existing.DbType;
                                        cmd.Parameters.Add(parm);
                                    }

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.BulkQuery", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static async Task<bool> Execute(SqliteCommand command)
        {
            try
            {
                if (!string.IsNullOrEmpty(BrainFile))
                {
                    SqliteConnection connection = await GetConnection(BrainFile);
                    using (SqliteConnection con = new SqliteConnection(connection.ConnectionString))
                    {
                        con.Open();

                        using (SqliteTransaction transaction = con.BeginTransaction())
                        {
                            using (SqliteCommand cmd = new SqliteCommand(command.CommandText, con, transaction))
                            {
                                foreach (SqliteParameter existing in command.Parameters)
                                {
                                    SqliteParameter parm = new SqliteParameter();
                                    parm.ParameterName = existing.ParameterName;
                                    parm.Value = existing.Value;
                                    parm.DbType = existing.DbType;
                                    cmd.Parameters.Add(parm);
                                }

                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Execute", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static async Task<DataTable> GetData(string sql, SqliteParameter[] parameters)
        {
            DataTable data = new DataTable();

            try
            {
                if (sql.Length > 0)
                {
                    SqliteConnection connection = await GetConnection(BrainFile);
                    using (SqliteConnection con = new SqliteConnection(connection.ConnectionString))
                    {
                        con.Open();

                        using (SqliteCommand cmd = new SqliteCommand(sql, con))
                        {
                            if (parameters != null)
                            {
                                if (parameters.Length > 0)
                                {
                                    for (int i = 0; i < parameters.Length; i++)
                                    {
                                        if (parameters[i] != null)
                                        {
                                            cmd.Parameters.Add(parameters[i]);
                                        }
                                    }
                                }
                            }

                            using (SqliteDataReader dataReader = cmd.ExecuteReader())
                            {
                                data.Load(dataReader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.GetData", ex.Message, ex.StackTrace);
            }

            return data;
        }

        public static async Task<List<SqliteCommand>> Wipe()
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                string[] tables = { "Outputs", "Topics", "PreWords", "ProWords", "Inputs", "Words" };
                foreach (string table in tables)
                {
                    commands.Add(new SqliteCommand("DELETE FROM " + table));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Wipe", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<int> Get_InputPriority(string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    List<SqliteParameter> parameters = new List<SqliteParameter>();

                    SqliteParameter parm = new SqliteParameter("@input", input);
                    parm.DbType = DbType.String;
                    parameters.Add(parm);

                    DataTable data = await GetData("SELECT Priority FROM Inputs WHERE Input = @input", parameters.ToArray());
                    if (data.Rows.Count > 0)
                    {
                        return int.Parse(data.Rows[0].ItemArray[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_InputPriority", ex.Message, ex.StackTrace);
            }

            return 0;
        }

        public static async Task<bool> InputHasWord(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    List<SqliteParameter> parameters = new List<SqliteParameter>();

                    SqliteParameter parm = new SqliteParameter("@word", word);
                    parm.DbType = DbType.String;
                    parameters.Add(parm);

                    DataTable data = await GetData("SELECT Input FROM Inputs WHERE INSTR(Input, @word) > 0", parameters.ToArray());
                    return data.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.InputHasWord", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static async Task<List<SqliteCommand>> Add_NewInput(string input)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteParameter parm = new SqliteParameter("@input", input);
                    parm.DbType = DbType.String;

                    string sql = "INSERT INTO Inputs (Input) SELECT @input WHERE NOT EXISTS (SELECT 1 FROM Inputs WHERE Input = @input)";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Add_NewInput", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Increase_InputPriorities(string input)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteParameter parm = new SqliteParameter("@input", input);
                    parm.DbType = DbType.String;

                    string sql = "UPDATE Inputs SET Priority = Priority + 1 WHERE Input = @input";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Increase_InputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Decrease_InputPriorities(string input)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteParameter parm = new SqliteParameter("@input", input);
                    parm.DbType = DbType.String;

                    string sql = "UPDATE Inputs SET Priority = Priority - 1 WHERE Input = @input";
                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(parm);
                    commands.Add(cmd);

                    string delete_sql = "DELETE FROM Inputs WHERE Priority < 1";
                    SqliteCommand delete_cmd = new SqliteCommand(delete_sql);
                    commands.Add(delete_cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Decrease_InputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<string>> Get_Words()
        {
            List<string> words = new List<string>();

            try
            {
                DataTable data = await GetData("SELECT Word FROM Words", null);

                foreach (DataRow row in data.Rows)
                {
                    string word = row.ItemArray[0].ToString();
                    words.Add(word);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_Words", ex.Message, ex.StackTrace);
            }

            return words;
        }

        public static async Task<string> Get_RandomWord()
        {
            string word = "";

            try
            {
                DataTable data = await GetData("SELECT Word FROM Words ORDER BY RANDOM() LIMIT 1", null);

                if (data.Rows.Count > 0)
                {
                    word = data.Rows[0].ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_RandomWord", ex.Message, ex.StackTrace);
            }

            return word;
        }

        public static async Task<string> Get_MinWord(string[] words)
        {
            string word = "";

            try
            {
                if (words.Length > 0)
                {
                    List<string> min_words = new List<string>();

                    int priority = await Get_MinPriority_Words(words);

                    string wordSet = await WordArray_To_String(words);

                    SqliteParameter parm = new SqliteParameter("@priority", priority);
                    parm.DbType = DbType.Int32;

                    SqliteParameter[] parms = { parm };
                    DataTable data = await GetData("SELECT Word FROM Words WHERE Priority = @priority AND Word IN (" + wordSet + ")", parms);

                    foreach (DataRow row in data.Rows)
                    {
                        string min_word = row.ItemArray[0].ToString();
                        min_words.Add(min_word);
                    }

                    if (min_words.Count > 0)
                    {
                        CryptoRandom random = new CryptoRandom();
                        int choice = random.Next(0, min_words.Count);
                        word = min_words[choice];
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_MinWord", ex.Message, ex.StackTrace);
            }

            return word;
        }

        public static async Task<string[]> Get_MinWords(string[] words)
        {
            List<string> min_words = new List<string>();

            try
            {
                if (words.Length > 0)
                {
                    int priority = await Get_MinPriority_Words(words);

                    string wordSet = await WordArray_To_String(words);

                    SqliteParameter parm = new SqliteParameter("@priority", priority);
                    parm.DbType = DbType.Int32;

                    SqliteParameter[] parms = { parm };
                    DataTable data = await GetData("SELECT Word FROM Words WHERE Priority = @priority AND Word IN (" + wordSet + ")", parms);

                    int total = data.Rows.Count;

                    foreach (DataRow row in data.Rows)
                    {
                        string word = row.ItemArray[0].ToString();
                        min_words.Add(word);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_MinWords", ex.Message, ex.StackTrace);
            }

            return min_words.ToArray();
        }

        public static async Task<int> Get_MinPriority_Words(string[] words)
        {
            int min = 0;

            try
            {
                if (words.Length > 0)
                {
                    string wordSet = await WordArray_To_String(words);

                    DataTable data = await GetData("SELECT MIN(Priority) AS MinPriority FROM Words WHERE Word IN (" + wordSet + ")", null);

                    if (data.Rows.Count > 0 &&
                        data.Rows[0].ItemArray.Length > 0)
                    {
                        string result = data.Rows[0].ItemArray[0].ToString();
                        if (!string.IsNullOrEmpty(result))
                        {
                            min = int.Parse(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_MinPriority_Words", ex.Message, ex.StackTrace);
            }

            return min;
        }

        public static async Task<List<SqliteCommand>> Add_Words(string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                foreach (string word in words)
                {
                    SqliteParameter parm = new SqliteParameter("@word", word);
                    parm.DbType = DbType.String;

                    string sql = "INSERT INTO Words (Word) SELECT @word WHERE NOT EXISTS (SELECT 1 FROM Words WHERE Word = @word)";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Add_Words", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<int> Get_Word_Priority(string word)
        {
            int priority = 0;

            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    SqliteParameter parm = new SqliteParameter("@word", word);
                    parm.DbType = DbType.String;

                    SqliteParameter[] parms = { parm };

                    DataTable data = await GetData("SELECT Priority FROM Words WHERE Word = @word", parms);

                    if (data.Rows.Count > 0 &&
                        data.Rows[0].ItemArray.Length > 0)
                    {
                        string result = data.Rows[0].ItemArray[0].ToString();
                        if (!string.IsNullOrEmpty(result))
                        {
                            priority = int.Parse(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_Word_Priority", ex.Message, ex.StackTrace);
            }

            return priority;
        }

        public static async Task<List<SqliteCommand>> Increase_WordPriorities(string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                foreach (string word in words)
                {
                    SqliteParameter parm = new SqliteParameter("@word", word);
                    parm.DbType = DbType.String;

                    string sql = "UPDATE Words SET Priority = Priority + 1 WHERE Word = @word";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Increase_WordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Decrease_WordPriorities(string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                foreach (string word in words)
                {
                    SqliteParameter parm = new SqliteParameter("@word", word);
                    parm.DbType = DbType.String;

                    string sql = "UPDATE Words SET Priority = Priority - 1 WHERE Word = @word";
                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(parm);
                    commands.Add(cmd);

                    string delete_sql = "DELETE FROM Words WHERE Priority < 1";
                    SqliteCommand delete_cmd = new SqliteCommand(delete_sql);
                    commands.Add(delete_cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Decrease_WordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<string> Get_PreWord(List<string> words, bool for_thinking)
        {
            string result = "";

            try
            {
                if (words.Count > 0)
                {
                    Dictionary<string, int> options = new Dictionary<string, int>();
                    List<DataRow> rows = new List<DataRow>();

                    int count = 1;

                    for (int i = 0; i < words.Count; i++)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        string word = words[i];

                        SqliteParameter word_parm = new SqliteParameter("@word", word);
                        word_parm.DbType = DbType.String;

                        SqliteParameter distance_parm = new SqliteParameter("@distance", count);
                        distance_parm.DbType = DbType.String;

                        SqliteParameter[] parms = { word_parm, distance_parm };
                        DataTable data = await GetData("SELECT * FROM PreWords WHERE Word = @word AND Distance = @distance", parms);

                        foreach (DataRow row in data.Rows)
                        {
                            if (for_thinking &&
                                !Options.CanThink)
                            {
                                break;
                            }

                            rows.Add(row);
                        }

                        count++;
                    }

                    foreach (DataRow row in rows)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        string pre_word = row.ItemArray[2].ToString();
                        int priority = int.Parse(row.ItemArray[3].ToString());
                        int distance = int.Parse(row.ItemArray[4].ToString());

                        if (distance == 1)
                        {
                            //Get options
                            if (!options.ContainsKey(pre_word))
                            {
                                options.Add(pre_word, priority);
                            }
                        }
                        else
                        {
                            //Reinforce options that match farther distances
                            if (options.Count > 0 &&
                                options.ContainsKey(pre_word))
                            {
                                options[pre_word]++;
                            }
                        }
                    }

                    //Get max priority from options
                    int max = 0;
                    foreach (var set in options)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        int priority = set.Value;
                        if (priority > max)
                        {
                            max = priority;
                        }
                    }

                    //Randomly select one with bias towards max priority
                    foreach (var set in options)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        int priority = set.Value;

                        CryptoRandom random = new CryptoRandom();
                        int choice = random.Next(0, max + 1);

                        if (priority >= choice)
                        {
                            result = set.Key;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_PreWord", ex.Message, ex.StackTrace);
            }

            return result;
        }

        public static async Task<List<SqliteCommand>> Add_PreWords(string[] words, string[] prewords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                for (int i = 0; i < words.Length; i++)
                {
                    SqliteParameter word_parm = new SqliteParameter("@word", words[i]);
                    word_parm.DbType = DbType.String;

                    SqliteParameter pre_word_parm = new SqliteParameter("@pre_word", prewords[i]);
                    pre_word_parm.DbType = DbType.String;

                    SqliteParameter distance_parm = new SqliteParameter("@distance", distances[i]);
                    distance_parm.DbType = DbType.Int32;

                    string sql = @"
                    INSERT INTO PreWords (Word, PreWord, Distance)
                    SELECT @word, @pre_word, @distance
                    WHERE NOT EXISTS (SELECT 1 FROM PreWords WHERE Word = @word AND PreWord = @pre_word AND Distance = @distance)
                    ";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(word_parm);
                    cmd.Parameters.Add(pre_word_parm);
                    cmd.Parameters.Add(distance_parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Add_PreWords", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Increase_PreWordPriorities(string[] words, string[] prewords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                for (int i = 0; i < words.Length; i++)
                {
                    SqliteParameter word_parm = new SqliteParameter("@word", words[i]);
                    word_parm.DbType = DbType.String;

                    SqliteParameter pre_word_parm = new SqliteParameter("@pre_word", prewords[i]);
                    pre_word_parm.DbType = DbType.String;

                    SqliteParameter distance_parm = new SqliteParameter("@distance", distances[i]);
                    distance_parm.DbType = DbType.Int32;

                    string sql = @"
                    UPDATE PreWords
                    SET Priority = Priority + 1
                    WHERE Word = @word AND PreWord = @pre_word AND Distance = @distance
                    ";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(word_parm);
                    cmd.Parameters.Add(pre_word_parm);
                    cmd.Parameters.Add(distance_parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Increase_PreWordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Decrease_PreWordPriorities(string[] words, string[] prewords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                for (int i = 0; i < words.Length; i++)
                {
                    SqliteParameter word_parm = new SqliteParameter("@word", words[i]);
                    word_parm.DbType = DbType.String;

                    SqliteParameter pre_word_parm = new SqliteParameter("@pre_word", prewords[i]);
                    pre_word_parm.DbType = DbType.String;

                    SqliteParameter distance_parm = new SqliteParameter("@distance", distances[i]);
                    distance_parm.DbType = DbType.Int32;

                    string sql = @"
                    UPDATE PreWords
                    SET Priority = Priority - 1
                    WHERE Word = @word AND PreWord = @pre_word AND Distance = @distance
                    ";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(word_parm);
                    cmd.Parameters.Add(pre_word_parm);
                    cmd.Parameters.Add(distance_parm);
                    commands.Add(cmd);
                }

                string delete_sql = @"DELETE FROM PreWords WHERE Priority < 1";
                SqliteCommand delete_cmd = new SqliteCommand(delete_sql);
                commands.Add(delete_cmd);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Decrease_PreWordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<string> Get_ProWord(List<string> words, bool for_thinking)
        {
            string result = "";

            try
            {
                if (words.Count > 0)
                {
                    Dictionary<string, int> options = new Dictionary<string, int>();
                    List<DataRow> rows = new List<DataRow>();

                    int count = 1;

                    for (int i = words.Count - 1; i >= 0; i--)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        string word = words[i];

                        SqliteParameter word_parm = new SqliteParameter("@word", word);
                        word_parm.DbType = DbType.String;

                        SqliteParameter distance_parm = new SqliteParameter("@distance", count);
                        distance_parm.DbType = DbType.String;

                        SqliteParameter[] parms = { word_parm, distance_parm };
                        DataTable data = await GetData("SELECT * FROM ProWords WHERE Word = @word AND Distance = @distance", parms);

                        foreach (DataRow row in data.Rows)
                        {
                            if (for_thinking &&
                                !Options.CanThink)
                            {
                                break;
                            }

                            rows.Add(row);
                        }

                        count++;
                    }

                    foreach (DataRow row in rows)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        string pro_word = row.ItemArray[2].ToString();
                        int priority = int.Parse(row.ItemArray[3].ToString());
                        int distance = int.Parse(row.ItemArray[4].ToString());

                        if (distance == 1)
                        {
                            //Get options
                            if (!options.ContainsKey(pro_word))
                            {
                                options.Add(pro_word, priority);
                            }
                        }
                        else
                        {
                            //Reinforce options that match farther distances
                            if (options.Count > 0 &&
                                options.ContainsKey(pro_word))
                            {
                                options[pro_word]++;
                            }
                        }
                    }

                    //Get max priority from options
                    int max = 0;
                    foreach (var set in options)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        int priority = set.Value;
                        if (priority > max)
                        {
                            max = priority;
                        }
                    }

                    //Randomly select one with bias towards max priority
                    foreach (var set in options)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        int priority = set.Value;

                        CryptoRandom random = new CryptoRandom();
                        int choice = random.Next(0, max + 1);

                        if (priority >= choice)
                        {
                            result = set.Key;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_ProWord", ex.Message, ex.StackTrace);
            }

            return result;
        }

        public static async Task<List<SqliteCommand>> Add_ProWords(string[] words, string[] prowords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                for (int i = 0; i < words.Length; i++)
                {
                    SqliteParameter word_parm = new SqliteParameter("@word", words[i]);
                    word_parm.DbType = DbType.String;

                    SqliteParameter pro_word_parm = new SqliteParameter("@pro_word", prowords[i]);
                    pro_word_parm.DbType = DbType.String;

                    SqliteParameter distance_parm = new SqliteParameter("@distance", distances[i]);
                    distance_parm.DbType = DbType.Int32;

                    string sql = @"
                    INSERT INTO ProWords (Word, ProWord, Distance)
                    SELECT @word, @pro_word, @distance
                    WHERE NOT EXISTS (SELECT 1 FROM ProWords WHERE Word = @word AND ProWord = @pro_word AND Distance = @distance)
                    ";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(word_parm);
                    cmd.Parameters.Add(pro_word_parm);
                    cmd.Parameters.Add(distance_parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Add_ProWords", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Increase_ProWordPriorities(string[] words, string[] prowords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                for (int i = 0; i < words.Length; i++)
                {
                    SqliteParameter word_parm = new SqliteParameter("@word", words[i]);
                    word_parm.DbType = DbType.String;

                    SqliteParameter pro_word_parm = new SqliteParameter("@pro_word", prowords[i]);
                    pro_word_parm.DbType = DbType.String;

                    SqliteParameter distance_parm = new SqliteParameter("@distance", distances[i]);
                    distance_parm.DbType = DbType.Int32;

                    string sql = @"
                    UPDATE ProWords
                    SET Priority = Priority + 1
                    WHERE Word = @word AND ProWord = @pro_word AND Distance = @distance
                    ";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(word_parm);
                    cmd.Parameters.Add(pro_word_parm);
                    cmd.Parameters.Add(distance_parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Increase_ProWordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Decrease_ProWordPriorities(string[] words, string[] prowords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                for (int i = 0; i < words.Length; i++)
                {
                    SqliteParameter word_parm = new SqliteParameter("@word", words[i]);
                    word_parm.DbType = DbType.String;

                    SqliteParameter pro_word_parm = new SqliteParameter("@pro_word", prowords[i]);
                    pro_word_parm.DbType = DbType.String;

                    SqliteParameter distance_parm = new SqliteParameter("@distance", distances[i]);
                    distance_parm.DbType = DbType.Int32;

                    string sql = @"
                    UPDATE ProWords
                    SET Priority = Priority - 1
                    WHERE Word = @word AND ProWord = @pro_word AND Distance = @distance
                    ";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(word_parm);
                    cmd.Parameters.Add(pro_word_parm);
                    cmd.Parameters.Add(distance_parm);
                    commands.Add(cmd);
                }

                string delete_sql = @"DELETE FROM ProWords WHERE Priority < 1";
                SqliteCommand delete_cmd = new SqliteCommand(delete_sql);
                commands.Add(delete_cmd);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Decrease_ProWordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> AddTopics(string input, string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input) &&
                    words.Length > 0)
                {
                    foreach (string word in words)
                    {
                        SqliteParameter input_parm = new SqliteParameter("@input", input);
                        input_parm.DbType = DbType.String;

                        SqliteParameter word_parm = new SqliteParameter("@word", word);
                        word_parm.DbType = DbType.String;

                        //Add new topics
                        string sql = @"
                        INSERT INTO Topics (Input, Topic) 
                        SELECT @input, @word 
                        WHERE NOT EXISTS (SELECT 1 FROM Topics WHERE Input = @input AND Topic = @word)
                        ";

                        SqliteCommand cmd = new SqliteCommand(sql);
                        cmd.Parameters.Add(input_parm);
                        cmd.Parameters.Add(word_parm);
                        commands.Add(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.AddTopics", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Increase_TopicPriorities(string input, string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input) &&
                    words.Length > 0)
                {
                    foreach (string word in words)
                    {
                        SqliteParameter input_parm = new SqliteParameter("@input", input);
                        input_parm.DbType = DbType.String;

                        SqliteParameter word_parm = new SqliteParameter("@word", word);
                        word_parm.DbType = DbType.String;

                        //Increase priority for existing topics
                        string sql = "UPDATE Topics SET Priority = Priority + 1 WHERE Input = @input AND Topic = @word";

                        SqliteCommand cmd = new SqliteCommand(sql);
                        cmd.Parameters.Add(input_parm);
                        cmd.Parameters.Add(word_parm);
                        commands.Add(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Increase_TopicPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Decrease_TopicPriorities(string input, string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input) &&
                    words.Length > 0)
                {
                    foreach (string word in words)
                    {
                        SqliteParameter input_parm = new SqliteParameter("@input", input);
                        input_parm.DbType = DbType.String;

                        SqliteParameter word_parm = new SqliteParameter("@word", word);
                        word_parm.DbType = DbType.String;

                        //Increase priority for existing topics
                        string sql = "UPDATE Topics SET Priority = Priority - 1 WHERE Input = @input AND Topic = @word";

                        SqliteCommand cmd = new SqliteCommand(sql);
                        cmd.Parameters.Add(input_parm);
                        cmd.Parameters.Add(word_parm);
                        commands.Add(cmd);
                    }
                }

                string delete_sql = @"DELETE FROM Topics WHERE Priority < 1";
                SqliteCommand delete_cmd = new SqliteCommand(delete_sql);
                commands.Add(delete_cmd);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Decrease_TopicPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Decrease_Topics_Unmatched(string input, string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input) &&
                    words.Length > 0)
                {
                    SqliteParameter input_parm = new SqliteParameter("@input", input);
                    input_parm.DbType = DbType.String;

                    string word_string = await WordArray_To_String(words);
                    string sql = "UPDATE Topics SET Priority = Priority - 1 WHERE Input = @input AND Topic NOT IN (" + word_string + ")";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(input_parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Decrease_Topics_Unmatched", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Clean_Topics(string input)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteParameter input_parm = new SqliteParameter("@input", input);
                    input_parm.DbType = DbType.String;

                    string sql = "DELETE FROM Topics WHERE Input = @input AND Priority < 1";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(input_parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Clean_Topics", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<bool> Topics_InputHasWord(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    List<SqliteParameter> parameters = new List<SqliteParameter>();

                    SqliteParameter parm = new SqliteParameter("@word", word);
                    parm.DbType = DbType.String;
                    parameters.Add(parm);

                    DataTable data = await GetData("SELECT Input FROM Topics WHERE INSTR(Input, @word) > 0", parameters.ToArray());
                    return data.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Topics_InputHasWord", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static async Task<string[]> Get_OutputsFromTopics(string[] words)
        {
            List<string> outputs = new List<string>();

            try
            {
                if (words.Length > 0)
                {
                    string wordSet = await WordArray_To_String(words);

                    //Get all the inputs that contain a matching topic from the wordset
                    DataTable inputsData = await GetData("SELECT DISTINCT Input FROM Topics WHERE Topic IN (" + wordSet + ")", null);

                    int total = inputsData.Rows.Count;

                    Dictionary<string, int> output_priority = new Dictionary<string, int>();
                    foreach (DataRow row in inputsData.Rows)
                    {
                        string input = row.ItemArray[0].ToString();

                        List<SqliteParameter> parameters = new List<SqliteParameter>();
                        SqliteParameter parm = new SqliteParameter("@input", input);
                        parm.DbType = DbType.String;
                        parameters.Add(parm);

                        //Get all the outputs for the given input
                        DataTable outputData = await GetData("SELECT DISTINCT Output FROM Outputs WHERE Input = @input", parameters.ToArray());
                        if (outputData.Rows.Count > 0)
                        {
                            string output = outputData.Rows[0].ItemArray[0].ToString();

                            //Get the priority of the output from the Input table
                            int priority = await Get_InputPriority(output);

                            //Add output/priority pair as a possibility
                            if (!output_priority.ContainsKey(output))
                            {
                                output_priority.Add(output, priority);
                            }
                        }
                    }

                    //Get the highest priority output
                    if (output_priority.Count > 0)
                    {
                        int max_priority = 0;
                        foreach (KeyValuePair<string, int> output_pair in output_priority)
                        {
                            if (output_pair.Value > max_priority)
                            {
                                max_priority = output_pair.Value;
                            }
                        }

                        foreach (KeyValuePair<string, int> output_pair in output_priority)
                        {
                            //Add any output matching the highest priority
                            if (output_pair.Value >= max_priority)
                            {
                                outputs.Add(output_pair.Key);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_OutputsFromTopics", ex.Message, ex.StackTrace);
            }

            return outputs.ToArray();
        }

        public static async Task<List<SqliteCommand>> Add_Outputs(string input, string output)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input) &&
                    !string.IsNullOrEmpty(output))
                {
                    SqliteParameter input_parm = new SqliteParameter("@input", input);
                    input_parm.DbType = DbType.String;

                    SqliteParameter output_parm = new SqliteParameter("@output", output);
                    output_parm.DbType = DbType.String;

                    string sql = @"
                    INSERT INTO Outputs (Input, Output)
                    SELECT @input, @output
                    WHERE NOT EXISTS (SELECT 1 FROM Outputs WHERE Input = @input AND Output = @output)";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(input_parm);
                    cmd.Parameters.Add(output_parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Add_Outputs", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Increase_OutputPriorities(string input, string output)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input) &&
                    !string.IsNullOrEmpty(output))
                {
                    SqliteParameter input_parm = new SqliteParameter("@input", input);
                    input_parm.DbType = DbType.String;

                    SqliteParameter output_parm = new SqliteParameter("@output", output);
                    output_parm.DbType = DbType.String;

                    string sql = @"
                    UPDATE Outputs
                    SET Priority = Priority + 1
                    WHERE Input = @input AND Output = @output";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(input_parm);
                    cmd.Parameters.Add(output_parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Increase_OutputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Increase_OutputPriorities(string output)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(output))
                {
                    SqliteParameter output_parm = new SqliteParameter("@output", output);
                    output_parm.DbType = DbType.String;

                    string sql = @"
                    UPDATE Outputs
                    SET Priority = Priority + 1
                    WHERE Output = @output";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(output_parm);
                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Increase_OutputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<SqliteCommand>> Decrease_OutputPriorities(string output)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(output))
                {
                    SqliteParameter output_parm = new SqliteParameter("@output", output);
                    output_parm.DbType = DbType.String;

                    string sql = @"
                    UPDATE Outputs
                    SET Priority = Priority - 1
                    WHERE Output = @output";

                    SqliteCommand cmd = new SqliteCommand(sql);
                    cmd.Parameters.Add(output_parm);
                    commands.Add(cmd);
                }

                string delete_sql = @"DELETE FROM Outputs WHERE Priority < 1";
                SqliteCommand delete_cmd = new SqliteCommand(delete_sql);
                commands.Add(delete_cmd);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Decrease_OutputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<string[]> Get_OutputsFromInput(string input)
        {
            List<string> outputs = new List<string>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteParameter input_parm = new SqliteParameter("@input", input);
                    input_parm.DbType = DbType.String;

                    SqliteParameter[] parms = { input_parm };
                    DataTable data = await GetData("SELECT Output FROM Outputs WHERE Input = @input", parms);

                    int total = data.Rows.Count;

                    foreach (DataRow row in data.Rows)
                    {
                        outputs.Add(row.ItemArray[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Get_OutputsFromInput", ex.Message, ex.StackTrace);
            }

            return outputs.ToArray();
        }

        public static async Task<bool> Output_InputHasWord(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    List<SqliteParameter> parameters = new List<SqliteParameter>();

                    SqliteParameter parm = new SqliteParameter("@word", word);
                    parm.DbType = DbType.String;
                    parameters.Add(parm);

                    DataTable data = await GetData("SELECT Input FROM Outputs WHERE INSTR(Input, @word) > 0", parameters.ToArray());
                    return data.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Output_InputHasWord", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static async Task<bool> Output_OutputHasWord(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    List<SqliteParameter> parameters = new List<SqliteParameter>();

                    SqliteParameter parm = new SqliteParameter("@word", word);
                    parm.DbType = DbType.String;
                    parameters.Add(parm);

                    DataTable data = await GetData("SELECT Output FROM Outputs WHERE INSTR(Output, @word) > 0", parameters.ToArray());
                    return data.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.Output_OutputHasWord", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static async Task<string> WordArray_To_String(string[] words)
        {
            string wordSet = "";

            try
            {
                for (int i = 0; i < words.Length; i++)
                {
                    string word = "";

                    //Check word for apostrophe which will break queries
                    string check_word = words[i];
                    for (int j = 0; j < check_word.Length; j++)
                    {
                        char c = check_word[j];
                        if (c == '\'')
                        {
                            word += c;
                            word += '\'';
                        }
                        else
                        {
                            word += c;
                        }
                    }

                    wordSet += "'" + word + "'";
                    if (i < words.Length - 1)
                    {
                        wordSet += ",";
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                Logger.AddLog("SqlUtil.WordArray_To_String", ex.Message, ex.StackTrace);
            }

            return wordSet;
        }
    }
}
