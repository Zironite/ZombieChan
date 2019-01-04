using UnityEngine;
using System.Collections;

public class ZRNGUIController : MonoBehaviour {

	private bool menuVisible = true;
    private bool gameStarted = false;

    [SerializeField]
	GameObject[] QueryObjects;
	
	string playModeString;

	// Use this for initialization
	void Start () {

		this.GetComponent<AmbientController>().changeShadow(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnGUI () {

		if (menuVisible == true)
		{
			GUI.BeginGroup (new Rect (50, 50, Screen.width - 100, Screen.width - 100));

			GUI.Box (new Rect (0, 0, Screen.width - 100, 270), "Zumbi-chan");

            if (gameStarted == true && GUI.Button(new Rect(Screen.width - 100 - 50, 10, 40, 40), "X"))
            {
                menuVisible = false;
            }

            if (GUI.Button(new Rect(40, 60, Screen.width - 180, 40), "New Game"))
            {
                menuVisible = false;
                gameStarted = true;
            }

			if (GUI.Button (new Rect (40, 130, Screen.width - 180, 40), "Credits"))
			{
					this.GetComponent<AmbientController>().changeShadow(true);
			}

			if (GUI.Button (new Rect (40, 200, Screen.width - 180, 40), "Epic Scenes"))
			{
				this.GetComponent<AmbientController>().changeParticle(AmbientController.ParticleType.PARTICLE_NONE);
			}

            GUI.EndGroup();
		}
		else
		{
			// ---------- Menu Button ----------
			if (GUI.Button (new Rect (Screen.width - 120 , 20, 100, 40), "Menu"))
			{
				menuVisible = true;
			}
		}
	}
}
