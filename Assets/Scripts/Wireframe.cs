using UnityEngine;
using System.Collections.Generic;

public class Wireframe : MonoBehaviour {
	public static bool wireframeEnabled = false;

    void Start()
    {
        wireframeEnabled = false;
    }

    void Update () {
		MeshFilter[] meshFilters = GameObject.FindObjectsOfType<MeshFilter>();

		foreach(MeshFilter mf in meshFilters)
        {
			if((LayerMask.GetMask("RealWorld") & mf.gameObject.layer) == 0 && mf.gameObject.GetComponent<WireframeConverter>() == null)
            {
				mf.gameObject.AddComponent<WireframeConverter>();
			}
        }
	}
}
