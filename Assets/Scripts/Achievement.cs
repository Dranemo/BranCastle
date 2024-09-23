using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public string id;
    public bool isUnlocked;
    public List<LanguageStringPair> namesList;

    [System.NonSerialized]
    public Dictionary<LanguageManager.language, string> names;

    public Achievement(string id)
    {
        this.id = id;
        Debug.Log("names : " + names);

        namesList = LanguageManager.ConvertDictionaryToList(names);
        
        this.isUnlocked = false;
    }

    // Méthode pour déverrouiller l'achievement
    public void Unlock()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
        }
    }
}
