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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("1st room"))
        {
            // dialog.enabled = true;
            displaybox.SetActive(true);
            foreach (char c in DIALOGTEXT[index].ToCharArray())
            {

                text.text += c;
                StartCoroutine(TypeLine());
                text.text = DIALOGTEXT[index];
            }
           

        }
        if (other.CompareTag("2nd room"))
        {
            // dialog.enabled = true;
            displaybox.SetActive(true);
            foreach (char c in DIALOGTEXT[index].ToCharArray())
            {

                text.text += c;
                StartCoroutine(TypeLine());
                text.text = DIALOGTEXT[index];
            }

        }
        if (other.CompareTag("3rd room"))
        {
            // dialog.enabled = true;
            displaybox.SetActive(true);
            foreach (char c in DIALOGTEXT[index].ToCharArray())
            {

                text.text += c;
                StartCoroutine(TypeLine());
                text.text = DIALOGTEXT[index];
            }

        }
        if (other.CompareTag("4th room"))
        {
            // dialog.enabled = true;
            displaybox.SetActive(true);
            foreach (char c in DIALOGTEXT[index].ToCharArray())
            {

                text.text += c;
                StartCoroutine(TypeLine());
                text.text = DIALOGTEXT[index];
            }

        }
        if (other.CompareTag("5th room"))
        {
            // dialog.enabled = true;
            displaybox.SetActive(true);
            foreach (char c in DIALOGTEXT[index].ToCharArray())
            {

                text.text += c;
                StartCoroutine(TypeLine());
                text.text = DIALOGTEXT[index];
            }

        }
        if (other.CompareTag("6th room"))
        {
            // dialog.enabled = true;
            displaybox.SetActive(true);
            foreach (char c in DIALOGTEXT[index].ToCharArray())
            {

                text.text += c;
                StartCoroutine(TypeLine());
                text.text = DIALOGTEXT[index];
            }

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
        if (other.CompareTag("1st room"))
        {
            //dialog.enabled = false;
            displaybox.SetActive(false);
        }

        if (other.CompareTag("2nd room"))
        {
            //dialog.enabled = false;
            displaybox.SetActive(false);
        }

        if (other.CompareTag("3rd room"))
        {
            //dialog.enabled = false;
            displaybox.SetActive(false);
        }

        if (other.CompareTag("4th room"))
        {
            //dialog.enabled = false;
            displaybox.SetActive(false);
        }

        if (other.CompareTag("5th room"))
        {
            //dialog.enabled = false;
            displaybox.SetActive(false);
        }
        if (other.CompareTag("6th room"))
        {
            //dialog.enabled = false;
            displaybox.SetActive(false);
        }


    }
}
