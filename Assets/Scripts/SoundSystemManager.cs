using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystemManager : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    public float bg_volume = 0.6f;

    public GameObject speaker_left;
    public GameObject speaker_mid;
    public GameObject speaker_right;

    private AudioSource speaker_left_AS;
    private AudioSource speaker_mid_AS;
    private AudioSource speaker_right_AS;


    private AudioClip clip_background;
    private AudioClip clip_count;
    private AudioClip clip_round;
    private AudioClip clip_rest;
    private AudioClip clip_success;
    private AudioClip clip_failure;
    private AudioClip clip_cheer;
    private AudioClip clip_beep_2D;



    // Start is called before the first frame update
    void Start()
    {
        // find all clips using resources
        clip_background = (AudioClip)Resources.Load("Sounds/background");

        clip_count = (AudioClip)Resources.Load("Sounds/count");

        clip_round = (AudioClip)Resources.Load("Sounds/round"); 
        clip_rest = (AudioClip)Resources.Load("Sounds/rest"); // finding

        clip_success = (AudioClip)Resources.Load("Sounds/success");
        clip_failure = (AudioClip)Resources.Load("Sounds/failure");

        clip_cheer = (AudioClip)Resources.Load("Sounds/cheer");

        clip_beep_2D = (AudioClip)Resources.Load("Sounds/beep_2D");

        // find Audio Sources
        speaker_left_AS = speaker_left.GetComponent<AudioSource>();
        speaker_mid_AS = speaker_mid.GetComponent<AudioSource>();
        speaker_right_AS = speaker_right.GetComponent<AudioSource>();

    }

    public void start_sequence()
    {
        StartCoroutine("start_seq");   
    }

    private IEnumerator start_seq()
    {
        speaker_mid_AS.volume = 0f;

        speaker_mid_AS.clip = clip_background;
        speaker_mid_AS.loop = true;
        speaker_mid_AS.Play();


        float currentTime = 0;
        float duration = 1f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            speaker_mid_AS.volume = Mathf.Lerp(0f, bg_volume, currentTime / duration);
            yield return null;
        }

        yield break;
    }

    public void finish_sequence()
    {
        StartCoroutine("finish_seq");
    }

    private IEnumerator finish_seq()
    {
        float currentTime = 0;
        float duration = 1f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            speaker_mid_AS.volume = Mathf.Lerp(bg_volume, 0f, currentTime / duration);
            yield return null;
        }

        speaker_mid_AS.Stop();

        yield break;
    }

    public void count()
    {
        //speaker_left_AS.volume = 0.5f;
        //speaker_right_AS.volume = 0.5f;

        speaker_left_AS.PlayOneShot(clip_count);
        speaker_right_AS.PlayOneShot(clip_count);
    }

    public void beep_2D()
    {
        speaker_left_AS.PlayOneShot(clip_beep_2D);
        speaker_right_AS.PlayOneShot(clip_beep_2D);
    }

    public void round()
    {
        speaker_left_AS.PlayOneShot(clip_round);
        speaker_right_AS.PlayOneShot(clip_round);
    }

    public void success()
    {
        speaker_left_AS.PlayOneShot(clip_success);
        speaker_right_AS.PlayOneShot(clip_success);
    }

    public void failure()
    {
        speaker_left_AS.PlayOneShot(clip_failure);
        speaker_right_AS.PlayOneShot(clip_failure);
    }

    public void rest()
    {
        speaker_left_AS.PlayOneShot(clip_rest);
        speaker_right_AS.PlayOneShot(clip_rest);
    }

    public void cheer()
    {
        speaker_left_AS.PlayOneShot(clip_cheer);
        speaker_right_AS.PlayOneShot(clip_cheer);
    }
}
