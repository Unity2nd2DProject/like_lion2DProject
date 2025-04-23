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
        PlayerController.Instance.SetCanMove(true);
    }

    private void PlantFinishTrigger()
    {
        anim.SetBool("Plant", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void WaterFinishTrigger()
    {
        anim.SetBool("Water", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void HarvestFinishTrigger()
    {
        anim.SetBool("Harvest", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void FishFinishTrigger()
    {
        anim.SetBool("Fish", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void AxeFinishTrigger()
    {
        anim.SetBool("Axe", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void GetWaterFinishTrigger()
    {
        anim.SetBool("GetWater", false);
        PlayerController.Instance.SetCanMove(true);
    }

}
