using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;



public class Keypad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;

    public void Number(int number)
    {
        Answer.text += number.ToString();
        Debug.Log("Number pressed: " + number);
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
            Number(1);
    }
}
