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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (volumePanel.activeSelf || creditPanel.activeSelf))
        {
            volumePanel.SetActive(false);
            creditPanel.SetActive(false);
        }
    }

    public void OnPlayGame() => SceneManager.LoadScene(1);

    public void OnQuitGame() => Application.Quit();

    public void OnVolumePanel() => volumePanel.SetActive(true);

    public void OnCreditPanel() => creditPanel.SetActive(true);
}
