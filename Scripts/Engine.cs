using UnityEngine;

public class Engine : MonoBehaviour
{
    public EngineData data;

    public bool isRunning = false;
    public bool clutchEngage = true;
    public float[] throttlePresets = { 0f, 0.25f, 0.5f, 0.75f, 1f };
    public int throttleIndex = 0;
    public float throttle => Input.GetKey(KeyCode.W) ? throttlePresets[throttleIndex] : 0f;
    public float rpm = 0f;
    public float rpmChangeSpeed = 3000f;
    public float speed = 0f;
    public int gear = 0;
    public float fuelConsumptionRate = 0f;

    void Update()
    {
        HandleKeyboardInput();
        if (isRunning)
        {
            float targetRPM = Mathf.Lerp(data.idleRPM, data.maxRPM, throttle);
            float gearFactor = GetGearFactor();
            rpm = Mathf.MoveTowards(rpm, targetRPM, (rpmChangeSpeed / gearFactor) * Time.deltaTime);

            if (clutchEngage)
                speed = rpm / data.gearRatios[gear] * 0.029f;
            else
                speed = Mathf.Lerp(speed, 0f, Time.deltaTime * 2f);

            fuelConsumptionRate = data.baseConsumption + (rpm / data.maxRPM) * throttle * data.consumptionFactor;
        }
        else
        {
            rpm = 0f;
            speed = Mathf.Lerp(speed, 0f, Time.deltaTime * 2f);
            fuelConsumptionRate = 0f;
        }
    }

    void HandleKeyboardInput()
    {
        //throttle
        if(Input.GetKeyDown(KeyCode.RightArrow))
            throttleIndex = Mathf.Min(throttleIndex + 1, throttlePresets.Length - 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            throttleIndex = Mathf.Max(throttleIndex - 1, 0);

        //gear
        if (Input.GetKeyDown(KeyCode.E)) UpShift();
        if (Input.GetKeyDown(KeyCode.Q)) DownShift();

        //clutch
        while (Input.GetKeyDown(KeyCode.Space))
            clutchEngage = false;

        // start/stop engine
        if (Input.GetKeyDown(KeyCode.S)) StartEngine();
        if (Input.GetKeyDown(KeyCode.X)) StopEngine();
    }

    public void StartEngine() => isRunning = true;
    public void StopEngine() => isRunning = false;
    public void UpShift()
    {
        if (gear < data.gearRatios.Length - 1)
        {
            int oldGear = gear;
            gear++;

            AdjustRPMAfterGearChange(oldGear, gear);
        }
    }

    public void DownShift()
    {
        if (gear > 0)
        {
            int oldGear = gear;
            gear--;

            AdjustRPMAfterGearChange(oldGear, gear);
        }
    }
    public void SetNeutral() => gear = 0;
    public float GetGearFactor()
    {
        return 1f + gear * 0.5f;
    }
    void AdjustRPMAfterGearChange(int oldGear, int newGear)
    {
        float oldRatio = data.gearRatios[oldGear];
        float newRatio = data.gearRatios[newGear];

        if (oldRatio > 0 && newRatio > 0)
        {
            float newRPM = rpm * (newRatio / oldRatio);
            rpm = Mathf.Clamp(newRPM, data.idleRPM, data.maxRPM);
        }
    }
}