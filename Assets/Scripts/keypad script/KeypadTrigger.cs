using UnityEngine;
using UnityEngine.Serialization;

public class KeypadTrigger : MonoBehaviour
{
    [SerializeField] private GameObject keypadPanel;
    
    private void Start()
    {
        keypadPanel.SetActive(false);
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            keypadPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            keypadPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

    


