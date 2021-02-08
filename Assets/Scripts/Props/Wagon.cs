using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public static Wagon Instance;

    public Sound wagonFallingFX;
    public Sound destroyRampFX;

    private bool isWagonFalling;

    private AudioSource wagonAudioSource;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.WagonSoundEventTag) && !isWagonFalling)
        {
            isWagonFalling = true;
            AudioManager.Instance.PlaySound(wagonFallingFX);
            StartCoroutine(PlayDoorDestroySoundCoroutine());
           
        }
    }

        void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        wagonAudioSource = GetComponent<AudioSource>();
        wagonAudioSource.Pause();
    }

    
    public void PlayWagonSoundMovement()
    {
        wagonAudioSource.UnPause();
        
    }

    public void StopWagonSoundMovement()
    {
        wagonAudioSource.Pause();
    
    }

    private IEnumerator PlayDoorDestroySoundCoroutine()
    {
        yield return new WaitForSeconds(2.7f);
        AudioManager.Instance.PlaySound(destroyRampFX);
    }


}
