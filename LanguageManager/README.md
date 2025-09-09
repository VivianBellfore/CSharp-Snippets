# What is this for?
This is a small but usefull language system that can fetch translations.
Its working with just a default language if you dont have user languages saved.
It is mainly made for a system where every user has saved there own language from a given language set.

<br>
<br>

# Installation and usage
After adding this folder to your project you need to change some things in the `LanuageManager.cs`:
- Line 27, change the default language if it should not be english.
- Line 45, the name of the namespace you are using.
- Line 68, Change the database SQL statement for your user language system.
- Line 106, update your user language if you have stored it somewhere.
- Line 119, save or update your system language if you want to store it.

And change the namespaces in the language `.cs` files also.

<br>

If you have a system where you are storing user languages, then use `SetUserLanguage` to add the user and the language.
The language needs to be a string, the name of the language cs file like `english` or `german`.
The user identifier is currently a ulong, change it in `cachedUserLanguages` at line 32 if it needs to be another type and dont forget to change the type in all other methodes also.

If you have multiple or changing system languages then you can set it with `SetSystemLanguage`.

<br>
<br>

# Adding language
Just copy the `english.cs` and change the file and class name to the language you want to add. Every language name needs to be unique!
Change the translation text ( second string entry ) but not the text id ( first string entry ).