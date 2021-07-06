using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_DeurOpener : MonoBehaviour
{
    public void DeurOpener(float rot)
    {
        transform.rotation = Quaternion.Euler(0, rot, 0);
    }
}
