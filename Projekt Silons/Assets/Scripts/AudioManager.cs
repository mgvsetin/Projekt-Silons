using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public EnemySounds[] enemySounds;
    public GameObject[] enemies;

    RoomTemplates room;
    private bool startSoundsPlaying = false;

    public AudioSource currentAudio;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        room = FindObjectOfType<RoomTemplates>();

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.loop = sound.loop;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }

      /*  foreach(EnemySounds enemySound in enemySounds)
        {
            foreach (GameObject enemy in enemies)
            {
                enemySound.source = enemy.AddComponent<AudioSource>();
                //enemy.GetComponent<Enemy>().enemyAudioSources.Add(enemySound.source);

                enemySound.source.clip = enemySound.clip;

                enemySound.source.loop = enemySound.loop;
                enemySound.source.playOnAwake = enemySound.playOnAwake;

                enemySound.source.volume = enemySound.volume;
                enemySound.source.pitch = enemySound.pitch;

                enemySound.source.spatialBlend = enemySound.spatialBlend;
                enemySound.source.rolloffMode = enemySound.rolloffMode;
                enemySound.source.minDistance = enemySound.minDistance;
                enemySound.source.maxDistance = enemySound.maxDistance;
            }
        } */
    }

    private void Start()
    {
        if(room != null)
        {
            if (room.exitSpawned)
            {
                Play("Backround Theme");
                startSoundsPlaying = true;
            }
        }

    }

    public void Update()
    {

        if(room != null)
        {
            if (room.exitSpawned && !startSoundsPlaying)
            {
                Play("Backround Theme");
                startSoundsPlaying = true;
            }
        }


        foreach (Sound sound in sounds)
        {
            if (PauseMenu.isPaused == true)
            {
                sound.source.Pause();
            }
            else
            {
                sound.source.UnPause();
            }
        }
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
        currentAudio = s.source;

    }

   
}
