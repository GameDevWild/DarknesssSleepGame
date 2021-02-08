using UnityEngine;


[CreateAssetMenu(menuName = "Darkness Sleep/Sound", fileName = "sound.asset")]
public class Sound : ScriptableObject
{
    [System.Serializable]
    public enum SoundType
    {
        MUSIC, FX, PLAYER, AMBIENT, ENEMY, MENU
    }
    public SoundType soundType;
    public AudioClip clip;
    [Range(0, 1f)]
    public float volume;
    public bool loop;
   

}