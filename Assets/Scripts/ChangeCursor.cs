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
        playerMovement = FindObjectOfType<PlayerMovement>(); // Initialiser playerMovement
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene.");
        }
        SetIdleCursor();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !playerMovement.isDrawingRectangle && !playerMovement.isHypnotizing)
        {
            Debug.Log("Mouse button held down, setting Click Cursor");
            SetClickCursor();
        }
        else if (playerMovement.isDrawingRectangle)
        {
            Debug.Log("Drawing Rectangle, setting Bat Cursor");
            SetBatCursor();
        }
        else if (playerMovement.isHypnotizing)
        {
            Debug.Log("Hypnotizing, setting Hypnotize Cursor");
            SetHypnotizeCursor();
        }
        else
        {
            Debug.Log("Setting Idle Cursor");
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
