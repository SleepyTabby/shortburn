using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTrigger : MonoBehaviour
{
    [SerializeField] private TiltPuzzle tilt;
    [SerializeField] private bool win;

    private void OnTriggerEnter(Collider other)
    {
        tilt.BallUpdate(win);
    }
}
