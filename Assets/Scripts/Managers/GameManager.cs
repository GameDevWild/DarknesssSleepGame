using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	private float restartCountdownTimer;
	private string temporalPowerUpsString;
	private string[] savedPowerUpsInPlayerPrefs;
	private AudioSource[] allAudioSources;
	
	[HideInInspector]
	public string[] savedCheckpointsInPlayerPrefs;

	[HideInInspector]
	public bool isGameActive, isGamePaused, isCheckPointActive;

	[HideInInspector]
	public float timeToGameOver = 604.0f;



	private void Awake()
	{
		Instance = this;
		isGameActive = true;
		isGamePaused = false;
		Cursor.visible = false;
		restartCountdownTimer = timeToGameOver - 4.0f;
		CheckGameState();
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

	}

	private void Start()
	{
		
		StartCoroutine(WaitForStarAnimationCoroutine());

	}


	private void Update()
	{ 
		if (isGameActive)
		{
			
			CheckTimeToGameOver();

			if (Input.GetButtonDown("Cancel"))
			{
				if (isGamePaused == false)
				{
					AudioManager.Instance.PlaySound(UIManager.Instance.musicPauseMenu);
					PauseAllAudios();
					Time.timeScale = 0;
					Cursor.visible = true;
					isGamePaused = true;
					UIManager.Instance.OpenPauseMenu();
					
					
				}
				else
				{
					UnPauseAllAudios();
					Time.timeScale = 1;
					isGamePaused = false;
					Cursor.visible = false;
					UIManager.Instance.CloseAllMenus();
				}
			}
		}

	}

	private void CheckGameState()
	{
		if (PlayerPrefs.GetInt(PlayerPrefsTags.ResetGame) == 1)
		{
			ResetGame();
		}
		if (PlayerPrefs.GetInt(PlayerPrefsTags.ResetGame) == 0)
		{
			SetContinueGamePlayerPrefs();
		}
	}

	public void CheckTimeToGameOver()
	{
		timeToGameOver -= Time.deltaTime;
		if (timeToGameOver <= 0)
		{
			GameOver(0.0f);
			UIManager.Instance.ShowTypeOfDeadText(3);
		}

	}

	private void SetContinueGamePlayerPrefs()
	{
		bool isContinueGame = true;
		Player.Instance.MovePlayerToCheckPoint(isContinueGame);
		timeToGameOver = PlayerPrefs.GetFloat(PlayerPrefsTags.CountDownTimer);
		Player.Instance.timeToAttack = PlayerPrefs.GetFloat(PlayerPrefsTags.TotalAttack);
		Checkpoint.Instance.LoadSavedCheckPointsOnContinueGame();
		LoadPowerUpsAchieved();
		CheckPuzzlesCompleted();



	}

	private void CheckPuzzlesCompleted()
	{
		if (PlayerPrefs.GetInt(PlayerPrefsTags.TriggerA) == 1)
		{
			GameObject doorTriggerA = GameObject.Find("BlockWagon");
			GameObject setTriggerA = GameObject.FindGameObjectWithTag(GameTags.TriggerATag);
			doorTriggerA.GetComponent<Animator>().SetTrigger("Open");
			setTriggerA.transform.rotation = Quaternion.Euler(0, -180, 0);
			setTriggerA.transform.GetChild(0).gameObject.SetActive(true);
			setTriggerA.tag = "Untagged";
		}
		if (PlayerPrefs.GetInt(PlayerPrefsTags.TriggerB) == 1)
		{
			
			GameObject doorTriggerB = GameObject.Find("Door_A");
			GameObject setTriggerB = GameObject.FindGameObjectWithTag(GameTags.TriggerBTag);
			doorTriggerB.GetComponent<Animator>().SetTrigger("Open");
			setTriggerB.transform.rotation = Quaternion.Euler(0, -180, 0);
			setTriggerB.transform.GetChild(0).gameObject.SetActive(true);
			setTriggerB.tag = "Untagged";

		}
		if (PlayerPrefs.GetInt(PlayerPrefsTags.TriggerC) == 1)
		{
			GameObject doorTriggerC = GameObject.Find("Door_B");
			GameObject setTriggerC = GameObject.FindGameObjectWithTag(GameTags.TriggerCTag);
			doorTriggerC.GetComponent<Animator>().SetTrigger("Open");
			setTriggerC.transform.rotation = Quaternion.Euler(0, -180, 0);
			setTriggerC.transform.GetChild(0).gameObject.SetActive(true);
			setTriggerC.tag = "Untagged";

		}
	}

	public void GameOver(float TimeToShowPanel)
	{
		
		AudioManager.Instance.PlaySound(UIManager.Instance.gameOverFX);
		StartCoroutine(UIManager.Instance.OpenGameOverMenuCoroutine(TimeToShowPanel));
		isGameActive = false;
	
	}

	public void LevelComplete()
	{
		PauseAllAudios();
		isGameActive = false;
		UIManager.Instance.finalWinText.SetActive(true);
		UIManager.Instance.BirdsWinAnimation();
		StartCoroutine(UIManager.Instance.LevelCompleteTransitionCoroutine(2.0f));
		ResetGame();
		
	}

	public void AddAttackPower()
	{
		
		if (Player.Instance.timeToAttack == 0)
		{
			Player.Instance.timeToAttack = 1.0f;
		}
		else
		{
			Player.Instance.timeToAttack = Mathf.Round(Player.Instance.timeToAttack) + 1.0f;
		}

		UIManager.Instance.UICheckPlayerTimeToAttack();

	}

	public void SavePlayerCheckpoint(Vector2 lastPlayerPosition)
	{
		PlayerPrefs.SetFloat(PlayerPrefsTags.PlayerCheckpointPosX, lastPlayerPosition.x);
		PlayerPrefs.SetFloat(PlayerPrefsTags.PlayerCheckpointPosY, lastPlayerPosition.y);
		PlayerPrefs.SetFloat(PlayerPrefsTags.CountDownTimer, timeToGameOver);
		PlayerPrefs.SetFloat(PlayerPrefsTags.TotalAttack, UIManager.Instance.attackLevelBarSlider.value);
		ManagePowerUpsAchieved();
		StartCoroutine(SaveLastChekpointCoroutine());

	}

	private void ResetGame()
	{
		PlayerPrefs.SetInt(PlayerPrefsTags.ResetGame, 1);
		PlayerPrefs.SetFloat(PlayerPrefsTags.PlayerCheckpointPosX, Player.Instance.initialPlayerPosition.x);
		PlayerPrefs.SetFloat(PlayerPrefsTags.PlayerCheckpointPosY, Player.Instance.initialPlayerPosition.y);
		PlayerPrefs.SetFloat(PlayerPrefsTags.CountDownTimer, restartCountdownTimer);
		PlayerPrefs.SetFloat(PlayerPrefsTags.TotalAttack, 0.0f);
		PlayerPrefs.SetString(PlayerPrefsTags.Checkpoints, "Unsaved");
		PlayerPrefs.SetString(PlayerPrefsTags.PowerUpReset, "Unsaved");
		PlayerPrefs.SetInt(PlayerPrefsTags.TriggerA, 0);
		PlayerPrefs.SetInt(PlayerPrefsTags.TriggerB, 0);
		PlayerPrefs.SetInt(PlayerPrefsTags.TriggerC, 0);
	}

	public void UnpauseGame()
	{
		AudioManager.Instance.PlaySound(UIManager.Instance.buttonClickFX);
		UnPauseAllAudios();
		Time.timeScale = 1.0f;
		isGamePaused = false;
		Cursor.visible = false;
		UIManager.Instance.CloseAllMenus();
	}

	public void LoadCheckPoint()
	{
		
		UnPauseAllAudios();
		bool isContinueGame = false;
		UIManager.Instance.CheckPointLoad();
		UIManager.Instance.UICheckPlayerTimeToAttack();
		UIManager.Instance.UIAttackLevelBarManage();
		Player.Instance.MovePlayerToCheckPoint(isContinueGame);
		isGameActive = true;
		UnpauseGame();
		timeToGameOver = PlayerPrefs.GetFloat(PlayerPrefsTags.CountDownTimer);
		Player.Instance.timeToAttack= PlayerPrefs.GetFloat(PlayerPrefsTags.TotalAttack);
	}

	public void ChangeSceneAfterPressButton(string sceneToLoad)
	{
		StartCoroutine(ChangeSceneAfterPressButtonCoroutine(sceneToLoad));
		isGameActive = false;
		Time.timeScale = 1.0f;
		StartCoroutine(UIManager.Instance.EndGameTransitionCoroutine(0.2f));
	}

	public void RestartGame(string sceneToLoad)
	{
		UnPauseAllAudios();
		PlayerPrefs.SetInt(PlayerPrefsTags.ResetGame, 1);
		StartCoroutine(ChangeSceneAfterPressButtonCoroutine(sceneToLoad));
		isGameActive = false;
		Time.timeScale = 1.0f;
		StartCoroutine(UIManager.Instance.EndGameTransitionCoroutine(0.2f));
	}

	private void ManagePowerUpsAchieved()
	{
		GameObject[] achievedPowerUps = Player.Instance.powerUpreset.ToArray();
		string[] temporalPowerUps = new string[achievedPowerUps.Length];
		var x = 0;
		foreach (GameObject powerUpName in achievedPowerUps)
		{
			temporalPowerUps[x] = powerUpName.name;
			x++;
		}
		temporalPowerUpsString = string.Join(",", temporalPowerUps);
		PlayerPrefs.SetString(PlayerPrefsTags.PowerUpReset, temporalPowerUpsString);
	}

	private void LoadPowerUpsAchieved()
	{
		savedPowerUpsInPlayerPrefs = PlayerPrefs.GetString(PlayerPrefsTags.PowerUpReset).Split(',');
		GameObject[] powerUpElements = GameObject.FindGameObjectsWithTag(GameTags.PowerUpTag);
		for (int i = 0; i < savedPowerUpsInPlayerPrefs.Length; i++)
		{
			foreach (GameObject powerUpsActive in powerUpElements)
			{

				if (savedPowerUpsInPlayerPrefs[i] == powerUpsActive.name)
				{
					powerUpsActive.transform.GetChild(1).gameObject.SetActive(false);
					
				}
			}
		}
	}

	public void PauseAllAudios()
	{
		foreach (AudioSource AudioSources in allAudioSources)
		{
			if (AudioSources.CompareTag("UI"))
			{
				AudioSources.UnPause();
			}
			else
			{
				AudioSources.Pause();
			}
		}
	}

	public void UnPauseAllAudios()
	{
		foreach (AudioSource AudioSources in allAudioSources)

			if (AudioSources.CompareTag("UI"))
			{
				AudioSources.Pause();
			}
			else
			{
				AudioSources.UnPause();
			}
	}

	private IEnumerator WaitForStarAnimationCoroutine()
	{
		yield return new WaitForSeconds(3.0f);
		Player.Instance.isAllowedToMove = true;
		
	}

	private IEnumerator SaveLastChekpointCoroutine()
	{
		yield return new WaitForSeconds(0.2f);
		Checkpoint.Instance.ManageSavedCheckpoints();
		PlayerPrefs.SetString(PlayerPrefsTags.Checkpoints, Checkpoint.Instance.temporalCheckpointsString);
		
	}

	IEnumerator ChangeSceneAfterPressButtonCoroutine(string sceneToLoad)
	{
		yield return new WaitForSeconds(1.2f);
		GameSceneManager.Instance.ChangeScene(sceneToLoad);
	}

}