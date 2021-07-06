using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TiltPuzzleBall : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private StudioEventEmitter ballSound;
    [SerializeField] private StudioEventEmitter playCollectSound;
    [SerializeField] private TiltPuzzle tilt;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (tilt.allowMovement)
        {
            if (rb.velocity.magnitude != 0 && !ballSound.IsPlaying()) ballSound.Play();
            else if (ballSound.IsPlaying() && rb.velocity.magnitude == 0) ballSound.Stop();
        }
        else if (ballSound.IsPlaying()) ballSound.Stop();
    }

    public void PlayCollectSound()
    {
        playCollectSound.Play();
    }
}
