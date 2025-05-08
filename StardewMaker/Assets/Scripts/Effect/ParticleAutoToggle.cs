using UnityEngine;

public class ParticleAutoToggle : MonoBehaviour
{
    public float activationDistance = 20f;

    private ParticleSystem particle;
    private ParticleSystem.EmissionModule emission;
    private Transform cam;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        emission = particle.emission;
        cam = Camera.main.transform;
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, cam.position);
        emission.enabled = dist < activationDistance;
    }
}
