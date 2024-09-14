using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShootBallContainer : MonoBehaviour {

    // Use this for initialization
    public Vector3 ball1pos, ball2pos, ball3pos, ballScale;
    public GameObject BgOfRotator;
    int BallCount;
    public bool StopShooting;
    public List<GameObject> MyGoalSuccessBalls;
    public GameManagerOld Gm;
    private GameObject Currentshootedball;

	void Start () {
        Gm = FindObjectOfType<GameManagerOld>();
        
        StopShooting = true;
        ball1pos = transform.GetChild(0).localPosition;
        ball2pos = transform.GetChild(1).localPosition;
        ball3pos = transform.GetChild(2).localPosition;
        ballScale = transform.GetChild(0).localScale;
     
    }



    int temp = 0;
	// Update is called once per frame
	void Update () {
        
        if (StopShooting)
        {
            if (transform.childCount != 0)
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    FindObjectOfType<Audiomanager>().myshootsound();
                    FindObjectOfType<TrignometricRotation>().RotationFrequency = new Vector3(0, 0, 0);
                    transform.GetChild(0).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    var dire = transform.TransformDirection(0, 0, 100);
                    transform.GetChild(0).GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.right * 1150);
                    Currentshootedball=transform.GetChild(0).gameObject;
                    transform.GetChild(0).parent = null;
                    Normaleffect();
                    BallCount = transform.childCount;
                    
                    Invoke("ChangePosBall", 2.2f);
                    StopShooting = false;
                }
               // Myplaymode = true;
            }
           // else
            //{
            // Myplaymode = false;
            // Invoke("gameover", 2.5f);
                
            //}
        }
       
       
	}
   // bool Myplaymode;
    
    void ChangePosBall()
    {
        StopShooting = true;
        FindObjectOfType<TrignometricRotation>().RotationFrequency = new Vector3(0, 0, 1.5f);
       
        if (BallCount == 2 )
            {
                transform.GetChild(0).localPosition = Vector3.Slerp(transform.GetChild(0).localPosition, ball1pos, 1.5f);
                transform.GetChild(1).localPosition = Vector3.Slerp(transform.GetChild(1).localPosition, ball2pos, 1.5f);
            }
            else if (BallCount == 1)
            {
                transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, ball1pos, 1.5f);
            }
            else
            {
            // Debug.Log("Try again");
            SceneManager.LoadScene("GameOver");
        }
              
    }
    
    public void Normaleffect()
    {
        if (Currentshootedball.transform.childCount==1 && Currentshootedball.transform.parent==null)
        {
            GameObject effect = Instantiate(Gm.normalEffect, Currentshootedball.transform.position, Currentshootedball.transform.rotation);
            effect.transform.SetParent(Currentshootedball.transform);
        }
        
       
    }
}
