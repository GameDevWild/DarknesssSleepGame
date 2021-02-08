using UnityEngine;

public class RandomTeleporterGate : MonoBehaviour
{
    public Transform[] allPosibleTargetGates;

    [HideInInspector]
    public bool teleportEnabled;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player.Instance.isAllowedToMove)
        {
            teleportEnabled = true;

        }
    }

    private void Update()
    {
 
        if (teleportEnabled)
        {
            TeleportPlayer();
            teleportEnabled = false;

        }
    }

    private void TeleportPlayer()
    {

        Player.Instance.isAllowedToMove = false;
        Player.Instance.PlayerTeleporter(allPosibleTargetGates[Random.Range(0, allPosibleTargetGates.Length)]);
        CameraManager.Instance.IncreaseCameraSpeed();

    }
}



