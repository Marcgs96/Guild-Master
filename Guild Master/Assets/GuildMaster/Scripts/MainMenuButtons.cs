using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuButtons : MonoBehaviour
{
    public AudioMixer mixer;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetVolume(float value)
    {
        mixer.SetFloat("Master", value);
    }
}
