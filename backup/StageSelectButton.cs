using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StageSelectButton : MonoBehaviour
{
    public int world = 1;
    public int stage = 1;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SelectStage);
    }

    private void SelectStage()
    {
        GameManager.Instance.LoadLevel(world, stage);
    }
}