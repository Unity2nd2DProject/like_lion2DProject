using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void PickFinishTrigger()
    {
        anim.SetBool("Pick", false);
    }

    private void PlantFinishTrigger()
    {
        anim.SetBool("Plant", false);
    }

    private void WaterFinishTrigger()
    {
        anim.SetBool("Water", false);
    }

    private void HarvestFinishTrigger()
    {
        anim.SetBool("Harvest", false);
    }

    private void FishFinishTrigger()
    {
        anim.SetBool("Fish", false);
    }

    private void AxeFinishTrigger()
    {
        anim.SetBool("Axe", false);
    }

    private void GetWaterFinishTrigger()
    {
        anim.SetBool("GetWater", false);
    }

}
