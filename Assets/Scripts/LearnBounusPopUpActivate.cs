using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnBounusPopUpActivate : MonoBehaviour
{
    [SerializeField] private GameObject LearnShadowPopUp;

    public void DisableLearnPopAup()
    {
        LearnShadowPopUp.SetActive(false);
    }
}
