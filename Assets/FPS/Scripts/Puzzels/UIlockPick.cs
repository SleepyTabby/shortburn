using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class UIlockPick : MonoBehaviour, Interactable
{
    public static UIlockPick instance;

    [SerializeField] GameObject gut;
    [SerializeField] GameObject pick;
    Vector3 gutRot;
    Vector3 pickRot;
    [SerializeField] Vector3 rotAxis;
    [SerializeField] float RotSpeed;
    [Range(0, 360)]
    float acceptableGoalDeviation;
    [SerializeField] float rotWrongPos;
    [SerializeField] float gutMaxRot;

    [Header("generic Stuff")]
    private bool runPuzzle;
    [SerializeField] GameObject lockUnpicked;
    //[SerializeField] Collider col;
    [SerializeField] private Transform puzzleCamLocation;
    private bool allowSolve;


    [Header("win condition")]
    [SerializeField] UnityEvent WinEvent;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }

    void Start()
    {
        acceptableGoalDeviation = Random.Range(15, 180);
    }

    public void Interact()
    {
        GameFlowManager.instance.allowPlayerToMove = false;
        runPuzzle = true;
        lockUnpicked.SetActive(true);
        //col.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //PlayerCharacterController.instance.moveCamToPosition(puzzleCamLocation.transform.position, puzzleCamLocation.transform.rotation, true);

    }
    DoorController doorCrtl;
    public void LockPickInter(DoorController door)
    {
        doorCrtl = door;
        Interact();
    }
   

    private void ClosePuzzle()
    {
        //col.enabled = true;
        runPuzzle = false;
        GameFlowManager.instance.allowPlayerToMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        lockUnpicked.SetActive(false);
        Cursor.visible = false;
        PlayerCharacterController.instance.moveCamToPosition(PlayerCharacterController.instance.transform.position + Vector3.up * 1.44f, PlayerCharacterController.instance.transform.rotation, false);
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
        
        if (Input.GetKey(KeyCode.D))
        {
            pick.transform.Rotate(rotAxis * RotSpeed * Time.deltaTime);
            pickRot += rotAxis * RotSpeed * Time.deltaTime;
            pickRot = FloatToRotation(pickRot);
        }
        if (Input.GetKey(KeyCode.A))
        {
            pick.transform.Rotate(-rotAxis * RotSpeed * Time.deltaTime);
            pickRot -= rotAxis * RotSpeed * Time.deltaTime;
            pickRot = FloatToRotation(pickRot);
        }
        float guts = RotAxisToFloat(gutRot);
        if (Input.GetKey(KeyCode.W) && guts < gutMaxRot)
        {
            float pick = RotAxisToFloat(pickRot);
            //Debug.Log(acceptableGoalDeviation);
            //rotate back 
            if (pick >= (acceptableGoalDeviation -10) && pick <= ((acceptableGoalDeviation + 10)))
            {
                if(guts > 85)
                {
                    WinEvent.Invoke();
                    ClosePuzzle();
                    doorCrtl.ChangeDeurState(false);
                }
                gut.transform.Rotate(rotAxis * RotSpeed * Time.deltaTime);
                gutRot += rotAxis * RotSpeed * Time.deltaTime;
                gutRot = FloatToRotation(gutRot);
            }
            
            else if (guts <= 25)
            {
                gut.transform.Rotate(rotAxis * RotSpeed * Time.deltaTime);
                gutRot += rotAxis * RotSpeed * Time.deltaTime;
                gutRot = FloatToRotation(gutRot);
            }
        }
        else if (!Input.GetKey(KeyCode.W) && guts > 5)
        {
            
                gut.transform.Rotate(-rotAxis * 3 * RotSpeed * Time.deltaTime);
                gutRot -= rotAxis * 3 * RotSpeed * Time.deltaTime;
                gutRot = FloatToRotation(gutRot);
            

        }
    }

    Vector3 FloatToRotation(float x, float y, float z)
    {
        float rotClampedX = x;
        float rotClampedY = y;
        float rotClampedZ = z;

        //clamp x rot
        if (rotClampedX != 0)
        {
            if (rotClampedX > 360)
            {
                rotClampedX = rotClampedX - 360;
            }
            else if (rotClampedX < 0)
            {
                rotClampedX = rotClampedX + 360;
            }
        }

        //clamp y rot
        if (rotClampedY != 0)
        {
            if (rotClampedY > 360)
            {
                rotClampedY = rotClampedY - 360;
            }
            else if (rotClampedX < 0)
            {
                rotClampedY = rotClampedY + 360;
            }
        }

        //clamp x rot
        if (rotClampedZ != 0)
        {
            if (rotClampedZ > 360)
            {
                rotClampedZ = rotClampedZ - 360;
            }
            else if (rotClampedX < 0)
            {
                rotClampedZ = rotClampedZ + 360;
            }
        }

        //return clamped usable rot for unity
        return new Vector3(rotClampedX, rotClampedY, rotClampedZ);
    }
    Vector3 FloatToRotation(Vector3 rot)
    {
        float rotClampedX = rot.x;
        float rotClampedY = rot.y;
        float rotClampedZ = rot.z;

        //clamp x rot
        if (rotClampedX != 0)
        {
            if (rotClampedX > 360)
            {
                rotClampedX = rotClampedX - 360;
            }
            else if (rotClampedX < 0)
            {
                rotClampedX = rotClampedX + 360;
            }
        }

        //clamp y rot
        if (rotClampedY != 0)
        {
            if (rotClampedY > 360)
            {
                rotClampedY = rotClampedY - 360;
            }
            else if (rotClampedY < 0)
            {
                rotClampedY = rotClampedY + 360;
            }
        }

        //clamp x rot
        if (rotClampedZ != 0)
        {
            if (rotClampedZ > 360)
            {
                rotClampedZ = rotClampedZ - 360;
            }
            else if (rotClampedZ < 0)
            {
                rotClampedZ = rotClampedZ + 360;
            }
        }

        //return clamped usable rot for unity
        return new Vector3(rotClampedX, rotClampedY, rotClampedZ);
    }
    float RotAxisToFloat(Vector3 rot)
    {
        float r = 0;
        bool e = false;
        if (rot.x != 0 )
        {
            r = rot.x;
        }
        if (rot.y != 0)
        {
            r = rot.y;
            if (e)
            {
                Debug.LogError("Can't rotate more that 1 axis when using the lockpicking puzzle");
                Debug.Break();
            }
        }
        if (rot.z != 0)
        {
            r = rot.z;
            if (e)
            {
                Debug.LogError("Can't rotate more that 1 axis when using the lockpicking puzzle");
                Debug.Break();
            }
        }
        return r;
    }
}
