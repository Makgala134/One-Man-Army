using UnityEngine;
using UnityEngine.UI;

public class Test4Button : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => Debug.Log("Button works"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
