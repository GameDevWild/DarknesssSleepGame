using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockingPlatform : MonoBehaviour
{
	private bool destroyPlatform;
	private Quaternion endRotationOfDestroy = Quaternion.Euler(0, 0, -90);
	private Rigidbody2D wagonRigidBody2D;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag(GameTags.WagonTag))
		{
			destroyPlatform = true;
			wagonRigidBody2D = collision.collider.GetComponent<Rigidbody2D>();

		}
	}


	void Update()
	{
		if (destroyPlatform)
		{
			DestroyPlatformMovement();
			StartCoroutine(WagonBrakeCoroutine());
		}
	}

	private void DestroyPlatformMovement()
	{

		transform.rotation = Quaternion.Lerp(transform.rotation, endRotationOfDestroy, 2.0f * Time.deltaTime);
	}

	IEnumerator WagonBrakeCoroutine()
	{
		yield return new WaitForSeconds(0.8f);
		wagonRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX;

	}
}
