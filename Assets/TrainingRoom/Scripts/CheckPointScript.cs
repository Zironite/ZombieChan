using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    GameObject parent;
    public static List<GameObject> checkPoints = new List<GameObject>();
    public static bool SleepEnded = false;
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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
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
                    checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint4").SetActive(true);
                    checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint4")
                        .transform.Find("Canvas").gameObject.SetActive(false);
                    checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint4")
                        .transform.Find("CanvasFail").gameObject.SetActive(false);
                    SleepEnded = true;
                    break;
                case "CheckPoint4":
                    if (SleepEnded == false)
                    {
                        checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint3").SetActive(true);
                        checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint4")
                            .transform.Find("CanvasFail").gameObject.SetActive(true);
                    }
                    else
                    {
                        parent.transform.Find("Canvas").gameObject.SetActive(true);
                        checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint3").SetActive(false);
                        checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint5").SetActive(true);
                        checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint5")
                            .transform.Find("Canvas").gameObject.SetActive(false);
                    }
                    break;
                case "CheckPoint5":
                    trainingAnimator.SetBool("Status", true);
                    parent.transform.Find("Canvas").gameObject.SetActive(true);
                    checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint6").SetActive(true);
                    GameObject canvas = checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint6")
                        .transform.Find("Canvas").gameObject;
                    checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint6")
                        .transform.Find("Canvas").gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            switch (parent.name)
            {
                case "CheckPoint1":
                    parent.SetActive(false);
                    break;
                case "CheckPoint2":
                    parent.SetActive(false);
                    break;
                case "CheckPoint3":
                    parent.transform.Find("Canvas").gameObject.SetActive(false);
                    StartCoroutine(RunToCheckPoint4());
                    break;
                case "CheckPoint4":
                    parent.SetActive(false);
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (parent.name == "CheckPoint6")
            {
                checkPoints.Single<GameObject>(checkpoint => checkpoint.name == "CheckPoint5").SetActive(false);
                parent.transform.Find("Canvas").gameObject.SetActive(true);
                StartCoroutine(ShowMenu());
            }
        }
    }

    private IEnumerator RunToCheckPoint4()
    {
        yield return new WaitForSeconds(3);
        SleepEnded = false;
    }

    private IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(3);
        MenuScript.menuVisible = true;
    }
}
