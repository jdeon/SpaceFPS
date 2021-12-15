using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class GamePadCursor : MonoBehaviour
{
    public static bool displayedCursor;

    [SerializeField]
    private PlayerInput playerInput;

    private RectTransform cursorRectTransform;
    private RectTransform canvasRectTransform;
    private Image cursorImage;
    private float cursorSpeed = 20f;

    private bool previouMousState;
    private Mouse virtualMouse;

    private string previousControleScheme;
    private string nameShemeGamePad;
    private string nameShemeKeyBoard;

    private bool gamepadMode;

    void Awake()
    {
        PlayerInputAction controller = new PlayerInputAction();
        nameShemeGamePad = controller.GamepadScheme.name;
        nameShemeKeyBoard = controller.KeyboardMouseScheme.name;

        /* 
         controller.PlayerActions.CameraView.performed += ctx => {
             onMotionDevice(ctx);
         };*/

        cursorImage = gameObject.GetComponent<Image>();
        cursorRectTransform = gameObject.GetComponent<RectTransform>();
        canvasRectTransform = transform.parent.gameObject.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        //controller.Enable();
        if(null == virtualMouse)
        {
            virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        } else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if(cursorRectTransform != null)
        {
            Vector2 position = cursorRectTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += onMotionDevice;
       // playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        InputSystem.onAfterUpdate -= onMotionDevice;
        //playerInput.onControlsChanged -= OnControlsChanged;

        if (null != virtualMouse && virtualMouse.added)
        {
            InputSystem.RemoveDevice(virtualMouse);
        }
        //controller.Disable();
    }

    private void onMotionDevice()
    {
        if(!displayedCursor || null == virtualMouse || null == Gamepad.current)
        {
            return;
        }

        Vector2 deltaInputValue = Gamepad.current.leftStick.ReadValue() * cursorSpeed;
        Vector2 newPosition = virtualMouse.position.ReadValue() + deltaInputValue;

        //ScreenLimit
        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaInputValue);

        bool isSouthButtonPressed = Gamepad.current.buttonSouth.isPressed;
        if(previouMousState != isSouthButtonPressed)
        {
            virtualMouse.CopyState<MouseState>(out MouseState mouseState);
            mouseState.WithButton(MouseButton.Left, isSouthButtonPressed);
            InputState.Change(virtualMouse, mouseState);
            previouMousState = isSouthButtonPressed;
        }

        anchorCursor(newPosition);
    }

    private void anchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, null, out anchoredPosition);
        cursorRectTransform.anchoredPosition = anchoredPosition;
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if (!displayedCursor)
        {
            return;
        }

        if(input.currentControlScheme == nameShemeKeyBoard && previousControleScheme != nameShemeKeyBoard)
        {
            gamepadMode = false;
            if (null != virtualMouse && virtualMouse.added && null != Mouse.current)
            {
                Mouse.current.WarpCursorPosition(virtualMouse.position.ReadValue());
            }
        }
        else if (input.currentControlScheme == nameShemeGamePad && previousControleScheme != nameShemeGamePad && null != Mouse.current) {
            if (null == virtualMouse)
            {
                virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (!virtualMouse.added)
            {
                InputSystem.AddDevice(virtualMouse);
            }
                
            InputState.Change(virtualMouse.position, Mouse.current.position.ReadValue());    
            anchorCursor(Mouse.current.position.ReadValue());
            gamepadMode = true;
        }

        previousControleScheme = input.currentControlScheme;
    }

    private void Update()
    {
        if(previousControleScheme != playerInput.currentControlScheme)
        {
            OnControlsChanged(playerInput);
        }

        if (!displayedCursor)
        {
            hideCursor();
        } else if (gamepadMode)
        {
            cursorImage.enabled = displayedCursor;
            Cursor.visible = false;
        } else
        {
            cursorImage.enabled = false;
            Cursor.visible = displayedCursor;
        }
    }

    public void hideCursor()
    {
        Cursor.visible = false;
        cursorImage.enabled = false;
    }
}
