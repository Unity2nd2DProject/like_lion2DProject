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
}
