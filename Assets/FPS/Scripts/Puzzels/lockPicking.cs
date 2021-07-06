using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class lockPicking : MonoBehaviour, Interactable
{
    [SerializeField] GameObject pick;
    [SerializeField] GameObject guts;
    [SerializeField] GameObject hull;
    [Range(-100, 100)]
    [SerializeField] float rotOffset;
    [SerializeField] float speed = 8;
    [SerializeField] float give;
    [SerializeField] float acceptableGoalDeviation;
    [SerializeField] Vector2 RotParaOffset;
    [Range(44.228f, -240f)]
    float goalRot;
    float adjustedSpeed;
    Quaternion baseRot;
    bool Finished;
    private bool allowSolve;

    private bool allowInteraction;

    [Header("generic Stuff")]
    private bool runPuzzle;
    [SerializeField] Collider col;
    [SerializeField] private Transform puzzleCamLocation;
    TypeWriterEffect textManager;

    [Header("Audio")]
    [SerializeField] private StudioEventEmitter lockpicking;

    [Header("win condition")]
    [SerializeField] UnityEvent WinEvent;
    void Start()
    {
        textManager = TypeWriterEffect.Instance;
        baseRot = transform.rotation;
        //float firstParameter = Random.Range(60f + RotParaOffset.x, -180f + RotParaOffset.y);
        //float secondParameter = Random.Range(180f + RotParaOffset.x, 109f + RotParaOffset.y);
        //goalRot = Random.Range(firstParameter, secondParameter);
        goalRot = Random.Range(20, 270);

        
    }

    public void Interact()
    {
        if (!Finished && allowSolve)
        {
            textManager.RunningPuzzle(Puzzle.lockpick);
            GameFlowManager.instance.allowPlayerToMove = false;
            runPuzzle = true;
            col.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerCharacterController.instance.moveCamToPosition(puzzleCamLocation.transform.position, puzzleCamLocation.transform.rotation, true);
            allowInteraction = true;
        }
    }

    public void AllowInteraction(bool state)
    {
        allowSolve = state;
    }

    private void ClosePuzzle()
    {
        transform.rotation = baseRot;
        //float firstParameter = Random.Range(60f, -180f);
        //float secondParameter = Random.Range(180f, 109f);
        //goalRot = Random.Range(firstParameter, secondParameter);
        lockpicking.Stop();
        allowInteraction = false;
        col.enabled = true;
        runPuzzle = false;
        GameFlowManager.instance.allowPlayerToMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerCharacterController.instance.moveCamToPosition(PlayerCharacterController.instance.transform.position + Vector3.up * 1.44f, PlayerCharacterController.instance.transform.rotation, false);
        
    }

    // check if interaction is being done 
    void Update()
    {
        if (allowInteraction)
        {
            OnInteractUpdate();
            if (PlayerInputHandler.instance.pressedKey(KeyCode.E)) ClosePuzzle(); //Closes Puzzle
        }
    }

    //fix fonts color 


    void OnInteractUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (!lockpicking.IsPlaying()) lockpicking.Play(); //Plays Lockpick Sound

            RaycastHit hit;
            var ray = PlayerCharacterController.instance.PlayerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) // max range
            {
                if (hit.collider != null)
                {
                    pick.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
                    pick.transform.rotation = Quaternion.Euler(0, pick.transform.rotation.eulerAngles.y + rotOffset, 0);
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            if (guts.transform.rotation.eulerAngles.y >= 268 && guts.transform.rotation.eulerAngles.y <= 280)
            {
                guts.transform.rotation = Quaternion.Euler(guts.transform.rotation.x, 278, guts.transform.rotation.z); // make a universal float for this 

                if (pick.transform.localEulerAngles.y >= (goalRot - acceptableGoalDeviation) && pick.transform.localEulerAngles.y <= (goalRot + acceptableGoalDeviation))
                {
                    Finished = true;
                    lockpicking.Stop();
                    WinEvent.Invoke();
                    textManager.CompletedPuzzle(Puzzle.lockpick);
                    ClosePuzzle();
                }
            }
            if(pick.transform.localEulerAngles.y >= (goalRot - acceptableGoalDeviation) && pick.transform.localEulerAngles.y <= (goalRot + acceptableGoalDeviation))
            {
                adjustedSpeed += (speed * 7f) * Time.deltaTime;
                guts.transform.Rotate(Vector3.up * Time.deltaTime * -adjustedSpeed);
            }
            else if (guts.transform.rotation.eulerAngles.y <= 220)
            {
                adjustedSpeed += (speed * 7f) * Time.deltaTime;
                guts.transform.Rotate(Vector3.up * Time.deltaTime * -adjustedSpeed);
            }
           
        }
        else
        {
            adjustedSpeed = speed;
            if (guts.transform.localEulerAngles.y <= (baseRot.eulerAngles.y - give) || guts.transform.localEulerAngles.y >= (baseRot.eulerAngles.y + give))
            {
                guts.transform.Rotate(Vector3.up * Time.deltaTime * (speed * 5));
            }
        }
    }
}
