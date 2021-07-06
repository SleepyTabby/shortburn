using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebusPieces : MonoBehaviour
{
    private float startingY;

    private RebusPuzzle rebus;
    
    /// <summary>
    /// called while user is holding mouse down on puzzle piece.
    /// </summary>
    private void MouseHold()
    {
        if (rebus.isActive)
        {
            Vector3 newPos = transform.position;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono), out hit))
            {
                newPos = hit.point;
                newPos.y = startingY;
            }
            transform.position = newPos;
        }
    }

    /// <summary>
    /// On mouse drag event
    /// </summary>
    private void OnMouseDrag()
    {
        if (rebus.isActive)
        {
            MouseHold();
        }
    }

    
    
    /// <summary>
    /// On mouse up event
    /// </summary>
    

    // Start is called before the first frame update
    void Start()
    {
        rebus = transform.root.GetComponent<RebusPuzzle>();
        startingY = transform.position.y;
    }
}
