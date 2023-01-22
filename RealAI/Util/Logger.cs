namespace RealAI.Util
{
    public static class Logger
    {
        #region Variables

        public static List<Log> Logs = new List<Log>();

        #endregion

        #region Methods

        public static void AddLog(string source, string message, string stack_trace)
        {
            Logs.Add(new Log(source, message, stack_trace));
        }

        public static void WriteLog()
        {
            if (Logs.Count > 0)
            {
                string filename = string.Format("{0}_{1:yyyy-MM-dd_hh-mm-ss}{2}", @"\log", DateTime.Now, ".txt");
                string path = Environment.CurrentDirectory + filename;

                string text = "";

                for (int i = 0; i < Logs.Count; i++)
                {
                    Log log = Logs[i];
                    text += "TimeStamp: " + log.TimeStamp.ToString() + Environment.NewLine;
                    text += "Source: " + log.Source + Environment.NewLine;
                    text += "Message: " + log.Message + Environment.NewLine;
                    text += "Stack Trace: " + log.StackTrace + Environment.NewLine;
                    text += Environment.NewLine;
                }

                File.WriteAllText(path, text);
            }
        }

        #endregion
    }

    public class Log
    {
        public DateTime TimeStamp;
        public string Source;
        public string Message;
        public string StackTrace;

        public Log(string source, string message, string stack_trace)
        {
            TimeStamp = DateTime.Now;
            Source = source;
            Message = message;
            StackTrace = stack_trace;
        }
    }
}
