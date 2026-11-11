using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;



public class Keypad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    TextMeshProUGUI answerText;
    public void Number(int number)
    {
        Answer.text += number.ToString();
    }
}
