using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Audiomanager : MonoBehaviour {
    public Button Sound_button;
    public Sprite soundoff;
    public Sprite soundon;
    public AudioSource myaudio;
    public AudioClip shootsound;
    public AudioClip goalsuccess;
    // Use this for initializationv
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if(SceneManager.GetActiveScene().name == "StartScene")
        {
            if (PlayerPrefs.GetInt("Sound", 1) == 1)
            {
                AudioListener.volume = 1;
                Sound_button.GetComponent<Image>().sprite = soundon;
            }
            else
            {

                AudioListener.volume = 0;
                Sound_button.GetComponent<Image>().sprite = soundoff;
            }
        }
    }
     
	// Update is called once per frame
	void Update () {
		
	}
public void Sound()
{
    if (PlayerPrefs.GetInt("Sound", 1) == 1)
    {
        PlayerPrefs.SetInt("Sound", 0);
        Sound_button.GetComponent<Image>().sprite = soundoff;
        AudioListener.volume = 0;

    }
    else
    {
        PlayerPrefs.SetInt("Sound", 1);
        Sound_button.GetComponent<Image>().sprite = soundon;
        AudioListener.volume = 1;
    }
   
}
    public void myshootsound()
    {
        myaudio.PlayOneShot(shootsound);
    }
    public void mygoalsound()
    {
        myaudio.PlayOneShot(goalsuccess);
    }
}
