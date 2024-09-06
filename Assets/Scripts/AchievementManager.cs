using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public List<Achievement> achievements = new List<Achievement>();

    string fileName = "Achievements.json";


    private void Awake()
    {
        SetInstance();
        LoadAchievements();
    }


    public enum AchievementID
    {
        firstFinish,
        firstLose,
        LoseUnit,
        WinBlood,
        Sun,
        LoseNine,
        Secret
    }





    void SetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }


    public void UnlockAchievement(AchievementID idEnum)
    {
        string id = idEnum.ToString();


        Achievement achievement = achievements.Find(a => a.id == id);

        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.Unlock();
            SaveAchievements();
        }
    }



    private void SaveAchievements()
    {
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        string json = JsonUtility.ToJson(new AchievementList(achievements), true);
        Debug.Log(json);



        File.WriteAllText(fullPath, json);
    }

    void LoadAchievements()
    {
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            AchievementList loadedAchievements = JsonUtility.FromJson<AchievementList>(json);
            achievements = loadedAchievements.achievements;
        }
        else
        {
            Debug.LogError("Le fichier JSON n'existe pas.");
            File.Create(fullPath);
        }
    }








    [System.Serializable]
    public class AchievementList
    {
        public List<Achievement> achievements;
        public AchievementList(List<Achievement> list)
        {
            achievements = list;

            foreach (Achievement ach in achievements)
            {
                Debug.Log(ach.id);
            }
        }
    }
}
