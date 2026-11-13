using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogChecker : MonoBehaviour
{
    // public Dialogue dialog;
    public GameObject displaybox;
    public TextMeshProUGUI text;
    public string[] DIALOGTEXT;
    public float textSpeed;
    private int index;
    private bool inRoomTwo = false;
    public BoxCollider thirdLesson;
    public GameObject TwoLesson;

    private void Update()
    {
        if (inRoomTwo)
        {
          
                if (Input.GetMouseButtonDown(0) )
                {
                    text.text = " LadyLuck: I heard some noise coming from here I came to check it out.  I gave the cadets a 15 recess, I'm supposed to be heading back. Why are all the screens showin no siginal?";
                    TwoLesson.SetActive(false);
                    thirdLesson.enabled = true;
                    
                }

            }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("1st room"))
        {
            if (index == 0)
            {
          
                text.text = "LadyLuck Thoughts : I must've dropped my ID card after training...Colonel Spike said she saw it in here.";


            }
        }
        else
        if (other.CompareTag("2nd room"))
        {
            displaybox.SetActive(true);
            text.text = "General Dane: Are the cadets finished with their training? What are you doing in the camera room? ";

            inRoomTwo = true;


        }
 

    }
    IEnumerator TypeLine()
    {
        foreach (char c in DIALOGTEXT[index].ToCharArray())
        {
            yield return new WaitForSeconds(textSpeed);
        }

    }

    private void OnTriggerExit(Collider other)
    {
   
        if (other.CompareTag("2nd room"))
        {
            //dialog.enabled = false;
            displaybox.SetActive(false);
        }

   
        
    }
    void NextLine()
    {
      
        if (index < DIALOGTEXT.Length - 1)
        {
            index++;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
