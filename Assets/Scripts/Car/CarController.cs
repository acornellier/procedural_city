using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [SerializeField] float _maxSpeed = 1;
    [SerializeField] float _accelerationFactor = 1;
    [SerializeField] float _turnFactor = 1;
    [SerializeField] float _driftFactor = 1;

    Rigidbody2D _body;

    float _rotation;

    [ReadOnly] [SerializeField] float _accelerationInput;
    [ReadOnly] [SerializeField] float _turnInput;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ApplyAcceleration();
        ApplySteering();
        KillOrthogonalVelocity();
    }

    public void SetInput(float turnInput, float accelerationInput)
    {
        _turnInput = turnInput;
        _accelerationInput = accelerationInput;
    }

    void ApplyAcceleration()
    {
        if (_accelerationInput == 0 || _body.velocity.magnitude > _maxSpeed)
            _body.drag = Mathf.Lerp(_body.drag, 3f, Time.fixedDeltaTime * 3f);
        else
            _body.drag = 0;

        if (_body.velocity.magnitude < _maxSpeed)
            _body.AddForce(_accelerationFactor * _accelerationInput * transform.up);
    }

    void ApplySteering()
    {
        var minSpeed = Mathf.Clamp01(_body.velocity.magnitude / 8);

        _rotation -= _turnFactor * _turnInput * minSpeed;
        _body.SetRotation(_rotation);
    }

    void KillOrthogonalVelocity()
    {
        var forwardVelocity = transform.up * Vector2.Dot(_body.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_body.velocity, transform.right);

        _body.velocity = forwardVelocity + rightVelocity * _driftFactor;
    }
}