using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleLightFlicker : MonoBehaviour
{
    private Light2D candleLight;  // Reference to the 2D Light component on the lamp
    public float minRadius = 3f;  // Minimum outer radius of the light
    public float maxRadius = 5f;  // Maximum outer radius of the light
    public float minIntensity = 0.5f;  // Minimum intensity of the light
    public float maxIntensity = 1.5f;  // Maximum intensity of the light
    public float flickerSpeed = 1f;  // Speed of the flickering effect (higher = slower flicker)

    private void Start()
    {
        // Get the Light2D component from a child of the lamp object
        candleLight = GetComponentInChildren<Light2D>();

        if (candleLight == null)
        {
            Debug.LogError("No Light2D component found in this lamp object.");
        }
    }

    private void Update()
    {
        if (candleLight != null)
        {
            // Get the new outer radius using PerlinNoise for smooth variation
            float radius = Mathf.Lerp(minRadius, maxRadius, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
            
            // Get the new intensity using PerlinNoise for smooth variation
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed + 100f, 0f));

            // Apply the radius and intensity to the light component
            candleLight.pointLightOuterRadius = radius;
            candleLight.intensity = intensity;
        }
    }
}