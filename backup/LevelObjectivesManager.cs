using UnityEngine;
using System.Linq;

public class LevelObjectivesManager : MonoBehaviour
{
    public static LevelObjectivesManager Instance { get; private set; }

    [Tooltip("Teaching stage: leave unchecked so mistakes are free. Testing stage: check this so mistakes cost a life.")]
    public bool loseLifeOnMistake = true;

    [Tooltip("Uncheck if completing all sockets should NOT automatically advance to the next level.")]
    public bool advanceOnComplete = true;

    [Header("Optional Wall Barrier")]
    [Tooltip("If set, this GameObject will be disabled once all sockets are filled.")]
    public GameObject barrier;

    private DataSocket[] sockets;

    private void Awake()
    {
        Instance = this;
        sockets = FindObjectsOfType<DataSocket>();
    }

    public void NotifySocketFilled(DataSocket socket)
    {
        if (sockets.All(s => s.filled))
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
}