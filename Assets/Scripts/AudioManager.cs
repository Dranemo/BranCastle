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
        mixer.GetFloat("MusicVolume", out musicVolume);
        mixer.GetFloat("SFXVolume", out sfxVolume);
    }

    void Update()
    {
        // Mettre à jour les volumes si nécessaire
        mixer.SetFloat("MusicVolume", musicVolume);
        mixer.SetFloat("SFXVolume", sfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp(volume, minVolume, maxVolume);
        mixer.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp(volume, minVolume, maxVolume);
        mixer.SetFloat("SFXVolume", sfxVolume);
    }

    public float GetMusicVolume()
    {
        mixer.GetFloat("MusicVolume", out float volume);
        return volume;
    }

    public float GetSFXVolume()
    {
        mixer.GetFloat("SFXVolume", out float volume);
        return volume;
    }
}
