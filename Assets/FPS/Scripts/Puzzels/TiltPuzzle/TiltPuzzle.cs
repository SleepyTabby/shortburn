using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TiltPuzzle : MonoBehaviour, Interactable //Made By Wesley
{
    [Header("Puzzle Settings")]
    [SerializeField] private Transform puzzleCamLocation;
    public bool allowMovement;
    [SerializeField] private float MaxTilt;

    [SerializeField] private Transform ball;
    private Vector3 ballSpawnPos;

    private Vector3 moveTo;

    private bool puzzleCompleted;
    [SerializeField] private UnityEvent puzzleFinish;
    TypeWriterEffect textManager;

    private void Start()
    {
        moveTo = transform.position;
        ballSpawnPos = ball.localPosition;
        if (TypeWriterEffect.Instance != null) textManager = TypeWriterEffect.Instance;
    }

    void Update()
    {
        if (allowMovement) //Handles TiltBox Rotation
        {
            Vector3 input = Vector3.zero;
            input.x = -Input.GetAxis("Horizontal");
            input.z = -Input.GetAxis("Vertical");
            transform.rotation = Quaternion.Euler(input * MaxTilt);

            if (PlayerInputHandler.instance.pressedKey(KeyCode.E)) CloseMenu(); //Closes Menu
        }
    }

    private void FixedUpdate()
    {
        Vector3 dir = (moveTo - transform.position).normalized;
        float speed = 5;

        //transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.position = transform.position + dir * speed * Time.fixedDeltaTime;
    }

    private void CloseMenu() //Puzzle Close (Stop)
    {
        
        //textManager.ClosingPuzzle();
        GameFlowManager.instance.allowPlayerToMove = true;
        allowMovement = false;
        moveTo = transform.position - (Vector3.up * 1.5f);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        PlayerCharacterController.instance.moveCamToPosition(PlayerCharacterController.instance.transform.position + Vector3.up * 1.44f, PlayerCharacterController.instance.transform.rotation,false );
        
    }

    void Interactable.Interact() //Puzzle Interaction (Start)
    {
        if (!puzzleCompleted)
        {

            if (textManager != null) textManager.RunningPuzzle(Puzzle.tilt);
            GameFlowManager.instance.allowPlayerToMove = false;
            moveTo = transform.position + (Vector3.up * 1.5f);
            allowMovement = true;
            PlayerCharacterController.instance.moveCamToPosition(puzzleCamLocation.transform.position, puzzleCamLocation.transform.rotation, true);
        }
    }

    public void BallUpdate(bool _finish) //When ball drops out of the box or finishes it will be updated here.
    {
        if (_finish && allowMovement)
        {
            puzzleCompleted = true;
            puzzleFinish.Invoke();
            if(textManager != null) textManager.CompletedPuzzle(Puzzle.tilt);
            CloseMenu();
        }
        else
        {
            ball.transform.localPosition = ballSpawnPos;
        }
    }
}

