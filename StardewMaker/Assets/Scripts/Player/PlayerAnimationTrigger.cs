using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    //private Animator anim;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
    }

    private void PickFinishTrigger()
    {
        PlayerController.Instance.Pick();
        PlayerController.Instance.anim.SetBool("Pick", false);
        PlayerController.Instance.SetCanMove(true);
        StaminaManager.Instance.ConsumeStamina();
    }

    private void PlantFinishTrigger()
    {
        PlayerController.Instance.Plant();
        PlayerController.Instance.anim.SetBool("Plant", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void WaterFinishTrigger()
    {
        PlayerController.Instance.Water();
        PlayerController.Instance.anim.SetBool("Water", false);
        PlayerController.Instance.SetCanMove(true);
        StaminaManager.Instance.ConsumeStamina();
    }

    private void HarvestFinishTrigger()
    {
        PlayerController.Instance.Harvest();
        PlayerController.Instance.anim.SetBool("Harvest", false);
        PlayerController.Instance.SetCanMove(true);
    }

    private void FishFinishTrigger()
    {
        PlayerController.Instance.Fish();
        PlayerController.Instance.anim.SetBool("Fish", false);
        PlayerController.Instance.SetCanMove(true);
        StaminaManager.Instance.ConsumeStamina();
    }

    private void AxeFinishTrigger()
    {
        PlayerController.Instance.Chop();
        PlayerController.Instance.anim.SetBool("Axe", false);
        PlayerController.Instance.SetCanMove(true);
        StaminaManager.Instance.ConsumeStamina();
    }

    private void GetWaterFinishTrigger()
    {
        PlayerController.Instance.GetWater();
        PlayerController.Instance.anim.SetBool("GetWater", false);
        PlayerController.Instance.SetCanMove(true);
        StaminaManager.Instance.ConsumeStamina();
    }

    private void FertlizeFinishTrigger()
    {
        PlayerController.Instance.Fertlize();
        PlayerController.Instance.anim.SetBool("Fertilize", false);
        PlayerController.Instance.SetCanMove(true);
        StaminaManager.Instance.ConsumeStamina();
    }

    private void PickSoundTrigger()
    {
        SoundManager.Instance.PlaySFX("Pick");
    }

    private void WaterSoundTrigger()
    {
        SoundManager.Instance.PlaySFX("Watering");
    }

    private void HarvestSoundTrigger()
    {
        SoundManager.Instance.PlaySFX("Harvest");
    }

    private void ThrowFloatSoundTrigger()
    {
        SoundManager.Instance.PlaySFX("Throw");
    }

    private void WindingReelSoundTrigger()
    {
        SoundManager.Instance.PlaySFX("Reel");
    }

    private void FishSoundTrigger()
    {
        SoundManager.Instance.PlaySFX("Fishing");
    }

    private void AxeSoundTrigger()
    {
        SoundManager.Instance.PlaySFX("Axe");
    }

    private void GetWaterSoundTrigger()
    {
        SoundManager.Instance.PlaySFX("GetWater");
    }

    private void SeedSoundTrigger()
    {
        SoundManager.Instance.PlaySFX("Seed");
    }
}
