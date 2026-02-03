using System.Linq;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropData data;
    private SpriteRenderer spriteRenderer;

    private int actualStage = 0;
    private float timePerStage;
    private int stagesCount;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.stages[actualStage];

        stagesCount = data.stages.Count();
        timePerStage = data.timeToGrow / stagesCount;
    }

    // Update is called once per frame
    void Update()
    {
        checkStage();
    }

    void checkStage()
    {
        int newStage = Mathf.Clamp(Mathf.FloorToInt(Time.time / timePerStage), 0, stagesCount - 1);
        if (newStage > actualStage)
        {
            changeStage(newStage);
        }
    }

    void changeStage(int stage)
    {
        spriteRenderer.sprite = data.stages[stage];
        actualStage = stage;
    }
}
