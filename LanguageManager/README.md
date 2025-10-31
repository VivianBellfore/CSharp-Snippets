# What is this for?
This is a small but usefull language system that can fetch translations.
Its working with just a default language if you dont have user languages saved.
It is mainly made for a system where every user has saved there own language from a given language set.

<br>
<br>

# Installation and usage
After adding this folder to your project you need to change some things in the `LanuageManager.cs`:
- Line 24, change the default language if it should not be english.
- Line 45, the name of the namespace you are using.


And change the namespaces in the language `.cs` files also.

<br>

If you have a system where you are storing user languages, then use `SetUserLanguage` to add the user and the language.
The language needs to be a string, the name of the language cs file like `english` or `german`.
The user identifier is currently a ulong, change it in `cachedUserLanguages` at line 32 if it needs to be another type and dont forget to change the type in all other methodes also.

If you have multiple or changing system languages then you can set it with `SetSystemLanguage`.

<br>

How to use it for a user with a saved id:<br>
`string translation = await LanguageManager.GetTranslation("dataSaved", User.Id);`

How to use it for a system language message:<br>
`string translation = await LanguageManager.GetTranslation("dataSaved", 0, LanguageManager.systemLanguage);`

How to use it with parameter:<br>
`string translation = await LanguageManager.GetTranslation("dataSaved", 0, LanguageManager.systemLanguage, para1, para2);`<br>
or<br>
`string translation = await LanguageManager.GetTranslation("dataSaved", User.Id, LanguageManager.systemLanguage, para1, para2);`

How to use a specific language:<br>
`string translation = await LanguageManager.GetTranslation("dataSaved", 0, "LANGUAGENAMEHERE");`<br>

<br>
<br>

# Adding language
Just copy the `english.cs` and change the file and class name to the language you want to add. Every language name needs to be unique!

Change the translation text ( second string entry ) but not the text id ( first string entry ).

