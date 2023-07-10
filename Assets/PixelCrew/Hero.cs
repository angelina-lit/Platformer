using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;

    //private float _direction;
    //private float _directionY;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
    }

    /*public void SetDirectionY(float directionY)
    {
        _directionY = directionY;
    }*/

    /* private void Update()
     {
         if (_direction.magnitude > 0)
         {
             var delta = _direction * _speed * Time.deltaTime;
             transform.position += new Vector3(delta.x, delta.y, transform.position.z);
         }
    else if (_directionY != 0)
    {
        var deltaY = _directionY * _speed * Time.deltaTime;
        var newYPosition = transform.position.y + deltaY;
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }
}*/
    public void SaySomething()
    {
        Debug.Log(message: "Something!");
    }
}
