using UnityEngine;

public class CarWheelMovementScript : MonoBehaviour {

    [Header("Wheel Attributes")]
    [SerializeField] Transform[] wheels;
    private float currSpeed;
    private float currAngSpeed;
    private float currPitch;
    private float wheelTurnValue = 1;
    private float wheelAngleTurnValue = 1;

    [Header("Audio Components")]
    [SerializeField]
    AudioClip engineRev;
    private AudioSource audioSource;

    //Component References
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        currSpeed = Remap(rb.velocity.magnitude, 0, 50, 0, 40);
        currPitch = Remap(currSpeed, 0, 40, 0.5f, 1.5f);
        currAngSpeed = Remap(rb.angularVelocity.magnitude, 0, 2.5f, 0, 20);

        if (Input.GetKeyDown(KeyCode.W))
            wheelTurnValue = 1;
        if (Input.GetKeyDown(KeyCode.S))
            wheelTurnValue = -1;
        if (Input.GetKeyDown(KeyCode.A))
            wheelAngleTurnValue = 1;
        if (Input.GetKeyDown(KeyCode.D))
            wheelAngleTurnValue = -1;

        foreach (Transform wheel in wheels)
        {
            wheel.rotation = Quaternion.Euler(currSpeed * wheelTurnValue, transform.rotation.eulerAngles.y, currAngSpeed * wheelAngleTurnValue);
        }

        audioSource.pitch = currPitch;

    }

    private void OnGUI()
    {
        
    }

    //Custom Function

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


}
