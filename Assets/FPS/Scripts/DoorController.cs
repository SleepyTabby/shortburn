using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DoorController : MonoBehaviour, Interactable //Made By Wesley
{
    private Animator ani;
    private bool doorOpen;
    [SerializeField] private bool openWay;
    public bool locked;

    [Header("Audio")]
    [SerializeField] private StudioEventEmitter doorSound;

    private void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetBool("OpenWay", openWay);
    }

    public void Interact() //Puzzle Interaction (Start)
    {
        if (!locked)
        {
            doorOpen = !doorOpen; //Flips the bool on Interaction
            if(doorSound != null) doorSound.Play();
            ani.SetBool("Door Open", doorOpen);
        }
    }

    public void ChangeDeurState(bool state)
    {
        locked = state;
    }
}
