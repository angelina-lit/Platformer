using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }

    /*public void OnVerticalMovement(InputAction.CallbackContext context)
    {
        var directionY = context.ReadValue<float>();
        _hero.SetDirectionY(directionY);
    }*/

    public void OnSaySomething(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.SaySomething();
        }
    }
}