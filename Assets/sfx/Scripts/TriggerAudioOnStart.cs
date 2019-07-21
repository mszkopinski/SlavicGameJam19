using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudioOnStart : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    private bool wasPlayed;

    [SerializeField] private AudioClip[] clips;
    
    [SerializeField] private float delay = 1f;

    private void Start()
    {
        if (!wasPlayed)
        {
            Invoke("PlaySound", delay);
        }
    }

    private void PlaySound()
    {
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
