using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	bool hasPressed = false;
	
	void Update () 
	{
		if(Input.touchCount > 0 || Input.GetMouseButton(0))
		{
			if (hasPressed == true)
				return;
			
			SceneManager.LoadScene (1);
			hasPressed = true;
		}
	}
}
