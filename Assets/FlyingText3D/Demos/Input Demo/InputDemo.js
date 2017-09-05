private var textObject : GameObject;
private var enteredText : String;
private var cursorChar = "-"[0];
private var acceptInput : boolean;

function Start () {
	InitializeText();
}

function InitializeText () {
	FlyingText.addRigidbodies = false;
	enteredText = "";
	acceptInput = true;
	textObject = FlyingText.GetObject ("-", Vector3(-7, 6, 0), Quaternion.identity);
	InvokeRepeating ("BlinkCursor", .5, .5);
}

function OnGUI () {
	if (!acceptInput) return;
	
	GUI.Label (Rect(10, 10, 500, 30), "Type some text! Hit return when done.");
}

function Update () {
	if (!acceptInput) return;
	
	for (var c in Input.inputString) {
		if (c == "\b"[0]) {
			if (enteredText.Length > 0) {
				enteredText = enteredText.Substring (0, enteredText.Length - 1);
			}
		}
		else if (c == "\n"[0] || c == "\r"[0]) {
			if (enteredText.Length > 0) {
				ExplodeText();
			}
		}
		else if (c == "<"[0] || c == ">"[0]) {
			// do nothing
		}
		else {
			enteredText += c;
		}
		FlyingText.UpdateObject (textObject, enteredText + cursorChar);
	}
}

function ExplodeText () {
	acceptInput = false;
	CancelInvoke ("BlinkCursor");
	Destroy (textObject);
	FlyingText.addRigidbodies = true;
	var letters = FlyingText.GetObjectsArray (enteredText, Vector3(-7, 6, 0), Quaternion.identity);
	for (var letter in letters) {
		letter.GetComponent(Rigidbody).useGravity = false;
		letter.GetComponent(Rigidbody).AddTorque (Vector3(Random.Range(-1.0, 1.0), Random.Range(-1.0, 1.0), Random.Range(-1.0, 1.0)) * 10.0);
		letter.GetComponent(Rigidbody).AddExplosionForce (390.0, Vector3(0, 1, 11), 15.0);
	}
	
	yield WaitForSeconds (5);
	for (var letter in letters) {
		Destroy (letter);
	}
	InitializeText();
}

function BlinkCursor () {
	if (cursorChar == "-"[0]) {
		cursorChar = " "[0];
	}
	else {
		cursorChar = "-"[0];
	}
	FlyingText.UpdateObject (textObject, enteredText + cursorChar);
}