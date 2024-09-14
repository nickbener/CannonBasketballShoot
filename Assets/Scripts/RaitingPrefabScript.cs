using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaitingPrefabScript : MonoBehaviour
{
    [HideInInspector] public int score; // ���� ������
    [HideInInspector] public int position; // ������� ������
    [HideInInspector] public Sprite avatar; // ������ ������
    [HideInInspector] public string nickname; // ������� ������

    public Image img;
    public Text scoreText;
    public Text positionText;
    public Text nicknameText;

    private void Update()
    {
        img.sprite = avatar;
        scoreText.text = score.ToString();
        positionText.text = position.ToString();
        nicknameText.text = nickname;
    }
}
