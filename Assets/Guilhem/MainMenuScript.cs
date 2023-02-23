using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Animator CameraAnimator;
    public GameObject Buttons;
    public GameObject settingsMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void SettingsMenu()
    {
        StartCoroutine(SettingsMenuPressed());
    }

    public void Back()
    {
        StartCoroutine(SettingsMenuDesactivation());
    }

    IEnumerator SettingsMenuPressed()
    {
        Buttons.SetActive(false);
        CameraAnimator.SetBool("IsPressed", true);
        yield return new WaitForSeconds(1.75f);
        settingsMenu.SetActive(true);
    }

    IEnumerator SettingsMenuDesactivation()
    {
        settingsMenu.SetActive(false);
        CameraAnimator.SetBool("IsPressed", false);
        yield return new WaitForSeconds(1.75f);
        Buttons.SetActive(true);
    }
}
