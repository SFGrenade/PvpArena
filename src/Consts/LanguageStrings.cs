using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PvpArena.Consts
{
    public class LanguageStrings
    {
        private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> _jsonDict;

        public LanguageStrings()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream s = asm.GetManifestResourceStream("PvpArena.Resources.Language.json"))
            {
                if (s == null) return;

                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                s.Dispose();

                string json = System.Text.Encoding.Default.GetString(buffer);

                _jsonDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json);
            }
        }

        public string Get(string key, string sheet)
        {
            GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
            try
            {
                return _jsonDict[lang.ToString()][sheet][key].Replace("<br>", "\n");
            }
            catch
            {
                return _jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet][key].Replace("<br>", "\n");
            }
        }

        public bool ContainsKey(string key, string sheet)
        {
            try
            {
                GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
                try
                {
                    return _jsonDict[lang.ToString()][sheet].ContainsKey(key);
                }
                catch
                {
                    try
                    {
                        return _jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet].ContainsKey(key);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}