using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalpointer : MonoBehaviour {

    private ShootBallContainer BSscript;
    private Pole PoleScript;

    private GameObject Ball;
    public GameObject ExtraBall;
    public GameObject nextball;
    public GameObject temp1,temp2,temp3;
    public string Ballname;
    private int CountofBalls;
    public int SuccessgoalsCount;
    private bool extragoalsuccess;
    private int ball1count, ball2count, ball3count;
    public UImanager ui;
    bool Goalsuccess;

	void Start () {
        ui = FindObjectOfType<UImanager>();
        Invoke("Storingballs", 0.3f);
        SuccessgoalsCount = 0;

        ball1count = 0;
        ball2count = 0;
        ball3count = 0;
        extragoalsuccess = false;
        Goalsuccess = false;
        BSscript = FindObjectOfType<ShootBallContainer>();
        PoleScript = FindObjectOfType<Pole>();
	}
    void Storingballs()
    {
        temp1 = BSscript.transform.GetChild(0).gameObject;
        temp2 = BSscript.transform.GetChild(1).gameObject;
        temp3 = BSscript.transform.GetChild(2).gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            FindObjectOfType<Audiomanager>().mygoalsound();
            ui.screenmiddlescore += 6;
            ui.MyScore();
            ui.Goalefefct();
            FindObjectOfType<UImanager>().heath();
             Ball = collision.gameObject;
            
            if (collision.gameObject.name == "1")
            {
                temp1 = collision.gameObject;
                ExtraBall = temp2;
            }
            if (collision.gameObject.name == "2")
            {
                temp2 = collision.gameObject;
                ExtraBall = temp3;
            }
            if (collision.gameObject.name == "3")
            {
                temp3 = collision.gameObject;
                ExtraBall = temp1;
            }

            Invoke("GoalSuccesBall", 2.0f);
            
            if (BSscript.transform.childCount == 0)
            {
                Debug.Log("child1");
                if (collision.gameObject.name == "1")
                {
                    ball1count++;

                    if (ball1count == 2)
                    {
                        Goalsuccess = false;

                        Invoke("AddingExtrabBall", 2.0f);
                        ball1count = 0;
                    }
                    ball2count = 0;
                    ball3count = 0;
                }
                if (collision.gameObject.name == "2")
                {
                    ball2count++;

                    if (ball2count == 2)
                    {
                        Goalsuccess = false;
                        Invoke("AddingExtrabBall", 2.0f);
                        ball2count = 0;
                    }
                    ball1count = 0;
                    ball3count = 0;
                }
                if (collision.gameObject.name == "3")
                {
                    ball3count++;

                    if (ball3count == 2)
                    {
                        Goalsuccess = false;
                        Invoke("AddingExtrabBall", 2.0f);
                        ball3count = 0;
                    }
                    ball1count = 0;
                    ball2count = 0;
                }
            }
          

        }
        else
        {
         //   SuccessgoalsCount = 0;
           // Debug.Log(SuccessgoalsCount);

        }
      
    }
    private void Update()
    {
        if (Goalsuccess)
        {
            Debug.Log("Update GS");
            
          //  Debug.Log(BSscript.transform.childCount);
            if (BSscript.transform.childCount == 1)
            {               
                Ball.transform.localPosition = Vector3.Slerp(Ball.transform.localPosition, BSscript.ball1pos, Time.deltaTime * 10.0f);   
                Invoke("Goalfail", 1);
            }  
            else if (BSscript.transform.childCount==2)
            {
                Ball.transform.localPosition = Vector3.Slerp(Ball.transform.localPosition, BSscript.ball2pos, Time.deltaTime * 10.0f);
                Invoke("Goalfail", 1);
            }
            else if (BSscript.transform.childCount == 3)
            {   
                Ball.transform.localPosition = Vector3.Slerp(Ball.transform.localPosition, BSscript.ball3pos, Time.deltaTime * 10.0f);
                Invoke("Goalfail", 1);
            }
        }
        else
        {
           
            if (extragoalsuccess)
            {
               // Debug.Log("two contine");
                Ball.transform.localPosition = Vector3.Slerp(Ball.transform.localPosition, BSscript.ball1pos, Time.deltaTime * 10.0f);
                ExtraBall.transform.localPosition = Vector3.Slerp(ExtraBall.transform.localPosition, BSscript.ball2pos, Time.deltaTime * 10.0f);
                Invoke("Goalfail", 1);
            }
        }
    }
    public void GoalSuccesBall()
    {
 
        if (BSscript.transform.childCount<=3)
        {
            if (Ball.transform.childCount > 1)
            {
                Destroy(Ball.transform.GetChild(1).gameObject);
            }
            Ball.GetComponent<SpriteRenderer>().enabled = true;
            Ball.GetComponent<CircleCollider2D>().enabled = true;
           
            Ball.gameObject.tag = "Ball";
            Ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //  Ball.transform.SetParent(BSscript.transform);
            Ball.transform.SetParent(BSscript.transform);
            Ball.transform.localPosition = nextball.transform.position;
            Ball.transform.localScale = BSscript.ballScale;
            Ball.transform.localEulerAngles = Vector3.zero;
            Goalsuccess = true;
        }
       
    }
    public void Goalfail()
    {
        Goalsuccess = false;
        extragoalsuccess = false;
    }
    public void AddingExtrabBall()
    {
        if (BSscript.transform.childCount <=3)
        {
            ui.screenmiddlescore += 8;
            ui.MyScore();
            Goalsuccess = false;
            if (Ball.transform.childCount > 1)
            {
                Destroy(Ball.transform.GetChild(1).gameObject);
            }
            Ball.GetComponent<SpriteRenderer>().enabled = true;
            Ball.GetComponent<CircleCollider2D>().enabled = true;
            
            ExtraBall.gameObject.tag = "Ball";
            ExtraBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //  ExtraBall.transform.SetParent(BSscript.transform);
            Ball.transform.SetParent(BSscript.transform);
            ExtraBall.transform.localPosition = nextball.transform.position;
            ExtraBall.transform.localScale = BSscript.ballScale;
            ExtraBall.transform.localEulerAngles = Vector3.zero;
            extragoalsuccess = true;
        }
        

    }
  
}
