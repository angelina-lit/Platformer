using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{ 
    [SerializeField] private Hero _hero;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _hero.SetDirection(-1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _hero.SetDirection(1);
        }
        else
        {
            _hero.SetDirection(0);
        }
    }
}