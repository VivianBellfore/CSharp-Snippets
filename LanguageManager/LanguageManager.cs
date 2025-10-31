using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;



namespace YOURNAMESPACEHERE
{
    /// <summary>
    /// Managing the system and user language for translations.
    /// </summary>
    internal class LanguageManager
    {
        // You need to change some things yourself:
		// - Line 24, change the default language if it should not be english.
		// - line 45, change name space.


		
		/// <summary>
		/// Default system language id.
		/// </summary>
		internal static string systemLanguage = "english";
		
        /// <summary>
		/// Caching user language data.<para/>
		/// </summary>
		internal static Dictionary<ulong, string> cachedUserLanguages = new Dictionary<ulong, string>();

        /// <summary>
		/// Contains all registered languages.
		/// </summary>
		public static Dictionary<string, Dictionary<string, string>> languages = new Dictionary<string, Dictionary<string, string>>();



		/// <summary>
		/// Getting all langues from the assembly.
		/// </summary>
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

			// If you are storing your system language in a data base then fetch it here like this:
			//object result = await MySqlWrapper.SQLExecuteScalar(
    			//"SELECT `language` FROM `guild_data`",
    			//new Dictionary<string, object> { });

			//if (result != null)
			    //systemLanguage = Convert.ToString(result);
        }

        /// <summary>
        /// Checking if a user has selected a language and gives back the language id.<br/>
        /// </summary>
        /// <returns>int - language id</returns>
        internal static async Task<string> GetUserLanguage(ulong userId)
        {
            if (cachedUserLanguages.ContainsKey(userId))
                return cachedUserLanguages[userId];

            object language = null;
			// Add the user language here instead of giving null if you have stored it somewhere. Like from a data base:
			//object language = await MySqlWrapper.SQLExecuteScalar(
    			//"SELECT `language` FROM `user_profile` WHERE `user_id` = user_id",
    			//new Dictionary<string, object>() { { "user_id", userId } });

			//if ( language == null )
				// Your error message system here.

            if (language == null)
            {
                cachedUserLanguages.Add(userId, systemLanguage);
                return systemLanguage;
            }
            else
            {
                cachedUserLanguages.Add(userId, Convert.ToString(language));
                return Convert.ToString(language);
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
			
			Dictionary<string, string> usedLanguage;

			if ( languages.ContainsKey(language) )
            	usedLanguage = languages[language];
			else if ( languages.ContainsKey(systemLanguage) )
				usedLanguage = languages[systemLanguage];
			else
				return $"Missing Translation! Text id was \"{textId}\".";

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
            // Add a database update here if you want to store it at the same time. Like this:
			//int updateCount = await MySqlWrapper.SQLExecuteNonQuery(
    			//"UPDATE `user_profile` SET `language` = @language WHERE `user_id` = @user_id",
    			//new Dictionary<string, object>() { { "user_id", userId }, { "language", language } });

			//if ( updateCount <= 0 )
				// Your error message system here.

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
            // Add or update a your system language here if you want to store it at the same time. Like this:
			//int updateCount = await MySqlWrapper.SQLExecuteNonQuery(
    			//"UPDATE `guild_data` SET `language` = @language WHERE `guild_id` = @guild_id",
    			//new Dictionary<string, object>() { { "guild_id", guildId }, { "language", language } });

			//if ( updateCount <= 0 )
				// Your error message system here.
			
			systemLanguage = language;
        }
    }
}


