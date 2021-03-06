﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTruncateByCharacter : MonoBehaviour {

	public Text text;

	GridLayoutGroup grid;

	float cellSize,textLength;

	void Start () {
		grid = GetComponentInParent<GridLayoutGroup> ();
		
		Debug.Log (grid.cellSize.x);
		cellSize = grid.cellSize.x;
		text = GetComponent<Text> ();
		// full text length
		Debug.Log (LayoutUtility.GetPreferredWidth (text.rectTransform));
		textLength = LayoutUtility.GetPreferredWidth (text.rectTransform);
	}

	void Update () {
		textLength = LayoutUtility.GetPreferredWidth (text.rectTransform);
		if (cellSize < textLength) {
			text.text = text.text.Substring(0, text.text.Length - 1);
			Debug.Log (text.text);
		}
	}
		
}
