using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint Instance;

    private string[] temporalCheckpoints;

    [HideInInspector]
    public Vector2 lastPlayerPosition;
    [HideInInspector]
    public GameObject[] savedCheckPoints;
    [HideInInspector]
    public string temporalCheckpointsString;


    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag(GameTags.PlayerTag))
        {
            AudioManager.Instance.PlaySound(Player.Instance.playerCheckpoint);
            UIManager.Instance.checkPointTrigger.SetTrigger("Saving");
            lastPlayerPosition = collision.gameObject.transform.position;
            GameManager.Instance.SavePlayerCheckpoint(lastPlayerPosition);
            Destroy(this.gameObject);
            
           
        }
	}

	private void Awake()
	{
        Instance = this;
	}

    public void ManageSavedCheckpoints()
    {
        savedCheckPoints = GameObject.FindGameObjectsWithTag(GameTags.CheckpointTag);
        temporalCheckpoints = new string[savedCheckPoints.Length];
        var x = 0;
        foreach (GameObject checkpointName in savedCheckPoints)
        {
            temporalCheckpoints[x] = checkpointName.name;
            x++;
        }
        temporalCheckpointsString = string.Join(",", temporalCheckpoints);
       
    }

    public void LoadSavedCheckPointsOnContinueGame()
    {
        GameManager.Instance.savedCheckpointsInPlayerPrefs = PlayerPrefs.GetString(PlayerPrefsTags.Checkpoints).Split(',');
        GameObject[] checkpointElements = GameObject.FindGameObjectsWithTag(GameTags.CheckpointTag);
        foreach (GameObject elementCheckpoint in checkpointElements)
        {
            elementCheckpoint.SetActive(false);
        }
        for (int i = 0; i < GameManager.Instance.savedCheckpointsInPlayerPrefs.Length; i++)
        {
            foreach (GameObject elementCheckpoint in checkpointElements)
            {

                if (GameManager.Instance.savedCheckpointsInPlayerPrefs[i] == elementCheckpoint.name)
                {
                    elementCheckpoint.SetActive(true);
                }
            }
        }
    }
}
