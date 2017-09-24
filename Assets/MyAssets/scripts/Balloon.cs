using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {
	public Material red;
	public Material green;
	public Material blue;
	public Material pink;
	public Material yellow;

	// Use this for initialization
	void Start () {
		List<Material> materialList = new List<Material> {red, green, blue, pink, yellow};
		this.GetComponentInChildren<MeshRenderer>().material = materialList[Random.Range(0, materialList.Count)];
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(0, 0.001f, 0);
	}
}
