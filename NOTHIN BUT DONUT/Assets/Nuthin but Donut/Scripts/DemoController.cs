using UnityEngine;
using UnityEngine.InputSystem;

public class DemoController : MonoBehaviour
{
    public Hand hand;
    public Trampoline trampoline;


    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            hand.Release();

        if (Keyboard.current.rKey.wasPressedThisFrame)
            hand.Reset();

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            trampoline.transform.Translate(Vector3.back * 0.25f, Space.World);
        
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            trampoline.transform.Translate(Vector3.forward * 0.25f, Space.World);
    }
}
