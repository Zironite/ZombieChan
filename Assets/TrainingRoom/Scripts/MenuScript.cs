using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    bool menuVisible = true;
    bool whoareweVisable = false;
    bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    void OnGUI()
    {
        if (menuVisible == true)
        {
            // ---------- Menu Buttons ----------
            GUI.BeginGroup(new Rect(50, 50, Screen.width - 100, Screen.height - 100));

            GUI.Box(new Rect(0, 0, Screen.width - 100, Screen.height - 100), "Zumbi-chan");

            if (gameStarted == true && GUI.Button(new Rect(Screen.width - 150, 10, 40, 40), "X"))
            {
                menuVisible = false;
                whoareweVisable = false;
            }

            if (GUI.Button(new Rect(40, 60, Screen.width - 180, 40), "New Game"))
            {
                menuVisible = false;
                gameStarted = true;
            }

            if (GUI.Button(new Rect(40, 130, Screen.width - 180, 40), "Restart Tutorial"))
            {
                menuVisible = false;
                gameStarted = true;
            }

            if (GUI.Button(new Rect(40, 200, Screen.width - 180, 40), "Who are we?"))
            {
                menuVisible = false;
                whoareweVisable = true;
            }

            GUI.EndGroup();
        }
        else
        {
            if (whoareweVisable == true)
            {
                GUI.BeginGroup(new Rect(200, 50, Screen.width - 400, Screen.height - 100));

                GUI.Box(new Rect(0, 0, Screen.width - 400, Screen.height - 100), "Zumbi-chan");

                if (GUI.Button(new Rect(Screen.width - 450, 10, 40, 40), "X"))
                {
                    menuVisible = true;
                    whoareweVisable = false;
                }

                GUI.EndGroup();
            }

            // ---------- Open menu Button ----------
            else if (GUI.Button(new Rect(Screen.width - 120, 20, 100, 40), "Menu"))
            {
                menuVisible = true;
            }
        }

        // Player can move only if no menu is open
        gameObject.GetComponent<SciFiSoldierController>().enabled = !menuVisible && !whoareweVisable;
        gameObject.GetComponent<Rigidbody>().useGravity = gameStarted;
    }
}
