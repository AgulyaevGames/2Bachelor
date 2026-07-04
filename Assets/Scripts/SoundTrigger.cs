using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    
    [SerializeField] private bool happensOnlyOnce = false;

    private bool triggered;
    // Start is called before the first frame update
    public AK.Wwise.Event triggerSoundEvent;

    public bool freezePlayer = false;
    
    public float freezeTimer = 1f;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player")) 
        {
            if(happensOnlyOnce && triggered)
                return;
            triggered = true;
            
            
            if (freezePlayer)
            {
                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePosition;

                IEnumerator freezeRoutine()
                {
                    yield return new WaitForSeconds(freezeTimer);

                    rb.constraints = RigidbodyConstraints.None;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
            // Post the event to the Wwise engine, attributing it to this GameObject
            triggerSoundEvent.Post(gameObject); 
        }
    }
}
