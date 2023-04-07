using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Serializable]
    public class Sound
    {
        public AudioClip auc;
        public string name;
    }
    [SerializeField]
    private Sound[] sfxSound, musicSound;
    [SerializeField]
    private AudioSource musicSource, sfxSource;
    public override void Start()
    {
        base.Start();
        musicSource.loop = true;
        sfxSource.volume = 0.3f;
        musicSource.volume = 0.3f;
    }
    
    public void PlayAudio(string name, bool isSFX)
    {
        Sound sound;
        if (isSFX)
        {
            if (!sfxSource) return;
            sound = Array.Find(sfxSound, s => string.Equals(s.name, name));
            if (sound != null)
            {
                sfxSource.PlayOneShot(sound.auc);
            }
            return;
        }
        if(!musicSource) return;
        sound = Array.Find(musicSound, s => string.Equals(name, s.name));
        if (sound != null)
        {
            musicSource.Stop();
            musicSource.clip = sound.auc;
            musicSource.Play();
        }
        
    }

    public void PauseOrResumeMusic(bool isPause)
    {
        if(isPause)
            musicSource.Pause();
        else musicSource.Play();
    }

    public void StopMusic()
    {
        if(musicSource) musicSource.Stop();
    }

    public void AudioVol(float vol,bool isSFX)
    {
        if (isSFX)
        {
            sfxSource.volume = vol;
            return;
        }
        musicSource.volume = vol;
    }

}