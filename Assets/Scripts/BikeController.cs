using UnityEngine;

public class BikeController : MonoBehaviour
{
    public static BikeController instance;  // Singleton instance

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform gameModel;

    private float maxSteerVelocity = 2f;
    private float maxForwardVelocity = 15f;

    private Vector2 input = Vector2.zero;

    private float accelerationMultiplier = 15f;
    private float brakeMultiplier = 15f;
    private float steerMultiplier = 5f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
                Debug.LogError("Rigidbody is missing on BikeController!");
        }
    }

    void Update()
    {
        if (rb != null)
        {
            gameModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        if (input.y > 0)
            Accelerate();
        else
            rb.linearDamping = 0.2f;  // Use drag instead of linearDamping (Corrected)

        if (input.y < 0)
            Brake();

        Steer();

        // Prevent bike from going backward
        if (rb.linearVelocity.z <= 0)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    void Accelerate()
    {
        if (rb == null) return;

        rb.linearDamping = 0; // Remove drag when accelerating
        if (rb.linearVelocity.z >= maxForwardVelocity)
            return;

        rb.AddForce(transform.forward * accelerationMultiplier * input.y, ForceMode.Acceleration);
    }

    void Brake()
    {
        if (rb == null) return;

        if (rb.linearVelocity.z <= 0)  // Prevent bike from going backward
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0);
            return;
        }

        rb.AddForce(-transform.forward * brakeMultiplier * Mathf.Abs(input.y), ForceMode.Acceleration);

        // If the velocity is very small, set it to zero to prevent drifting backward
        if (rb.linearVelocity.z < 0.5f)  
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0);
        }
    }

    void Steer()
    {
        if (rb == null) return;

        if (Mathf.Abs(input.x) > 0)
        {
            float speedBaseSteerLimit = rb.linearVelocity.z / 10f;
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);

            rb.AddForce(transform.right * steerMultiplier * input.x * speedBaseSteerLimit, ForceMode.Acceleration);

            float normalizedX = rb.linearVelocity.x / maxSteerVelocity;
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            rb.linearVelocity = new Vector3(normalizedX * maxSteerVelocity, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z), Time.fixedDeltaTime * 3);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        input = inputVector.normalized;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TrafficVehicle"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);

            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.ShowGameOverMenu();
            }
            else
            {
                Debug.LogError("UIManager not found in scene!");
            }
        }
    }

    public float GetCurrentSpeed()
    {
        return rb != null ? rb.linearVelocity.magnitude : 0f;
    }
}
