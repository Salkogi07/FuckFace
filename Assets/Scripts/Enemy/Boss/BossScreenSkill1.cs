using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScreenSkill1 : MonoBehaviour
{
    public GameObject ui;
    public Text timeText;
    public float timeLimit;
    float timeCurrent;

    private void OnEnable()
    {
        timeCurrent = timeLimit;
        ui.SetActive(false);
    }

    private void Update()
    {
        timeCurrent -= Time.deltaTime;
        if (timeCurrent <= 0)
        {
            ui.SetActive(true);
            gameObject.SetActive(false);
        }

        timeText.text = timeCurrent.ToString("F2");
    }
}
