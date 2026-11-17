using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ListMatchManager : MonoBehaviour
{
    public static ListMatchManager Instance { get; private set; }

    [Tooltip("The correct order, including duplicates, e.g. apple, banana, cherry, apple")]
    public List<string> targetList = new List<string> { "apple", "banana", "cherry", "apple" };

    [Tooltip("If true, items can be collected in any order, but duplicate counts still matter. If false, strict order is enforced.")]
    public bool orderMatters = true;

    [Tooltip("If true, a wrong item costs a life instead of reloading the whole level.")]
    public bool loseLifeInsteadOfReset = false;

    [Header("Optional Wall Barrier")]
    public GameObject barrier;
    public bool advanceOnComplete = true;

    private int nextIndex = 0;
    private Dictionary<string, int> remainingCounts;

    private void Awake()
    {
        Instance = this;

        if (!orderMatters)
        {
            remainingCounts = new Dictionary<string, int>();
            foreach (string item in targetList)
            {
                if (!remainingCounts.ContainsKey(item))
                {
                    remainingCounts[item] = 0;
                }
                remainingCounts[item]++;
            }
        }
    }

    public bool TryCollect(string itemName)
    {
        return orderMatters ? TryCollectOrdered(itemName) : TryCollectAnyOrder(itemName);
    }

    private bool TryCollectOrdered(string itemName)
    {
        if (nextIndex >= targetList.Count) return false;

        if (itemName == targetList[nextIndex])
        {
            nextIndex++;

            if (nextIndex >= targetList.Count)
            {
                CompleteList();
            }
            return true;
        }
        else
        {
            HandleWrongPickup();
            return false;
        }
    }

    private bool TryCollectAnyOrder(string itemName)
    {
        if (remainingCounts.TryGetValue(itemName, out int count) && count > 0)
        {
            remainingCounts[itemName]--;

            if (remainingCounts.Values.All(c => c <= 0))
            {
                CompleteList();
            }
            return true;
        }
        else
        {
            HandleWrongPickup();
            return false;
        }
    }

    private void HandleWrongPickup()
    {
        if (loseLifeInsteadOfReset)
        {
            GameManager.Instance.LoseLife();
        }
        else
        {
            GameManager.Instance.ResetLevelDelayed(0.1f);
        }
    }

    private void CompleteList()
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