using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wall : MonoBehaviour {
    private BasketBall BS;
  public  int countofgoalssuccess;
    public int levelchanger;
    private GameObject temp;
    private ShootBallContainer SbContainerscript;
    private GameManagerOld GM;
    void Start()
    {
        SbContainerscript = FindObjectOfType<ShootBallContainer>();
        GM = FindObjectOfType<GameManagerOld>();
    }
    int gameover = 0;
 
    public void OnCollisionEnter2D(Collision2D collision)
    {     
      BS = collision.gameObject.GetComponent<BasketBall>();

       
        if (collision.gameObject.tag == "Ball")
        {
           // Debug.Log(BS.goalsuccess);
            if (BS.goalsuccess)
            {
                 countofgoalssuccess++;
                if (countofgoalssuccess >= 2)
                {
                   AddingDiffererntBallsAndEffects();
                }
                 levelchanger++;
              //  Invoke("AddingDiffererntBallsAndEffects", 3.5f);
                // Debug.Log(countofgoalssuccess);
                collision.gameObject.tag = "Untagged";
                FindObjectOfType<Pole>().ContinueTwoSuccssgoals();
                  BS.goalsuccess = false;
                
            }
            else
            {
                if(collision.gameObject.tag!= "Untagged")
                {
                    BS.goalsuccess = false;
                    countofgoalssuccess = 0;
                    AddingDiffererntBallsAndEffects();
                    if (BS.goalfailcount == 0)
                    {
                        
                        gameover++;
                        if (gameover == 3)
                            
                        {
                            
                           Debug.Log(gameover);
                           FindObjectOfType<UImanager>().gameover();
                          gameover = 0;
                            

                        }
                        BS.goalfailcount = 1;
                        

                    }
                   
                   
                }
                
            }
        }      
       
    }
    GameObject particleffect;
    bool ballonwall = false;
    private void OnCollisionStay2D(Collision2D collision)
    {
        Invoke("HideBall", 1.5f);
        ballonwall = true;
        temp = collision.gameObject;
        destroyingNormaleeffect();
     //   Debug.Log(collision.gameObject.name);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        ballonwall = false;
    }
    void HideBall()
    {
        if (temp.transform.parent == null)
        {
            if (ballonwall)
            {
                temp.GetComponent<SpriteRenderer>().enabled = false;
                temp.GetComponent<CircleCollider2D>().enabled = false;
                temp.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
            
           
        }
       
    }
    void destroyingNormaleeffect()
    {
        if (temp.transform.childCount == 2)
        {
            if (temp.transform.GetChild(1) != null)
            {
                Destroy(temp.transform.GetChild(1).gameObject);
                
            }
        }

    }
    private GameObject[] balls;
    private GameObject BallParticleeffects;
   public void AddingDiffererntBallsAndEffects()
    {
        balls = GameObject.FindGameObjectsWithTag("Ball");

        if (countofgoalssuccess == 3)
        {
            for(int i = 0; i <= balls.Length-1; i++)
            {
             
                balls[i].GetComponent<SpriteRenderer>().sprite = GM.BALLtextures[0];
                BallParticleeffects = Instantiate(GM.ballparticleeffects[0], balls[i].transform.position, balls[i].transform.rotation);
                BallParticleeffects.transform.SetParent(balls[i].transform);
                
            }
        }
        else if (countofgoalssuccess == 4)
        {
            for (int i = 0; i <= balls.Length - 1; i++)
            {
                if (balls[i].transform.childCount > 1 && balls[i].transform.parent!=null)
                {
                    Destroy(balls[i].transform.GetChild(1).gameObject);
                }
                balls[i].GetComponent<SpriteRenderer>().sprite = GM.BALLtextures[1];
                BallParticleeffects = Instantiate(GM.ballparticleeffects[1], balls[i].transform.position, balls[i].transform.rotation);
                BallParticleeffects.transform.SetParent(balls[i].transform);

            }
        }
        else if (countofgoalssuccess == 5)
        {
            for (int i = 0; i <= balls.Length - 1; i++)
            {
                if (balls[i].transform.childCount > 1 && balls[i].transform.parent != null)
                {
                    Destroy(balls[i].transform.GetChild(1).gameObject);
                }
                balls[i].GetComponent<SpriteRenderer>().sprite = GM.BALLtextures[2];
                BallParticleeffects = Instantiate(GM.ballparticleeffects[2], balls[i].transform.position, balls[i].transform.rotation);
                BallParticleeffects.transform.SetParent(balls[i].transform);

            }
        }
        else if (countofgoalssuccess == 6)
        {
            for (int i = 0; i <= balls.Length - 1; i++)
            {
                if (balls[i].transform.childCount > 1 && balls[i].transform.parent != null)
                {
                    Destroy(balls[i].transform.GetChild(1).gameObject);
                }
                balls[i].GetComponent<SpriteRenderer>().sprite = GM.BALLtextures[3];
                BallParticleeffects = Instantiate(GM.ballparticleeffects[3], balls[i].transform.position, balls[i].transform.rotation);
                BallParticleeffects.transform.SetParent(balls[i].transform);

            }
        }
        else if (countofgoalssuccess == 7)
        {
            for (int i = 0; i <= balls.Length - 1; i++)
            {
                if (balls[i].transform.childCount > 1 && balls[i].transform.parent != null)
                {
                    Destroy(balls[i].transform.GetChild(1).gameObject);
                }
                balls[i].GetComponent<SpriteRenderer>().sprite = GM.BALLtextures[5];
                BallParticleeffects = Instantiate(GM.ballparticleeffects[5], balls[i].transform.position, balls[i].transform.rotation);
                BallParticleeffects.transform.SetParent(balls[i].transform);

            }
        }
        else if (countofgoalssuccess == 8)
        {
            for (int i = 0; i <= balls.Length - 1; i++)
            {
                if (balls[i].transform.childCount > 1 && balls[i].transform.parent != null)
                {
                    Destroy(balls[i].transform.GetChild(1).gameObject);
                }
                balls[i].GetComponent<SpriteRenderer>().sprite = GM.BALLtextures[6];
                BallParticleeffects = Instantiate(GM.ballparticleeffects[6], balls[i].transform.position, balls[i].transform.rotation);
                BallParticleeffects.transform.SetParent(balls[i].transform);

            }
        }
        else if (countofgoalssuccess == 9)
        {
            for (int i = 0; i <= balls.Length - 1; i++)
            {
                if (balls[i].transform.childCount > 1 && balls[i].transform.parent != null)
                {
                    Destroy(balls[i].transform.GetChild(1).gameObject);
                }
                balls[i].GetComponent<SpriteRenderer>().sprite = GM.BALLtextures[2];
                BallParticleeffects = Instantiate(GM.ballparticleeffects[2], balls[i].transform.position, balls[i].transform.rotation);
                BallParticleeffects.transform.SetParent(balls[i].transform);

            }
        }
        else if (countofgoalssuccess >9)
        {
            int ran = Random.Range(0, 7);
            for (int i = 0; i <= balls.Length - 1; i++)
            {
                if (balls[i].transform.childCount > 1 && balls[i].transform.parent != null)
                {
                    Destroy(balls[i].transform.GetChild(1).gameObject);
                }
                balls[i].GetComponent<SpriteRenderer>().sprite = GM.BALLtextures[ran];
                BallParticleeffects = Instantiate(GM.ballparticleeffects[ran], balls[i].transform.position, balls[i].transform.rotation);
                BallParticleeffects.transform.SetParent(balls[i].transform);

            }
        }

        else if(countofgoalssuccess==0 )
        {
           // Debug.Log("check");
            for (int i=0;i<=balls.Length-1; i++)
            {
                balls[i].GetComponent<SpriteRenderer>().sprite = GM.BALLtextures[7];
                if (balls[i].transform.childCount > 1)
                {
                    Destroy(balls[i].transform.GetChild(1).gameObject);
                }
               
            }
        }
        
        

        
    }

}
