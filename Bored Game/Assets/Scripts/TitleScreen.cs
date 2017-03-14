using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {
    public Button PlayGame;
    public Button ShowInstructions;
    public Button QuitGame;
    public Button MenuButton;
    public Text InstructionText;
    public RawImage UNO;
	

	public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Instructions()
    {
        InstructionText.gameObject.SetActive(true);
        MenuButton.gameObject.SetActive(true);
        PlayGame.gameObject.SetActive(false);
        QuitGame.gameObject.SetActive(false);
        ShowInstructions.gameObject.SetActive(false);
        UNO.gameObject.SetActive(false);
    }

    public void Menu()
    {
        InstructionText.gameObject.SetActive(false);
        MenuButton.gameObject.SetActive(false);
        PlayGame.gameObject.SetActive(true);
        QuitGame.gameObject.SetActive(true);
        ShowInstructions.gameObject.SetActive(true);
        UNO.gameObject.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
