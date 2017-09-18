using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DynamicGrid : MonoBehaviour {

	int col;
	void Start () {
		GameObject canvas = GameObject.Find("Canvas");
		RectTransform parent = canvas.GetComponent<RectTransform>();
		GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup>();
		col = grid.constraintCount;
		float space = grid.spacing.x * (col-1) + grid.padding.left + grid.padding.right;

		grid.cellSize = new Vector2((parent.rect.width-space)/col, (parent.rect.width-space)/col);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
