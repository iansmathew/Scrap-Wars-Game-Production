using UnityEngine;

public class HoverCarControl : MonoBehaviour
{
    [Header("Hover Properties")]
    public float hoverForce = 1000.0f;
    public float hoverHeight = 2.0f;
    public Transform[] hoverPoints;

    [Header("Velocity Properties")]
    public float forwardVel = 8000.0f;
    public float backwardVel = 2500.0f;
    public float turnStrength = 1000.0f;

    private float currLinearVel = 0.0f;     //current linear vel to be applied per phyiscs tick
    private float currAngularVel = 0.0f;    //cuurent angular vel to be applied per phyiscs tick
    private float linearAcl = 0.0f;
    private float deltaAcl = 0.01f;
    private float hoverFluc = 0.0f;         //float to multiply by sin(time) to 'bounce' vehicle on idle
    private float angularForcePercent = 0.0f;

    //Misc Members
    private Rigidbody rBody;
    private float deadZone = 0.1f;          //float to ignore Input Axis
    private int layerMaskInvert;            //layerMask that contains all but 'Character' layer
    private HUDScript HUD;

    [Header("VFX Properties")]
    [SerializeField]
    GameObject sparkPrefab;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();

        HUD = HUDScript.Instance;
        layerMaskInvert = 1 << gameObject.layer; //set layer mask to car's layer
        layerMaskInvert = ~layerMaskInvert;      // invert layer mask
    }

    private void Update()
    {
        currLinearVel = 0.0f;
        currAngularVel = 0.0f;

        //Calculating linear velocities
        linearAcl += (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) ? deltaAcl : -deltaAcl;
        linearAcl = Mathf.Clamp(linearAcl, 0, 1);

        float aclAxis = Input.GetAxis("Vertical");
        if (aclAxis > deadZone)                                 //if input is forward and not 0
            currLinearVel = aclAxis * forwardVel * linearAcl;
        else if (aclAxis < deadZone)                            //if input is backward and not 0
            currLinearVel = aclAxis * backwardVel * linearAcl;

        //Calculation percent according to speed
        angularForcePercent = Remap(rBody.velocity.magnitude, 0, 50, 0, 0.3f);
        angularForcePercent = 1 - angularForcePercent;

        //Calculating angular velocities
        float turnAxis = Input.GetAxis("Horizontal");
        //float turnDirection = (Input.GetKey(KeyCode.S)) ? -1 : 1; //if car turns when reversing, invert the turn directions //TURNDIRECTION INVERSED : DISABLED
        if (Mathf.Abs(turnAxis) > deadZone)
            currAngularVel = turnAxis * turnStrength * angularForcePercent; //* turnDirection; //TURNDIRECTION INVERSED : DISABLED

        //Setting hoverFluctuate to 'bounce' the car
        hoverFluc = Mathf.Abs(Mathf.Sin(Time.time));
        hoverFluc = Mathf.Clamp(hoverFluc, 0.5f, 1.0f);     //car hovers from half its height to full height

        //Checking if car is upside down
        if (Vector3.Dot(transform.up, Vector3.down) > 0.95f)
        {
            HUD.SetResetCarPanel(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                Vector3 resetPosVec = transform.position;
                resetPosVec.y = 2.5f;
                Quaternion resetRot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                transform.SetPositionAndRotation(resetPosVec, resetRot);

                HUD.SetResetCarPanel(false);
            }

        }

        //Sound Controls

        if (Input.GetKeyDown(KeyCode.W))
        {
            //audioManager.PlayOneShot(engineStartClip);
        }

    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        //Applying hover forces to each thruster
        for (int i = 0; i < hoverPoints.Length; i++)
        {
            Transform hoverPoint = hoverPoints[i];
            if (Physics.Raycast(hoverPoint.position, Vector3.down, out hit, hoverHeight, layerMaskInvert)) //if successful raycast at hoverHeight distance
            {
                Vector3 thrustForce = Vector3.up * hoverForce * (1.0f - hit.distance / hoverHeight) * hoverFluc;
                rBody.AddForceAtPosition(thrustForce, hoverPoint.position); //apply global upwards thrust to car at each hoverpoint
            }
            //else //if raycast not detected at hoverHeight distance 
            //     //check if car is tilting and apply axle balance forces
            //{
            //    if (transform.position.y > hoverPoint.position.y)
            //        rBody.AddForceAtPosition(hoverPoint.up * hoverForce, hoverPoint.position);  //apply upward force relative to thruster position to 'un-tilt' / 'balance' the car 
            //    else
            //        rBody.AddForceAtPosition(hoverPoint.up * -hoverForce, hoverPoint.position); //apply upward force relative to thruster position to 'un-tilt' / 'balance' the car 
            //}
        }

        //Applying linear forces
        if (currLinearVel != 0.0f)
            rBody.AddForce(transform.forward * currLinearVel);

        if (currAngularVel != 0.0f)
            rBody.AddRelativeTorque(Vector3.up * currAngularVel);

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            PlayerHealthScript.Instance.TakeDamage(0.5f);
        }

        Instantiate(sparkPrefab, other.contacts[0].point, Quaternion.Euler(other.contacts[0].normal));
    }

    private void OnGUI()
    {

        //GUI.Label(new Rect(10, 10, 100, 100), angularForcePercent.ToString());

        //float speed = rBody.velocity.magnitude;
        //GUI.Label(new Rect(10, 10, 100, 100), speed.ToString());

    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
