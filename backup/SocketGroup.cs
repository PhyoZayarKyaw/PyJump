using UnityEngine;
using System.Linq;

public class SocketGroup : MonoBehaviour
{
    [Tooltip("The specific sockets that belong to this group.")]
    public DataSocket[] sockets;

    [Tooltip("All walls/barriers that disappear once every socket in THIS group is filled.")]
    public GameObject[] barriers;

    [Tooltip("If true, completing this group also advances to the next level.")]
    public bool advanceOnComplete = false;

    public void NotifySocketFilled(DataSocket socket)
    {
        if (sockets.All(s => s != null && s.filled))
        {
            foreach (GameObject barrier in barriers)
            {
                if (barrier != null)
                {
                    barrier.SetActive(false);
                }
            }

            if (advanceOnComplete)
            {
                GameManager.Instance.NextLevel();
            }
        }
    }
}