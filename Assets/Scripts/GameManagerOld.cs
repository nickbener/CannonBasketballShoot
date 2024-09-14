using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerOld : MonoBehaviour {

    public GameObject shooter;

    public GameObject BgOfRotator;
    public List<Sprite> BALLtextures = new List<Sprite>();
    public List<GameObject> ballparticleeffects = new List<GameObject>();
    public GameObject normalEffect;
    // Use this for initialization
    void Start () {

        
	}

    // Update is called once per frame
    public void changePosofShooter1()
    {
        FindObjectOfType<TrignometricRotation>().StartRotation = new Vector3(0, 0, 135);
        shooter.transform.position = new Vector3(2.8f, -3.96f, 0);
        shooter.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 135.0f);
        BgOfRotator.transform.position = new Vector3(2.78f,-4.01f,0);

    }
    public void changePosofShooter2()
    {
        FindObjectOfType<TrignometricRotation>().StartRotation = new Vector3(0, 0, 45);
        shooter.transform.position = new Vector3(-2.8f, -4.04f, 0);
        shooter.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
        BgOfRotator.transform.position = new Vector3(-2.82f, -4.01f, 0);
    }
}
