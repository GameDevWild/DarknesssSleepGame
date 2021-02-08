using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
	public Animator startGameTransition;
	public Button continueGame;
	public Sound menuSound;
	public Sound buttonSoundRegular;
	public Sound buttonSoundGame;
	public Sound birdsSound;

	private string checkPointChecker;


	void Start()
	{
		MenuAudioManager.Instance.PlaySound(menuSound);
		StartCoroutine(PlayBirdsBackgroundSound());
		Cursor.visible = true;
		PlayerPrefs.SetInt(PlayerPrefsTags.ResetGame, 0);
		checkPointChecker = PlayerPrefs.GetString(PlayerPrefsTags.Checkpoints);
		if (checkPointChecker!= "Unsaved")
		{
			continueGame.interactable = true; 
		}
	
	}

	public void ChangeSceneAfterPressButton(string sceneToLoad)
	{
		startGameTransition.SetTrigger("FadeOutExit");
		MenuAudioManager.Instance.PlaySound(buttonSoundGame);
		MenuAudioManager.Instance.musicAudioSource.volume = 0;
		StartCoroutine(ChangeSceneAfterPressButtonCoroutine(sceneToLoad));
	}

	public void OpenPanelAfterPressButton(GameObject panelToOpen)
	{
		MenuAudioManager.Instance.PlaySound(buttonSoundRegular);
		StartCoroutine(OpenPanelAfterPressButtonCoroutine(panelToOpen));
	}

	public void CloseGameAfterPressButton()
	{
		GameSceneManager.Instance.CloseGame();
	}

	public void NewGameResetPlayerPrefs()
	{
		PlayerPrefs.SetInt(PlayerPrefsTags.ResetGame, 1);
	}

	public void ContinueGameWithoutResetPlayerPrefs()
	{
		PlayerPrefs.SetInt(PlayerPrefsTags.ResetGame, 0);
	}

	IEnumerator ChangeSceneAfterPressButtonCoroutine(string sceneToLoad)
	{
		yield return new WaitForSeconds(3.0f);
		GameSceneManager.Instance.ChangeScene(sceneToLoad);
	}

	IEnumerator PlayBirdsBackgroundSound()
	{
		yield return new WaitForSeconds (2.2f);
		MenuAudioManager.Instance.PlaySound(birdsSound);
	}

	IEnumerator OpenPanelAfterPressButtonCoroutine(GameObject panelToOpen)
	{
		yield return new WaitForSeconds(buttonSoundRegular.clip.length);
		GameObject[] menuElements = GameObject.FindGameObjectsWithTag(GameTags.MenuTag);
		foreach (GameObject elementMenu in menuElements)
		{
			elementMenu.SetActive(false);
		}
		panelToOpen.SetActive(true);
	}

	
}
