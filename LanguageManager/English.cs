using System.Collections.Generic;



namespace YOURNAMESPACEHERE.Language
{
    /// <summary>
    /// Contains all english text strings for the <seealso cref="LanguageManager"/>.
    /// </summary>
    public class English
    {
        /// <summary>
        /// Contains all english translations.<para/>
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
            {"dataSaved",               "[ :white_check_mark: ] Your data have been saved successfully!"},
            {"deletedUserData",         "[ :white_check_mark: ] All data associated with your account has been deleted!"},
            {"deletedGuildData",        "[ :white_check_mark: ] All data related to your server has been deleted!"},
            #endregion

            #region Errors
            {"generalError",            "[ :x: ] An error occurred. The error was reported automatically."},
            {"noUserDataFound",         "[ :x: ] No data was found associated with your account."},
            {"functionNotWhileTimeout", "[ :x: ] You cannot use functions while you have a timeout running. Your timeout ends at {0}!"}, // 0 = time when timeout is ending
            #endregion
        };
    }
}