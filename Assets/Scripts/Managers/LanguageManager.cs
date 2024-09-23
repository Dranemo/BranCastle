using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class LanguageManager
{
    public enum language
    {
        fr, 
        en
    }

    public static language currentLanguage = language.en;

    public static string GetText(string key)
    {
        return key;
    }

    // Convertir le dictionnaire en une liste de paires clé-valeur
    public static List<LanguageStringPair> ConvertDictionaryToList(Dictionary<LanguageManager.language, string> dictionary)
    {
        List<LanguageStringPair> list = new List<LanguageStringPair>();
        foreach (var kvp in dictionary)
        {
            list.Add(new LanguageStringPair { key = kvp.Key, value = kvp.Value });
        }
        return list;
    }

    // Convertir la liste de paires clé-valeur en un dictionnaire
    public static Dictionary<LanguageManager.language, string> ConvertListToDictionary(List<LanguageStringPair> list)
    {
        Dictionary<LanguageManager.language, string> dictionary = new Dictionary<LanguageManager.language, string>();
        foreach (var pair in list)
        {
            dictionary[pair.key] = pair.value;
        }
        return dictionary;
    }

}


[System.Serializable]
public class LanguageStringPair
{
    public LanguageManager.language key;
    public string value;
}
