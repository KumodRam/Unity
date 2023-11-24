using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstPersonMove : MonoBehaviour{

    [Header("References")]
    public Rigidbody rb;
    public Transform head;
    public Camera camera;


    [Header("Configuration")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float itemPickupDistance;


    // [Header("Camera Effects")]
    // public float baseCameraFov = 60f;
    // public float baseCameraHeight = 0.85f;

    // public float walkBobbingRate = 0.75f;
    // public float runBobbingRate = 1f;
    // public float maxWalkBobbingOffset = 0.2f;
    // public float maxRunBobbingOffset = 0.3f;


    [Header("Runtime")]
    Vector3 newVelocity;
    bool isGrounded = false;
    bool isJumping = false;

    Transform attachedObject = null;
    float attachedDistance = 0f;


    // Start is called before the first frame update
    void Start(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; //locked the mouse curson on centre of screen
    }

    // Update is called once per frame
    void Update() {
        //Horizontal roatation
        transform.Rotate(Vector3.up *Input.GetAxis("Mouse X") * 2f);

        newVelocity = Vector3.up * rb.velocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed :walkSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;

        if(isGrounded){
        if(Input.GetKeyDown(KeyCode.Space) && !isJumping){
                newVelocity.y = jumpSpeed;
                isJumping = true;
            }
        }

        RaycastHit hit;
        bool cast = Physics.Raycast(head.position, head.forward, out hit, itemPickupDistance);


        if (Input.GetKeyDown(KeyCode.F)) {
        //  Drop the picked object
        if (attachedObject != null) {
            attachedObject.SetParent(null);

            //  Disable is kinematic for the rigidbody, if any
            if (attachedObject.GetComponent<Rigidbody>() != null)
                attachedObject.GetComponent<Rigidbody>().isKinematic = false;

            //  Enable the collider, if any
            if (attachedObject.GetComponent<Collider>() != null)
                attachedObject.GetComponent<Collider>().enabled = true;

            attachedObject = null;
        }
        //  Pick up an object
        else {
            if (cast) {
                if (hit.transform.CompareTag("pickable")) {
                    attachedObject = hit.transform;
                    attachedObject.SetParent(transform);

                    attachedDistance = Vector3.Distance(attachedObject.position, head.position);

                    //  Enable is kinematic for the rigidbody, if any
                    if (attachedObject.GetComponent<Rigidbody>() != null)
                        attachedObject.GetComponent<Rigidbody>().isKinematic = true;

                    //  Disable the collider, if any
                    //  This is necessary
                    if (attachedObject.GetComponent<Collider>() != null)
                        attachedObject.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
    }

    void FixedUpdate(){ 
        rb.velocity = transform.TransformDirection(newVelocity);

        //  Shoot a ray of 1 unit towards the ground
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f)) {
            isGrounded = true;
        }
        else isGrounded = false;
    }

    void LateUpdate(){
        //Vertical rotation
        Vector3 e = head.eulerAngles;
        e.x -= Input.GetAxis("Mouse Y") * 2f;
        e.x = RestrictAngle(e.x,-85f,85f);
        head.eulerAngles = e;



        if (attachedObject != null) {
            attachedObject.position = head.position + head.forward * attachedDistance;
            attachedObject.Rotate(transform.right * Input.mouseScrollDelta.y * 30f, Space.World);
        }
     }
    

    //clamp the verticle head rotation(prevent bending backward)

    public static float RestrictAngle(float angle,float angleMin,float angleMax){
        if(angle > 180)
        angle -= 360;
        else if (angle <-180)
        angle += 360;

        if(angle > angleMax)
        angle = angleMax;
        
        if(angle < angleMin)
        angle = angleMin;

        return angle;
    }

    void OnCollisionStay(Collision col){
        isGrounded = true;
        isJumping = false;
    }


    void OnCollisionExit(Collision col){
        isGrounded = false;
    }
}

