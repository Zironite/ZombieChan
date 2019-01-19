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
    public GameObject spine;
    Quaternion spineRotation;
    public GameObject gunMuzzleFlash;
    bool isFiring;

    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();
        defaultCemeraPosition = new Vector3(0,1,-1);
        playerController = GetComponent<PlayerController>();
        playerController.SetArsenal("Rifle");
        resettingCameraPosition = false;
        spineRotation = Quaternion.Euler(355.6f, 354.2f, 7.8f);
        isFiring = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float moveForwardAmount = Input.GetAxis("Vertical");
        float rotateAmount = Input.GetAxis("Horizontal");
        float mouseMoveAmountX = Input.GetAxis("Mouse X");
        float mouseMoveAmountY = Input.GetAxis("Mouse Y");
        var yAngle = mouseMoveAmountY*Time.fixedDeltaTime*movementSpeed*10;
        var maxAngle = 60;
        var minAngle = 20;
        var cameraDir = Camera.main.transform.position - GetComponent<Transform>().position;
        var angleBetweenCameraAndFloor = Vector3.Angle(cameraDir,
            new Vector3(cameraDir.x,0,cameraDir.z));
        var newYAngle = Mathf.Clamp(angleBetweenCameraAndFloor+yAngle,minAngle,maxAngle)-angleBetweenCameraAndFloor;
                
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
            GetComponent<Transform>().RotateAround(GetComponent<Transform>().position,Vector3.up,
                mouseMoveAmountX*Time.fixedDeltaTime*movementSpeed*10);
            Debug.Log(yAngle);
            Debug.Log(spine.transform.localRotation.eulerAngles);
            spine.transform.localRotation = spineRotation;
            spine.transform.localRotation = Quaternion.Lerp(spine.transform.localRotation,
                spine.transform.localRotation*Quaternion.Euler(0,0,-yAngle*4),0.1f);
            spineRotation = spine.transform.localRotation;
            Debug.Log(spine.transform.localRotation.eulerAngles);
            var relativeCameraRotation = Quaternion.Euler(-spineRotation.eulerAngles.z,0,0);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                spine.transform.position - 0.7f*spine.transform.up - 0.4f*spine.transform.right - 0.1f*spine.transform.forward,
                0.1f);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation,
                GetComponent<Transform>().rotation*relativeCameraRotation,0.1f);

            if(Input.GetMouseButton(0) && !isFiring) {
                isFiring = true;
                actions.Attack();
                StartCoroutine(ActivateMuzzleFlash());
            }
        } else {
            if (Input.GetMouseButtonUp(1)) {
                resettingCameraPosition = true;
                spineRotation = Quaternion.Euler(355.6f, 354.2f, 7.8f);
            }
            GetComponent<Transform>().position += GetComponent<Transform>().forward*moveForwardAmount*
                animator.GetFloat("Speed")*movementSpeed*Time.fixedDeltaTime;
            GetComponent<Transform>().RotateAround(GetComponent<Transform>().position,Vector3.up,
                rotateAmount*Time.fixedDeltaTime*movementSpeed*10);
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
                    Mathf.Clamp(mouseMoveAmountX,-5,5)*Time.fixedDeltaTime*movementSpeed*10);
                
                Camera.main.transform.RotateAround(GetComponent<Transform>().position, Camera.main.transform.right,
                    newYAngle);
                Camera.main.transform.LookAt(GetComponent<Transform>().position + Vector3.up);
            }
        }
    }

    public IEnumerator ActivateMuzzleFlash() {
        gunMuzzleFlash.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        gunMuzzleFlash.GetComponent<Light>().enabled = false;
        StartCoroutine(FiringCooldown());
    }

    public IEnumerator FiringCooldown() {
        yield return new WaitForSeconds(0.3f);
        isFiring = false;
    }
}
