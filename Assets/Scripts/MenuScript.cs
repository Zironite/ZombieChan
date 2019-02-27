using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public static bool menuVisible = true;
    bool whoareweVisable = false;
    static bool tutorialStarted = false;
    static bool gameStarted = false;

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

            GUI.Box(new Rect(50, 0, Screen.width - 200, Screen.height - 100), "Zumbi-chan");

            if ((gameStarted == true || tutorialStarted == true) && GUI.Button(new Rect(Screen.width - 140, 0, 40, 40), "X"))
            {
                menuVisible = false;
                whoareweVisable = false;
            }

            string gameText;
            if (gameStarted)
                gameText = "Restart Game";
            else
                gameText = "New Game";
            if (GUI.Button(new Rect(50, 20, Screen.width - 200, (Screen.height - 100 - 20) / 3), gameText))
            {
                menuVisible = false;
                tutorialStarted = false;
                gameStarted = true;
                SceneManager.LoadScene("BaseRoad", LoadSceneMode.Single);
            }

            string tutorialText;
            if (tutorialStarted)
                tutorialText = "Restart Tutorial";
            else
                tutorialText = "Start Tutorial";
            if (GUI.Button(new Rect(50, (Screen.height - 100 - 20) / 3 + 20, Screen.width - 200, (Screen.height - 100 - 20) / 3), tutorialText))
            {
                menuVisible = false;
                tutorialStarted = true;
                gameStarted = false;
                CheckPointScript.checkPoints = new List<GameObject>();
                SceneManager.LoadScene("Training box", LoadSceneMode.Single);
            }

            if (GUI.Button(new Rect(50, (Screen.height - 100 - 20) * 2 / 3 + 19, Screen.width - 200, (Screen.height - 100 - 20) / 3), "Who are we?"))
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
                GUI.BeginGroup(new Rect(300, 200, Screen.width - 540, Screen.height - 400));

                GUI.Box(new Rect(0, 0, Screen.width - 600, Screen.height - 400), "Zumbi-chan");

                if (GUI.Button(new Rect(Screen.width - 590, 0, 40, 40), "X"))
                {
                    menuVisible = true;
                    whoareweVisable = false;
                }

                GUI.Label(new Rect(10, (Screen.height - 400) / 3, Screen.width - 620, 40), "Maor Yakov Walter - 316046952");
                GUI.Label(new Rect(10, (Screen.height - 400) * 2 / 3, Screen.width - 620, 40), "Liron Levi - 207981713");

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
        gameObject.GetComponent<Rigidbody>().useGravity = (tutorialStarted || gameStarted) && 
            gameObject.GetComponent<SciFiSoldierController>().health > 0;
    }
}
