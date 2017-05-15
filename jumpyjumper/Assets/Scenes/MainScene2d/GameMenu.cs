using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

	public Button pauseButton;

	public Button restartButton;
	public Button resumeButton;

	public CanvasGroup menu;
	public CanvasGroup hud;

	void Start()
	{
		ResumeGame ();

		pauseButton.onClick.AddListener (delegate() {
			PauseGame();
		});

		restartButton.onClick.AddListener (delegate() {
			RestartGame();
		});

		resumeButton.onClick.AddListener (delegate() {
			ResumeGame();
		});
	}

	void PauseGame()
	{
		Time.timeScale = 0.0f;

		menu.alpha = 1.0f;
		menu.interactable = true;
		menu.blocksRaycasts = true;

		hud.alpha = 0.0f;
		hud.interactable = false;
		hud.blocksRaycasts = false;
	}

	void ResumeGame()
	{
		Time.timeScale = 1.0f;

		menu.alpha = 0.0f;
		menu.interactable = false;
		menu.blocksRaycasts = false;

		hud.alpha = 1.0f;
		hud.interactable = true;
		hud.blocksRaycasts = true;
	}

	void RestartGame()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

}
