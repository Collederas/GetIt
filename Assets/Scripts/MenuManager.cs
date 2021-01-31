using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public InputAction menuAction;
    public Canvas menuCanvas;
    public bool canOpen = true;
    private bool _isOpen = false;
    public LevelBuilder LevelBuilder;

    void Awake()
    {
        menuAction.performed += Menu;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        menuAction.Enable();
    }

    void Menu(InputAction.CallbackContext ctx)
    {
        Debug.Log(_isOpen);
        Debug.Log(canOpen);
        
        if (_isOpen)
        {
            Resume();
            return;
        }

        if (canOpen && !_isOpen)
        {
            _isOpen = true;
            gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        menuCanvas.gameObject.SetActive(false);
        _isOpen = false;

        Time.timeScale = 1;
    }
}
