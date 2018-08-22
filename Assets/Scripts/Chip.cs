using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour 
{
	public int chipValue = 5;                                     // value of single chip
	List<GameObject> currentCollisions = new List<GameObject>();  // holds all colliders and is crucial to determine what field is player betting on
	private bool valueAdded = false;                              // is chip taken from stack and put to the table?
	int no;

	void Start () 
	{
		
	}
	

	void Update () 
	{
		//if (Input.GetMouseButton(0))
		//	print ("down");
	}

	void OnMouseDown()
	{
		
		//this.transform.localScale = new Vector2( 0.20f, 0.20f );


		if( this.tag == "Stack" )
		{
			GameObject newChip;
			newChip = Instantiate( this.gameObject, new Vector2( this.transform.position.x, this.transform.position.y ), Quaternion.identity );
			newChip.tag = "Stack";
		}

		//newChip.GetComponent<SpriteRenderer>().sortingOrder = 0;
	}
	void OnMouseUp()
	{
		//this.transform.localScale = new Vector2( 0.15f, 0.15f );
		this.tag = "CHIP";
		//print( this.name );

		if( !valueAdded )
		{
			GameManager.gm.TotalBet += chipValue;
			GameManager.gm.TotalMoney -= this.chipValue;
			valueAdded = true;
			//GameManager.gm.RemoveNoBetText();
			GameManager.gm.SetInfoText( "" );
		}

		GameManager.gm.UpdateUI();
			
	}

	void OnMouseDrag ()
	{
		//this.transform.position = Input.mousePosition;

		//float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		Vector2 pos_move = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		transform.position = new Vector2( pos_move.x, pos_move.y);


	}

	public int GetChipValue()
	{
		return chipValue;
	}

	public Vector2 GetChipPosition()
	{
		return this.transform.position;
	}

	public List<GameObject> GetChipCollisions()
	{
		return currentCollisions;
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		currentCollisions.Add( other.gameObject );
	}

	void OnTriggerExit2D (Collider2D other)
	{
		currentCollisions.Remove( other.gameObject );
	}		
}
