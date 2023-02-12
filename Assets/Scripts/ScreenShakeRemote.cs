using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeRemote : MonoBehaviour
{
    public void AddTrauma(float trauma)
    {
        GameManager.Instance.GetGlobalComponent<ScreenShaker>().AddTrauma(trauma);
    }
}
