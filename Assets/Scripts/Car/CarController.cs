using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [FormerlySerializedAs("_maxSpeed")]
    [SerializeField]
    float maxSpeed = 1;

    [FormerlySerializedAs("_accelerationFactor")]
    [SerializeField]
    float accelerationFactor = 1;

    [FormerlySerializedAs("_turnFactor")]
    [SerializeField]
    float turnFactor = 1;

    [FormerlySerializedAs("_driftFactor")]
    [SerializeField]
    float driftFactor = 1;

    Rigidbody2D _body;

    float _rotation;

    [FormerlySerializedAs("_accelerationInput")]
    [ReadOnly]
    [SerializeField]
    float accelerationInput;

    [FormerlySerializedAs("_turnInput")]
    [ReadOnly]
    [SerializeField]
    float turnInput;

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
        this.turnInput = turnInput;
        this.accelerationInput = accelerationInput;
    }

    void ApplyAcceleration()
    {
        if (accelerationInput == 0 || _body.velocity.magnitude > maxSpeed)
            _body.drag = Mathf.Lerp(_body.drag, 3f, Time.fixedDeltaTime * 3f);
        else
            _body.drag = 0;

        if (_body.velocity.magnitude < maxSpeed)
            _body.AddForce(accelerationFactor * accelerationInput * transform.up);
    }

    void ApplySteering()
    {
        var minSpeed = Mathf.Clamp01(_body.velocity.magnitude / 8);

        _rotation -= turnFactor * turnInput * minSpeed;
        _body.SetRotation(_rotation);
    }

    void KillOrthogonalVelocity()
    {
        var forwardVelocity = transform.up * Vector2.Dot(_body.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_body.velocity, transform.right);

        _body.velocity = forwardVelocity + rightVelocity * driftFactor;
    }
}