using UnityEngine;

public class DisableCanvas : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private bool setActive = false;

    void Start()
    {
        canvas = GetComponent<Canvas>();

        canvas.enabled = setActive;
    }
}
