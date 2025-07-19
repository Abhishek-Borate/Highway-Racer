using UnityEngine;
using System.Collections;

public class TrafficVehicle : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float laneChangeAmount = 3f; // Distance to move left or right
    [SerializeField] private float minLaneChangeTime = 2f; // Minimum time before changing lanes
    [SerializeField] private float maxLaneChangeTime = 5f; // Maximum time before changing lanes
    [SerializeField] private float laneChangeSpeed = 2f; // Speed of lane change
    [SerializeField] private float roadMinX = -3.5f; // Left boundary of the road
    [SerializeField] private float roadMaxX = 8.9f; // Right boundary of the road

    private float targetX; // Target X position for lane change

    void Start()
    {
        StartCoroutine(ContinuousLaneChange());
    }

    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    IEnumerator ContinuousLaneChange()
    {
        while (true) // Keep changing lanes randomly
        {
            yield return new WaitForSeconds(Random.Range(minLaneChangeTime, maxLaneChangeTime));

            int direction = Random.Range(0, 2) == 0 ? -1 : 1; // -1 for left, 1 for right
            targetX = transform.position.x + (direction * laneChangeAmount);

            // Ensure the car stays within the road limits
            if (targetX < roadMinX)
            {
                targetX = transform.position.x + laneChangeAmount; // Force right movement
            }
            else if (targetX > roadMaxX)
            {
                targetX = transform.position.x - laneChangeAmount; // Force left movement
            }

            StartCoroutine(SmoothLaneChange());
        }
    }

    IEnumerator SmoothLaneChange()
    {
        while (Mathf.Abs(transform.position.x - targetX) > 0.1f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(targetX, transform.position.y, transform.position.z),
                Time.deltaTime * laneChangeSpeed
            );
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DespawnZone"))
        {
            Destroy(gameObject);
        }

        if(other.CompareTag("TrafficVehicle")){
            Destroy(other.gameObject);
        }
    }
}
