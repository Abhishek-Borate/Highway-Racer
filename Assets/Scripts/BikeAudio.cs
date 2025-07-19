using UnityEngine;

public class BikeAudio : MonoBehaviour
{
    public float speed = 15.0f;
    private float turnSpeed = 70.0f;

    public AudioSource bikeSound;
    public AudioClip bikeClip;

    private float horizontalInput;
    private float verticalInput;
    
    void Start()
    {
        // Ensure AudioSource is assigned
        if (bikeSound == null)
        {
            bikeSound = gameObject.AddComponent<AudioSource>(); // Add if missing
        }

        bikeSound.loop = true; // Set looping
        bikeSound.clip = bikeClip; // Assign the clip
        bikeSound.playOnAwake = false; // Don't play automatically
    }

    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime);

        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput);

        // Play sound when moving forward/backward
        if (Mathf.Abs(verticalInput) > 0)
        {
            if (!bikeSound.isPlaying)
            {
                Debug.Log("Playing bike sound");
                bikeSound.Play();
            }
        }
        else
        {
            if (bikeSound.isPlaying)
            {
                Debug.Log("Stopping bike sound");
                bikeSound.Stop();
            }
        }
    }
}
