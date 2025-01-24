using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] GameObject InfoPopUp;

    public void PlayButtonClick()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void InfoButtonClick()
    {
        InfoPopUp.SetActive(true);
    }
    public void PopUpExitButtonClick()
    {
        InfoPopUp.SetActive(false);
    }
}
