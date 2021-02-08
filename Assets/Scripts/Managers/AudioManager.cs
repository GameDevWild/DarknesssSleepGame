using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	public AudioSource musicAudioSource, generalFxAudioSource, playerAudioSource, enemyAudioSource, menuAudioSource;


	private void Awake()
	{
		Instance = this;
	}

	public void PlaySound(Sound sound)
	{
		if (sound.soundType == Sound.SoundType.MUSIC)
		{
			PlayMusic(sound);
		}
		else if (sound.soundType == Sound.SoundType.FX)
		{
			PlayFXSound(sound);
		}
		else if (sound.soundType == Sound.SoundType.PLAYER)
		{
			PlayPlayerSound(sound);
		}
		else if (sound.soundType == Sound.SoundType.ENEMY)
		{
			PlayEnemySound(sound);
		}
		else if (sound.soundType == Sound.SoundType.MENU)
		{
			PlayMenuSound(sound);
		}
	}


	private void PlayMusic(Sound sound)
	{
		musicAudioSource.clip = sound.clip;
		musicAudioSource.volume = sound.volume;
		musicAudioSource.loop = sound.loop;

		musicAudioSource.Play();
	}

	private void PlayFXSound(Sound sound)
	{
		generalFxAudioSource.clip = sound.clip;
		generalFxAudioSource.volume = sound.volume;
		generalFxAudioSource.loop = sound.loop;

		generalFxAudioSource.PlayOneShot(generalFxAudioSource.clip, generalFxAudioSource.volume);
	}

	private void PlayPlayerSound(Sound sound)
	{
		playerAudioSource.clip = sound.clip;
		playerAudioSource.volume = sound.volume;
		playerAudioSource.loop = sound.loop;

		playerAudioSource.PlayOneShot(playerAudioSource.clip, playerAudioSource.volume);
	}


	private void PlayEnemySound(Sound sound)
	{
		enemyAudioSource.clip = sound.clip;
		enemyAudioSource.volume = sound.volume;
		enemyAudioSource.loop = sound.loop;

		enemyAudioSource.PlayOneShot(enemyAudioSource.clip, enemyAudioSource.volume);
	}

	private void PlayMenuSound(Sound sound)
	{
		menuAudioSource.clip = sound.clip;
		menuAudioSource.volume = sound.volume;
		menuAudioSource.loop = sound.loop;
		menuAudioSource.Play();
	}
}