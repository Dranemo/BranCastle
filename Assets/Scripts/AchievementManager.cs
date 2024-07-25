using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public List<Achievement> achievements = new List<Achievement>();

    private void Awake()
    {
        SetInstance();
        SetAchievements();
    }


    public enum AchievementID
    {
        firstFinish
    }




    private void SetAchievements()
    {
        achievements.Add(new Achievement("firstFinish", "Gagnez le jeu !"));
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
            // Sauvegarder l'état des achievements ici
        }
    }
}
