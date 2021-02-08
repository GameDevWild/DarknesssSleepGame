using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{
	public static Trigger Instance;

	public Sound pullTriggerFX;

	private GameObject doorToOpen;
	private Animator doorToOpenAnimator;


	private void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.CompareTag(GameTags.PlayerTag))
		{
			if (tag == GameTags.TriggerATag)
			{
				AudioManager.Instance.PlaySound(pullTriggerFX);
				Checkpoint.Instance.lastPlayerPosition = collision.gameObject.transform.position;
				GameManager.Instance.SavePlayerCheckpoint(Checkpoint.Instance.lastPlayerPosition);
				this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
				doorToOpen = GameObject.Find("BlockWagon");
				PlayerPrefs.SetInt(PlayerPrefsTags.TriggerA, 1);
				transform.rotation = Quaternion.Euler(0, -180, 0);
				GameObject destroyTeleporter = GameObject.Find("Teleporter_1");
				destroyTeleporter.SetActive(false);
				StartCoroutine(DelayTriggerActiveCoroutine());

			}
			else if (tag == GameTags.TriggerBTag)
			{
				AudioManager.Instance.PlaySound(pullTriggerFX);
				Checkpoint.Instance.lastPlayerPosition = collision.gameObject.transform.position;
				GameManager.Instance.SavePlayerCheckpoint(Checkpoint.Instance.lastPlayerPosition);
				this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
				doorToOpen = GameObject.Find("Door_A");
				PlayerPrefs.SetInt(PlayerPrefsTags.TriggerB, 1);
				transform.rotation = Quaternion.Euler(0, -180, 0);
				StartCoroutine(DelayTriggerActiveCoroutine());
			}

			else if (tag == GameTags.TriggerCTag)
			{
				AudioManager.Instance.PlaySound(pullTriggerFX);
				Checkpoint.Instance.lastPlayerPosition = collision.gameObject.transform.position;
				GameManager.Instance.SavePlayerCheckpoint(Checkpoint.Instance.lastPlayerPosition);
				this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
				doorToOpen = GameObject.Find("Door_B");
				PlayerPrefs.SetInt(PlayerPrefsTags.TriggerC, 1);
				transform.rotation = Quaternion.Euler(0, -180, 0);
				StartCoroutine(DelayTriggerActiveCoroutine());
			}
			

		}

		if (collision.CompareTag(GameTags.WagonTag))
		{
			doorToOpen = GameObject.Find("BlockingPlatform");
			StartCoroutine(DelayTriggerActiveCoroutine());
		}
	}

	private void TriggerActive(GameObject doorToOpen)
	{
		doorToOpenAnimator = doorToOpen.GetComponent<Animator>();
		
		if (doorToOpen.name != "BlockingPlatform")
		{
			doorToOpenAnimator.SetTrigger("Open");
		}
		if (doorToOpen.name != "Door_B") 
		{
			CameraManager.Instance.CameraAnimations(doorToOpen);
		}
		tag = "Untagged";
	}

	IEnumerator DelayTriggerActiveCoroutine()
	{
		yield return new WaitForSeconds(0.25f);
		TriggerActive(doorToOpen);
	}
   
}
