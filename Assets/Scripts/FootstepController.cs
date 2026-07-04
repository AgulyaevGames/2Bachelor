using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AK.Wwise.Event footstepEvent;
    public AK.Wwise.RTPC speedRTPC;

    private Rigidbody rb;
    private float stepTimer;

    public float footstepInterval = 0.5f; // Base interval at full speed

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stepTimer = footstepInterval; // first step fires after one normal interval
    }

    void Update()
    {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float speed = horizontalVelocity.magnitude;

        if (speedRTPC != null)
        {
            //ARTEM CHECK HERE
            //changes ALL sounds
            //speedRTPC.SetGlobalValue(speed);
            //speedRTPC.SetValue(gameObject, speed);
            //AkSoundEngine.SetRTPCValue("speed", speed, gameObject, 20);
        }


        if (IsGrounded() && speed > 0.1f)
        {
            float clampedSpeed = Mathf.Clamp(speed, 1f, 6f);
            float scaledInterval = footstepInterval * (6f / clampedSpeed);

            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                footstepEvent.Post(gameObject);
                stepTimer = scaledInterval;
            }
        }
        else
        {
            stepTimer = footstepInterval; // reset so first step fires promptly
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}