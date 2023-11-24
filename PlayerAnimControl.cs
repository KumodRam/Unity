using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float ySpeed = 10f;
    // public float isWalking = 0.2f;
    
    private Animator anim;

    void Start(){
        anim = GetComponent<Animator>();

    }
     void Update(){
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
            anim.SetBool("isWalking", true);
        }
        else{
            anim.SetBool("isWalking", false);
        }

         if(Input.GetKey(KeyCode.UpArrow)){
            anim.SetBool("isClimbingUp", true);
            transform.position += new Vector3(0,ySpeed,0);
        }
        else{
            anim.SetBool("isClimbingUp", false);
        }
     }




}
