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
        //TODO voir si on peut mieux faire
        RaycastHit hit;
        Vector3 coor = Mouse.current.position.ReadValue();
        Camera gameCamera = Camera.current;

        if (Physics.Raycast(gameCamera.ScreenPointToRay(coor), out hit))
        {
            return hit.collider;
        }
        else
        {
            return null;
        }
    }
}
