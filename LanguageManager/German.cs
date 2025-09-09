using System.Collections.Generic;



namespace YOURNAMESPACEHERE.Language
{
    /// <summary>
    /// Contains all german text strings for the <seealso cref="LanguageManager"/>.
    /// </summary>
    public class German
    {
        /// <summary>
        /// Contains all german translations.<para/>
        /// Key = name string.<br/>
        /// Value = text string, the actual text output in this language.
        /// </summary>
        public static Dictionary<string, string> LanguageDictionary = new Dictionary<string, string>()
        {
            // The first string is the name-id of this translation text, dont change it!
            // The second string is the text output, you can change that at any time. But you have to restart the system to update the text.
            // You can add parameter to the text with {0} in text line.
            // Please write informations to parameters so it can be understand here without searching the origin.

            

            #region System general
            {"dataSaved",               "[ :white_check_mark: ] Deine Angaben wurden erfolgreich gespeichert!"},
            {"deletedUserData",         "[ :white_check_mark: ] Alle Daten in Verbindung zu deinem Account wurden gelöscht!"},
            {"deletedGuildData",        "[ :white_check_mark: ] Alle Daten in Verbindung zu deinem Server wurden gelöscht!"},
            #endregion

            #region Errors
            {"generalError",            "[ :x: ] Es ist ein Fehler aufgetreten. Das Problem wurde automatisch gemeldet."},
            {"noUserDataFound",         "[ :x: ] Es wurden keine Daten in Verbindung zu deinem Account gefunden."},
            {"functionNotWhileTimeout", "[ :x: ] Du kannst keine Funktionen benutzen während du einen Timeout laufen hast. Dein Timeout endet um {0}!"}, // 0 = time when timeout is ending
            #endregion
        };
    }
}