using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PhotoPreview : MonoBehaviour {

	public string photoPath;
	RawImage img;
	// Use this for initialization
	void Start () {
		//Debug.Log(photoPath);
		img = this.GetComponent<RawImage>();
		img.texture = ReadTexture(photoPath, Screen.width, Screen.height);
		// img.texture = ReadTexture("Assets/Resources/screenshot.png", Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	byte[] ReadPngFile(string path){
    FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
    BinaryReader bin = new BinaryReader(fileStream);
    byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);
     
    bin.Close();
     
    return values;
}
 
Texture ReadTexture(string path, int width, int height){
    byte[] readBinary = ReadPngFile(path);
     
    Texture2D texture = new Texture2D(width, height);
    texture.LoadImage(readBinary);
     
    return texture;
}
}
