using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScreenSkill2 : MonoBehaviour
{
    public Color[] screenColors;
    public int affectCount;
    public float timeLimit;
    public Text timeText;
    float timeCurrent;
    public Image bgColor;

    private void OnEnable()
    {
        timeCurrent = timeLimit;
        bgColor.color = screenColors[affectCount%5];
        affectCount++;
    }

    private void Update()
    {
        timeCurrent -= Time.deltaTime;
        if(timeCurrent <= 0)
        {
            gameObject.SetActive(false);
        }

        timeText.text = timeCurrent.ToString("F2");
    }
}
