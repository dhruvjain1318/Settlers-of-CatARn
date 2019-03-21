using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {


    // The state of the marker is used to initialize animation, selction, etc.
    public enum MarkerState
    {
        Spawned,
        Animating,
        Selected,
        Deselected,
        Idle,
    };

    public ParticleSystem selectionparticles;

    [HideInInspector]
    public MarkerState tilestate;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (tilestate == TileBehaviour.MarkerState.Selected)
        {
            _Selected();
        }

        if (tilestate == TileBehaviour.MarkerState.Deselected)
        {
            _Deselected();
        }
	}

    void _Selected()
    {
        selectionparticles.Play();
    }

    void _Deselected()
    {
        selectionparticles.Stop();

    }

    void _Animating()
    {


    }
}
