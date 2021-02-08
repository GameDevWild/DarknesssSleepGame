using UnityEngine;

public class GroundCheck : MonoBehaviour
{
	[HideInInspector]
	public bool isGrounded;

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag(GameTags.GroundTag) || collision.CompareTag(GameTags.PlatformTag) || collision.CompareTag(GameTags.WagonTag))
		{
			isGrounded = true;
		}


	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(GameTags.GroundTag))
		{
			isGrounded = true;

		}


	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag(GameTags.GroundTag) || collision.CompareTag(GameTags.PlatformTag) || collision.CompareTag(GameTags.WagonTag))
		{
			isGrounded = false;

		}
	}
}