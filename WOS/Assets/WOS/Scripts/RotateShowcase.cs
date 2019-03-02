using UnityEngine;
using System.Collections;

public class RotateShowcase : MonoBehaviour 
{
	public int speed;
	public float friction;
	public float lerpSpeed;

	private float yDeg;
	private Quaternion fromRotation;
	private Quaternion toRotation;

	// Use this for initialization
	void Start () 
	{
		resetPosition ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButton(0)) 
		{
			yDeg -= Input.GetAxis("Mouse X") * speed * friction;
			fromRotation = transform.rotation;
			toRotation = Quaternion.Euler(0,yDeg,0);
			transform.rotation = Quaternion.Lerp(fromRotation,toRotation,Time.deltaTime  * lerpSpeed);
		}
	}

	public void resetPosition() 
	{
		transform.rotation = new Quaternion (0, 220, 0, 0);
	}
}
