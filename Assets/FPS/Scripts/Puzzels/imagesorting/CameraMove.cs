using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float yOffSet;

    private float timeIndex;

    public void MoveCamera(GameObject go)
    {
        StartCoroutine(Move(go));
    }
    
    private IEnumerator Move(GameObject go)
    {
        Debug.Log("beniis " + timeIndex);
        timeIndex += Time.deltaTime;
        Vector3 pos = go.transform.position;
        Vector3.Lerp(pos, transform.position, timeIndex);
        go.transform.position = pos;
        yield return null;
        StartCoroutine(Move(go));
    }
}
