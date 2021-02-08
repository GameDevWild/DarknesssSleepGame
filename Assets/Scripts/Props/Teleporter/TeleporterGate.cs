using UnityEngine;

public class TeleporterGate : MonoBehaviour
{
    
    public Transform targetTeleporter;
   

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
        Player.Instance.PlayerTeleporter(targetTeleporter);
        CameraManager.Instance.IncreaseCameraSpeed();
 
    }
}
