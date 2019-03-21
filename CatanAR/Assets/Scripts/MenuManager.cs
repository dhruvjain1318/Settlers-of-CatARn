using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public Button start;

	// Use this for initialization
	void Start () {

        start.onClick.AddListener(start_game);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void start_game()

    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);


    }

}
