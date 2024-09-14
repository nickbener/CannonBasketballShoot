using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRate : MonoBehaviour {
    
    public void linkRate()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.gamezmonster.cannonbasketball");
    }

    public void ExitThisGame()
    {
        Application.Quit();
    }
}
