using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    public GameObject stageButtonPrefab;
    public Transform gridParent;
    public int totalStages = 20;
    public int world = 1;

    public void PopulateGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= totalStages; i++)
        {
            GameObject buttonObj = Instantiate(stageButtonPrefab, gridParent);
            StageButton button = buttonObj.GetComponent<StageButton>();
            button.world = world;
            button.stage = i;
            button.Refresh();
        }
    }
}