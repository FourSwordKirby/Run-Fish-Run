using UnityEngine;
using System.Collections;

public class CatScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log("living");
        Destroy(gameObject);
    }  
}
