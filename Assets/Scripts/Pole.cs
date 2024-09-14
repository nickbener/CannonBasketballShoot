using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour {

    // Use this for initialization
    public GameObject Pole_1_right;
    public GameObject Pole_2_right;
    public GameObject Pole_3_right;
    public GameObject Pole_1_left;
    public GameObject Pole_2_left;
    public GameObject Pole_3_left;
    private Wall WallScript;
    public GameManagerOld GM;
    //  iTween.MoveAdd(gameObject,iTween.Hash("x",5,"time",1f,"easeType",iTween.EaseType.linear,"loopType",iTween.LoopType.pingPong))
    void Start () {
        GM = FindObjectOfType<GameManagerOld>();
        WallScript = FindObjectOfType<Wall>();
       iTween.MoveTo(Pole_2_left, new Vector3(-1.917f, 1, 0), 0.5f);
      // ContinueTwoSuccssgoals();

    }
	public void ContinueTwoSuccssgoals()
    {
        
        if (WallScript.levelchanger == 2)
        {
            iTween.MoveTo(Pole_2_left, new Vector3(-5, 0.915F, 0), 0.5f);
            iTween.MoveTo(Pole_3_left, new Vector3(-1.917f, -1, 0), 0.5f);
        }
         if(WallScript.levelchanger >= 3)
        {
            if(WallScript.levelchanger == 3)
            {
                iTween.MoveTo(Pole_3_left, new Vector3(-1.917f, -1.5f, 0), 0.5f);
            }
            if (WallScript.levelchanger == 5)
            {
                iTween.MoveTo(Pole_3_left, iTween.Hash("x", -1.917f, "y", 1, "time", 3f, "easyType", iTween.EaseType.linear, "loopType", iTween.LoopType.none));
            }
            if (WallScript.levelchanger == 6)
            {
                iTween.MoveTo(Pole_3_left, new Vector3(-1.917f, 3f, 0), 0.5f);
            }
            if (WallScript.levelchanger == 7)
            {
               // iTween.MoveTo(Pole_3_left, new Vector3(-5, -1.5f, 0), 0.5f);
                iTween.MoveTo(Pole_3_left, iTween.Hash("x", -1.917f, "y", -1, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 2f, "loopType", iTween.LoopType.pingPong));
            }
            if (WallScript.levelchanger == 8)
            {
                iTween.MoveTo(Pole_3_left, iTween.Hash("x", -4.66f, "y", -1.5f, "time", 0.5f, "easeType", iTween.EaseType.linear, "speed", 2f, "loopType", iTween.LoopType.none));
                iTween.MoveTo(Pole_1_left, new Vector3(-1.917f, 4, 0), 0.5f);
                iTween.MoveTo(Pole_2_left, new Vector3(-1.917f, 1, 0), 0.5f);

            }
            if (WallScript.levelchanger == 10)
            {
                iTween.MoveTo(Pole_1_left, new Vector3(-1.917f, 2, 0), 0.5f);
                iTween.MoveTo(Pole_2_left, new Vector3(-1.917f, 0, 0), 0.5f);
            }
            if (WallScript.levelchanger == 12)
            {
                iTween.MoveTo(Pole_2_left, new Vector3(-4.917f, 0, 0), 0.5f);
                iTween.MoveTo(Pole_1_left, iTween.Hash("x", -1.917f, "y", 0, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 2.7f, "loopType", iTween.LoopType.pingPong));
                // iTween.MoveTo(Pole_1_left, new Vector3(-1.917f, 2, 0), 0.5f);
                 
            }
            if (WallScript.levelchanger == 14)
            {
                Debug.Log("9leve");
                GM.changePosofShooter1();
                iTween.MoveTo(Pole_1_left, new Vector3(-4.917f, 2, 0), 0.5f);
                iTween.MoveTo(Pole_2_left, new Vector3(-4.917f, 0, 0), 0.5f);
                iTween.MoveTo(Pole_1_right, new Vector3(1.96f, 4, 0), 0.5f);
                iTween.MoveTo(Pole_2_right, new Vector3(1.96f, 1.49f, 0), 0.5f);
                iTween.MoveTo(Pole_3_right, new Vector3(1.96f, -0.91f, 0), 0.5f);
                Invoke("movingrightpoles1", 2.0f);

            }
            if (WallScript.levelchanger == 16)
            {
                iTween.MoveTo(Pole_1_right, new Vector3(3.73f, 4, 0), 0.5f);
                iTween.MoveTo(Pole_2_right, new Vector3(3.73f, 1.49f, 0), 0.5f);
                iTween.MoveTo(Pole_3_right, new Vector3(3.73f, -0.91f, 0), 0.5f);
                Invoke("rightpoleposition", 0.5f);
            }
            if (WallScript.levelchanger == 19)
            {
                GM.changePosofShooter2();
                iTween.MoveTo(Pole_1_right, new Vector3(5.96f, 3.81f, 0), 0.5f);
                iTween.MoveTo(Pole_2_right, new Vector3(4.96f, 2.43f, 0), 0.5f);
                iTween.MoveTo(Pole_3_right, new Vector3(4.96f, 3.73f, 0), 0.5f);
                iTween.MoveTo(Pole_1_left, new Vector3(-1.917f, 2, 0), 0.5f);
                iTween.MoveTo(Pole_2_left, new Vector3(-1.917f, 0, 0), 0.5f);
                Invoke("level12Leftpoles", 0.5f);
            }
            if (WallScript.levelchanger == 21)
            {
                iTween.MoveTo(Pole_1_left, new Vector3(-4.917f, 2, 0), 0.5f);
                iTween.MoveTo(Pole_2_left, new Vector3(-4.917f, 0, 0), 0.5f);
                iTween.MoveTo(Pole_3_left, new Vector3(-1.917f, 0, 0), 0.5f);
                Invoke("level13leftpole", 0.5f);
            }
            if (WallScript.levelchanger == 22)
            {
                iTween.MoveTo(Pole_1_left, new Vector3(-1.917f, 2, 0), 0.5f);
               // iTween.MoveTo(Pole_2_left, new Vector3(-4.917f, 0, 0), 0.5f);
                iTween.MoveTo(Pole_3_left, new Vector3(-4.917f, 0, 0), 0.5f);
                Invoke("level14leftpole", 0.5f);
            }
            if (WallScript.levelchanger == 23)
            {
                iTween.MoveTo(Pole_1_left, new Vector3(-4.917f, 2, 0), 0.5f);
                 iTween.MoveTo(Pole_2_left, new Vector3(-1.917f, 0, 0), 0.5f);
                  iTween.MoveTo(Pole_3_left, new Vector3(-1.917f, 0, 0), 0.5f);
                Invoke("level15leftpole", 0.5f);
            }
            if (WallScript.levelchanger == 25)
            {
                iTween.MoveTo(Pole_1_left, new Vector3(-1.917f, 2, 0), 0.5f);
                iTween.MoveTo(Pole_2_left, new Vector3(-4.917f, 0, 0), 0.5f);
                iTween.MoveTo(Pole_3_left, new Vector3(-4.917f, 0, 0), 0.5f);
                Invoke("level16leftpole", 0.5f);
            }

        }
        
    }
    //FOR LEVEL NO10;
    void movingrightpoles1()
    {
        iTween.MoveTo(Pole_1_right, iTween.Hash("x", 1.96f, "y", 3.5f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 0.5f,"delay",0.5, "loopType", iTween.LoopType.pingPong));
        iTween.MoveTo(Pole_2_right, iTween.Hash("x", 1.96f, "y", 1, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 0.5f, "delay", 0.8, "loopType", iTween.LoopType.pingPong));
        iTween.MoveTo(Pole_3_right, iTween.Hash("x", 1.96f, "y", -1.41f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 0.5f, "delay", 0.3, "loopType", iTween.LoopType.pingPong));
    }
    //FOR LEVEL NO 11;
    void rightpoleposition()
    {
        iTween.MoveTo(Pole_1_right, new Vector3(1.96f, 3.81f, 0), 0.5f);
        iTween.MoveTo(Pole_2_right, new Vector3(1.96f, 2.43f, 0), 0.5f);
        iTween.MoveTo(Pole_3_right, new Vector3(1.96f, 3.73f, 0), 0.5f);
        Invoke("movingrightpoles2", 1.5f);
    }
    //FOR LEVEL NO 11;
    void movingrightpoles2()
    {
        iTween.MoveTo(Pole_1_right, iTween.Hash("x", 1.96f, "y", 3.3f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 0.5f, "delay", 0.8, "loopType", iTween.LoopType.pingPong));
        iTween.MoveTo(Pole_2_right, iTween.Hash("x", 1.96f, "y", 1.3f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 0.5f, "delay", 0.5, "loopType", iTween.LoopType.pingPong));
        iTween.MoveTo(Pole_3_right, iTween.Hash("x", 1.96f, "y", -0.4f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 0.5f, "delay", 0.2, "loopType", iTween.LoopType.pingPong));
    }
    //FOR LEVEL 12
    void level12Leftpoles()
    {
        iTween.MoveTo(Pole_1_left, iTween.Hash("x", -1.917f, "y", 2.28f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 2.2f, "delay", 0.2, "loopType", iTween.LoopType.pingPong));
        iTween.MoveTo(Pole_2_left, iTween.Hash("x", -1.917f, "y", -0.97f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 2f, "delay", 0.9, "loopType", iTween.LoopType.pingPong));
    }
    //forlevel 13
    void level13leftpole()
    {
        iTween.MoveTo(Pole_3_left, iTween.Hash("x", -1.917f, "y", 4f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 2.3f, "delay", 0.2, "loopType", iTween.LoopType.pingPong));
    }
    //forlevel 14
    void level14leftpole()
    {
        iTween.MoveTo(Pole_1_left, iTween.Hash("x", -1.917f, "y", 1f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 2.1f, "delay", 0.2, "loopType", iTween.LoopType.pingPong));
    }
    //forlevel 15
    void level15leftpole()
    {
        iTween.MoveTo(Pole_2_left, iTween.Hash("x", -1.917f, "y", 4f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 1.5f, "delay", 0.5f, "loopType", iTween.LoopType.pingPong));
        iTween.MoveTo(Pole_3_left, iTween.Hash("x", -1.917f, "y", 0.98f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 1.1f, "delay", 0.9f, "loopType", iTween.LoopType.pingPong));
    }
    //forlevel 16
    void level16leftpole()
    {
        iTween.MoveTo(Pole_1_left, iTween.Hash("x", -1.917f, "y", 5.28f, "time", 4f, "easeType", iTween.EaseType.linear, "speed", 2.1f, "delay", 0.3, "loopType", iTween.LoopType.pingPong));
    }


}
