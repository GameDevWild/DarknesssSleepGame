using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GamePlayAudioPlayer : MonoBehaviour
{
	public static GamePlayAudioPlayer Instance;
   
    public Sound introFXSound;
	public Sound ambientForest;


	private void Awake()
	{
		Instance = this;
	}

	private void Start()
    {
        AudioManager.Instance.PlaySound(introFXSound);
		AudioManager.Instance.PlaySound(ambientForest);
		
    }

}