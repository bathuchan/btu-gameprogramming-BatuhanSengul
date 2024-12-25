using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixerGroup mainMixerGroup;

    public Sound[] sounds;
    public float fadeInTime;
    public float fadeOutTime;

    private List<Sound> unassignedPlayerSounds = new List<Sound>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            if (s.attachedOnPlayer)
            {
                unassignedPlayerSounds.Add(s);
            }
            else
            {
                AttachSoundToObject(s, s.attachedObject != null ? s.attachedObject : gameObject);
            }
        }
    }

    private void Start()
    {
        Play("MainTheme");
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            OnPlayerLoaded(player);
        }
    }

    public void OnPlayerLoaded(GameObject newPlayer)
    {
        foreach (Sound s in unassignedPlayerSounds.ToArray())
        {
            if (newPlayer != null)
            {
                AttachSoundToObject(s, newPlayer);
                //unassignedPlayerSounds.Remove(s); 
            }
        }
    }

    


    public void AttachSoundToObject(Sound s, GameObject targetObject)
    {
        if (s.source != null)
        {
            Destroy(s.source); 
        }

        s.attachedObject = targetObject;
        s.source = targetObject.AddComponent<AudioSource>();


        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.loop = s.loop;
        s.source.playOnAwake = s.playOnAwake;
        s.source.outputAudioMixerGroup = s.mixerGroup;
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void PlayAtLocation(string sound, Vector3 pos,float spatialBlendValue,float minRangeValue, float maxRangeValue)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        GameObject tempAudioObject = new GameObject("TempAudio_" + sound);
        tempAudioObject.transform.position = pos;
        tempAudioObject.transform.parent=Referances.Instance.audioContainer.transform;

        AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();

        tempAudioSource.clip = s.clip;
        tempAudioSource.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        tempAudioSource.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
        tempAudioSource.outputAudioMixerGroup = s.mixerGroup;
        tempAudioSource.spatialBlend = spatialBlendValue;
        tempAudioSource.loop = false;
        tempAudioSource.minDistance = minRangeValue;
        tempAudioSource.maxDistance = maxRangeValue;
        

        tempAudioSource.Play();

        Destroy(tempAudioObject, s.clip.length / tempAudioSource.pitch);
    }

}
