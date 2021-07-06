using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LigthHandler : MonoBehaviour
{
    [SerializeField] private List<int> lights;
    [SerializeField] private Light lightObj;

    public void LightStarter()
    {
        StartCoroutine(lightChanger());
    }

    private IEnumerator lightChanger()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                for (int g = 0; g < lights[i]; g++)
                {
                    lightObj.enabled = true;
                    yield return new WaitForSeconds(.3f);
                    lightObj.enabled = false;
                    yield return new WaitForSeconds(.3f);
                }
                yield return new WaitForSeconds(1.3f);
            }
            yield return new WaitForSeconds(3);
        }
    }

}
