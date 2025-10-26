using UnityEngine;

[CreateAssetMenu(fileName = "VolumenConfigurationSO", menuName = "Scriptable Objects/VolumenConfigurationSO")]
public class VolumenConfigurationSO : ScriptableObject
{
    private float volume = 0.5f;

    public float Volume { get => volume; set => volume = value; }
}
