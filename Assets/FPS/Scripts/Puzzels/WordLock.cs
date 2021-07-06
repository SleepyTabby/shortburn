using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WordLock : MonoBehaviour, Interactable
{

   

    [Header("Dial Rotation")]
    [SerializeField] int dialLettersAmount = 26;
    //decide rotation
    [Tooltip("Fill in 1 if rotate around that angle fill in 0 if not rotate around that area")]
    public RotateAroundAxis rotationAxis;
    Vector3 axis;
    
    float angle;
    float nearestDistance = float.MaxValue;
    int nearestDial;

    [Header("Generic Stuff")]
    [SerializeField] Collider col;
    private bool runPuzzle;
    [SerializeField] private Transform puzzleCamLocation;
    TypeWriterEffect textManager;


    [Header("Win condition")]
    [SerializeField] UnityEvent WinEvent;
    bool Finished;
    private bool allowSolve;
    int correctDials = 0;

    //each dial is stored in this and has their own info and variables
    [System.Serializable]
    public class Dial
    {
        public GameObject dial    ;
        public int CURRENTnumb;
        public bool Active = true ;
        public int whichLetter   ;
    }

    //the axis around which to rotate
    [System.Serializable]
    public class RotateAroundAxis
    {
        public string info = "click around which axis should be rotated";
        public bool x;
        public bool y;
        public bool z;
    }

    [Header("Dials")]
    [SerializeField] Dial[] dials;

    //on pressing E activate this code
    public void Interact()
    {
        if (!Finished && allowSolve) // check if you've solved it or if you aren't in range
        {
            textManager.RunningPuzzle(Puzzle.word);
            GameFlowManager.instance.allowPlayerToMove = false;
            runPuzzle = true;
            col.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerCharacterController.instance.moveCamToPosition(puzzleCamLocation.transform.position, puzzleCamLocation.transform.rotation, true);
        }
    }

    //on pressing E or completing the puzzle it will close the puzzle
    private void ClosePuzzle()
    {
        col.enabled = true;
        runPuzzle = false;
        GameFlowManager.instance.allowPlayerToMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerCharacterController.instance.moveCamToPosition(PlayerCharacterController.instance.transform.position + Vector3.up * 1.44f, PlayerCharacterController.instance.transform.rotation, false);
    }

    private void Start()
    {
        textManager = TypeWriterEffect.Instance; //pulls the singleton from dialogue script 
        CheckRotationPoint();
    }

    /// <summary>
    /// check which axis to rotate around and if there has been an error(rotating more that 1 axis)
    /// </summary>
    void CheckRotationPoint()
    {
        int check = 0;
        if (rotationAxis.x)
        {
            check++;
            axis.x = 1;
        }
        if (rotationAxis.y)
        {
            check++;
            axis.y = 1;
        }
        if (rotationAxis.z)
        {
            check++;
            axis.z = 1;
        }
        if(check > 1)
        {
            Debug.LogError("you can rotate around more than 1 axis");
            Debug.Break();
        }
    }
    private void Update()
    {
        if (runPuzzle)
        {
            OnInteractUpdate();//if the puzzle is allowed to run run this update 
            if (PlayerInputHandler.instance.pressedKey(KeyCode.E) == true)
            {
                ClosePuzzle(); //Closes Menu
            }
        }
    }
    
    void OnInteractUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //shoot out a raycast to the colliders used by the puzzle to check if a dial should move up or down
            RaycastHit hit;
            var ray = PlayerCharacterController.instance.PlayerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 3f)) // max range
            {
                if (hit.collider != null) //if nothing is hit then  dont turn the dials 
                {
                    bool up = false;
                    if (hit.collider.name == "up Collider") //if de collider associated with up gets hit by the raycast set up to true
                    {
                        up = true;
                    }
                    for (int i = 0; i < dials.Length; i++)
                    {
                        if (Vector3.Distance(hit.point, dials[i].dial.transform.position) <= nearestDistance)
                        {
                            nearestDistance = Vector3.Distance(hit.point, dials[i].dial.transform.position);
                            nearestDial = i;
                        }
                    }
                    if (up ) // move dial up
                    {
                        dials[nearestDial].dial.transform.rotation = Quaternion.Euler(new Vector3(dials[nearestDial].dial.transform.rotation.eulerAngles.x, dials[nearestDial].dial.transform.rotation.eulerAngles.y, dials[nearestDial].dial.transform.rotation.eulerAngles.z) + (axis * -angle));
                        dials[nearestDial].CURRENTnumb++;
                        
                        if (dials[nearestDial].CURRENTnumb > 9)
                        {
                            dials[nearestDial].CURRENTnumb = 0;
                        }
                        
                        checkEasyComb();
                    }
                    else if ( !up) // move dial down
                    {
                        dials[nearestDial].dial.transform.rotation = Quaternion.Euler(new Vector3(dials[nearestDial].dial.transform.rotation.eulerAngles.x, dials[nearestDial].dial.transform.rotation.eulerAngles.y, dials[nearestDial].dial.transform.rotation.eulerAngles.z) + (axis * angle));
                        dials[nearestDial].CURRENTnumb--;
                       
                        if (dials[nearestDial].CURRENTnumb < 0)
                        {
                            dials[nearestDial].CURRENTnumb = 9;
                        }
                        checkEasyComb();
                    }
                    up = false;
                    nearestDistance = float.MaxValue;
                    nearestDial = 0;
                }
            }
            
        }
    }

    //check if all dials are on the correct rotation 
    void checkEasyComb()
    {
        int cor = 0;
        for (int i = 0; i < dials.Length; i++)
        {
           
            if (dials[i].CURRENTnumb == dials[i].whichLetter)
            {
                cor++;
                Debug.Log("i am supposed to work");
            }
            if(cor >= dials.Length)
            {
                Debug.Log("it worky");
                textManager.CompletedPuzzle(Puzzle.word);
                Finished = true;
                WinEvent.Invoke();
                StartCoroutine(ToMainMenu());
                ClosePuzzle();
            }
            Debug.Log(cor);
        }
        
    }
    

    public void AllowInteraction(bool state)
    {
        allowSolve = state;
    }

    public IEnumerator ToMainMenu()
    {
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene(0);
    }
}