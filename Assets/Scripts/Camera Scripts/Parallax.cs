using UnityEngine;

public class Parallex : MonoBehaviour {

	private float length, startPos;
	public GameObject cam;
	public float parallexEffect;
	public bool isAffectedOnY;

	void Start () {
		startPos = transform.position.x;
		length = GetComponent<SpriteRenderer>().bounds.size.x;
	}
	
	void FixedUpdate () {
		float temp = (cam.transform.position.x * (1-parallexEffect));
		float dist = (cam.transform.position.x*parallexEffect);

	if (isAffectedOnY)
		{
			transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
		} else {
			transform.position = new Vector3(startPos + dist, 0f, transform.position.z);
		}

		if      (temp > startPos + length) startPos += length;
		else if (temp < startPos - length) startPos -= length;
	}

}
