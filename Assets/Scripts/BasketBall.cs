using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : MonoBehaviour {

   public bool goalsuccess;
    public int goalfailcount;
    void Start()
    {
        goalfailcount = 0;
        goalsuccess = false;
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoalPointer")
        {
            goalsuccess = true;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stars")
        {
            FindObjectOfType<UImanager>().StarsCount += 1;
            FindObjectOfType<UImanager>().MyStarsStars();
            Destroy(collision.gameObject);
        }
    }
}
