using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairScript : MonoBehaviour
{
    public GameObject gunMuzzleFlash;
    Color defaultColor;
    Color aimedOnEnemyColor;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = new Color(0,0,0,0.75f);
        aimedOnEnemyColor = new Color(256,0,0,defaultColor.a);
        GetComponent<Image>().material.color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        var screenPoint = 
            Camera.main.WorldToScreenPoint(gunMuzzleFlash.transform.position + 
                100000*Camera.main.transform.forward);
        var rectTransform = (RectTransform) transform;
        rectTransform.position = new Vector3(screenPoint.x,screenPoint.y,0);
        RaycastHit info;
        if (Physics.Raycast(gunMuzzleFlash.transform.position, Camera.main.transform.forward, out info)) {
            RaycastHit secondHitInfo;
            
            if(Physics.Raycast(gunMuzzleFlash.transform.position, Camera.main.transform.forward, out secondHitInfo, 
                Mathf.Infinity, ~0)) {
                if ((info.point - secondHitInfo.point).magnitude <= 1f) {
                    GetComponent<Image>().material.color = 
                        Color.Lerp(GetComponent<Image>().material.color,aimedOnEnemyColor,0.1f);
                }
            }
        } else {
            GetComponent<Image>().material.color = Color.Lerp(GetComponent<Image>().material.color,defaultColor,0.5f);
        }
    }
}
