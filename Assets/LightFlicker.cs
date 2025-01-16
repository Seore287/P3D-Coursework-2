using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public bool transformLight = true;

    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minimumPulse = 1f;
    [SerializeField] private float maximumPulse = 3f;

    private Light lightSource;
    private Vector3 startPosition;
    private Vector3 target = Vector3.zero;
    private float time = .5f;
    private float timer = 0f;

    void Start()
    {
        startPosition = transform.position;
        lightSource = GetComponent<Light>();
    }

    void Update()
    {
        float scale = minimumPulse + Mathf.PingPong(Time.time * pulseSpeed, maximumPulse - minimumPulse);
        lightSource.intensity = scale;

        if (transformLight)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                timer = time;
                target = startPosition + Random.insideUnitSphere * 0.03f;
            }
            transform.position = Vector3.Lerp(transform.position, target, 0.01f);
        }
    }
}
