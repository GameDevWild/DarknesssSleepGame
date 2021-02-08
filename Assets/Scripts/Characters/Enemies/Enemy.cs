using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
	
	public Transform[] patrolPoints;
	public Transform playerPosition;

	public float enemyPatrolSpeed = 1.5f;
	public float enemyFollowSpeed = 2.5f;
	public float minDistanceToPlayer;


	public Sound enemyDeadFX;
	public Sound enemyImpactPlayerFX;
	public Sound enemyResurrectionFX;
	

	private int waypointIndex = 0;
	private bool isPatrolling;
	private bool isRecovering;
	private bool isAttacking;
	private bool isDead;
	private bool isSoundAttackingPlaying;
	private Vector2 initialPosition;



	private Animator enemyAnimator;


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag(GameTags.PlayerTag))
		{
			CheckTypeOfImpact();
		
		}
			
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.CompareTag(GameTags.PlayerTag))
		{
			ReturnToInitialPosition();

		}

	}

	private void Awake()
	{
		enemyAnimator = GetComponent<Animator>();
		
	}

	private void Start()
	{
		initialPosition = transform.position;
		if (patrolPoints.Length > 0)
		{
			transform.position = patrolPoints[waypointIndex].position;
			isPatrolling = true;
		}
		else
		{
			isPatrolling = false;
		}
		


	}

	private void Update()
	{
		if (!isDead)
		{
			Patrol();
		}
		
		
	}

	private void Patrol()
	{
		if (isPatrolling)
		{
			transform.position = Vector2.MoveTowards(transform.position, patrolPoints[waypointIndex].position, enemyPatrolSpeed * Time.deltaTime);
			CheckPatrolPoint();
		}

		if (!isRecovering && GameManager.Instance.isGameActive == true)
		{
		
			DetectPlayer();
			
		}
		else
		{
			
			ReturnToInitialPosition();
		}
		
	}

	private void DetectPlayer()
	{
		
		if (Vector2.Distance(transform.position,playerPosition.position) <= minDistanceToPlayer && Player.Instance.isHidding == false)
		{
			
			this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
			isPatrolling = false;
			transform.position = Vector2.MoveTowards(transform.position, playerPosition.position, enemyFollowSpeed * Time.deltaTime);
		}
		else
		{
			this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
			ReturnToInitialPosition();
		}

	}

	private void CheckPatrolPoint()
	{
		if (transform.position == patrolPoints[waypointIndex].transform.position)
		{
			waypointIndex += 1;
		}
		if (waypointIndex == patrolPoints.Length)
		{
			waypointIndex = 0;
		}

	}

	private void ReturnToInitialPosition()
	{
		
		if (patrolPoints.Length > 0)
		{
			isPatrolling = true;
	
		}
		else
		{
			transform.position = Vector2.MoveTowards(transform.position, initialPosition, enemyPatrolSpeed * Time.deltaTime);
			
		}
		StartCoroutine(RecoverAttackofEnemyCoroutine());
		
		
	}

	private void CheckTypeOfImpact()
	{
		if (Player.Instance.isAttacking)
		{
			this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
			isDead = true;
			EnemyDead();
			
		}
		else
		{
			AudioManager.Instance.PlaySound(enemyImpactPlayerFX);
			this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
			isRecovering = true;
			ReturnToInitialPosition();
		}
	}

	private void EnemyDead()
	{
		AudioManager.Instance.PlaySound(enemyDeadFX);
		enemyAnimator.SetTrigger("isDead");
		StartCoroutine(EnemyResurrectionCoroutine());
	}

	public void PlayEnemyResurrectionSound()
	{
		AudioManager.Instance.PlaySound(enemyResurrectionFX);
	}
	

	IEnumerator RecoverAttackofEnemyCoroutine()
	{
		yield return new WaitForSeconds(4.0f);
		isRecovering = false;
	}

	IEnumerator EnemyResurrectionCoroutine()
	{
		yield return new WaitForSeconds(4.0f);
		isDead = false;
	}

	

}

