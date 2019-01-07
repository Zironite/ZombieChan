using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SciFiSoldierController : MonoBehaviour
{
    Actions actions;

    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<Actions>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveForwardAmount = Input.GetAxis("Vertical");
        float rotateAmount = Input.GetAxis("Horizontal");
        float mouseMoveAmount = Input.GetAxis("Mouse X");
        
        if(Mathf.Abs(moveForwardAmount) > 0) {
            GetComponent<Transform>().rotation = Quaternion.LookRotation(Camera.main.transform.forward,Vector3.up);
            Camera.main.transform.RotateAround(GetComponent<Transform>().position, Vector3.up, 
                Vector3.Angle(Camera.main.transform.forward,GetComponent<Transform>().forward));
            actions.Walk();
        } else {
            actions.Stay();
        }
        GetComponent<Transform>().position += Camera.main.transform.forward*moveForwardAmount*Time.fixedDeltaTime;
        GetComponent<Transform>().RotateAround(GetComponent<Transform>().position,Vector3.up,rotateAmount*Time.fixedDeltaTime*40);
        Camera.main.transform.RotateAround(GetComponent<Transform>().position,Vector3.up,mouseMoveAmount*Time.fixedDeltaTime*40);
    }
}
