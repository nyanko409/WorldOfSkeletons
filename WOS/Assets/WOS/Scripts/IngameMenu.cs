using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour 
{
	private Button continueGame;
	private Button backToMainMenu;
	private Button quitGame;
	public static bool isPaused = false;

	// Use this for initialization
	void Start () 
	{
		isPaused = false;
		Time.timeScale = 1f;
		quitGame = GameObject.Find ("Quit Game").GetComponent<Button>();
		quitGame.gameObject.SetActive (false);
		continueGame = GameObject.Find ("Continue Game").GetComponent<Button> ();
		continueGame.gameObject.SetActive (false);
		backToMainMenu = GameObject.Find ("Back To Main Menu").GetComponent<Button> ();
		backToMainMenu.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape) && !isPaused && !PlayerStats.isDead && !SkillTreeMage.isOpen && !CharacterInfo.isOpen && !ActionBar.isOpen && !Inventory.isOpen)
		{
			pause ();
		} 
		else if (Input.GetKeyDown (KeyCode.Escape) && isPaused && !PlayerStats.isDead)
		{
			resume ();
		} 
		else if (Input.GetKeyDown (KeyCode.Escape) && SkillTreeMage.isOpen)
		{
			SkillTreeMage.isOpen = false;
		} 
		else if (Input.GetKeyDown (KeyCode.Escape) && CharacterInfo.isOpen)
		{
			CharacterInfo.isOpen = false;
		} 
		else if (Input.GetKeyDown (KeyCode.Escape) && ActionBar.isOpen)
		{
			ActionBar.isOpen = false;
		}
		else if (Input.GetKeyDown (KeyCode.Escape) && Inventory.isOpen)
		{
			Inventory.isOpen = false;
		}
	}

	void pause() 
	{
		Time.timeScale = 0f;
		isPaused = true;
		quitGame.gameObject.SetActive (true);
		continueGame.gameObject.SetActive (true);
		backToMainMenu.gameObject.SetActive (true);
	}

	void resume() 
	{
		Time.timeScale = 1f;
		isPaused = false;
		quitGame.gameObject.SetActive (false);
		continueGame.gameObject.SetActive (false);
		backToMainMenu.gameObject.SetActive (false);
	}

	public void QuitGame()
	{
		GameObject.FindGameObjectWithTag ("Respawn").GetComponent<AudioSource> ().Play ();
		Time.timeScale = 1f;
		SaveLoadData.saveDataAtQuittingGame ();
		Application.Quit ();
	}

	public void ContinueGame() 
	{
		GameObject.FindGameObjectWithTag ("Respawn").GetComponent<AudioSource> ().Play ();
		resume ();
	}

	public void BackToMainMenu() 
	{
		GameObject.FindGameObjectWithTag ("Respawn").GetComponent<AudioSource> ().Play ();
		SaveLoadData.saveDataAtQuittingGame ();
		SceneManager.LoadScene(0);
	}
}
