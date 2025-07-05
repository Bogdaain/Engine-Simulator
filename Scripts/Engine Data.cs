using UnityEngine;

[CreateAssetMenu(menuName ="Engine/Engine Data")]

public class EngineData : ScriptableObject
{
    [Header("Cylinder Configuration")]
    [Tooltip("Cylinder number")]
    public int cylinderCount = 4;
    [Tooltip("Firing Order")]
    public int[] firingOrder = new int[] { 1, 3, 4, 2 };

    [Header("RPM")]
    public float idleRPM = 800f;
    public float maxRPM = 7000f;

    [Header("Transmision")]
    public float[] gearRatios = { 0f, 3.625f, 2.115f, 1.529f, 1.125f, 0.911f, 0.734f };

    [Header("Fuel Consumption")]
    public float baseConsumption = 0.25f;
    public float consumptionFactor = 10f;

    [Header("General")]
    public string engineName = "I4_Honda";
    public AudioClip engineSound;
}