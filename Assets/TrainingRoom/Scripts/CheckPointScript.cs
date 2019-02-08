using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    GameObject parent;
    public static List<GameObject> checkPoints = new List<GameObject>();
    private Animator trainingAnimator;
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        trainingAnimator = GameObject.FindGameObjectWithTag("TrainingWall").GetComponent<Animator>();
        checkPoints.Add(parent);
        if (parent.name != "CheckPoint1")
        {
            parent.SetActive(false);
        }
        else
        {
            parent.transform.Find("Canvas").gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (parent.name)
        {
            case "CheckPoint1":
                parent.transform.Find("Canvas").gameObject.SetActive(true);
                checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint2").SetActive(true);
                checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint2")
                    .transform.Find("Canvas").gameObject.SetActive(false);
                break;
            case "CheckPoint2":
                trainingAnimator.SetBool("Status", false);
                parent.transform.Find("Canvas").gameObject.SetActive(true);
                checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint3").SetActive(true);
                checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint3")
                    .transform.Find("Canvas").gameObject.SetActive(false);
                break;
            case "CheckPoint3":
                parent.transform.Find("Canvas").gameObject.SetActive(true);
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        switch (parent.name)
        {
            case "CheckPoint1":
                parent.SetActive(false);
                break;
            case "CheckPoint2":
                parent.SetActive(false);
                break;
        }
    }
}
