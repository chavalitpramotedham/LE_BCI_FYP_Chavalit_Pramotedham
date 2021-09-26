using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystemManager : MonoBehaviour
{
    public GameObject speaker_left;
    public GameObject speaker_right;

    private AudioSource speaker_left_AS;
    private AudioSource speaker_right_AS;

    private AudioClip clip_count;
    private AudioClip clip_round;
    private AudioClip clip_rest;
    private AudioClip clip_success;
    private AudioClip clip_failure;
    private AudioClip clip_cheer;

    // Start is called before the first frame update
    void Start()
    {
        // find all clips using resources

        // find Audio Sources
        speaker_left_AS = speaker_left.GetComponent<AudioSource>();
        speaker_right_AS = speaker_right.GetComponent<AudioSource>();
    }




}
