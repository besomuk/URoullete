using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// � 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net
public class DragAndDrop : MonoBehaviour 
{
    public float moveSpeed;
    public float offset = 0.05f;
    private bool following;

    // Use this for initialization
    void Start () 
	{
        following = false;
        offset += 10;
    }
  
  // Update is called once per frame
	void Update () 
	{
		//if (Input.GetMouseButtonDown(0) && ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).magnitude <= offset))
		if (Input.GetMouseButtonDown(0))
        {
            if (following)
            {
                following = false;
            }
            else
            {
                following = true;
            }
        }

        if (following)
        {
            this.transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), moveSpeed);
        }
	}
}