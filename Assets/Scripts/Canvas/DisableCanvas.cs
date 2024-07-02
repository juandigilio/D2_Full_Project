using UnityEngine;

public class DisableCanvas : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private bool setActive = false;

    /// <summary>
    /// Set canvas as disable at start
    /// </summary>
    void Start()
    {
        canvas = GetComponent<Canvas>();

        canvas.enabled = setActive;
    }
}
