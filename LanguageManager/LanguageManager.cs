using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;



namespace YOURNAMESPACEHERE
{
    /// <summary>
    /// Managing the system or user language and translation.
    /// </summary>
    internal class LanguageManager
    {
        // You need to change some things yourself:
		// - Line 27, change the default language if it should not be english.
        // - Line 45, the name of the namespace you are using.
		// - Line 68, Change the database SQL statement for your user language system.
		// - Line 106, update your user language if you have stored it somewhere.
		// - Line 119, save or update your system language if you want to store it.


		
		/// <summary>
        /// Holding the system language id.
        /// </summary>
		internal static string systemLanguage = "english";
		
        /// <summary>
        /// Holding user language data.
        /// </summary>
        internal static Dictionary<ulong, string> cachedUserLanguages = new Dictionary<ulong, string>();

        /// <summary>
        /// Contains all registered languages.
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> languages = new Dictionary<string, Dictionary<string, string>>();



        public static void LoadLanguages()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var languageTypes = assembly.GetTypes().Where(t => t.Namespace == "YOURNAMESPACEHERE.Language");

            foreach (var type in languageTypes)
            {
                var field = type.GetField("LanguageDictionary", BindingFlags.Public | BindingFlags.Static);
                if (field != null && field.FieldType == typeof(Dictionary<string, string>))
                {
                    var dict = (Dictionary<string, string>)field.GetValue(null);
                    var languageName = type.Name.ToLower();
                    languages.Add(languageName, dict);
                }
            }
        }

        /// <summary>
        /// Checking if a user has selected a language and gives back the language id.<br/>
        /// </summary>
        /// <returns>int - language id</returns>
        internal static async Task<string> GetUserLanguage(ulong userId)
        {
            if (cachedUserLanguages.ContainsKey(userId))
                return cachedUserLanguages[userId];

            object language = null; // Add the user language here instead of giving null if you have stored it somewhere.

            if (language == null)
            {
                cachedUserLanguages.Add(userId, systemLanguage);
                return systemLanguage;
            }
            else
            {
                cachedUserLanguages.Add(userId, language.ToString());
                return language.ToString();
            }
        }

        /// <summary>
        /// Checking the user or system language and gives back a translation for the given text id.<br/>
		/// Using system language: set user id to 0 and add a stirng for the system language or use the saved system language.<br/>
        /// Using parameter: Alway add the system langage and parameter after this, if you are using {0} in the translation text.
        /// </summary>
        /// <returns>string - text message</returns>
        internal static async Task<string> GetTranslation(string textId, ulong userId, string language = systemLanguage, params object[] args)
        {
            if ( userId != 0)
                language = await GetUserLanguage(userId);

            Dictionary<string, string> usedLanguage = languages[language];

            if (usedLanguage.ContainsKey(textId))
                return args.Length > 0 ? string.Format(usedLanguage[textId], args) : usedLanguage[textId];
            else
                return $"Missing Translation! Text id was \"{textId}\".";
        }

        /// <summary>
        /// Setting a language id for a user to the data base.
        /// </summary>
        internal static async Task SetUserLanguage(string language, ulong userId)
        {
            // Add a database update here if you want to store it at the same time.

            if (cachedUserLanguages.ContainsKey(userId))
                cachedUserLanguages[userId] = language;
            else
                cachedUserLanguages.Add(userId, language);
        }

        /// <summary>
        /// Setting a language id for a guild to the data base.
        /// </summary>
        internal static async Task SetSystemLanguage(string language)
        {
            // Add or update a your system language here if you want to store it at the same time.
			
			systemLanguage = language;
        }
    }
}
