using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAutoToggle : MonoBehaviour
{
    public float activationDistance = 15f;

    private Light2D light2D;
    private Transform cam;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, cam.position);
        light2D.enabled = dist < activationDistance;
    }
}
