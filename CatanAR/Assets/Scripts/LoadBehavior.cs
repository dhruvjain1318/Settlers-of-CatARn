using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class LoadBehavior : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehavior;
    private GameObject loadedobj;
	// Use this for initialization
	void Start () {
        mTrackableBehavior = GetComponent<TrackableBehaviour>();
        if(mTrackableBehavior)
        {
            mTrackableBehavior.RegisterTrackableEventHandler(this);
        }
	}
	
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (this.tag == "Forest")
            {
                loadedobj = Instantiate(Resources.Load("Prefabs/Forest") as GameObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                loadedobj.transform.parent = this.transform;
                loadedobj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                loadedobj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                loadedobj.transform.localRotation = Quaternion.identity;
            }
            if (this.tag == "Grain")
            {
                loadedobj = Instantiate(Resources.Load("Prefabs/Wheat Field") as GameObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                loadedobj.transform.parent = this.transform;
                loadedobj.transform.localPosition = new Vector3(-1.4f, 0.0f, -0.1f);
                loadedobj.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                loadedobj.transform.localRotation = Quaternion.identity;
            }
            if (this.tag == "Pasture")
            {
                loadedobj = Instantiate(Resources.Load("Prefabs/Animals") as GameObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                loadedobj.transform.parent = this.transform;
                loadedobj.transform.localPosition = new Vector3(0.4f, 0.2f, 0.12f);
                loadedobj.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                loadedobj.transform.localRotation = Quaternion.identity;
            }
            if (this.tag == "Ore")
            {
                loadedobj = Instantiate(Resources.Load("Prefabs/Quarry") as GameObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                loadedobj.transform.parent = this.transform;
                loadedobj.transform.localPosition = new Vector3(0.75f, -0.04f, -1.91f);
                loadedobj.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                loadedobj.transform.localRotation = Quaternion.identity;
            }
            if (this.tag == "Brick")
            {
                loadedobj = Instantiate(Resources.Load("Prefabs/Brick House") as GameObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                loadedobj.transform.parent = this.transform;
                loadedobj.transform.localPosition = new Vector3(-0.25f, 0.07f, -0.11f);
                loadedobj.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
                loadedobj.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            Debug.Log("Well at least I did something");
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
