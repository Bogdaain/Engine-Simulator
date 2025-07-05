using TMPro;
using UnityEngine;

public class EngineUI : MonoBehaviour
{
    public Engine engine;

    public TMP_Text rpmText;
    public TMP_Text speedText;
    public TMP_Text gearText;
    public TMP_Text throttleText;
    public TMP_Text fuelText;

    void Update()
    {
        if (engine == null) return;

        rpmText.text = $"RPM: {engine.rpm:F0}";
        speedText.text = $"Speed: {engine.speed:F1} km/h";
        gearText.text = engine.gear == 0 ? "Gear: N" : $"Gear: {engine.gear}";
        throttleText.text = $"Throttle: {engine.throttle * 100f:F0}%";
        fuelText.text = $"Fuel: {engine.fuelConsumptionRate:F2} L/h";
    }
}