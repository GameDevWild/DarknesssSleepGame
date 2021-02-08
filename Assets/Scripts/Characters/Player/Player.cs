using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player Instance;
	public float playerSpeed = 3.5f;
	public float playerJumpForce = 2.0f;
	public Vector2 initialPlayerPosition;
	public GroundCheck groundCheck;

	[Header("Player Sound FX")]
	public Sound playerJump;
	public Sound playerAttacking;
	public Sound playerDead;
	public Sound playerteleporterFX;
	public Sound playerPowerUpFX;
	public Sound playerEmptyAttack;
	public Sound playerCheckpoint;

	private float horizontalInput;
	private float powerSpeed = 1.0f;

	private bool playerShouldDead;


	private Rigidbody2D playerRigidBody2D;
	private Animator playerAnimator;
	private Transform currentPlatform;

	[HideInInspector]
	public bool isMovingRight, isAllowedToMove, isAttacking, isHidding, isCrouch,isPushing;

	[HideInInspector]
	public float timeToAttack;

	[HideInInspector]
	public List<GameObject> powerUpreset = new List<GameObject>();

	private void OnCollisionEnter2D(Collision2D collision)
	{


		if (collision.collider.CompareTag(GameTags.PlatformTag))
		{
			if (groundCheck.isGrounded)
			{
				currentPlatform = collision.collider.transform;
				transform.SetParent(currentPlatform);
			}

		}
		if (collision.collider.CompareTag(GameTags.TrapTag))
		{
			if (GameManager.Instance.isGameActive)
			{
				PlayerDead(1);
			}

		}

		if (collision.collider.CompareTag(GameTags.EnemyTag))
		{
			if (!isAttacking)
			{
				PlayerDead(0);

			}
		}
		
		
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.collider.CompareTag(GameTags.WagonTag))
		{
			isPushing = true;

		}
		
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.CompareTag(GameTags.PlatformTag))
		{
			currentPlatform = collision.collider.transform;
			transform.SetParent(null);
		}
		if (collision.collider.CompareTag(GameTags.WagonTag))
		{
			isPushing = false;
		
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(GameTags.TrapTag))
		{
			if (GameManager.Instance.isGameActive)
			{
				PlayerDead(1);
			}
		}

		if (collision.CompareTag(GameTags.HideTag))
		{
			isHidding = true;
		}

		if (collision.CompareTag(GameTags.PowerUpTag))
		{
			AudioManager.Instance.PlaySound(playerPowerUpFX);
			GameManager.Instance.AddAttackPower();
			powerUpreset.Add(collision.gameObject);
			collision.gameObject.tag = "Untagged";
			Destroy(collision.transform.GetChild(1).gameObject);
			Animator powerUptext = collision.transform.GetChild(2).gameObject.GetComponent<Animator>();
			powerUptext.SetTrigger("PowerUpGet");
			

		}
		if (collision.CompareTag(GameTags.ExitTag))
		{
			GameManager.Instance.LevelComplete();
			gameObject.SetActive(false);
		}

	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag(GameTags.HideTag))
		{
			isHidding = true;
		}

	}

	private void OnTriggerExit2D(Collider2D collision)
	{

		if (collision.CompareTag(GameTags.HideTag))
		{
			isHidding = false;
		}


	}


	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		playerRigidBody2D = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
		isAllowedToMove = false;
		playerAnimator.SetBool("isGameActive", true);
		if (PlayerPrefs.GetInt(PlayerPrefsTags.ResetGame) == 1)
		{
			transform.position = initialPlayerPosition;
		}
		


	}

	void Update()
	{

		if (GameManager.Instance.isGameActive && !GameManager.Instance.isGamePaused)
		{
			HorizontalMovement();
			Jump();
			CheckFalling();
			Attack();
			Roll();
			Crouch();

		}

	}

	private void HorizontalMovement()
	{

		horizontalInput = Input.GetAxis("Horizontal");
		if (Input.GetKey(KeyCode.LeftShift) && groundCheck.isGrounded && isAllowedToMove)
		{
			powerSpeed = 1.25f;

		}
		else if (Input.GetKeyUp(KeyCode.LeftShift) || isAllowedToMove)
		{
			powerSpeed = 1.0f;

		}
		else if (!isAllowedToMove)
		{
			powerSpeed = 0f;
		}


		playerRigidBody2D.velocity = new Vector2((horizontalInput * playerSpeed * powerSpeed), playerRigidBody2D.velocity.y);

		if (playerRigidBody2D.velocity.x > 0)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 0);
			isMovingRight = true;

		}
		else if (playerRigidBody2D.velocity.x < 0)
		{

			transform.localRotation = Quaternion.Euler(0, 180, 0);
			isMovingRight = false;

		}
		if (groundCheck.isGrounded)
		{

			playerAnimator.SetFloat("VelocityX", Mathf.Abs(playerRigidBody2D.velocity.x));
			playerAnimator.SetBool("isGrounded", true);

		}
		else if (groundCheck.isGrounded == false)
		{

			playerAnimator.SetFloat("VelocityX", Mathf.Abs(0));
			playerAnimator.SetBool("isGrounded", false);

		}

		if (isPushing && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
		{
			Wagon.Instance.PlayWagonSoundMovement();
		}
		else if ((isPushing) && (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)))
		{
			Wagon.Instance.StopWagonSoundMovement();
		}
		else if (!isPushing)
		{
			Wagon.Instance.StopWagonSoundMovement();
		}

	}

	private void Jump()
	{

		playerAnimator.SetFloat("VelocityY", playerRigidBody2D.velocity.y);
		if (groundCheck.isGrounded && (Input.GetKeyDown(KeyCode.Space)))
		{

			AudioManager.Instance.PlaySound(playerJump);
			playerAnimator.SetTrigger("Jump");
			playerRigidBody2D.velocity = new Vector2(playerRigidBody2D.velocity.x, playerJumpForce);



		}

	}

	private void CheckFalling()
	{
		
		if (playerRigidBody2D.velocity.y < -13.5)
		{
			playerShouldDead = true;
			CameraManager.Instance.IncreaseCameraSpeed();


		}
		if (groundCheck.isGrounded && playerShouldDead)
		{
			PlayerDead(2);
		}
	}

	private void Attack()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && timeToAttack > 0)
		{
			AudioManager.Instance.PlaySound(playerAttacking);
		}

		if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.E)) && timeToAttack > 0)
		{
			
			playerAnimator.SetBool("isAttacking", true);
			isAttacking = true;
			timeToAttack -= Time.deltaTime;
			UIManager.Instance.UIAttackLevelBarManage();


		}
		else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E) || timeToAttack <= 0)
		{
			isAttacking = false;
			playerAnimator.SetBool("isAttacking", false);
		}

		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && timeToAttack <= 0)
		{
			AudioManager.Instance.PlaySound(playerEmptyAttack);
			UIManager.Instance.emptyAttackLeverBar.SetTrigger("EmptyBar");
		}

	}

	private void Roll()
	{
		if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftControl))
		{
			playerAnimator.SetBool("Roll", true);
			powerSpeed = 1.25f;

		}
		else if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.LeftControl))
		{
			
			playerAnimator.SetBool("Roll", false);
			powerSpeed = 1.0f;
		}
	}

	private void Crouch()
	{
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			isCrouch = true;
			playerAnimator.SetBool("Crouch", true);
			playerRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;


		
		}
		else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
		{
			isCrouch = false;
			playerAnimator.SetBool("Crouch", false);
			playerRigidBody2D.constraints = RigidbodyConstraints2D.None;
			playerRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}



	private void PlayerDead(int DeadTypeNumber)
	{
		if (playerAnimator.GetBool("isGameActive") == true)
		{
			AudioManager.Instance.PlaySound(playerDead);
		}
		GameManager.Instance.GameOver(1.5f);
		UIManager.Instance.enemyImpactUI.SetTrigger("EnemyImpactUI");
		UIManager.Instance.ShowTypeOfDeadText(DeadTypeNumber);
		playerRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
		playerShouldDead = false;
		playerAnimator.SetBool("isGameActive", false);
		playerAnimator.SetTrigger("Death");
		
		
	}

	public void PlayerTeleporter(Transform targetTeleporter)
	{
		AudioManager.Instance.PlaySound(playerteleporterFX);
		transform.position = new Vector3(targetTeleporter.transform.position.x, targetTeleporter.transform.position.y - 1.0f, targetTeleporter.transform.position.z);
		StartCoroutine(ReturnMovementPlayerCoroutine());
	}

	public void MovePlayerToCheckPoint(bool isContinueGame)
	{
		if (!isContinueGame)
		{
			RestartAnimatorParametersPlayer();
			playerRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		transform.position = new Vector2(PlayerPrefs.GetFloat(PlayerPrefsTags.PlayerCheckpointPosX), PlayerPrefs.GetFloat(PlayerPrefsTags.PlayerCheckpointPosY));
		
		
	}

	private void RestartAnimatorParametersPlayer()
	{
		playerAnimator.SetBool("isGameActive", true);
		playerAnimator.ResetTrigger("Death");
		playerAnimator.SetBool("Crouch", false);
		playerAnimator.SetBool("Roll", false);
		playerAnimator.SetBool("isAttacking", false);
		playerAnimator.SetFloat("VelocityY", 0);
		playerAnimator.SetFloat("VelocityX", 0);
		playerAnimator.Play("Player_Idle");

	}

	private IEnumerator ReturnMovementPlayerCoroutine()
	{
		yield return new WaitForSeconds(1.0f);
		isAllowedToMove = true;
	}

	

}
