using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Controls;
using RealAI.Pages;

namespace RealAI.Util
{
    public static class SQLUtil
    {
        public static string BrainFile;
        public static List<string> BrainList = new List<string>();

        public static SqliteConnection GetConnection(string fileName)
        {
            try
            {
                string path = AppUtil.GetBrainFile(fileName + ".brain");
                return new SqliteConnection("Data Source=" + path);
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.GetSqlConnection", ex.Message, ex.StackTrace);
            }

            return null;
        }

        public static string NewBrain(string fileName)
        {
            try
            {
                List<SqliteCommand> commands = new List<SqliteCommand>()
                {
                    new SqliteCommand
                    {
                        CommandText = @"
                            CREATE TABLE Inputs
                            (
                                ID INTEGER PRIMARY KEY,
                                Input TEXT,
                                Priority INTEGER DEFAULT 1
                            )"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"CREATE INDEX idx_Inputs ON Inputs (input)"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"
                            CREATE TABLE Words
                            (
                                ID INTEGER PRIMARY KEY,
                                Word TEXT,
                                Priority INTEGER DEFAULT 1
                            )"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"CREATE INDEX idx_Words ON Words (Word)"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"
                            CREATE TABLE Outputs
                            (
                                ID INTEGER PRIMARY KEY,
                                Input TEXT,
                                Output TEXT,
                                Priority INTEGER DEFAULT 1
                            )"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"CREATE INDEX idx_Outputs ON Outputs (Input, Output)"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"
                            CREATE TABLE Topics
                            (
                                ID INTEGER PRIMARY KEY,
                                Input TEXT,
                                Topic TEXT,
                                Priority INTEGER DEFAULT 1
                            )"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"CREATE INDEX idx_Topics ON Topics (Input, Topic)"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"
                            CREATE TABLE PreWords
                            (
                                ID INTEGER PRIMARY KEY,
                                Word TEXT,
                                PreWord TEXT,
                                Priority INTEGER DEFAULT 1,
                                Distance INTEGER
                            )"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"CREATE INDEX idx_PreWords ON PreWords (Word, PreWord, Distance)"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"
                            CREATE TABLE ProWords
                            (
                                ID INTEGER PRIMARY KEY,
                                Word TEXT,
                                ProWord TEXT,
                                Priority INTEGER DEFAULT 1,
                                Distance INTEGER
                            )"
                    },
                    new SqliteCommand
                    {
                        CommandText = @"CREATE INDEX idx_ProWords ON ProWords (Word, ProWord, Distance)"
                    },
                };

                bool success = BulkExecute(commands, fileName);
                if (success)
                {
                    BrainFile = fileName;
                    BrainList.Add(fileName);
                    string file = AppUtil.GetInternalPath("BrainList.txt");
                    File.WriteAllLines(file, BrainList);

                    return "SUCCESS";
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SQLUtil.NewBrain", ex.Message, ex.StackTrace);
                return ex.Message;
            }

            return "Failed to create '" + fileName + "' brain.";
        }

        public static string DeleteBrain(string fileName)
        {
            try
            {
                string file = AppUtil.GetBrainFile(fileName + ".brain");
                if (File.Exists(file))
                {
                    File.Delete(file);
                    BrainList.Remove(fileName);
                    AppUtil.SaveBrainList();

                    string historyFolder = AppUtil.GetHistoryPath(fileName);
                    if (Directory.Exists(historyFolder))
                    {
                        Directory.Delete(historyFolder, true);
                    }

                    if (BrainFile == fileName)
                    {
                        BrainFile = null;

                        if (Brains.lb_LoadedBrain != null)
                        {
                            Brains.lb_LoadedBrain.Text = "No Brain Loaded";
                        }
                        
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
                Logger.AddLog("SQLUtil.NewBrain", ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        public static bool BulkExecute(List<SqliteCommand> commands)
        {
            try
            {
                if (!string.IsNullOrEmpty(BrainFile))
                {
                    SqliteConnection connection = GetConnection(BrainFile);
                    using (SqliteConnection con = new SqliteConnection(connection.ConnectionString))
                    {
                        con.Open();

                        using (SqliteTransaction transaction = con.BeginTransaction())
                        {
                            int commandCount = commands.Count;
                            for (int c = 0; c < commandCount; c++)
                            {
                                SqliteCommand command = commands[c];

                                using (SqliteCommand cmd = new SqliteCommand(command.CommandText, con, transaction))
                                {
                                    int parameterCount = command.Parameters.Count;
                                    for (int p = 0; p < parameterCount; p++)
                                    {
                                        SqliteParameter existing = command.Parameters[p];

                                        cmd.Parameters.Add(new SqliteParameter
                                        {
                                            ParameterName = existing.ParameterName,
                                            Value = existing.Value,
                                            DbType = existing.DbType
                                        });
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
                Logger.AddLog("SqlUtil.BulkQuery", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static bool BulkExecute(List<SqliteCommand> commands, string fileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    SqliteConnection connection = GetConnection(fileName);
                    using (SqliteConnection con = new SqliteConnection(connection.ConnectionString))
                    {
                        con.Open();

                        using (SqliteTransaction transaction = con.BeginTransaction())
                        {
                            int commandCount = commands.Count;
                            for (int c = 0; c < commandCount; c++)
                            {
                                SqliteCommand command = commands[c];

                                using (SqliteCommand cmd = new SqliteCommand(command.CommandText, con, transaction))
                                {
                                    int parameterCount = command.Parameters.Count;
                                    for (int p = 0; p < parameterCount; p++)
                                    {
                                        SqliteParameter existing = command.Parameters[p];

                                        cmd.Parameters.Add(new SqliteParameter
                                        {
                                            ParameterName = existing.ParameterName,
                                            Value = existing.Value,
                                            DbType = existing.DbType
                                        });
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
                Logger.AddLog("SqlUtil.BulkQuery", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static bool BulkExecute(List<SqliteCommand> commands, string stepMessage, ProgressBar progressBar, Label progressLabel, Label progressStep, DateTime progressStart, CancellationToken cancelToken)
        {
            try
            {
                double count = 0;
                double total = commands.Count;

                AppUtil.UpdateProgress(progressBar, 0, progressLabel, progressStart);

                if (!string.IsNullOrEmpty(BrainFile))
                {
                    SqliteConnection connection = GetConnection(BrainFile);
                    using (SqliteConnection con = new SqliteConnection(connection.ConnectionString))
                    {
                        con.Open();

                        using (SqliteTransaction transaction = con.BeginTransaction())
                        {
                            for (int i = 0; i < total; i++)
                            {
                                if (cancelToken.IsCancellationRequested)
                                {
                                    break;
                                }

                                MainThread.InvokeOnMainThreadAsync(() =>
                                {
                                    progressStep.Text = stepMessage + " (" + count + "/" + total + ")";
                                });

                                SqliteCommand command = commands[i];

                                using (SqliteCommand cmd = new SqliteCommand(command.CommandText, con, transaction))
                                {
                                    int parameterCount = command.Parameters.Count;
                                    for (int p = 0; p < parameterCount; p++)
                                    {
                                        SqliteParameter existing = command.Parameters[p];

                                        cmd.Parameters.Add(new SqliteParameter
                                        {
                                            ParameterName = existing.ParameterName,
                                            Value = existing.Value,
                                            DbType = existing.DbType
                                        });
                                    }

                                    cmd.ExecuteNonQuery();
                                }

                                count++;
                                AppUtil.UpdateProgress(progressBar, count / total, progressLabel, progressStart);
                                Thread.Sleep(10);
                            }

                            transaction.Commit();
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.BulkQuery", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static bool Execute(SqliteCommand command, string fileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    SqliteConnection connection = GetConnection(fileName);
                    using (SqliteConnection con = new SqliteConnection(connection.ConnectionString))
                    {
                        con.Open();

                        using (SqliteTransaction transaction = con.BeginTransaction())
                        {
                            using (SqliteCommand cmd = new SqliteCommand(command.CommandText, con, transaction))
                            {
                                int parameterCount = command.Parameters.Count;
                                for (int p = 0; p < parameterCount; p++)
                                {
                                    SqliteParameter existing = command.Parameters[p];

                                    cmd.Parameters.Add(new SqliteParameter
                                    {
                                        ParameterName = existing.ParameterName,
                                        Value = existing.Value,
                                        DbType = existing.DbType
                                    });
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
                Logger.AddLog("SqlUtil.Execute", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static DataTable GetData(string sql, SqliteParameter[] parameters)
        {
            DataTable data = new DataTable();

            try
            {
                if (sql.Any())
                {
                    SqliteConnection connection = GetConnection(BrainFile);
                    using (SqliteConnection con = new SqliteConnection(connection.ConnectionString))
                    {
                        con.Open();

                        using (SqliteCommand cmd = new SqliteCommand(sql, con))
                        {
                            if (parameters != null)
                            {
                                int parameterCount = parameters.Length;
                                for (int i = 0; i < parameterCount; i++)
                                {
                                    SqliteParameter parameter = parameters[i];
                                    if (parameter != null)
                                    {
                                        cmd.Parameters.Add(parameter);
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
                Logger.AddLog("SqlUtil.GetData", ex.Message, ex.StackTrace);
            }

            return data;
        }

        public static DataTable GetData(string sql, SqliteParameter[] parameters, string fileName)
        {
            DataTable data = new DataTable();

            try
            {
                if (sql.Any())
                {
                    SqliteConnection connection = GetConnection(fileName);
                    using (SqliteConnection con = new SqliteConnection(connection.ConnectionString))
                    {
                        con.Open();

                        using (SqliteCommand cmd = new SqliteCommand(sql, con))
                        {
                            if (parameters != null)
                            {
                                int parameterCount = parameters.Length;
                                for (int i = 0; i < parameterCount; i++)
                                {
                                    SqliteParameter parameter = parameters[i];
                                    if (parameter != null)
                                    {
                                        cmd.Parameters.Add(parameter);
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
                Logger.AddLog("SqlUtil.GetData", ex.Message, ex.StackTrace);
            }

            return data;
        }

        public static List<SqliteCommand> Wipe()
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                string[] tables = { "Outputs", "Topics", "PreWords", "ProWords", "Inputs", "Words" };

                int count = tables.Length;
                for (int i = 0; i < count; i++)
                {
                    commands.Add(new SqliteCommand("DELETE FROM " + tables[i]));
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Wipe", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<Input>> Get_Inputs(string fileName)
        {
            List<Input> inputs = new List<Input>();

            try
            {
                DataTable data = GetData("SELECT ID, Input, Priority FROM Inputs", null, fileName);

                int rowCount = data.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = data.Rows[i];

                    inputs.Add(new Input
                    {
                        id = int.Parse(row.ItemArray[0].ToString()),
                        input = row.ItemArray[1].ToString(),
                        priority = int.Parse(row.ItemArray[2].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                await Logger.AddLog("SqlUtil.Get_Inputs", ex.Message, ex.StackTrace);
            }

            return inputs;
        }

        public static int Get_InputPriority(string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    List<SqliteParameter> parameters = new List<SqliteParameter>()
                    {
                        new SqliteParameter
                        {
                            ParameterName = "@input",
                            Value = input,
                            DbType = DbType.String
                        }
                    };

                    DataTable data = GetData("SELECT Priority FROM Inputs WHERE Input = @input", parameters.ToArray());
                    if (data.Rows.Count > 0)
                    {
                        string result = data.Rows[0].ItemArray[0] as string;
                        if (!string.IsNullOrEmpty(result))
                        {
                            return int.Parse(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_InputPriority", ex.Message, ex.StackTrace);
            }

            return 0;
        }

        public static bool InputHasWord(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    SqliteParameter[] parms =
                    {
                        new SqliteParameter
                        {
                            ParameterName = "@word",
                            Value = word,
                            DbType = DbType.String
                        }
                    };

                    DataTable data = GetData("SELECT Input FROM Inputs WHERE INSTR(Input, @word) > 0", parms);
                    return data.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.InputHasWord", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static List<SqliteCommand> Add_NewInput(string input)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteCommand cmd = new SqliteCommand("INSERT INTO Inputs (Input) SELECT @input WHERE NOT EXISTS (SELECT 1 FROM Inputs WHERE Input = @input)");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@input",
                        Value = input,
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Add_NewInput", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Increase_InputPriorities(string input)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteCommand cmd = new SqliteCommand("UPDATE Inputs SET Priority = Priority + 1 WHERE Input = @input");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@input",
                        Value = input,
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Increase_InputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Decrease_InputPriorities(string input)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteCommand cmd = new SqliteCommand("UPDATE Inputs SET Priority = Priority - 1 WHERE Input = @input");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@input",
                        Value = input,
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                    commands.Add(new SqliteCommand("DELETE FROM Inputs WHERE Priority < 1"));
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Decrease_InputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<Word>> Get_Words(string fileName)
        {
            List<Word> words = new List<Word>();

            try
            {
                DataTable data = GetData("SELECT ID, Word, Priority FROM Words", null, fileName);

                int rowCount = data.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = data.Rows[i];

                    words.Add(new Word
                    {
                        id = int.Parse(row.ItemArray[0].ToString()),
                        word = row.ItemArray[1].ToString(),
                        priority = int.Parse(row.ItemArray[2].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                await Logger.AddLog("SqlUtil.Get_Words", ex.Message, ex.StackTrace);
            }

            return words;
        }

        public static string Get_RandomWord()
        {
            string word = "";

            try
            {
                DataTable data = GetData("SELECT Word FROM Words ORDER BY RANDOM() LIMIT 1", null);

                if (data.Rows.Count > 0)
                {
                    word = data.Rows[0].ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_RandomWord", ex.Message, ex.StackTrace);
            }

            return word;
        }

        public static string Get_MinWord(string[] words)
        {
            string word = "";

            try
            {
                if (words.Any())
                {
                    List<string> min_words = new List<string>();

                    int priority = Get_MinPriority_Words(words);

                    string wordSet = WordArray_To_String(words);

                    SqliteParameter[] parms =
                    { 
                        new SqliteParameter
                        {
                            ParameterName = "@priority",
                            Value = priority,
                            DbType = DbType.Int32
                        }
                    };

                    DataTable data = GetData("SELECT Word FROM Words WHERE Priority = @priority AND Word IN (" + wordSet + ")", parms);

                    int count = data.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string min_word = data.Rows[i].ItemArray[0] as string;
                        if (!string.IsNullOrEmpty(min_word))
                        {
                            min_words.Add(min_word);
                        }
                    }

                    int minWordCount = min_words.Count;
                    if (minWordCount > 0)
                    {
                        CryptoRandom random = new CryptoRandom();
                        int choice = random.Next(0, minWordCount);
                        word = min_words[choice];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_MinWord", ex.Message, ex.StackTrace);
            }

            return word;
        }

        public static string[] Get_MinWords(string[] words)
        {
            List<string> min_words = new List<string>();

            try
            {
                if (words.Any())
                {
                    int priority = Get_MinPriority_Words(words);

                    string wordSet = WordArray_To_String(words);

                    SqliteParameter[] parms =
                    {
                        new SqliteParameter
                        {
                            ParameterName = "@priority",
                            Value = priority,
                            DbType = DbType.Int32
                        }
                    };

                    DataTable data = GetData("SELECT Word FROM Words WHERE Priority = @priority AND Word IN (" + wordSet + ")", parms);

                    int count = data.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string word = data.Rows[i].ItemArray[0] as string;
                        if (!string.IsNullOrEmpty(word))
                        {
                            min_words.Add(word);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_MinWords", ex.Message, ex.StackTrace);
            }

            return min_words.ToArray();
        }

        public static int Get_MinPriority_Words(string[] words)
        {
            try
            {
                if (words.Any())
                {
                    string wordSet = WordArray_To_String(words);

                    DataTable data = GetData("SELECT MIN(Priority) AS MinPriority FROM Words WHERE Word IN (" + wordSet + ")", null);

                    if (data.Rows.Count > 0 &&
                        data.Rows[0].ItemArray.Length > 0)
                    {
                        string result = data.Rows[0].ItemArray[0] as string;
                        if (!string.IsNullOrEmpty(result))
                        {
                            return int.Parse(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_MinPriority_Words", ex.Message, ex.StackTrace);
            }

            return 0;
        }

        public static List<SqliteCommand> Add_Words(string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                int count = words.Length;
                for (int i = 0; i < count; i++)
                {
                    SqliteCommand cmd = new SqliteCommand("INSERT INTO Words (Word) SELECT @word WHERE NOT EXISTS (SELECT 1 FROM Words WHERE Word = @word)");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = words[i],
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Add_Words", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static int Get_Word_Priority(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    SqliteParameter[] parms =
                    {
                        new SqliteParameter
                        {
                            ParameterName = "@word",
                            Value = word,
                            DbType = DbType.String
                        }
                    };

                    DataTable data = GetData("SELECT Priority FROM Words WHERE Word = @word", parms);

                    if (data.Rows.Count > 0 &&
                        data.Rows[0].ItemArray.Length > 0)
                    {
                        string result = data.Rows[0].ItemArray[0] as string;
                        if (!string.IsNullOrEmpty(result))
                        {
                            return int.Parse(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_Word_Priority", ex.Message, ex.StackTrace);
            }

            return 0;
        }

        public static List<SqliteCommand> Increase_WordPriorities(string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                int count = words.Length;
                for (int i = 0; i < count; i++)
                {
                    SqliteCommand cmd = new SqliteCommand("UPDATE Words SET Priority = Priority + 1 WHERE Word = @word");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = words[i],
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Increase_WordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Decrease_WordPriorities(string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                int count = words.Length;
                for (int i = 0; i < count; i++)
                {
                    SqliteCommand cmd = new SqliteCommand("UPDATE Words SET Priority = Priority - 1 WHERE Word = @word");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = words[i],
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                    commands.Add(new SqliteCommand("DELETE FROM Words WHERE Priority < 1"));
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Decrease_WordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<PreWord>> Get_PreWords(string fileName)
        {
            List<PreWord> pre_words = new List<PreWord>();

            try
            {
                DataTable data = GetData("SELECT ID, Word, PreWord, Priority, Distance FROM PreWords", null, fileName);

                int rowCount = data.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = data.Rows[i];

                    pre_words.Add(new PreWord
                    {
                        id = int.Parse(row.ItemArray[0].ToString()),
                        word = row.ItemArray[1].ToString(),
                        preWord = row.ItemArray[2].ToString(),
                        priority = int.Parse(row.ItemArray[3].ToString()),
                        distance = int.Parse(row.ItemArray[4].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                await Logger.AddLog("SqlUtil.Get_PreWords", ex.Message, ex.StackTrace);
            }

            return pre_words;
        }

        public static string Get_PreWord(List<string> words, bool for_thinking)
        {
            string result = "";

            try
            {
                if (words.Any())
                {
                    Dictionary<string, int> options = new Dictionary<string, int>();
                    List<DataRow> rows = new List<DataRow>();

                    int count = 1;
                    int wordsCount = words.Count;

                    for (int i = 0; i < wordsCount; i++)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        SqliteParameter[] parms =
                        {
                            new SqliteParameter
                            {
                                ParameterName = "@word",
                                Value = words[i],
                                DbType = DbType.String
                            },
                            new SqliteParameter
                            {
                                ParameterName = "@distance",
                                Value = count,
                                DbType = DbType.Int32
                            }
                        };

                        DataTable data = GetData("SELECT * FROM PreWords WHERE Word = @word AND Distance = @distance", parms);

                        int rowCount = data.Rows.Count;
                        for (int r = 0; r < rowCount; r++)
                        {
                            if (for_thinking &&
                                !Options.CanThink)
                            {
                                break;
                            }

                            rows.Add(data.Rows[r]);
                        }

                        count++;
                    }

                    int rowCount2 = rows.Count;
                    for (int r2 = 0; r2 < rowCount2; r2++)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        DataRow row = rows[r2];

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
                            if (options.Any() &&
                                options.ContainsKey(pre_word))
                            {
                                options[pre_word]++;
                            }
                        }
                    }

                    //Get max priority from options
                    int optionCount = options.Count;
                    if (optionCount > 0)
                    {
                        int max = options.Values.Max();

                        //Randomly select one with bias towards max priority
                        for (int o = 0; o < optionCount; o++)
                        {
                            if (for_thinking &&
                                !Options.CanThink)
                            {
                                break;
                            }

                            var set = options.ElementAt(o);
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
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_PreWord", ex.Message, ex.StackTrace);
            }

            return result;
        }

        public static List<SqliteCommand> Add_PreWords(string[] words, string[] prewords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                int wordsCount = words.Length;
                for (int i = 0; i < wordsCount; i++)
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        INSERT INTO PreWords (Word, PreWord, Distance)
                        SELECT @word, @pre_word, @distance
                        WHERE NOT EXISTS (SELECT 1 FROM PreWords WHERE Word = @word AND PreWord = @pre_word AND Distance = @distance)
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = words[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@pre_word",
                        Value = prewords[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@distance",
                        Value = distances[i],
                        DbType = DbType.Int32
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Add_PreWords", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Increase_PreWordPriorities(string[] words, string[] prewords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                int wordsCount = words.Length;
                for (int i = 0; i < wordsCount; i++)
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        UPDATE PreWords
                        SET Priority = Priority + 1
                        WHERE Word = @word AND PreWord = @pre_word AND Distance = @distance
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = words[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@pre_word",
                        Value = prewords[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@distance",
                        Value = distances[i],
                        DbType = DbType.Int32
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Increase_PreWordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Decrease_PreWordPriorities(string[] words, string[] prewords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                int wordsCount = words.Length;
                for (int i = 0; i < wordsCount; i++)
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        UPDATE PreWords
                        SET Priority = Priority - 1
                        WHERE Word = @word AND PreWord = @pre_word AND Distance = @distance
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = words[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@pre_word",
                        Value = prewords[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@distance",
                        Value = distances[i],
                        DbType = DbType.Int32
                    });

                    commands.Add(cmd);
                }

                commands.Add(new SqliteCommand(@"DELETE FROM PreWords WHERE Priority < 1"));
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Decrease_PreWordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<ProWord>> Get_ProWords(string fileName)
        {
            List<ProWord> pro_words = new List<ProWord>();

            try
            {
                DataTable data = GetData("SELECT ID, Word, ProWord, Priority, Distance FROM ProWords", null, fileName);

                int rowCount = data.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = data.Rows[i];

                    pro_words.Add(new ProWord
                    {
                        id = int.Parse(row.ItemArray[0].ToString()),
                        word = row.ItemArray[1].ToString(),
                        proWord = row.ItemArray[2].ToString(),
                        priority = int.Parse(row.ItemArray[3].ToString()),
                        distance = int.Parse(row.ItemArray[4].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                await Logger.AddLog("SqlUtil.Get_ProWords", ex.Message, ex.StackTrace);
            }

            return pro_words;
        }

        public static string Get_ProWord(List<string> words, bool for_thinking)
        {
            string result = "";

            try
            {
                if (words.Any())
                {
                    Dictionary<string, int> options = new Dictionary<string, int>();
                    List<DataRow> rows = new List<DataRow>();

                    int count = 1;
                    int wordsCount = words.Count;

                    for (int i = wordsCount - 1; i >= 0; i--)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        SqliteParameter[] parms =
                        {
                            new SqliteParameter
                            {
                                ParameterName = "@word",
                                Value = words[i],
                                DbType = DbType.String
                            },
                            new SqliteParameter
                            {
                                ParameterName = "@distance",
                                Value = count,
                                DbType = DbType.Int32
                            }
                        };

                        DataTable data = GetData("SELECT * FROM ProWords WHERE Word = @word AND Distance = @distance", parms);

                        int rowCount = data.Rows.Count;
                        for (int r = 0; r < rowCount; r++)
                        {
                            if (for_thinking &&
                                !Options.CanThink)
                            {
                                break;
                            }

                            rows.Add(data.Rows[r]);
                        }

                        count++;
                    }

                    int rowCount2 = rows.Count;
                    for (int r2 = 0; r2 < rowCount2; r2++)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        DataRow row = rows[r2];

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
                    int optionCount = options.Count;
                    if (optionCount > 0)
                    {
                        int max = options.Values.Max();

                        //Randomly select one with bias towards max priority
                        for (int o = 0; o < optionCount; o++)
                        {
                            if (for_thinking &&
                                !Options.CanThink)
                            {
                                break;
                            }

                            var set = options.ElementAt(o);
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
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_ProWord", ex.Message, ex.StackTrace);
            }

            return result;
        }

        public static List<SqliteCommand> Add_ProWords(string[] words, string[] prowords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                int wordsCount = words.Length;
                for (int i = 0; i < wordsCount; i++)
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        INSERT INTO ProWords (Word, ProWord, Distance)
                        SELECT @word, @pro_word, @distance
                        WHERE NOT EXISTS (SELECT 1 FROM ProWords WHERE Word = @word AND ProWord = @pro_word AND Distance = @distance)
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = words[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@pro_word",
                        Value = prowords[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@distance",
                        Value = distances[i],
                        DbType = DbType.Int32
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Add_ProWords", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Increase_ProWordPriorities(string[] words, string[] prowords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                int wordsCount = words.Length;
                for (int i = 0; i < wordsCount; i++)
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        UPDATE ProWords
                        SET Priority = Priority + 1
                        WHERE Word = @word AND ProWord = @pro_word AND Distance = @distance
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = words[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@pro_word",
                        Value = prowords[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@distance",
                        Value = distances[i],
                        DbType = DbType.Int32
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Increase_ProWordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Decrease_ProWordPriorities(string[] words, string[] prowords, int[] distances)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                int wordsCount = words.Length;
                for (int i = 0; i < wordsCount; i++)
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        UPDATE ProWords
                        SET Priority = Priority - 1
                        WHERE Word = @word AND ProWord = @pro_word AND Distance = @distance
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@word",
                        Value = words[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@pro_word",
                        Value = prowords[i],
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@distance",
                        Value = distances[i],
                        DbType = DbType.Int32
                    });

                    commands.Add(cmd);
                }

                commands.Add(new SqliteCommand(@"DELETE FROM ProWords WHERE Priority < 1"));
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Decrease_ProWordPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static async Task<List<Topic>> Get_Topics(string fileName)
        {
            List<Topic> topics = new List<Topic>();

            try
            {
                DataTable data = GetData("SELECT ID, Input, Topic, Priority FROM Topics", null, fileName);

                int rowCount = data.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = data.Rows[i];

                    topics.Add(new Topic
                    {
                        id = int.Parse(row.ItemArray[0].ToString()),
                        input = row.ItemArray[1].ToString(),
                        topic = row.ItemArray[2].ToString(),
                        priority = int.Parse(row.ItemArray[3].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                await Logger.AddLog("SqlUtil.Get_Topics", ex.Message, ex.StackTrace);
            }

            return topics;
        }

        public static List<SqliteCommand> AddTopics(string input, string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    int wordsCount = words.Length;
                    for (int i = 0; i < wordsCount; i++)
                    {
                        SqliteCommand cmd = new SqliteCommand(@"
                            INSERT INTO Topics (Input, Topic) 
                            SELECT @input, @word 
                            WHERE NOT EXISTS (SELECT 1 FROM Topics WHERE Input = @input AND Topic = @word)
                        ");

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "@input",
                            Value = input,
                            DbType = DbType.String
                        });

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "@word",
                            Value = words[i],
                            DbType = DbType.String
                        });

                        commands.Add(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.AddTopics", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Increase_TopicPriorities(string input, string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    int wordsCount = words.Length;
                    for (int i = 0; i < wordsCount; i++)
                    {
                        SqliteCommand cmd = new SqliteCommand("UPDATE Topics SET Priority = Priority + 1 WHERE Input = @input AND Topic = @word");

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "@input",
                            Value = input,
                            DbType = DbType.String
                        });

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "@word",
                            Value = words[i],
                            DbType = DbType.String
                        });

                        commands.Add(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Increase_TopicPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Decrease_TopicPriorities(string input, string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    int wordsCount = words.Length;
                    for (int i = 0; i < wordsCount; i++)
                    {
                        SqliteCommand cmd = new SqliteCommand("UPDATE Topics SET Priority = Priority - 1 WHERE Input = @input AND Topic = @word");

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "@input",
                            Value = input,
                            DbType = DbType.String
                        });

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "@word",
                            Value = words[i],
                            DbType = DbType.String
                        });

                        commands.Add(cmd);
                    }
                }

                commands.Add(new SqliteCommand(@"DELETE FROM Topics WHERE Priority < 1"));
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Decrease_TopicPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Decrease_Topics_Unmatched(string input, string[] words)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input) &&
                    words.Any())
                {
                    string word_string = WordArray_To_String(words);
                    SqliteCommand cmd = new SqliteCommand("UPDATE Topics SET Priority = Priority - 1 WHERE Input = @input AND Topic NOT IN (" + word_string + ")");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@input",
                        Value = input,
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Decrease_Topics_Unmatched", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Clean_Topics(string input)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteCommand cmd = new SqliteCommand("DELETE FROM Topics WHERE Input = @input AND Priority < 1");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@input",
                        Value = input,
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Clean_Topics", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static bool Topics_InputHasWord(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    SqliteParameter[] parms =
                    {
                        new SqliteParameter
                        {
                            ParameterName = "@word",
                            Value = word,
                            DbType = DbType.String
                        }
                    };

                    DataTable data = GetData("SELECT Input FROM Topics WHERE INSTR(Input, @word) > 0", parms);
                    return data.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Topics_InputHasWord", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static string[] Get_OutputsFromTopics(string[] words)
        {
            List<string> outputs = new List<string>();

            try
            {
                if (words.Length > 0)
                {
                    string wordSet = WordArray_To_String(words);

                    //Get all the inputs that contain a matching topic from the wordset
                    DataTable inputsData = GetData("SELECT DISTINCT Input FROM Topics WHERE Topic IN (" + wordSet + ")", null);

                    int total = inputsData.Rows.Count;

                    Dictionary<string, int> output_priority = new Dictionary<string, int>();
                    for (int i = 0; i < total; i++)
                    {
                        string input = inputsData.Rows[i].ItemArray[0].ToString();

                        SqliteParameter[] parms =
                        {
                            new SqliteParameter
                            {
                                ParameterName = "@input",
                                Value = input,
                                DbType = DbType.String
                            }
                        };

                        //Get all the outputs for the given input
                        DataTable outputData = GetData("SELECT DISTINCT Output FROM Outputs WHERE Input = @input", parms);
                        if (outputData.Rows.Count > 0)
                        {
                            string output = outputData.Rows[0].ItemArray[0] as string;
                            if (!string.IsNullOrEmpty(output))
                            {
                                //Get the priority of the output from the Input table
                                int priority = Get_InputPriority(output);

                                //Add output/priority pair as a possibility
                                if (!output_priority.ContainsKey(output))
                                {
                                    output_priority.Add(output, priority);
                                }
                            }
                        }
                    }

                    //Get the highest priority output
                    if (output_priority.Any())
                    {
                        int max_priority = output_priority.Values.Max();
                        Dictionary<string, int> maxOutputs = output_priority.Where(x => x.Value >= max_priority).ToDictionary(dict => dict.Key, dict => dict.Value);
                        if (maxOutputs.Any())
                        {
                            outputs.AddRange(maxOutputs.Keys);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_OutputsFromTopics", ex.Message, ex.StackTrace);
            }

            return outputs.ToArray();
        }

        public static async Task<List<Output>> Get_Outputs(string fileName)
        {
            List<Output> outputs = new List<Output>();

            try
            {
                DataTable data = GetData("SELECT ID, Input, Output, Priority FROM Outputs", null, fileName);

                int rowCount = data.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = data.Rows[i];

                    outputs.Add(new Output
                    {
                        id = int.Parse(row.ItemArray[0].ToString()),
                        input = row.ItemArray[1].ToString(),
                        output = row.ItemArray[2].ToString(),
                        priority = int.Parse(row.ItemArray[3].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                await Logger.AddLog("SqlUtil.Get_Outputs", ex.Message, ex.StackTrace);
            }

            return outputs;
        }

        public static List<SqliteCommand> Add_Outputs(string input, string output)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input) &&
                    !string.IsNullOrEmpty(output))
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        INSERT INTO Outputs (Input, Output)
                        SELECT @input, @output
                        WHERE NOT EXISTS (SELECT 1 FROM Outputs WHERE Input = @input AND Output = @output)
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@input",
                        Value = input,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@output",
                        Value = output,
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Add_Outputs", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Increase_OutputPriorities(string input, string output)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(input) &&
                    !string.IsNullOrEmpty(output))
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        UPDATE Outputs
                        SET Priority = Priority + 1
                        WHERE Input = @input AND Output = @output
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@input",
                        Value = input,
                        DbType = DbType.String
                    });

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@output",
                        Value = output,
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Increase_OutputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Increase_OutputPriorities(string output)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(output))
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        UPDATE Outputs
                        SET Priority = Priority + 1
                        WHERE Output = @output
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@output",
                        Value = output,
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Increase_OutputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> Decrease_OutputPriorities(string output)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                if (!string.IsNullOrEmpty(output))
                {
                    SqliteCommand cmd = new SqliteCommand(@"
                        UPDATE Outputs
                        SET Priority = Priority - 1
                        WHERE Output = @output
                    ");

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "@output",
                        Value = output,
                        DbType = DbType.String
                    });

                    commands.Add(cmd);
                }

                commands.Add(new SqliteCommand(@"DELETE FROM Outputs WHERE Priority < 1"));
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Decrease_OutputPriorities", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static string[] Get_OutputsFromInput(string input)
        {
            List<string> outputs = new List<string>();

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    SqliteParameter[] parms =
                    {
                        new SqliteParameter
                        {
                            ParameterName = "@input",
                            Value = input,
                            DbType = DbType.String
                        }
                    };

                    DataTable data = GetData("SELECT Output FROM Outputs WHERE Input = @input", parms);

                    int rowCount = data.Rows.Count;
                    for (int i = 0; i < rowCount; i++)
                    {
                        string output = data.Rows[i].ItemArray[0] as string;
                        if (!string.IsNullOrEmpty(output))
                        {
                            outputs.Add(output);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Get_OutputsFromInput", ex.Message, ex.StackTrace);
            }

            return outputs.ToArray();
        }

        public static bool Output_InputHasWord(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    SqliteParameter[] parms =
                    {
                        new SqliteParameter
                        {
                            ParameterName = "@word",
                            Value = word,
                            DbType = DbType.String
                        }
                    };

                    DataTable data = GetData("SELECT Input FROM Outputs WHERE INSTR(Input, @word) > 0", parms);
                    return data.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Output_InputHasWord", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static bool Output_OutputHasWord(string word)
        {
            try
            {
                if (!string.IsNullOrEmpty(word))
                {
                    SqliteParameter[] parms =
                    {
                        new SqliteParameter
                        {
                            ParameterName = "@word",
                            Value = word,
                            DbType = DbType.String
                        }
                    };

                    DataTable data = GetData("SELECT Output FROM Outputs WHERE INSTR(Output, @word) > 0", parms);
                    return data.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.Output_OutputHasWord", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static string WordArray_To_String(string[] words)
        {
            string wordSet = "";

            try
            {
                int wordsCount = words.Length;
                for (int i = 0; i < wordsCount; i++)
                {
                    string word = "";

                    //Check word for apostrophe which will break queries
                    string check_word = words[i];

                    int checkWordCount = check_word.Length;
                    for (int j = 0; j < checkWordCount; j++)
                    {
                        char c = check_word[j];
                        if (c == '\'')
                        {
                            //Add second apostrophe to escape it
                            word += c;
                            word += '\'';
                        }
                        else
                        {
                            word += c;
                        }
                    }

                    wordSet += "'" + word + "'";
                    if (i < wordsCount - 1)
                    {
                        wordSet += ",";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("SqlUtil.WordArray_To_String", ex.Message, ex.StackTrace);
            }

            return wordSet;
        }
    }
}
