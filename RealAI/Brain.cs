using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using RealAI.Util;
using RealAI.Pages;

namespace RealAI
{
    public static class Brain
    {
        #region Variables

        public static string Input;
        public static string Response;
        public static string LastResponse;
        public static string CleanInput;
        public static string[] WordArray;
        public static string[] Topics;

        public static string Thought;
        public static string LastThought;
        public static string[] WordArray_Thinking;

        public static bool GenerateAnotherResponse;

        #endregion

        #region Methods

        public static readonly Regex NormalCharacters = new Regex(@"^[\p{L}\p{M}\p{N}\s]+$");

        public static string GapSpecials(string old_string)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                int length = old_string.Length;
                for (var i = 0; i < length; i++)
                {
                    string value = old_string[i].ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (!NormalCharacters.IsMatch(value) &&
                            value != "'" &&
                            value != "’")
                        {
                            if (value == ".")
                            {
                                if (i > 0)
                                {
                                    if (old_string[i - 1] != '.')
                                    {
                                        sb.Append(" ");
                                    }

                                    sb.Append(value);
                                }
                                else
                                {
                                    sb.Append(value);
                                }

                                if (i < old_string.Length - 1)
                                {
                                    if (old_string[i + 1] != ' ' &&
                                        old_string[i + 1] != '.')
                                    {
                                        sb.Append(" ");
                                    }
                                }
                            }
                            else
                            {
                                sb.Append(" ");
                                sb.Append(value);

                                if (i < old_string.Length - 1)
                                {
                                    if (old_string[i + 1] != ' ')
                                    {
                                        sb.Append(" ");
                                    }
                                }
                            }
                        }
                        else if (value == "\r" ||
                                 value == "\n")
                        {

                        }
                        else
                        {
                            sb.Append(value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.GapSpecials", ex.Message, ex.StackTrace);
            }

            return sb.ToString();
        }

        public static string UnGapSpecials(string old_string)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                int length = old_string.Length - 1;
                for (int i = 0; i < length; i++)
                {
                    string value = old_string[i].ToString();
                    string next_value = old_string[i + 1].ToString();

                    if (!NormalCharacters.IsMatch(next_value) &&
                        next_value != ":" &&
                        value == " ")
                    {
                        //Skip spaces before special characters
                    }
                    else
                    {
                        sb.Append(value);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.UnGapSpecials", ex.Message, ex.StackTrace);
            }

            return sb.ToString();
        }

        private static void PrepInput()
        {
            try
            {
                CleanInput = RulesCheck(Input);
                string gapped = GapSpecials(CleanInput);
                WordArray = gapped.Trim(' ').Split(' ');

                if (!GenerateAnotherResponse &&
                    WordArray.Length > 0)
                {
                    List<SqliteCommand> commands = new List<SqliteCommand>();
                    commands.AddRange(AddInputs(CleanInput));
                    commands.AddRange(AddWords(WordArray));
                    commands.AddRange(AddPreWords(WordArray));
                    commands.AddRange(AddProWords(WordArray));
                    SQLUtil.BulkExecute(commands);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.PrepInput", ex.Message, ex.StackTrace);
            }
        }

        public static bool Encourage(string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    string gapped = GapSpecials(message);
                    string[] word_array = gapped.Trim(' ').Split(' ');
                    if (word_array.Length > 1)
                    {
                        List<string> words = new List<string>();
                        List<string> pre_words = new List<string>();
                        List<string> pro_words = new List<string>();
                        List<int> distances = new List<int>();
                        List<SqliteCommand> commands = new List<SqliteCommand>();

                        for (int i = 1; i < word_array.Length; i++)
                        {
                            int count = 1;
                            for (int j = i - 1; j >= 0; j--)
                            {
                                if (!string.IsNullOrEmpty(word_array[i]) &&
                                    !string.IsNullOrEmpty(word_array[j]))
                                {
                                    words.Add(word_array[i]);
                                    pre_words.Add(word_array[j]);
                                    distances.Add(count);
                                    count++;
                                }
                            }
                        }

                        commands.AddRange(SQLUtil.Increase_PreWordPriorities(words.ToArray(), pre_words.ToArray(), distances.ToArray()));

                        words = new List<string>();
                        distances = new List<int>();

                        for (int i = 0; i < word_array.Length - 1; i++)
                        {
                            var count = 1;
                            for (int j = i + 1; j <= word_array.Length - 1; j++)
                            {
                                if (!string.IsNullOrEmpty(word_array[i]) &&
                                    !string.IsNullOrEmpty(word_array[j]))
                                {
                                    words.Add(word_array[i]);
                                    pro_words.Add(word_array[j]);
                                    distances.Add(count);
                                    count++;
                                }
                            }
                        }

                        commands.AddRange(SQLUtil.Increase_ProWordPriorities(words.ToArray(), pro_words.ToArray(), distances.ToArray()));

                        commands.AddRange(SQLUtil.Increase_OutputPriorities(message));
                        commands.AddRange(SQLUtil.Increase_InputPriorities(message));
                        commands.AddRange(SQLUtil.Increase_WordPriorities(word_array));

                        return SQLUtil.BulkExecute(commands);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.Encourage", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static bool Discourage(string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    string gapped = GapSpecials(message);
                    string[] word_array = gapped.Trim(' ').Split(' ');
                    if (word_array.Length > 1)
                    {
                        List<string> words = new List<string>();
                        List<string> pre_words = new List<string>();
                        List<string> pro_words = new List<string>();
                        List<int> distances = new List<int>();
                        List<SqliteCommand> commands = new List<SqliteCommand>();

                        for (int i = 1; i < word_array.Length; i++)
                        {
                            int count = 1;
                            for (int j = i - 1; j >= 0; j--)
                            {
                                if (!string.IsNullOrEmpty(word_array[i]) &&
                                    !string.IsNullOrEmpty(word_array[j]))
                                {
                                    words.Add(word_array[i]);
                                    pre_words.Add(word_array[j]);
                                    distances.Add(count);
                                    count++;
                                }
                            }
                        }

                        commands.AddRange(SQLUtil.Decrease_PreWordPriorities(words.ToArray(), pre_words.ToArray(), distances.ToArray()));

                        words = new List<string>();
                        distances = new List<int>();

                        for (int i = 0; i < word_array.Length - 1; i++)
                        {
                            var count = 1;
                            for (int j = i + 1; j <= word_array.Length - 1; j++)
                            {
                                if (!string.IsNullOrEmpty(word_array[i]) &&
                                    !string.IsNullOrEmpty(word_array[j]))
                                {
                                    words.Add(word_array[i]);
                                    pro_words.Add(word_array[j]);
                                    distances.Add(count);
                                    count++;
                                }
                            }
                        }

                        commands.AddRange(SQLUtil.Decrease_ProWordPriorities(words.ToArray(), pro_words.ToArray(), distances.ToArray()));

                        commands.AddRange(SQLUtil.Decrease_TopicPriorities(message, word_array));
                        commands.AddRange(SQLUtil.Decrease_OutputPriorities(message));
                        commands.AddRange(SQLUtil.Decrease_InputPriorities(message));
                        commands.AddRange(SQLUtil.Decrease_WordPriorities(word_array));

                        return SQLUtil.BulkExecute(commands);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.Discourage", ex.Message, ex.StackTrace);
            }

            return false;
        }

        public static List<SqliteCommand> AddInputs(string input)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                commands.AddRange(SQLUtil.Increase_InputPriorities(input));
                commands.AddRange(SQLUtil.Add_NewInput(input));
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.AddInputs", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> AddWords(string[] word_array)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                commands.AddRange(SQLUtil.Increase_WordPriorities(word_array));
                commands.AddRange(SQLUtil.Add_Words(word_array));
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.AddWords", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> AddPreWords(string[] word_array)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                List<string> words = new List<string>();
                List<string> pre_words = new List<string>();
                List<int> distances = new List<int>();

                int length = word_array.Length;
                for (int i = 1; i < length; i++)
                {
                    int count = 1;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (!string.IsNullOrEmpty(word_array[i]) &&
                            !string.IsNullOrEmpty(word_array[j]))
                        {
                            words.Add(word_array[i]);
                            pre_words.Add(word_array[j]);
                            distances.Add(count);
                            count++;
                        }
                    }
                }

                commands.AddRange(SQLUtil.Increase_PreWordPriorities(words.ToArray(), pre_words.ToArray(), distances.ToArray()));
                commands.AddRange(SQLUtil.Add_PreWords(words.ToArray(), pre_words.ToArray(), distances.ToArray()));
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.AddPreWords", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        public static List<SqliteCommand> AddProWords(string[] word_array)
        {
            List<SqliteCommand> commands = new List<SqliteCommand>();

            try
            {
                List<string> words = new List<string>();
                List<string> pro_words = new List<string>();
                List<int> distances = new List<int>();

                int length = word_array.Length - 1;
                for (int i = 0; i < length; i++)
                {
                    var count = 1;
                    for (int j = i + 1; j <= length; j++)
                    {
                        if (!string.IsNullOrEmpty(word_array[i]) &&
                            !string.IsNullOrEmpty(word_array[j]))
                        {
                            words.Add(word_array[i]);
                            pro_words.Add(word_array[j]);
                            distances.Add(count);
                            count++;
                        }
                    }
                }

                commands.AddRange(SQLUtil.Increase_ProWordPriorities(words.ToArray(), pro_words.ToArray(), distances.ToArray()));
                commands.AddRange(SQLUtil.Add_ProWords(words.ToArray(), pro_words.ToArray(), distances.ToArray()));
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.AddProWords", ex.Message, ex.StackTrace);
            }

            return commands;
        }

        private static void UpdateTopics(string input, string[] topics)
        {
            try
            {
                List<SqliteCommand> commands = new List<SqliteCommand>();
                commands.AddRange(SQLUtil.Increase_TopicPriorities(input, topics));
                commands.AddRange(SQLUtil.AddTopics(input, topics));
                commands.AddRange(SQLUtil.Decrease_Topics_Unmatched(input, topics));
                commands.AddRange(SQLUtil.Clean_Topics(input));
                SQLUtil.BulkExecute(commands);
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.UpdateTopics", ex.Message, ex.StackTrace);
            }
        }

        private static void UpdateOutputs(string input, string output)
        {
            try
            {
                List<SqliteCommand> commands = new List<SqliteCommand>();
                commands.AddRange(SQLUtil.Increase_OutputPriorities(output, input));
                commands.AddRange(SQLUtil.Add_Outputs(output, input));
                SQLUtil.BulkExecute(commands);
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.UpdateOutputs", ex.Message, ex.StackTrace);
            }
        }

        public static string Respond(bool initiating)
        {
            Response = "";

            try
            {
                List<string> outputs = new List<string>();

                if (initiating)
                {
                    if (WordArray != null)
                    {
                        if (WordArray.Length > 0)
                        {
                            if (Topics != null &&
                                Options.TopicResponding)
                            {
                                //Get topic-based response
                                if (Topics.Length > 0)
                                {
                                    outputs.AddRange(SQLUtil.Get_OutputsFromTopics(Topics));
                                }
                            }
                            AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.2, Talk.lb_ResponseTime, Talk.ResponseStart);

                            if (outputs.Count == 0 &&
                                Options.WholeResponding)
                            {
                                //Get direct response
                                if (!string.IsNullOrEmpty(CleanInput))
                                {
                                    string[] outs = SQLUtil.Get_OutputsFromInput(CleanInput);
                                    outputs.AddRange(outs.ToList());
                                }
                            }
                            AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.4, Talk.lb_ResponseTime, Talk.ResponseStart);

                            if (outputs.Count == 0 &&
                                Options.ProceduralResponding)
                            {
                                //Get generated response
                                string min_word = SQLUtil.Get_MinWord(WordArray);
                                outputs.Add(GenerateResponse(min_word, false));
                            }
                            AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.6, Talk.lb_ResponseTime, Talk.ResponseStart);
                        }
                    }

                    //Still got nothing? Say something random.
                    if (outputs.Count == 0 &&
                        Options.ProceduralResponding)
                    {
                        string word = SQLUtil.Get_RandomWord();
                        outputs.Add(GenerateResponse(word, false));
                    }
                    AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.8, Talk.lb_ResponseTime, Talk.ResponseStart);
                }
                else
                {
                    PrepInput();

                    if (WordArray.Length > 0)
                    {
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.1, Talk.lb_ResponseTime, Talk.ResponseStart);

                        //Add input as output to last response
                        if (!GenerateAnotherResponse &&
                            !string.IsNullOrEmpty(LastResponse))
                        {
                            UpdateOutputs(CleanInput, LastResponse);
                        }
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.2, Talk.lb_ResponseTime, Talk.ResponseStart);

