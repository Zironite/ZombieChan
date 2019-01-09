using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SciFiSoldierController : MonoBehaviour
{
    Actions actions;
    Animator animator;
    PlayerController playerController;
    Vector3 defaultCemeraPosition;
    bool resettingCameraPosition;

    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();
        defaultCemeraPosition = new Vector3(0,1,-1);
        playerController = GetComponent<PlayerController>();
        playerController.SetArsenal("Rifle");
        resettingCameraPosition = false;
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

        if(Input.GetKeyDown(KeyCode.Space)) {
            actions.Jump();
            GetComponent<Rigidbody>().AddForce(Vector3.up*3,ForceMode.Impulse);
        }

        if(Input.GetKeyDown(KeyCode.C)) {
            actions.Sitting();
        }

        if(Input.GetMouseButton(1)) {
            actions.Aiming();
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                GetComponent<Transform>().position + GetComponent<Transform>().rotation*new Vector3(-0.2f,0.9f,-0.5f),
                0.1f);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation,
                GetComponent<Transform>().rotation,0.1f);

            if(Input.GetMouseButton(0)) {
                actions.Attack();
            }
        } else {
            if (Input.GetMouseButtonUp(1)) {
                resettingCameraPosition = true;
            }
            GetComponent<Transform>().position += GetComponent<Transform>().forward*moveForwardAmount*
                animator.GetFloat("Speed")*Time.fixedDeltaTime;
            GetComponent<Transform>().RotateAround(GetComponent<Transform>().position,Vector3.up,
                rotateAmount*Time.fixedDeltaTime*movementSpeed);
            if(resettingCameraPosition) {
                var newCameraPosition = GetComponent<Transform>().position + GetComponent<Transform>().rotation*defaultCemeraPosition;
                var newCameraRotation = Quaternion.LookRotation(GetComponent<Transform>().forward,Vector3.up);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCameraPosition, 0.1f);
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, newCameraRotation, 0.1f);

                if((Camera.main.transform.position - newCameraPosition).magnitude <= 0.001f &&
                    (Camera.main.transform.rotation.eulerAngles - newCameraRotation.eulerAngles).magnitude <= 0.001f) {
                        resettingCameraPosition = false;
                    }
            } else if(moveForwardAmount == 0) {
                Camera.main.transform.RotateAround(GetComponent<Transform>().position,Vector3.up,
                    Mathf.Clamp(mouseMoveAmountX,-5,5)*Time.fixedDeltaTime*movementSpeed);
                var yAngle = mouseMoveAmountY*Time.fixedDeltaTime*movementSpeed;
                var maxAngle = 60;
                var minAngle = 20;
                var cameraDir = Camera.main.transform.position - GetComponent<Transform>().position;
                var angleBetweenCameraAndFloor = Vector3.Angle(cameraDir,
                    new Vector3(cameraDir.x,0,cameraDir.z));
                var newYAngle = Mathf.Clamp(angleBetweenCameraAndFloor+yAngle,minAngle,maxAngle)-angleBetweenCameraAndFloor;
                Camera.main.transform.RotateAround(GetComponent<Transform>().position, Camera.main.transform.right,
                    newYAngle);
                Camera.main.transform.LookAt(GetComponent<Transform>().position + Vector3.up);
            }
        }
    }
}
