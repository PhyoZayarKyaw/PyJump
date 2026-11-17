using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance { get; private set; }

    public int power { get; private set; } = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddPower(int amount)
    {
        power += amount;
    }
}