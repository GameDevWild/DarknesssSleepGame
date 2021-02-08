using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeGate : MonoBehaviour
{
    public float waitTimeToOpen = 1.5f;
    public float waitTimeToClose = 1.5f;
    public AudioSource spikeGateAudioSource;

    private Animator spikeGateAnimator;
    
   
	private void Start()
	{
        spikeGateAnimator = GetComponent<Animator>();
        StartCoroutine(OpenSpikeGateCoroutine());
    }

    public void PlayCloseSpikeGateFXSound()
    {
        if (GameManager.Instance.isGameActive)
        {
            spikeGateAudioSource.PlayOneShot(spikeGateAudioSource.clip, spikeGateAudioSource.volume);
        }
    }

    IEnumerator OpenSpikeGateCoroutine()
    {
		yield return new WaitForSeconds(waitTimeToOpen);
        spikeGateAnimator.SetTrigger("Open");
        StartCoroutine(CloseSpikeGateCoroutine());


    }

    IEnumerator CloseSpikeGateCoroutine()
    {
        yield return new WaitForSeconds(waitTimeToClose);
        spikeGateAnimator.SetTrigger("Close");
        StartCoroutine(OpenSpikeGateCoroutine());
    }

    
}
