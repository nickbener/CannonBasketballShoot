using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _speed = 1.0f;

    private bool _isIncreasing = true;

    public float SliderValue
    {
        get { return _slider.value; }
        set { _slider.value = value; }
    }

    void Update()
    {
        if (_isIncreasing)
        {
            _slider.value += Time.deltaTime * _speed;

            if (_slider.value >= 1.0f)
            {
                _slider.value = 1.0f;
                _isIncreasing = false;
            }
        }
        else
        {
            _slider.value -= Time.deltaTime * _speed;

            if (_slider.value <= 0.0f)
            {
                _slider.value = 0.0f;
                _isIncreasing = true;
            }
        }
    }

    public void StartMovement()
    {
        _slider.value = 0.0f;
        _isIncreasing = true;
    }
}
