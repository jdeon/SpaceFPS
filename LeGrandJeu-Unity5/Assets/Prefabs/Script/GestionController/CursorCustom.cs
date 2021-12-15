using UnityEngine;
using UnityEngine.InputSystem;

public class CursorCustom : MonoBehaviour
{
    //private PlayerInputAction controller;

    private static bool activate;

    public static bool Activate
    {
        get { return activate; }
        set
        {
            activate = value;
            GamePadCursor.displayedCursor = value;
        }
    }

    public static Collider findClickCollider()
    {
        RaycastHit hit;
        Vector3 coor = Mouse.current.position.ReadValue();
        Camera gameCamera = Camera.main;

        if (null != gameCamera && Physics.Raycast(gameCamera.ScreenPointToRay(coor), out hit))
        {
            return hit.collider;
        }
        else
        {
            return null;
        }
    }
}
