using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;

    private float _scroll_pos = 0;
    private float[] pos;

    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }


        if (Input.GetMouseButton(0))
        {
            _scroll_pos = _scrollbar.value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (_scroll_pos < pos[i] + (distance / 2) && _scroll_pos > pos[i] - (distance / 2))
                {
                    _scrollbar.value = Mathf.Lerp(_scrollbar.value, pos[i], 0.1f);
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            if (_scroll_pos < pos[i] + (distance / 2) && _scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
}
