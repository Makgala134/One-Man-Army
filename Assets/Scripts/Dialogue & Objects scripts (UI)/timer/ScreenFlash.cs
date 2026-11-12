using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]

public class ScreenFlash : MonoBehaviour
{
    Image _image = null;
    Coroutine currentFlashRoutine = null;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
    }
   
    public void StartFlash(float secondsForOneFlash, float maxAlpha, Color newColor)
    {
        _image.color = newColor;
         
        //ensure maxAlpha isnt above 1

        maxAlpha = Mathf.Clamp(maxAlpha, 0, 1);

        if (currentFlashRoutine != null)
        {
            StopCoroutine(currentFlashRoutine);
            currentFlashRoutine = StartCoroutine(Flash(secondsForOneFlash, maxAlpha));
        }
    }
    
    IEnumerator Flash(float secondsForOneFlash, float maxAlpha)
    {
        //animate the flash in
        float flashInDuration = secondsForOneFlash / 2;
        for (float t = 0; t < flashInDuration; t += Time.deltaTime)
        {
            //create  a new color change 
            Color colorThisFrame = _image.color;
            colorThisFrame.a = Mathf.Lerp(0, maxAlpha, t  / flashInDuration); 

            //apply it
            _image.color = colorThisFrame;

            //wait untile the next frame
            yield return null;
        }

        //animate the flash out
    }
}
