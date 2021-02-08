using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; 
    public TMP_Text  countDownTimer, attackBarText, TypeOfDeadText;
    public GameObject attackLevelBar, pauseMenu, optionsPanel, howToPlayPanel, gameOverMenu,startGame,finalWinText;
    public Animator enemyImpactUI, emptyAttackLeverBar, checkPointTrigger, checkPointLoad, startTransitionFadeIN, exitTransitionFadeOut, birdsAnimation;
    public Sound introFXSound, birdsWinFX, gameOverFX, musicPauseMenu, musicGameOverMenu, buttonClickFX, finalMusic, introBellsFX;
    public AudioSource menuMusicAudioSource;
    
    private Animator finalWinTextAnimator;

    [HideInInspector]
    public Slider attackLevelBarSlider;

     
    

	private void Awake()
	{
        Instance = this;
        attackLevelBarSlider = attackLevelBar.GetComponent<Slider>();
        startTransitionFadeIN = startGame.GetComponent<Animator>();
        finalWinTextAnimator = finalWinText.GetComponent<Animator>();
    }

	private void Start()
	{
        AudioManager.Instance.PlaySound(introFXSound);
        StartCoroutine(StartGameTransitionCoroutine());
        attackLevelBarSlider.value = PlayerPrefs.GetFloat(PlayerPrefsTags.TotalAttack);
        UICheckPlayerTimeToAttack();
        UIAttackLevelBarManage();

    }

	void Update()
    {
        if (GameManager.Instance.isGameActive)
        {
            PrintCountdownTimer();
        }
    }

    private void PrintCountdownTimer()
    {
        float minutes = Mathf.FloorToInt(GameManager.Instance.timeToGameOver / 60);
        float seconds = Mathf.FloorToInt(GameManager.Instance.timeToGameOver % 60);
        countDownTimer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void UICheckPlayerTimeToAttack()

    {   attackLevelBarSlider.maxValue= Player.Instance.timeToAttack;
        attackLevelBarSlider.value = Player.Instance.timeToAttack;
        attackBarText.text = "Attack x " + attackLevelBarSlider.value;

    }

    public void UIAttackLevelBarManage()
    {
        if (attackLevelBarSlider.value > 0)
        {
            attackLevelBarSlider.value = attackLevelBarSlider.value - 1.0f * Time.deltaTime;
            attackBarText.text = "Attack x " + Mathf.Round(attackLevelBarSlider.value);
        }
        if (Player.Instance.timeToAttack <= 0)
        {
        
            emptyAttackLeverBar.SetTrigger("EmptyBar");
            attackBarText.text = "Attack x " + Mathf.Round(attackLevelBarSlider.value);
        }
    }

    public void OpenPauseMenu()
    {
        AudioManager.Instance.PlaySound(buttonClickFX);
        pauseMenu.SetActive(true);
        pauseMenu.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        pauseMenu.gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }


    public void OpenHowToPlayPanel()
    {
        AudioManager.Instance.PlaySound(buttonClickFX);
        pauseMenu.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        pauseMenu.gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }


    public void OpenGameOverMenu()
    {
        AudioManager.Instance.PlaySound(musicGameOverMenu);
        StartCoroutine(PlayGameOverMusicCoroutine());
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void CloseAllMenus()
    {
       
        GameObject[] menuElements = GameObject.FindGameObjectsWithTag("Menu");
        foreach (GameObject elementMenu in menuElements)
        {
            elementMenu.SetActive(false);
        }
    }

    public void BirdsWinAnimation()
    {
        birdsAnimation.SetTrigger("BirdsWin");
        AudioManager.Instance.PlaySound(birdsWinFX);
        AudioManager.Instance.PlaySound(finalMusic);
    }

    public void CheckPointLoad()
    {
       
        checkPointLoad.SetTrigger("CheckpointLoad");
    }

    public void ShowTypeOfDeadText(int DeadTypeNumber)
    {
		switch (DeadTypeNumber)
		{
            case 0:
                TypeOfDeadText.text = "You got caught in the nightmare";    
                break;
            case 1:
                TypeOfDeadText.text = "Traps aren't your friends";
                break;
            case 2:
                TypeOfDeadText.text = "You fell from too high";
                break;
            case 3:
                TypeOfDeadText.text = "Time to wake up is over";
                break;
        }
	}

    

    private IEnumerator StartGameTransitionCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        startTransitionFadeIN.SetTrigger("FadeINStart");
        StartCoroutine(DestroyStartAnimationCoroutine());
    }

    public IEnumerator DestroyStartAnimationCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlaySound(introBellsFX);
        startGame.SetActive(false);
    }

    public IEnumerator EndGameTransitionCoroutine(float WaitForStartAnimation)
    {
        yield return new WaitForSeconds(WaitForStartAnimation);
        exitTransitionFadeOut.SetTrigger("FadeOutExit");
        GameManager.Instance.ChangeSceneAfterPressButton("MainMenu");
    }

    public IEnumerator LevelCompleteTransitionCoroutine(float WaitForStartAnimation)
    {
        yield return new WaitForSeconds(WaitForStartAnimation);
        exitTransitionFadeOut.SetTrigger("FadeOutExit");
        finalWinTextAnimator.SetTrigger("FinalWinText");
        StartCoroutine(WaitForWinLevelAnimationCoroutine());
    }

    public IEnumerator OpenGameOverMenuCoroutine(float TimeToShowPanel)
    {
        yield return new WaitForSeconds(TimeToShowPanel);
        OpenGameOverMenu();
        Cursor.visible = true;
    }

    private IEnumerator PlayGameOverMusicCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.PauseAllAudios();
        
    }

    private IEnumerator WaitForWinLevelAnimationCoroutine()
    {
        yield return new WaitForSeconds(6.0F);
        GameManager.Instance.ChangeSceneAfterPressButton("MainMenu");
    }

  



}
