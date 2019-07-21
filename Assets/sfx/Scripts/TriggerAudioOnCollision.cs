using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class TriggerAudioOnCollision : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    private bool wasPlayed;

    [SerializeField] private AudioClip[] clips;


    private void OnCollisionEnter(Collision other)
    {
        if (!wasPlayed && clips.Length > 0)
        {
            source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            wasPlayed = true;
        }
    }
}
