using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SciFiSoldierController : MonoBehaviour
{
    Actions actions;
    Animator animator;
    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveForwardAmount = Input.GetAxis("Vertical");
        float rotateAmount = Input.GetAxis("Horizontal");
        float mouseMoveAmount = Input.GetAxis("Mouse X");
        
        if(Mathf.Abs(moveForwardAmount) > 0) {
            GetComponent<Transform>().rotation = Quaternion.Lerp(GetComponent<Transform>().rotation,
                Quaternion.LookRotation(Camera.main.transform.forward,Vector3.up),0.1f);
            
            Camera.main.transform.RotateAround(GetComponent<Transform>().position, Vector3.up, 
                Mathf.Lerp(0,Vector3.SignedAngle(Camera.main.transform.forward,
                    GetComponent<Transform>().forward,
                    Vector3.up),0.1f));
            if(Input.GetKey(KeyCode.LeftShift)) {
                actions.Run();
            } else {
                actions.Walk();
            }
        } else {
            actions.Stay();
        }
        GetComponent<Transform>().position += GetComponent<Transform>().forward*moveForwardAmount*
            animator.GetFloat("Speed")*Time.fixedDeltaTime;
        GetComponent<Transform>().RotateAround(GetComponent<Transform>().position,Vector3.up,
            rotateAmount*Time.fixedDeltaTime*movementSpeed);
        if(moveForwardAmount == 0) {
            Camera.main.transform.RotateAround(GetComponent<Transform>().position,Vector3.up,
                mouseMoveAmount*Time.fixedDeltaTime*movementSpeed);
        }
    }
}
