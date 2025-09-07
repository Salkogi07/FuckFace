using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public GameObject[] subUI;

    public void MoveToScene(string sceneName)
    {
        Debug.Log(sceneName + "���� �̵�");
        SceneManager.LoadScene(sceneName);
    }

    public void OnClickOpenUI(int index)
    {
        subUI[index].gameObject.SetActive(true);
    }

    public void OnClickCloseUI(int index)
    {
        subUI[index].gameObject.SetActive(false);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
