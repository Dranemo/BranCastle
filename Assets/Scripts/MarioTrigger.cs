using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MarioTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Mario Triggered");
            AchievementManager.instance.UnlockAchievement(AchievementManager.AchievementID.Secret);
        }
    }
}
