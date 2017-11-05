using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    GameObject target;
    [SerializeField]
    float xMax;
    [SerializeField]
    float yMax;
    [SerializeField]
    float xMin;
    [SerializeField]
    float yMin;

	[Range(0f, 0.1f)]
	[SerializeField]
	float horizontalMoveSpace = 0.03f;
	[Range(0f, 0.1f)]
	[SerializeField]
	float verticalMoveSpace = 0.03f;

	[SerializeField]
	float smoothing = 0.5f;

    float leftCameraBound;
    float rightCameraBound;
	float upperCameraBound;
	float lowerCameraBound;

   	Vector2 velocity = Vector2.zero;

    void Start()
	{
        target = GameObject.FindGameObjectWithTag("Player");

		leftCameraBound = 0.5f - horizontalMoveSpace;
		rightCameraBound = 0.5f + horizontalMoveSpace;

		lowerCameraBound = 0.5f - verticalMoveSpace;
		upperCameraBound = 0.5f + verticalMoveSpace;

        //rightbound = Camera.main.ViewportToWorldPoint(new Vector3(Mathf.Abs(0.5f - rightCameraBound), 0, 0)).x;
        //Debug.Log(rightbound);
        //lastPos = target.transform.position;
    }

    /*
    bool MovingDiagonally()
    {
        currentPos = target.transform.position;
        if ((currentPos.x < lastPos.x || currentPos.x > lastPos.x) && (currentPos.y < lastPos.y || currentPos.y > lastPos.y))
        {
            movingDiagonally = true;
            //Debug.Log("You are moving diagonally! currentPos: " + currentPos + "lastPos: " + lastPos);
        }
        else
        {
            movingDiagonally = false;
        }
        lastPos = currentPos;

        return movingDiagonally;
    }
    */

    void LateUpdate()
    {
        // Holds the target camera positions with smoothing applied.
		Vector2 smooth = new Vector2(transform.position.x, transform.position.y);

        if (Camera.main.WorldToViewportPoint(target.transform.position).x <= (leftCameraBound - 0.01f))
        {
			smooth.x = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothing);
        } 

		else if (Camera.main.WorldToViewportPoint(target.transform.position).x >= (rightCameraBound + 0.01f)) 
		{
            smooth.x = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothing);
        }

		if (Camera.main.WorldToViewportPoint(target.transform.position).y <= (lowerCameraBound - 0.01f))
		{
			smooth.y = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothing);
		}

		else if (Camera.main.WorldToViewportPoint(target.transform.position).y >= (upperCameraBound + 0.01f))
		{
			smooth.y = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothing);
		}

		transform.position = new Vector3(Mathf.Clamp(smooth.x, xMin, xMax), Mathf.Clamp(smooth.y, yMin, yMax), transform.position.z);
			
    }
}