                        //Get lowest priority words from input
                        Topics = SQLUtil.Get_MinWords(WordArray);
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.3, Talk.lb_ResponseTime, Talk.ResponseStart);

                        if (!GenerateAnotherResponse &&
                            Topics.Length > 0)
                        {
                            //Add words as topics for input
                            UpdateTopics(CleanInput, Topics);
                        }
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.4, Talk.lb_ResponseTime, Talk.ResponseStart);

                        //Get topic-based response
                        if (Topics.Length > 0 &&
                            Options.TopicResponding)
                        {
                            //Get highest priority output(s) from matching topics
                            outputs.AddRange(SQLUtil.Get_OutputsFromTopics(Topics));
                        }
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.5, Talk.lb_ResponseTime, Talk.ResponseStart);

                        if (outputs.Count == 0 &&
                            Options.WholeResponding)
                        {
                            //Get direct response
                            string[] outs = SQLUtil.Get_OutputsFromInput(CleanInput);
                            outputs.AddRange(outs.ToList());
                        }
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.6, Talk.lb_ResponseTime, Talk.ResponseStart);

                        string min_word = "";
                        if (outputs.Count == 0)
                        {
                            //Get lowest priority word
                            min_word = SQLUtil.Get_MinWord(WordArray);
                        }
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.7, Talk.lb_ResponseTime, Talk.ResponseStart);

                        if (outputs.Count == 0 &&
                            !string.IsNullOrEmpty(min_word) &&
                            Options.ProceduralResponding)
                        {
                            //Generate response from lowest priority word
                            outputs.Add(GenerateResponse(min_word, false));
                        }
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.7, Talk.lb_ResponseTime, Talk.ResponseStart);

                        string random_word = "";
                        if (outputs.Count == 0)
                        {
                            //Still got nothing? Pick a random word.
                            random_word = SQLUtil.Get_RandomWord();
                        }
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.8, Talk.lb_ResponseTime, Talk.ResponseStart);

                        if (outputs.Count == 0 &&
                            !string.IsNullOrEmpty(random_word) &&
                            Options.ProceduralResponding)
                        {
                            //Generate response from random word
                            outputs.Add(GenerateResponse(random_word, false));
                        }
                        AppUtil.UpdateProgress(Talk.pb_ResponseTime, 0.9, Talk.lb_ResponseTime, Talk.ResponseStart);
                    }
                }

                if (outputs.Count > 0)
                {
                    //Choose a response from outputs at random
                    CryptoRandom random = new CryptoRandom();
                    int choice = random.Next(0, outputs.Count);

                    Response = outputs[choice];

                    if (!string.IsNullOrEmpty(Response))
                    {
                        //Clean up response
                        Response = RulesCheck(Response);

                        if (!string.IsNullOrEmpty(Response))
                        {
                            //Set response as last response
                            LastResponse = Response;

                            //Interrupt thinking
                            LastThought = "";
                        }
                    }
                }
                AppUtil.UpdateProgress(Talk.pb_ResponseTime, 1, Talk.lb_ResponseTime, Talk.ResponseStart);
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.Respond", ex.Message, ex.StackTrace);
            }

            return Response;
        }

        public static string GenerateResponse(string topic, bool for_thinking)
        {
            string response = "";

            try
            {
                if (!string.IsNullOrEmpty(topic))
                {
                    List<string> response_words = new List<string>();
                    bool words_found = true;

                    //Start with topic word
                    response_words.Add(topic);

                    //Add pre-words
                    while (words_found)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        //Get pre-word
                        string pre_word = SQLUtil.Get_PreWord(response_words, for_thinking);
                        if (!string.IsNullOrEmpty(pre_word))
                        {
                            //Add it
                            response_words.Insert(0, pre_word);
                        }
                        else
                        {
                            break;
                        }

                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        //Check for duplication
                        if (response_words.Count > 0)
                        {
                            List<string> new_response_words = HandleDuplication(response_words);
                            if (new_response_words.Count < response_words.Count)
                            {
                                break;
                            }
                        }

                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }
                    }

                    //Add pro-words
                    while (words_found)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        //Get pro-word
                        string pro_word = SQLUtil.Get_ProWord(response_words, for_thinking);
                        if (!string.IsNullOrEmpty(pro_word))
                        {
                            //Add it
                            response_words.Add(pro_word);
                        }
                        else
                        {
                            break;
                        }

                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        //Check for duplication
                        if (response_words.Count > 0)
                        {
                            List<string> new_response_words = HandleDuplication(response_words);
                            if (new_response_words.Count < response_words.Count)
                            {
                                break;
                            }
                        }

                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }
                    }

                    //Turn list of words into single string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < response_words.Count; i++)
                    {
                        if (for_thinking &&
                            !Options.CanThink)
                        {
                            break;
                        }

                        sb.Append(response_words[i]);

                        if (i < response_words.Count - 1)
                        {
                            sb.Append(" ");
                        }
                    }

                    if ((for_thinking && Options.CanThink) ||
                        !for_thinking)
                    {
                        response = sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.GenerateResponse", ex.Message, ex.StackTrace);
            }

            return response;
        }

        public static List<string> HandleDuplication(List<string> words)
        {
            List<string> results = new List<string>(words);

            try
            {
                bool dup_found = false;
                int dup_startIndex = 0;
                int dup_wordCount = 0;

                if (results.Count >= 4)
                {
                    for (int length = 4; length <= results.Count; length += 2)
                    {
                        int count = (int)Math.Floor((decimal)length / 2);

                        for (var i = 0; i <= results.Count - length; i++)
                        {
                            var first_chunk = "";
                            var second_chunk = "";

                            for (var c = i; c < count + i; c++)
                            {
                                first_chunk += results[c];
                                second_chunk += results[count + c];
                            }

                            if (first_chunk == second_chunk)
                            {
                                dup_found = true;
                                dup_startIndex = i;
                                dup_wordCount = count;
                                break;
                            }
                        }

                        if (dup_found)
                        {
                            break;
                        }
                    }

                    if (dup_found)
                    {
                        results.RemoveRange(dup_startIndex, dup_wordCount);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.HandleDuplication", ex.Message, ex.StackTrace);
            }

            return results;
        }

        public static string RulesCheck(string old_string)
        {
            string new_string = "";

            try
            {
                if (old_string.Any())
                {
                    new_string = old_string;

                    //Capitalize first word
                    char first_letter = new_string[0];
                    if (first_letter != char.ToUpper(first_letter))
                    {
                        new_string = char.ToUpper(first_letter) + new_string.Substring(1);
                    }

                    //Remove spaces before special characters
                    new_string = UnGapSpecials(new_string);

                    //Set ending punctuation if missing
                    int length = new_string.Length;
                    if (length > 0)
                    {
                        char last_letter = new_string[length - 1];
                        if (NormalCharacters.IsMatch(last_letter.ToString()))
                        {
                            new_string += ".";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.RulesCheck", ex.Message, ex.StackTrace);
            }

            return new_string;
        }

        private static void PrepThought(string respond_to)
        {
            try
            {
                string gapped = GapSpecials(respond_to);
                WordArray_Thinking = gapped.Trim(' ').Split(' ');
                if (WordArray_Thinking.Length > 0)
                {
                    if (!string.IsNullOrEmpty(WordArray_Thinking[0]) &&
                        Options.CanLearnFromThinking)
                    {
                        List<SqliteCommand> commands = new List<SqliteCommand>();
                        commands.AddRange(AddInputs(respond_to));
                        commands.AddRange(AddWords(WordArray_Thinking));
                        commands.AddRange(AddPreWords(WordArray_Thinking));
                        commands.AddRange(AddProWords(WordArray_Thinking));
                        SQLUtil.BulkExecute(commands);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.PrepThought", ex.Message, ex.StackTrace);
            }
        }

        public static string Think()
        {
            Thought = "";

            try
            {
                string respond_to = "";
                List<string> outputs = new List<string>();

                if (!string.IsNullOrEmpty(LastThought))
                {
                    respond_to = LastThought;
                }
                else if (!string.IsNullOrEmpty(CleanInput))
                {
                    respond_to = CleanInput;
                }

                if (!string.IsNullOrEmpty(respond_to))
                {
                    PrepThought(respond_to);
                    if (WordArray_Thinking.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(WordArray_Thinking[0]))
                        {
                            string[] topics = SQLUtil.Get_MinWords(WordArray_Thinking);

                            if (topics.Length > 0)
                            {
                                if (Options.CanLearnFromThinking)
                                {
                                    UpdateTopics(respond_to, topics);
                                }
                            }

                            if (topics.Length > 0 &&
                                Options.TopicResponding)
                            {
                                //Get topic-based response
                                outputs.AddRange(SQLUtil.Get_OutputsFromTopics(topics));
                            }

                            if (outputs.Count == 0 &&
                                Options.WholeResponding)
                            {
                                //Get direct response
                                string[] outs = SQLUtil.Get_OutputsFromInput(respond_to);
                                outputs.AddRange(outs.ToList());
                            }

                            if (outputs.Count == 0 &&
                                Options.ProceduralResponding)
                            {
                                //Get generated response
                                string min_word = SQLUtil.Get_MinWord(WordArray_Thinking);

                                if (!string.IsNullOrEmpty(min_word))
                                {
                                    outputs.Add(GenerateResponse(min_word, true));
                                }
                            }

                            if (outputs.Count == 0 &&
                                Options.ProceduralResponding)
                            {
                                string word = SQLUtil.Get_RandomWord();
                                outputs.Add(GenerateResponse(word, true));
                            }
                        }
                    }
                }

                if (outputs.Count > 0)
                {
                    //Choose a response from outputs
                    CryptoRandom random = new CryptoRandom();
                    int choice = random.Next(0, outputs.Count);

                    Thought = outputs[choice];

                    if (!string.IsNullOrEmpty(Thought))
                    {
                        //Clean up response
                        Thought = RulesCheck(Thought);

                        if (!string.IsNullOrEmpty(Thought))
                        {
                            if (!string.IsNullOrEmpty(LastThought) &&
                                Options.CanLearnFromThinking)
                            {
                                UpdateOutputs(Thought, LastThought);
                            }

                            //Set response as last response
                            LastThought = Thought;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog("Brain.Think", ex.Message, ex.StackTrace);
            }

            return Thought;
        }

        #endregion
    }
}
