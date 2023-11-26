using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-3)] //Run beafor all scripts
public class InputManager : Singleton<InputManager>
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    #endregion

    private PlayerControls playerControls;
    private Camera mainCamera;
    private Swipe action;

    private void Awake()
    {
        playerControls = new PlayerControls();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        if (Accelerometer.current != null)
            InputSystem.EnableDevice(Accelerometer.current);
    }

    private void OnDisable()
    {
        playerControls.Disable();
        if (Accelerometer.current != null)
            InputSystem.DisableDevice(Accelerometer.current);
    }

    void Start()
    {
        playerControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
            OnStartTouch(playerControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)context.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
            OnEndTouch(playerControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)context.time);
    }

    public Vector2 PrimaryPosition()
    {
        Vector3 position = playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
        position.z = mainCamera.nearClipPlane;
        return mainCamera.ScreenToWorldPoint(position);
    }

    public Swipe GetAction()
    {
        return action;
    }

    public void SetAction(Swipe action)
    {
        this.action = action;
    }

    public Vector3 GetAccelorometer()
    {
        if (Accelerometer.current != null)
            return Accelerometer.current.acceleration.ReadValue();
        else
            return Vector3.zero;
    }
}
