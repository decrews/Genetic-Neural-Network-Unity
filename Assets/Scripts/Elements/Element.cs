using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour {
	public abstract void UseElement(Vector3 pos, Vector2 dir);
}
