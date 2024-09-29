using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static AchievementManager;

public class LanguageManager : MonoBehaviour
{
    public enum language
    {
        fr, 
        en
    }
    private string fileName = "TextsLanguage.json";

    static List<Dictionary<string, string>> lgs = new List<Dictionary<string, string>>();



    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadLanguage();
    }



    private void LoadLanguage()
    {
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            Debug.Log("JSON lu : " + json);

            LangueList loadedLg = JsonUtility.FromJson<LangueList>(json);
            if (loadedLg != null && loadedLg.textsLanguages != null)
            {
                Debug.Log("Nombre de langues chargées : " + loadedLg.textsLanguages.Count);

                // Convertir les listes de paires clé-valeur en dictionnaires après la désérialisation
                foreach (var lg in loadedLg.textsLanguages)
                {
                    Dictionary<string, string> dico = new Dictionary<string, string>();

                    dico.Add("id", lg.id);
                    dico.Add("play", lg.play);
                    dico.Add("achievement", lg.achievement);
                    dico.Add("options", lg.options);
                    dico.Add("quit", lg.quit);
                    dico.Add("back", lg.back);
                    dico.Add("display", lg.display);
                    dico.Add("fullscreen", lg.fullscreen);
                    dico.Add("windowed", lg.windowed);
                    dico.Add("windowedFullscreen", lg.windowedFullscreen);
                    dico.Add("music", lg.music);
                    dico.Add("sound", lg.sound);
                    dico.Add("language", lg.language);
                    dico.Add("previous", lg.previous);
                    dico.Add("next", lg.next);
                    dico.Add("skip", lg.skip);
                    dico.Add("tutoP1Main", lg.tutoP1Main);
                    dico.Add("tutoP1Text", lg.tutoP1Text);
                    dico.Add("tutoP2Main", lg.tutoP2Main);
                    dico.Add("tutoP2Text", lg.tutoP2Text);
                    dico.Add("tutoP3Main", lg.tutoP3Main);
                    dico.Add("tutoP3Text", lg.tutoP3Text);
                    dico.Add("tutoP4Main", lg.tutoP4Main);
                    dico.Add("tutoP4Text", lg.tutoP4Text);
                    dico.Add("tutoP5Main", lg.tutoP5Main);
                    dico.Add("tutoP5Text", lg.tutoP5Text);
                    dico.Add("tutoP6Main", lg.tutoP6Main);
                    dico.Add("tutoP6Text", lg.tutoP6Text);
                    dico.Add("pause", lg.pause);
                    dico.Add("resume", lg.resume);
                    dico.Add("menu", lg.menu);
                    dico.Add("blood", lg.blood);
                    dico.Add("wave", lg.wave);
                    dico.Add("cost", lg.cost);
                    dico.Add("ritualDamage", lg.ritualDamage);
                    dico.Add("gameOver", lg.gameOver);
                    dico.Add("victory", lg.victory);
                    dico.Add("portalOpen", lg.portalOpen);
                    dico.Add("waveDisplay", lg.waveDisplay);
                    dico.Add("score", lg.score);
                    dico.Add("replay", lg.replay);

                    lgs.Add(dico);
                }
            }
            else
            {
                Debug.LogError("La désérialisation a échoué ou la liste des langues est vide.");
            }
        }
        else
        {
            Debug.LogError("Le fichier JSON n'existe pas.");
            File.Create(fullPath);
        }
    }







    public static language currentLanguage = language.en;

    public static string GetText(string key)
    {
        foreach (var lg in lgs)
        {
            if (lg["id"] == currentLanguage.ToString())
            {
                return lg[key];
            }
        }

        return "ERROR";
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


    [System.Serializable]
    class TextsLanguage
    {
        public string id;
        public string play;
        public string achievement;
        public string options;
        public string quit;
        public string back;
        public string display;
        public string fullscreen;
        public string windowed;
        public string windowedFullscreen;
        public string music;
        public string sound;
        public string language;
        public string previous;
        public string next;
        public string skip;
        public string tutoP1Main;
        public string tutoP1Text;
        public string tutoP2Main;
        public string tutoP2Text;
        public string tutoP3Main;
        public string tutoP3Text;
        public string tutoP4Main;
        public string tutoP4Text;
        public string tutoP5Main;
        public string tutoP5Text;
        public string tutoP6Main;
        public string tutoP6Text;
        public string pause;
        public string resume;
        public string menu;
        public string blood;
        public string wave;
        public string cost;
        public string ritualDamage;
        public string gameOver;
        public string victory;
        public string portalOpen;
        public string waveDisplay;
        public string score;
        public string replay;
    }

    [System.Serializable]
    class LangueList
    {
        public List<TextsLanguage> textsLanguages;
    }

}


[System.Serializable]
public class LanguageStringPair
{
    public LanguageManager.language key;
    public string value;
}

