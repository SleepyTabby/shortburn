using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RebusPuzzle : MonoBehaviour,Interactable
{
    private bool isFinished;

    public bool isActive { private set; get; }
    public Transform camPos;

    public InputField inputField;

    public string CorrectText;

    public UnityEvent OnComplete;

    TypeWriterEffect textManager;
    /// <summary>
    /// on interact
    /// </summary>
    public void Interact()
    {
        if (!isActive && !isFinished)
        {
            textManager.RunningPuzzle(Puzzle.news);
            GameFlowManager.instance.allowPlayerToMove = false;
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            PlayerCharacterController.instance.moveCamToPosition(camPos.position, camPos.rotation, true);
            isActive = true;
        }
        else
        {
            GameFlowManager.instance.allowPlayerToMove = true;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            PlayerCharacterController.instance.moveCamToPosition(PlayerCharacterController.instance.transform.position + Vector3.up * 1.44f, PlayerCharacterController.instance.transform.rotation, false);
            isActive = false;
        }
        
    }

    void CheckText()
    {
        if (inputField.text == CorrectText)
        {
            Debug.Log("Value correct");
            textManager.CompletedPuzzle(Puzzle.news);
            isFinished = true;
            OnComplete.Invoke();
            Interact();
            inputField.readOnly = true;
        }
    }

    private void Start()
    {
        textManager = TypeWriterEffect.Instance;
        //if (inputField.transform.parent.GetComponent<Canvas>().worldCamera == null)
        //{
         //   string s = inputField + " parent canvas event camera is unassigned!";
         //   Debug.LogException(new Exception(s), inputField.gameObject);
        //}
        //inputField.transform.parent.GetComponent<Canvas>().worldCamera = Camera.current;
        //inputField.onValueChanged.AddListener(delegate(string arg0) {CheckText();});
        //inputField.readOnly = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (PlayerInputHandler.instance.pressedKey(KeyCode.E))
            {
                Interact();
            }
        }
        
    }
}
