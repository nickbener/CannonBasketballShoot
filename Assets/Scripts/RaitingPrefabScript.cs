using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaitingPrefabScript : MonoBehaviour
{
    [HideInInspector] public int score; // Счёт игрока
    [HideInInspector] public int position; // Позиция игрока
    [HideInInspector] public Sprite avatar; // Аватар игрока
    [HideInInspector] public string nickname; // Никнейм игрока

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
