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

    /// <summary>
    /// Initializes components and subscribes to input events.
    /// </summary>
    private void Awake()
    {
        keyText = GetComponent<TextMeshProUGUI>();
        buttonImage = GetComponent<Image>();

        InputManager.OnKeyboardActive += SetKeyboard;
        InputManager.OnGamepadActive += SetGamepad;
    }

    /// <summary>
    /// Unsubscribes from input events when the component is disabled.
    /// </summary>
    private void OnDisable()
    {
        InputManager.OnKeyboardActive -= SetKeyboard;
        InputManager.OnGamepadActive -= SetGamepad;
    }

    /// <summary>
    /// Calls the Flash method every frame.
    /// </summary>
    void Update()
    {
        Flash();
    }

    /// <summary>
    /// Handles the flashing effect for the component.
    /// </summary>
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

    /// <summary>
    /// Sets the component to keyboard mode.
    /// </summary>
    private void SetKeyboard()
    {
        if (keyText)
        {
            keyText.enabled = true;
        }

        if (buttonImage)
        {
            buttonImage.enabled = false;
        }
    }

    /// <summary>
    /// Sets the component to gamepad mode.
    /// </summary>
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
