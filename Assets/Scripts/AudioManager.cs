using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    const float minVolume = -80f;
    const float maxVolume = 20f;

    [Range(minVolume, maxVolume)]
    public float musicVolume;
    [Range(minVolume, maxVolume)]
    public float sfxVolume;

    void Start()
    {
        // Initialiser les volumes à partir des valeurs actuelles du mixer
        mixer.GetFloat("Music", out musicVolume);
        mixer.GetFloat("SFX", out sfxVolume);
    }

    void Update()
    {
        // Mettre à jour les volumes si nécessaire
        mixer.SetFloat("Music", musicVolume);
        mixer.SetFloat("SFX", sfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp(volume, minVolume, maxVolume);
        mixer.SetFloat("Music", musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp(volume, minVolume, maxVolume);
        mixer.SetFloat("SFX", sfxVolume);
    }

    public float GetMusicVolume()
    {
        mixer.GetFloat("Music", out float volume);
        return volume;
    }

    public float GetSFXVolume()
    {
        mixer.GetFloat("SFX", out float volume);
        return volume;
    }
}
