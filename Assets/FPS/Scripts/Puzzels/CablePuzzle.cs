using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class CablePuzzle : MonoBehaviour, Interactable //Made By Wesley
{
    private bool runPuzzle;
    private bool puzzleFinished;
    [Header("Puzzle Settings")]
    [SerializeField] private Transform puzzleCamLocation;
    [SerializeField] private LayerMask puzzleLayer;
    [SerializeField] private GameObject[] wiresStartPoint;
    [SerializeField] private GameObject[] wiresEndPoint;
    [SerializeField] private LineRenderer[] wires;
    private int CableSelected;

    private int[] combination;
    [SerializeField] private int[] goodCombination;

    [SerializeField] private UnityEvent finishEvent;

    [Header("Audio")]
    [SerializeField] private StudioEventEmitter cableGrab;
    [SerializeField] private StudioEventEmitter cableConnect;

    public Vector3 TEMPVECTOR;

    TypeWriterEffect textManager;

    public void Interact() //Puzzle Interaction (Start)
    {
        if (!puzzleFinished)
        {
            textManager.RunningPuzzle(Puzzle.wires);
            GameFlowManager.instance.allowPlayerToMove = false;
            runPuzzle = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerCharacterController.instance.moveCamToPosition(puzzleCamLocation.transform.position, puzzleCamLocation.transform.rotation, true);
        }
    }

    private void ClosePuzzle() //Puzzle Close (Stop)
    {
        runPuzzle = false;
        GameFlowManager.instance.allowPlayerToMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerCharacterController.instance.moveCamToPosition(PlayerCharacterController.instance.transform.position + Vector3.up * 1.44f, PlayerCharacterController.instance.transform.rotation, false);
    }

    private void Start()
    {
        textManager = TypeWriterEffect.Instance;
        combination = new int[goodCombination.Length];
        CableSelected = -1;
        for (int i = 0; i < wires.Length; i++) //Updates all Wires (Position & Color)
        {
            wires[i].SetPosition(0, wiresStartPoint[i].transform.localPosition);
            wires[i].SetPosition(1, wiresStartPoint[i].transform.localPosition);
            Color32 cableColor = wiresStartPoint[i].GetComponent<MeshRenderer>().material.color;
            wires[i].startColor =cableColor;
            wires[i].endColor =cableColor;
        }
    }

    void Update()
    {
        if (runPuzzle) { 
        Ray ray = PlayerCharacterController.instance.PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f, puzzleLayer)) {
                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = 0; i < wiresStartPoint.Length; i++) //Checks if RayCast hits Connector.
                    {
                        if (hit.transform.gameObject == wiresStartPoint[i])
                        {
                            cableGrab.Play();
                            CableSelected = i;
                            break;
                        }
                    }
                }
            }

            if (CableSelected != -1) //Runs when a cable is selected.
            {
                bool complete = false;
                if (hit.collider != null) wires[CableSelected].SetPosition(1, hit.transform.localPosition);
                #region removed
                //else
                //{
                //    Vector3 mousePos = PlayerCharacterController.instance.PlayerCamera.ScreenToWorldPoint(Input.mousePosition);
                //    TEMPVECTOR = mousePos;
                //    //mousePos.z = mousePos.x;
                //    //mousePos.x = -.732f;
                //    Vector3 calc = mousePos;
                //    calc -= transform.position;
                //    wires[CableSelected].SetPosition(1, calc);
                //}
                #endregion

                if (Input.GetMouseButtonUp(0)) //When the player releases the Left MouseButton the position is getting checked for valid position.
                {
                    for (int i = 0; i < wiresEndPoint.Length; i++)
                    {
                        if (hit.collider != null && hit.transform.gameObject == wiresEndPoint[i])
                        {
                            cableConnect.Play();
                            combination[CableSelected] = i + 1;
                            wires[CableSelected].SetPosition(1, wiresEndPoint[i].transform.localPosition);
                            complete = true;
                        }
                    }
                    if (!complete) wires[CableSelected].SetPosition(1, wiresStartPoint[CableSelected].transform.localPosition); //When the cable isn't connected it will be reset.
                    CableSelected = -1;

                    for (int i = 0; i < combination.Length; i++) //Checks if every wire is assigned a connector.
                    {
                        if (combination[i] == 0)
                        {
                            break;
                        }
                        if (i == combination.Length - 1)
                        {
                            CheckCombination();
                        }
                    }
                }
            }

            if (PlayerInputHandler.instance.pressedKey(KeyCode.E)) ClosePuzzle(); //Closes Menu
        }
    }

    private void CheckCombination() //Checks the Puzzle Combination.
    {
        int goodComb = 0;
        for (int i = 0; i < combination.Length; i++)
        {
            if (combination[i] != goodCombination[i]) break;
            goodComb++;
        }
        if (goodComb == goodCombination.Length)
        {
            textManager.CompletedPuzzle(Puzzle.wires);
            puzzleFinished = true;
            finishEvent.Invoke();
            ClosePuzzle();
        }
    }
}
