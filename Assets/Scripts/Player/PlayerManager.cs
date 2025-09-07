using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] private FollowCamera[] cameras;

    public PlayerMovement[] players;
    private int selectPlayer;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public void SetUp()
    {
        SetPlayer(0);
    }

    private void SetPlayer(int index)
    {
        for(int i = 0; i < players.Length; i++)
        {
            if(index == i)
                players[i].isAI = false;
            else
                players[i].isAI = true;
        }

        selectPlayer = index;

        foreach(var cam in cameras)
        {
            cam.SetTarget(players[selectPlayer].gameObject);
        }

        UIManager.Instance.ShowMsg((selectPlayer + 1) + "�� �÷��̾ ���õǾ����ϴ�.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPlayer(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPlayer(2);
        }

        float dieCount = 0;
        foreach(var playerDie in players)
        {
            if (playerDie.GetComponent<PlayerStats>().isDie)
            {
                dieCount++;
            }
            if(dieCount >= 3)
            {
                UIManager.Instance.GameOver();
            }
        }
    }

    public GameObject GetMasterPlayer() => players[selectPlayer].gameObject;
}
