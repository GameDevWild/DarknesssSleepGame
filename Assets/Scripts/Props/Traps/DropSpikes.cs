using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpikes : MonoBehaviour
{
    public float trapSpeedMoveUp = 2.5f;
    public float timeLoadDelay = 0.5f;
    public float trapSpeedMoveDown = 6.0f;
    public float timeDropDelay = 1.0f;

    public AudioSource dropSpikesGateAudioSource;

    private bool firstDrop;
    private bool isFalling;

    private Vector2 initialPosition;
    private Vector2 finalPosition;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.GroundTag))
        {
            firstDrop = false;
            finalPosition = new Vector2(transform.position.x, transform.position.y);
            PlayCloseDoorSpikesGateFXSound();
        }
    }
    void Start()
    {

            initialPosition = new Vector2(transform.position.x, transform.position.y);
            firstDrop = true;
     
    }

    void Update()
    {

        if (firstDrop)
        {
            transform.position -= new Vector3(0, 2.0f * Time.deltaTime, 0);

        }
        else
        {
            if (GameManager.Instance.isGameActive)
            {
                VerticalMovement();
            }
            else
            {
                StartCoroutine(LoadTrapDelayCoroutine());
            }
        }

        


    }

    private void VerticalMovement()
    {
        if (isFalling)
        {
            StartCoroutine(DropTrapDelayCoroutine());
        }
        else
        {
            StartCoroutine(LoadTrapDelayCoroutine());
        }

        if (transform.position.y >= initialPosition.y)
        {
            isFalling = true;
            
        }
        else if (transform.position.y <= finalPosition.y)
        {
            

            isFalling = false;

           
        }
    }

    private void ReturnToInitialPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, trapSpeedMoveUp * Time.deltaTime);
    }

    private void GoToFinalPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, finalPosition, trapSpeedMoveDown * Time.deltaTime);
    }

    public void PlayCloseDoorSpikesGateFXSound()
    {
        dropSpikesGateAudioSource.PlayOneShot(dropSpikesGateAudioSource.clip, dropSpikesGateAudioSource.volume);
    }


    private IEnumerator LoadTrapDelayCoroutine()
    {
        yield return new WaitForSeconds(timeLoadDelay);
        ReturnToInitialPosition();

    }

    private IEnumerator DropTrapDelayCoroutine()
    {
        yield return new WaitForSeconds(timeDropDelay);
        GoToFinalPosition();


    }
}

