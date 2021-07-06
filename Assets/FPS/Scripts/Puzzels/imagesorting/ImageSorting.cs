using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ImageSorting : MonoBehaviour, Interactable
{
    public UnityEvent OnFinishPuzzle;
    
    public bool isFinished = false;

    public bool isActive;
    
    private readonly SnappingGrid snappingGrid;
    TypeWriterEffect textManager;

    public float snappingGridSize;
    public int snappingGridLength;

    public Transform boardTransd;

    [SerializeField] private Transform camPos;
    
    public ImageSorting()
    {
        snappingGrid = new SnappingGrid(this);
    }

    public float snappingDistance;
    
    [SerializeField] public Vector3[] PuzzleSnappingGrid { get; private set;}

    int inPlace = 0;
    int notInPlace = 0;
    /// <summary>
    /// checks if all pieces are in correct place
    /// </summary>
    public void CheckPieces()
    {
        isFinished = true;
        
            for ( int i = 0; i < GetComponentsInChildren<puzzlePiece>().Length; ++i ) {
                if ( GetComponentsInChildren<puzzlePiece>()[ i ].isInPosition == false )
                {
                    isFinished = false;
                    return;
                }
            }

           // isFinished = true; //zodat het makkelijker is 

        
        if (isFinished)
        {
            OnFinishPuzzle.Invoke();
            Interact();
            return;
        }
    }
    
    /// <summary>
    /// on interact
    /// </summary>
    public void Interact()
    {
        
        if (!isActive && !isFinished)
        {
            textManager.RunningPuzzle(Puzzle.sorting);
            GameFlowManager.instance.allowPlayerToMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerCharacterController.instance.moveCamToPosition(camPos.position, camPos.rotation, true);
            isActive = true;
        }
        else
        {
            GameFlowManager.instance.allowPlayerToMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerCharacterController.instance.moveCamToPosition(PlayerCharacterController.instance.transform.position + Vector3.up * 1.44f, PlayerCharacterController.instance.transform.rotation, false);
            isActive = false;
        }
        
    }

    // awake is called before the first frame update
    void Awake()
    {
        PuzzleSnappingGrid = snappingGrid.BuildGrid(snappingGridSize, snappingGridLength, snappingGridLength);
        /*
        foreach (var VARIABLE in PuzzleSnappingGrid)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = VARIABLE;
            go.transform.localScale = Vector3.one * 0.03f;
        }
        */
    }

    void Start()
    {
        textManager = TypeWriterEffect.Instance;
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

            if(PlayerInputHandler.instance.pressedKey(KeyCode.Space))
            {
                isFinished = true;
                OnFinishPuzzle.Invoke();
                textManager.CompletedPuzzle(Puzzle.sorting);
                Interact();
            }
        }
        
    }
}
