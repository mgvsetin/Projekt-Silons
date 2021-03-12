using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public EnemySounds[] enemySounds;
    public GameObject[] enemies;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.loop = sound.loop;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }

        foreach(EnemySounds enemySound in enemySounds)
        {
            foreach (GameObject enemy in enemies)
            {
                enemySound.source = enemy.AddComponent<AudioSource>();


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
        }
    }

    private void Start()
    {
        Play("Backround Theme");
    }

    public void Update()
    {
        foreach(Sound sound in sounds)
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

        foreach (EnemySounds enemySound in enemySounds)
        {
            if (PauseMenu.isPaused == true)
            {
                enemySound.source.Pause();
            }
            else
            {
                enemySound.source.UnPause();
            }
        }
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void EnemySoundPlay(string name)
    {
        EnemySounds es = Array.Find(enemySounds, enemysound => enemysound.name == name);
        if (!PauseMenu.isPaused)
        {
            es.source.Play();
        }
        else
        {
            es.source.Pause();
        }
    }

    public void StopEnemySound(string name)
    {
        EnemySounds es = Array.Find(enemySounds, enemysound => enemysound.name == name);
        es.source.Stop();
    }
}
