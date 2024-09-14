using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public int loadingRnd;
    public Slider loadingSlider;
    public Text loadingText;
    public GameObject LoadCanvas;
    private float loading;

    private void Update()
    {
        loading += 1f;
        loadingRnd = Mathf.RoundToInt(loading);
        loadingSlider.value = loadingRnd;
        if(loading >= 100)
        {
            LoadCanvas.SetActive(false);
        }
        loadingText.text = loadingRnd.ToString(loadingRnd + "%");
    }
}