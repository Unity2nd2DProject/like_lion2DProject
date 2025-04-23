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
        Debug.Log("PickFinishTrigger!");
        PlayerController.Instance.Pick();
        anim.SetBool("Pick", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void PlantFinishTrigger()
    {
        Debug.Log("PlantFinishTrigger!");
        PlayerController.Instance.Plant();
        anim.SetBool("Plant", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void WaterFinishTrigger()
    {
        Debug.Log("WaterFinishTrigger");
        PlayerController.Instance.Water();
        anim.SetBool("Water", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void HarvestFinishTrigger()
    {
        Debug.Log("HarvestFinishTrigger");
        PlayerController.Instance.Harvest();
        anim.SetBool("Harvest", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void FishFinishTrigger()
    {
        Debug.Log("FishFinishTrigger");
        PlayerController.Instance.Fish();
        anim.SetBool("Fish", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void AxeFinishTrigger()
    {
        Debug.Log("AxeFinishTrigger");
        PlayerController.Instance.Chop();
        anim.SetBool("Axe", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void GetWaterFinishTrigger()
    {
        Debug.Log("GetWaterFinishTrigger");
        PlayerController.Instance.GetWater();
        anim.SetBool("GetWater", false);
        PlayerController.Instance.SetCanMove(true);
    }

}
