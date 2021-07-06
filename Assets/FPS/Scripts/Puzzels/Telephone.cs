using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Telephone : MonoBehaviour, Interactable
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject dial;
    [Range(-100, 100)]
    [SerializeField] float rotOffset;
    float baseRot;
    [SerializeField] List<GameObject> number = new List<GameObject>();
    [SerializeField] string[] numberCode;
    [SerializeField] GameObject point;
    GameObject chosenNumber;
    int correct;

    [Header("generic Stuff")]
    private bool runPuzzle;
    [SerializeField] private Transform puzzleCamLocation;
    TypeWriterEffect textManager;

    [Header("win condition")]
    [SerializeField] UnityEvent WinEvent;

    bool resetDial;

    private bool finished;
    private bool allowSolve;


    void Start()
    {
        baseRot = dial.transform.rotation.eulerAngles.y;
        if(TypeWriterEffect.Instance != null) textManager = TypeWriterEffect.Instance;
    }
    //on pressing E activate this code
    public void Interact()
    {
        if (!finished && allowSolve) // check if you've solved it or if you aren't in range
        {
            if (textManager != null) textManager.RunningPuzzle(Puzzle.phone);
            GameFlowManager.instance.allowPlayerToMove = false;
            runPuzzle = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerCharacterController.instance.moveCamToPosition(puzzleCamLocation.transform.position, puzzleCamLocation.transform.rotation, true);
        }
    }
    //on pressing E or completing the puzzle it will close the puzzle
    private void ClosePuzzle()
    {
        runPuzzle = false;
        GameFlowManager.instance.allowPlayerToMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerCharacterController.instance.moveCamToPosition(PlayerCharacterController.instance.transform.position + Vector3.up * 1.44f, PlayerCharacterController.instance.transform.rotation, false);
    }
    public void AllowInteraction(bool state)
    {
        allowSolve = state;
    }

    void Update()
    {
        if (runPuzzle)
        {
            OnInteractUpdate();
            if (PlayerInputHandler.instance.pressedKey(KeyCode.E) == true) ClosePuzzle(); //Closes Menu
        }
        
    }
    void OnInteractUpdate()
    {
        if (Input.GetMouseButton(0))// moves the pointer to location the player wants to input 
        {
            resetDial = false;
            RaycastHit hit;
            var ray = PlayerCharacterController.instance.PlayerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) // max range
            {
                if (hit.collider != null)
                {
                    if (Vector3.Distance(hit.collider.gameObject.transform.position, player.transform.position) <= 5)
                    {
                        dial.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
                        dial.transform.localRotation = Quaternion.Euler(0, dial.transform.localRotation.eulerAngles.y + rotOffset, 0);
                    }
                }
            }
        }
        //when the player releases the mouse it will check which number is closest to the end of the pointer and if the number is the correct one it will check if the next one is also correct 
        //if this is not correct it will reset itself
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            var ray = PlayerCharacterController.instance.PlayerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) // max range
            {
                if (hit.collider != null)
                {
                    float closest = Mathf.Infinity;
                    GameObject empty = null;
                    foreach (GameObject children in number)
                    {
                        Vector3 directionToTarget = children.transform.position - point.transform.position;
                        float distance = directionToTarget.sqrMagnitude;
                        if (distance < closest)
                        {
                            closest = distance;
                            empty = children;
                        }
                        if (children == number[9])
                        {
                            Debug.Log("reached the end        this one  won: " + empty.name);
                            chosenNumber = empty;
                            if (empty.name == numberCode[correct])
                            {
                                correct += 1;
                                if (correct >= numberCode.Length)
                                {
                                    //win condition
                                    textManager.CompletedPuzzle(Puzzle.phone);
                                    finished = true;
                                    WinEvent.Invoke();
                                    ClosePuzzle();
                                }
                            }
                            else
                            {
                                correct = 0;
                            }
                            resetDial = true;
                        }
                    }
                }
            }
        }
    }
}
