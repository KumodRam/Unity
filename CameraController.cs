using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  //This is camera controller script that is used for how player is looking
    public Transform followTarget;
    public float distance = 5f;
    public float rotationSpeed = 2f;
    public float minVerticleAngle = -5;
    public float maxVerticleAngle = 45; 

    public Vector2 framingOffset;

     // for invert the camera
    public bool invertX;
    public bool invertY;
   
    float rotationX;
    float rotationY;

    float invertXVal;
    float invertYVal;


    private void Start(){
      //for hide the cursor in game mode
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked; 
    }
   private void Update(){

    invertXVal = (invertX) ? -1: 1;
    invertYVal = (invertY) ? -1: 1;

    //rotation of camera according to mouse move
    rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;
    rotationX += Input.GetAxis("Mouse Y") * invertYVal * rotationSpeed;
    rotationX = Mathf.Clamp(rotationX, minVerticleAngle, maxVerticleAngle);

    var targetRotation = Quaternion.Euler(rotationX,rotationY,0);
    var focusPosition = followTarget.position + new Vector3(framingOffset.x,framingOffset.y);
    transform.position = focusPosition - targetRotation* new Vector3(0,0,distance);

    //rotation with camera & camera always look at the player
    transform.rotation = targetRotation;

   }

   public Quaternion planerRotation => Quaternion.Euler(0,rotationY,0); 
}
