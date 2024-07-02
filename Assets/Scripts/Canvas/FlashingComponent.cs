using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FlashingComponent : MonoBehaviour
{
    private TextMeshProUGUI keyText;
    private Image buttonImage;
    private bool isGamepad;
    private bool isIncreasing = true;
    private Color color;
    private float actualTone;
    private const float MAX_TONE = 1.0f;
    private const float MIN_TONE = 0.0f;
    [SerializeField] private float FLASH_SPEED = 2.2f;

    public static Action OnCheckInput;

    private void Awake()
    {
        keyText = GetComponent<TextMeshProUGUI>();
        buttonImage = GetComponent<Image>();

        InputManager.OnKeyboardActive += SetKeyboard;
        InputManager.OnGamepadActive += SetGamepad;
    }

    private void OnDisable()
    {
        InputManager.OnKeyboardActive -= SetKeyboard;
        InputManager.OnGamepadActive -= SetGamepad;
    }

    void Update()
    {
        Flash();
    }

    private void Flash()
    {
        if (keyText != null)
        {
            color = keyText.color;
        }
        else
        {
            color = buttonImage.color;
        }

        actualTone = color.r;

        if (isIncreasing)
        {
            actualTone += FLASH_SPEED * Time.deltaTime;
        }
        else
        {
            actualTone -= FLASH_SPEED * Time.deltaTime;
        }

        if (actualTone > MAX_TONE)
        {
            actualTone = MAX_TONE;
            isIncreasing = false;
        }
        else if (actualTone < MIN_TONE)
        {
            actualTone = MIN_TONE;
            isIncreasing = true;
        }

        color.r = actualTone;
        color.g = actualTone;
        color.b = actualTone;

        if (keyText != null)
        {
            keyText.color = color;
        }
        else
        {
            buttonImage.color = color;
        }    
    }

    private void SetKeyboard()
    {
        if (keyText)
        {
            keyText.enabled = true;
        }

        if(buttonImage)
        {
            buttonImage.enabled = false;
        }
    }

    private void SetGamepad()
    {
        if (keyText)
        {
            keyText.enabled = false;
        }

        if (buttonImage)
        {
            buttonImage.enabled = true;
        }
    }
}
