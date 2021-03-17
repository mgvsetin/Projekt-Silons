using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public AudioManager audio;
    private float waitTime;
    public List<UnityEngine.Video.VideoClip> clips;
    [SerializeField]private float buttonsPressed = 0f;
    Player player;
    
   

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        audio = FindObjectOfType<AudioManager>();

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 3:
                StartCoroutine(playSound1());
                break;

            case 4:
                StartCoroutine(playTutorial2());
                break;
        }
    }

    IEnumerator playSound1()
    {
        player.canMove = false;
        audio.Play("Welcome");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Mouse");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Basics");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Try it");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        player.canMove = true;

    }

    IEnumerator playSound2()
    {
        audio.Play("Well done");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Types of Movement");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Running");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Try it");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);

    }
    IEnumerator playSound3()
    {
        audio.Play("Well done");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Crouching");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Try it");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);

    }

    IEnumerator playSound4()
    {
        audio.Play("Well done");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Crouch Sprinting");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Try it");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);

    }

    IEnumerator playSound5()
    {
        audio.Play("Well done");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        player.rb.velocity = Vector2.zero;
        player.canMove = false;
        audio.Play("Into Cover");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Try it");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        player.canMove = true;

    }

    IEnumerator playSound6()
    {
        audio.Play("Well done");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        player.rb.velocity = Vector2.zero;
        player.canMove = false;
        audio.Play("Out Of Cover");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Try it");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        player.canMove = true;

    }
    IEnumerator playSound7()
    {
        audio.Play("Well done");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        audio.Play("Finish 1");
        yield return new WaitForSeconds(audio.currentAudio.clip.length);
        SceneManager.LoadScene(4);

    }
    IEnumerator playTutorial2()
    {
        var videoPlayer = FindObjectOfType<UnityEngine.Video.VideoPlayer>();
        videoPlayer.clip = clips[0];
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        videoPlayer.clip = clips[1];
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        videoPlayer.clip = clips[2];
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        videoPlayer.clip = clips[3];
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        videoPlayer.clip = clips[4];
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        videoPlayer.clip = clips[5];
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            if(player.canMove)
            {
                if(buttonsPressed < 4)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        buttonsPressed++;
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        buttonsPressed++;
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        buttonsPressed++;
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        buttonsPressed++;
                    }

                    if (buttonsPressed == 4)
                    {
                        StartCoroutine(playSound2());
                    }
                }
                if(buttonsPressed == 4)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        buttonsPressed++;
                    }
                    if (buttonsPressed == 5)
                    {
                        StartCoroutine(playSound3());
                    }
                }
                if (buttonsPressed == 5 || buttonsPressed == 6)
                {
                    if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        buttonsPressed++;
                    }
                    if (buttonsPressed == 7)
                    {
                        StartCoroutine(playSound4());
                    }
                }
                if (buttonsPressed == 7)
                {
                    if (player.crouched)
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            buttonsPressed++;
                        }
                    }
                    if (buttonsPressed == 8)
                    {
                        StartCoroutine(playSound5());
                    }
                }
                if (buttonsPressed == 8)
                {
                    if (player.behindCover)
                    {
                        buttonsPressed++;
                    }
                    if (buttonsPressed == 9)
                    {
                        StartCoroutine(playSound6());
                    }

                }
                if (buttonsPressed == 9)
                {
                    if (!player.behindCover)
                    {
                        buttonsPressed++;
                    }
                    if (buttonsPressed == 10)
                    {
                        StartCoroutine(playSound7());
                    }
                }
            }

        }
    }
}
