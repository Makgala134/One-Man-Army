using UnityEngine;

public class TimerStopZone : MonoBehaviour
{
    [SerializeField] private Timer timer;
    private bool triggered = false;
    private Timer time; // Reference to the Timer script

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            time.StopTimer(); // Call the StopTimer method on the Timer script
            Debug.Log("Timer stopped");

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
