using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Texture2D idleCursor;
    [SerializeField] private Texture2D batCursor;
    [SerializeField] private Texture2D hypnotizeCursor;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        SetIdleCursor();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) )
        {
            SetClickCursor();
        }
        else
        {
            SetIdleCursor();
        }
    }

    public void SetClickCursor()
    {
        Cursor.SetCursor(clickCursor, hotSpot, cursorMode);
    }

    public void SetIdleCursor()
    {
        Cursor.SetCursor(idleCursor, hotSpot, cursorMode);
    }

    public void SetBatCursor()
    {
        Cursor.SetCursor(batCursor, hotSpot, cursorMode);
    }

    public void SetHypnotizeCursor()
    {
        Cursor.SetCursor(hypnotizeCursor, hotSpot, cursorMode);
    }
}
