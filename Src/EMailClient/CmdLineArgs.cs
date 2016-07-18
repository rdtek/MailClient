using System;
using System.Collections.Generic;

namespace MailClient
{
    class CmdLineArgs
    {
        private string[] m_originalArgs = null;
        private Dictionary<string, string> m_argsDict 
            = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public CmdLineArgs(string[] args)
        {
            m_originalArgs = args;
            m_argsDict = ParseArgs(args);
        }

        public string Get(int argIndex)
        {
            return m_originalArgs[argIndex];
        }

        public bool Exists(int argIndex)
        {
            return m_originalArgs != null 
                && m_originalArgs.Length >= argIndex + 1
                && m_originalArgs[argIndex].Length >= 1;
        }

        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            var dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            if (args == null || args.Length <= 0) return dict;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.IndexOf("/") == 0)
                { /* found commad key */

                    string command = arg; string parameter = "";

                    if (args.Length >= i + 2)
                    {
                        string nextArg = args[i + 1];
                        if (nextArg.IndexOf("/") == 0) { continue; }
                        else { parameter = nextArg; }
                    }
                    dict.Add(command, parameter);
                }
            }

            return dict;
        }

        public string GetArg(string argName, string defaultValue = "")
        {
            string result = defaultValue;

            if (CheckDictKey(m_argsDict, argName))
            {
                result = m_argsDict[argName];
            }

            return result;
        }

        private static bool CheckDictKey(Dictionary<string, string> dict, string key)
        {
            return dict.ContainsKey(key) && dict[key].Length >= 1;
        }

        public override string ToString()
        {

            string result = "CmdLineArgs: { ";
            foreach (KeyValuePair<string, string> kvp in m_argsDict)
            {
                result += string.Format("[Key = {0}, Value = {1}] ", kvp.Key, kvp.Value);
            }
            result += "}";

            return result;
        }
    }

}
