using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject volumePanel;
    [SerializeField]
    private GameObject creditPanel;

    public void OnPlayGame() => SceneManager.LoadScene(1);

    public void OnQuitGame() => Application.Quit();
}
