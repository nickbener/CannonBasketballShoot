using System;
using System.Collections;
using System.Collections.Generic;
//using Managers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UImanager : MonoBehaviour {

    // Use this for initialization
    public Animator goalanim;
    bool goal;
    public Text TEXT_topScore;
    public Text Text_middlescore;
    public Text Text_Stars;
    public int Screentopscore;
    public int screenmiddlescore;
    public int StarsCount;
    public Image fillamount;
    
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _rateButton;

    private static bool InterstitialsEnabled()
    {
        return !Application.isEditor && SceneManager.GetActiveScene().name == "GameOver";
    }

    private void OnEnable()
    {
        //if (InterstitialsEnabled())
        //{
        //    _playButton.onClick.AddListener(AdsManager.Instance.ShowInterstitial);
        //    _rateButton.onClick.AddListener(AdsManager.Instance.ShowInterstitial);
        //}
    }

    void Start () {
        goal = false;
        TEXT_topScore.text=PlayerPrefs.GetInt("ScreenTopScore", Screentopscore).ToString();
        Text_Stars.text=PlayerPrefs.GetInt("StarsCount", StarsCount).ToString();
	}
	
    public void OnRetryButtonPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MyStarsStars()
    {
        
        Text_Stars.text = StarsCount.ToString();
        if (PlayerPrefs.GetInt("StarsCount") < StarsCount)
        {
            PlayerPrefs.SetInt("StarsCount", StarsCount);
            Text_Stars.text = StarsCount.ToString();
        }  
    }
    public void MyScore()
    {
        Text_middlescore.text = screenmiddlescore.ToString();
        if (PlayerPrefs.GetInt("ScreenTopScore") < screenmiddlescore)
        {
            PlayerPrefs.SetInt("ScreenTopScore", screenmiddlescore);
            TEXT_topScore.text = screenmiddlescore.ToString();
        }
    }
    public void Goalefefct()
    {
        
        if (goal == false)
        {
            goalanim.SetBool("goal", goal);
            goal = true;
            
        }
        else if (goal == true)
        {
            goalanim.SetBool("goal", goal);
            goal = false;
        }


    }
    public void heath()
    {
        fillamount.fillAmount += 0.1f;
    }

    public void onPlayButtonPress()
    {
        //AdManager.instance.showInterstitial();
        SceneManager.LoadScene("1");
    }
    public void gameover()
    {

        //SceneManager.LoadScene("StartScene");*
        SceneManager.LoadScene("GameOver");

    }

    private void OnDisable()
    {
        //if (InterstitialsEnabled())
        //{
        //    _playButton.onClick.RemoveListener(AdsManager.Instance.ShowInterstitial);
        //    _rateButton.onClick.RemoveListener(AdsManager.Instance.ShowInterstitial);
        //}
    }
}