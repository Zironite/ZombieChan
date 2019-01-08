using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SciFiSoldierController : MonoBehaviour
{
    Actions actions;
    Animator animator;
    
    Vector3 defaultCemeraPosition;

    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();
        defaultCemeraPosition = new Vector3(0,1,-1);
    }

    // Update is called once per frame
    void Update()
    {
        float moveForwardAmount = Input.GetAxis("Vertical");
        float rotateAmount = Input.GetAxis("Horizontal");
        float mouseMoveAmountX = Input.GetAxis("Mouse X");
        float mouseMoveAmountY = Input.GetAxis("Mouse Y");

        if(Mathf.Abs(moveForwardAmount) > 0) {
            GetComponent<Transform>().rotation = Quaternion.Lerp(GetComponent<Transform>().rotation,
                Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x,0,Camera.main.transform.forward.z),Vector3.up),0.1f);
            
            Camera.main.transform.RotateAround(GetComponent<Transform>().position, Vector3.up, 
                Mathf.Lerp(0,Vector3.SignedAngle(Camera.main.transform.forward,
                    GetComponent<Transform>().forward,
                    Vector3.up),0.1f));
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation,
                Quaternion.LookRotation(GetComponent<Transform>().forward,Vector3.up),0.1f);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                GetComponent<Transform>().position + GetComponent<Transform>().rotation*defaultCemeraPosition, 0.1f);
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
                mouseMoveAmountX*Time.fixedDeltaTime*movementSpeed);
            Camera.main.transform.RotateAround(GetComponent<Transform>().position, Camera.main.transform.right,
                mouseMoveAmountY*Time.fixedDeltaTime*movementSpeed);
            Camera.main.transform.LookAt(GetComponent<Transform>().position + Vector3.up);
        }
    }
}
