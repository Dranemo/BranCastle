using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Unit
{
    private AudioSource audioSource;

    void Awake()
    {
        // Obtenez la r�f�rence � l'AudioSource
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        health = 100;
        damage = 10;
        attackSpeed = 0.1f;
        bloodCost = 10;

        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource n'est pas attach� au GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
