using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{

	public static MenuAudioManager Instance;
    public AudioSource musicAudioSource;
	public AudioSource fxAudioSource;

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
	
	}

	private void Awake()
	{
		Instance = this;	
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
		fxAudioSource.clip = sound.clip;
		fxAudioSource.volume = sound.volume;
		fxAudioSource.loop = sound.loop;

		fxAudioSource.PlayOneShot(fxAudioSource.clip, fxAudioSource.volume);
	}
}
