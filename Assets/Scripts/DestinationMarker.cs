using UnityEngine;

public class DestinationMarker : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    
    private Vector3 originalScale;
    
    void Start()
    {
        originalScale = transform.localScale;
    }
    
    void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        transform.localScale = originalScale * scale;
    }
}