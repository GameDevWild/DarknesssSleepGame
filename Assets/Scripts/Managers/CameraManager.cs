using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public Transform targetPlayer;
    public float smoothSpeed = 1.5f;
    public float leftLimitBound;
    public float rightLimitBound;
    public float topLimitBound;
    public float bottomLimitBound;

    private Vector3 offset;
    private Vector3 getPosition;
    private Vector3 getPositionOffset;
    private Vector3 smoothedPosition;
    private float checkOffsetY;
    private float setSmoothSpeed;
    private bool isAnimation;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (targetPlayer == null)
        {
            targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        }
        offset = new Vector3(1, 1, 0);
        setSmoothSpeed = smoothSpeed;
       
    }

    void FixedUpdate()
    {
        if (!isAnimation)
        {
            if (Player.Instance.isCrouch)
            {
                getPosition = new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y - 1.0f, -10.0f);
            }
            else
            {
                getPosition = new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y, -10.0f);
            }
        }
        
        RegularCamera();
       
    }

    private void RegularCamera()
    {
       
        if (!Player.Instance.isMovingRight)
        {
            offset = new Vector3(-1, 1, 0);
        }
        else
        {
            offset = new Vector3(1, 1, 0);
        }

        getPositionOffset = getPosition + offset;
        smoothedPosition = Vector3.Slerp(transform.position, getPositionOffset, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimitBound, rightLimitBound), Mathf.Clamp(transform.position.y, bottomLimitBound, topLimitBound), -10.0f);
    }

    public void IncreaseCameraSpeed()
    {
        smoothSpeed = setSmoothSpeed * 3;
        StartCoroutine(ReturnToRegularSpeedCoroutine());
    }

    public void CameraAnimations(GameObject doorToOpen)
    {
        isAnimation = true;
        Player.Instance.isAllowedToMove = false;
        getPosition = new Vector3(doorToOpen.transform.position.x, doorToOpen.transform.position.y, -10.0f);
        StartCoroutine(DisableCameraAnimationsCoroutine());
    }

    IEnumerator ReturnToRegularSpeedCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        smoothSpeed = setSmoothSpeed;
    }

    IEnumerator DisableCameraAnimationsCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        isAnimation = false;
        Player.Instance.isAllowedToMove = true;
    }



}
