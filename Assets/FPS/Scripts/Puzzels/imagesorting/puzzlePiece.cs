using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles moving the puzzle pieces and snapping them to place.
/// </summary>
public class puzzlePiece : MonoBehaviour
{
    [SerializeField] private Vector3 _correctPosition;
    
    private float startingY;

    private ImageSorting imgSorting;
    
    public bool isInPosition;

    public Vector3 worldPos;
    
    /// <summary>
    /// called while user is holding mouse down on puzzle piece.
    /// </summary>
    private void MouseHold()
    {
        if (imgSorting.isActive)
        {
            //Debug.Log("eeoooeeee");

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
        if (imgSorting.isActive)
        {
            MouseHold();
        }
        
    }

    /// <summary>
    /// Try snapping piece to nearest position
    /// </summary>
    private void TrySnapping()
    {
        if (imgSorting.isActive)
        {
            foreach (var GridPoint in imgSorting.PuzzleSnappingGrid)
            {
                if (Vector3.Distance(transform.position, GridPoint) < imgSorting.snappingDistance)
                {
                    transform.position = GridPoint;
                    worldPos = transform.position;
                    if (GridPoint == _correctPosition)
                    {
                        isInPosition = true;
                        imgSorting.CheckPieces();
                    }
                    else
                    {
                        isInPosition = false;
                    }
                }
            }

        }
    }
    
    /// <summary>
    /// On mouse up event
    /// </summary>
    private void OnMouseUp()
    {
        if (imgSorting.isActive)
        {
            TrySnapping();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startingY = transform.position.y;
        imgSorting = transform.root.GetComponent<ImageSorting>();

        float currentClosest = Mathf.Infinity;
        for (int i = 0; i < imgSorting.PuzzleSnappingGrid.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, imgSorting.PuzzleSnappingGrid[i]);
            if (dist < currentClosest)
            {
                currentClosest = dist;
                _correctPosition = imgSorting.PuzzleSnappingGrid[i];
            }
        }

        transform.position = GameObject.Find("PiecePile").transform.position +
                              new Vector3(Random.Range(-0.45f, 0.35f), Random.Range(0, 0.01f), Random.Range(-0.25f, 0.25f));
    }
}
