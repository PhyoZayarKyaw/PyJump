using UnityEngine;
using System.Collections.Generic;

public class OrderedStompManager : MonoBehaviour
{
    public static OrderedStompManager Instance { get; private set; }

    [Tooltip("The correct stomp order, e.g. (0,0), (0,1), (0,2), (1,0)...")]
    public List<Vector2Int> targetOrder = new List<Vector2Int>
    {
        new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,2),
        new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(1,2),
        new Vector2Int(2,0), new Vector2Int(2,1), new Vector2Int(2,2),
    };

    [Header("Optional Wall Barrier")]
    public GameObject barrier;
    public bool advanceOnComplete = true;

    private int nextIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsNext(Vector2Int coordinate)
    {
        if (nextIndex >= targetOrder.Count) return false;
        return coordinate == targetOrder[nextIndex];
    }

    public void RegisterCorrectStomp()
    {
        nextIndex++;

        if (nextIndex >= targetOrder.Count)
        {
            if (barrier != null)
            {
                barrier.SetActive(false);
            }

            if (advanceOnComplete)
            {
                GameManager.Instance.NextLevel();
            }
        }
    }

    public void RegisterWrongStomp()
    {
        //GameManager.Instance.ResetLevel(0.1f);
        GameManager.Instance.LoseLife();
    }

    
}