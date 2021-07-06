using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("rotation settings")]
    [SerializeField] float maxRot = 260;
    [SerializeField] float minRot = 80;
    [SerializeField] float rotOffset;
    [SerializeField] float distanceOffsSw;// the distance needed to switch the offset from the front to behind of the door
    [SerializeField] float rotSpeed;
    bool active;
    bool door;
    float angle;

    [Header("player Settings")]
    [SerializeField] Transform playerLoc;
    Vector3 probe;
    [SerializeField] float doorProbeOffset; //if the player is interacting with the door decides to put the point the door moves towards with an offset before or behind the door 
    [SerializeField] float minDistForActive;//minimal distance to activate the door and this gets checked when pressing mouse button together with this variable 
    [SerializeField] float maxRay;
    RaycastHit hit;

    [Header("hinge")]
    [SerializeField] Transform hinge;
    [SerializeField] Transform lerpHinge;

    [Header("camera")]
    [SerializeField] Camera cam;

    [Header("Debug")]
    [SerializeField] bool GetRot;
    
    void Start()
    {
        playerLoc = PlayerCharacterController.instance.transform;
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (active)
        {
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, maxRay))
            {
                if(hit.collider.name == "door")
                {
                    door = true;
                }
                
            }
            if (door)
            {
                probe = hit.point + (playerLoc.transform.forward * doorProbeOffset);
                lerpHinge.transform.LookAt(probe); // make custom look at funciton 
                lerpHinge.transform.rotation = Quaternion.Euler(0, lerpHinge.transform.rotation.eulerAngles.y + rotOffset, 0);
                angle = lerpHinge.rotation.eulerAngles.y;
                if (angle > minRot && angle < 170)
                {
                    angle = minRot;
                }
                if (angle < maxRot && angle > 140)
                {
                    angle = maxRot;
                }
                lerpHinge.transform.rotation = Quaternion.Euler(0, angle, 0);
                hinge.rotation = Quaternion.Slerp(hinge.rotation, lerpHinge.rotation, 0.1f); // interpolate between lerp hinge and hinge
            }
        }
    }

    //lerp the door to look towards the 3d point in space that is ... in front of the player 
    void Update()
    {
        if (Input.GetMouseButton(0) && Vector3.Distance(playerLoc.transform.position, transform.position) <= minDistForActive) // make own distance function for lolz make own engine portf
        {
            if (!active)
            {
                active = true;
            }
        }
        else
        {
            active = false;
            door = false;
        }
        if (GetRot)
        {
            Debug.Log(hinge.eulerAngles.y);
            GetRot = false;
        }

        //if (Input.GetMouseButton(0))
        //{
        //    resetDial = false;
        //    RaycastHit hit;
        //    var ray = cam.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hit)) // max range
        //    {
        //        if (hit.collider != null)
        //        {
        //            if (Vector3.Distance(hit.collider.gameObject.transform.position, player.transform.position) <= 5)
        //            {
        //                dial.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
        //                dial.transform.rotation = Quaternion.Euler(0, dial.transform.rotation.eulerAngles.y + rotOffset, 0);
        //            }
        //        }
        //    }
        //}
    }
}
