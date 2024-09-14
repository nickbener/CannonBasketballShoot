using System;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("[Tester] OnApplicationPause status: " + pauseStatus);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("[Tester] OnApplicationFocus hasFocus: " + hasFocus);
    }
}