using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableManager : MonoBehaviour 
{ 
	public Button btnRoll;              // spin wheel button
	public Button btnClear;             // clear UI button
	public Button btnMoney;             // button for reseting money
	public Text rolledNumberLabel;      // "It's "number" text"

	public GameObject[] chips;          // list of chips currently on board
	public GameObject chip_win_glow;    // glow effect for winning chips
	public GameObject wheel;            // wheel
	public GameObject camera;           // holds info on camera
	public GameObject canvas;           // holds info on canvas
	public GameObject canvasNumber;     // canvas that holds info for showing on Wheel screem
	public GameObject ball;             // ball. It has pivot outside it, so it can rotate and stay on number 
	public GameObject rolledNumberIcon; // icon that shows what number is winning on table itself

	private Vector2 neutralRolledIconPosition = new Vector2 (10,-5); // neutral position ( some where outside camera view )

	private string fieldNamePrefix = "F_"; // field prefix.

	decimal win = 0;
	decimal bet = 0 ;
	 
	private bool checkOne = false;      // check if only one number is hit

	private float wheelRotationTime = 6;
	int roll;

	void Start () 
	{
		btnRoll.onClick.AddListener( BtnRollClick );      // events for btnRoll
		btnClear.onClick.AddListener( BtnClearClick );    // events for btnClear
		btnMoney.onClick.AddListener( BtnMoney );         // events for btnMoney

		canvasNumber.SetActive( false );

	}

	/** Reset money */
	void BtnMoney ()
	{
		GameManager.gm.TotalMoney = 1000;
		GameManager.gm.UpdateUI();
	}

	/** Clear all, reset bets and wins */
	void BtnClearClick()
	{
		btnRoll.gameObject.SetActive( true );

		GameManager.gm.TotalMoney += GameManager.gm.TotalWin;
		GameManager.gm.TotalBet = 0;
		GameManager.gm.TotalWin = 0;
		GameManager.gm.txtRolledNumber.text = "Rolled: " ;
		GameManager.gm.UpdateUI();

		KillChips();
	}

	/** Make me rich, roll the wheel */
	void BtnRollClick ()
	{
		GameManager.gm.TotalWin = 0;
		checkOne = true;

		if( GameManager.gm.TotalBet == 0 )              // is bet on the table?
		{
			GameManager.gm.SetInfoText( "NO BET" );
		}
		else
		{
			btnRoll.gameObject.SetActive( false );

			/** Animate camera **/
			roll = GameManager.gm.GetRandomNumber();    /// get the random number
			int angle;

			/** Hashtable for setting angle and numbers */
			#region hash table for angles and number
			Hashtable hashtable = new Hashtable();
			hashtable.Add("1", 273);
			hashtable.Add("2", 92);
			hashtable.Add("3", 312);
			hashtable.Add("4", 132);
			hashtable.Add("5", 350);
			hashtable.Add("6", 170);
			hashtable.Add("7", 28);
			hashtable.Add("8", 208);
			hashtable.Add("9", 68);
			hashtable.Add("10", -114);
			hashtable.Add("11", -323);
			hashtable.Add("12", 218);
			hashtable.Add("13", 283);
			hashtable.Add("14", 104);
			hashtable.Add("15", 322);
			hashtable.Add("16", 141);
			hashtable.Add("17", 0);
			hashtable.Add("18", 178);
			hashtable.Add("19", 198);
			hashtable.Add("20", 19);
			hashtable.Add("21", 160);
			hashtable.Add("22", -19);
			hashtable.Add("23", 123);
			hashtable.Add("24", -57);
			hashtable.Add("25", 235);
			hashtable.Add("26", 57);
			hashtable.Add("27", -105);
			hashtable.Add("28", 75);
			hashtable.Add("29", -133);
			hashtable.Add("30", 47);
			hashtable.Add("31", 188);
			hashtable.Add("32", 10);
			hashtable.Add("33", 151);
			hashtable.Add("34", -28);
			hashtable.Add("35", 112);
			hashtable.Add("36", -66);
			hashtable.Add("0", 85);
			#endregion

			angle = (int)hashtable[roll.ToString()]; 

			ball.transform.localEulerAngles = new Vector3(0, 0, angle);  // put ball to winning number
				
			canvas.SetActive( false );             // turn off UI

			iTween.MoveTo ( camera, iTween.Hash (  // move camera to wheel
				"x", wheel.transform.position.x,
				"y", wheel.transform.position.y,
				"time", 1 ));


			iTween.RotateBy ( ball, iTween.Hash (  // rotate wheel for 20 times for 5 seconds. This creates illusion that number is determined in this step
				"x", 0,
				"y", 0,
				"z", 20,
				"time", 10,
			    "oncomplete", "showNumber",
				"oncompletetarget", gameObject ));


			iTween.MoveTo ( camera, iTween.Hash (  // after some time, move back to table
				"x", 0,
				"y", 0,
				"delay", 12, 
				"onstart", "StartMovingBackEvents",
				"onstarttarget", gameObject,
				"oncomplete", "CameraIsBack",
				"oncompletetarget", gameObject,
				"time", 1 ));
			
			PlaceRolledNumber( roll ); // put rolled number to table

			chips = GameObject.FindGameObjectsWithTag( "CHIP" ); // get all chips

			for( int i = 0; i < chips.Length; i++ )
			{
				int chipValue = chips[i].gameObject.GetComponent<Chip>().GetChipValue();              // get chip value
				List<GameObject> col = chips[i].gameObject.GetComponent<Chip>().GetChipCollisions();  // get chip collisions
				Vector2 chipPosition = chips[i].gameObject.GetComponent<Chip>().GetChipPosition();    // get chip position
				CheckWinningsForSingleChip( roll, chipValue, col, chipPosition );                     // check winnings for chip
							
			}

			print( chips.Length );

			GameManager.gm.UpdateUI(); // update ui
		}
	}

	void CheckWinningsForSingleChip ( int rolledNumber, int chipValue, List<GameObject> currentCollisions, Vector2 chipPosition )
	{
		print( "pocinje provera " );
		win = 0;
		bet = chipValue;

		//print( "Ukupno kolizija: " + currentCollisions.Count );

		/* proveri da li smo nesto dobili */
		for( int i = 0; i < currentCollisions.Count; i++ )
		{
			int no; // number on which chip is on

			// make number of field name
			string temp_string;
			temp_string = currentCollisions[i].name.Replace( fieldNamePrefix, "" );
			no = int.Parse( temp_string );

			// regular numbers
			if( no == rolledNumber )
			{
				if( currentCollisions.Count == 1 )
				{
					win = bet * 34 / 2 + 1;             // called twice ???
					GameManager.gm.TotalWin += win;
					MakeWinShadow( chipPosition );
					break;
					//checkOne = true;
				}
				else
				if( currentCollisions.Count == 2 )
				{
					win = bet * 16 / 2 + 1;
				}
				else
				if( currentCollisions.Count == 3 )
				{
					win = bet * 10 / 2 + 1;
				}
				else
				if( currentCollisions.Count == 4 )
				{
					win = bet * 8 / 2;
				}
				else
				{
					win = 0;
				}
				GameManager.gm.TotalWin += win;

				MakeWinShadow( chipPosition );
			}
			else
			{
				// check other winnings 
				if( currentCollisions.Count == 1 )
				{							
					Check_RedBlack( rolledNumber, chipValue, currentCollisions, chipPosition );
					Check_OddEven( rolledNumber, chipValue, currentCollisions, chipPosition );
					Check_Dozen( rolledNumber, chipValue, currentCollisions, chipPosition );
					Check_Half( rolledNumber, chipValue, currentCollisions, chipPosition );
					Check_Lines( rolledNumber, chipValue, currentCollisions, chipPosition );

				}
			}

		}
			//txtBetNumbers.text += no.ToString() + " ";




	}

	/** check for reds and blacks */
	void Check_RedBlack( int rolledNumber, int chipValue, List<GameObject> currentCollisions, Vector2 chipPosition )
	{
		int win_temp = 0;
		string color;
		if( GameManager.gm.isNumberBlack( rolledNumber ) )
			color = "BLACK";
		else
			color = "RED";

		if ( color == "RED" && currentCollisions[0].name == "F_100")
		{
			win_temp += 1 * chipValue;
			MakeWinShadow( chipPosition );
		}
		if ( color == "BLACK" && currentCollisions[0].name == "F_101")
		{
			win_temp += 1 * chipValue;

			MakeWinShadow( chipPosition );

		}	
		GameManager.gm.TotalWin += win_temp;

	}

	/** check for odd even */
	void Check_OddEven( int rolledNumber, int chipValue, List<GameObject> currentCollisions, Vector2 chipPosition )
	{
		int win_temp = 0;
		bool isEven = GameManager.gm.isNumberEven( rolledNumber );
		if( isEven && currentCollisions[0].name == "F_102" )
		{
			win_temp += 1 * chipValue;
			MakeWinShadow( chipPosition );
		}
		if( !isEven && currentCollisions[0].name == "F_103" )
		{
			win_temp += 1 * chipValue;
			MakeWinShadow( chipPosition );
		}
		GameManager.gm.TotalWin += win_temp;

	}

	/** check for dozens */
	void Check_Dozen( int rolledNumber, int chipValue, List<GameObject> currentCollisions, Vector2 chipPosition )
	{
		int win_temp = 0;
		if( currentCollisions[0].name == "F_104" && (rolledNumber >= 1 && rolledNumber <= 12) )
		{
			win_temp += 2 * chipValue;
			MakeWinShadow( chipPosition );
		}
		if( currentCollisions[0].name == "F_105" && (rolledNumber >= 13 && rolledNumber <= 24) )
		{
			win_temp += 2 * chipValue;
			MakeWinShadow( chipPosition );
		}
		if( currentCollisions[0].name == "F_106" && (rolledNumber >= 25 && rolledNumber <= 36) )
		{
			win_temp += 2 * chipValue;
			MakeWinShadow( chipPosition );
		}

		GameManager.gm.TotalWin += win_temp;

	}

	// checks for 1 - 18 and 19 - 36
	void Check_Half ( int rolledNumber, int chipValue, List<GameObject> currentCollisions, Vector2 chipPosition )
	{
		int win_temp = 1;
		if ( currentCollisions[0].name == "F_107" && ( rolledNumber >= 1 && rolledNumber <= 18 ) )
		{
			win_temp = 1 * chipValue;
			MakeWinShadow( chipPosition );
		}
		if ( currentCollisions[0].name == "F_108" && ( rolledNumber >= 19 && rolledNumber <= 36 ) )
		{
			win_temp = 1 * chipValue;
			MakeWinShadow( chipPosition );
		}			

		GameManager.gm.TotalWin += win_temp;
	}

	// Check Lines
	void Check_Lines ( int rolledNumber, int chipValue, List<GameObject> currentCollisions, Vector2 chipPosition )
	{
		int win_temp = 1;

		// check left column
		if( currentCollisions[0].name == "F_109" )
		{
			for( int i = 1; i <= 34; i += 3 )
			{
				if( rolledNumber == i )
				{
					win_temp = 2 * chipValue;
					MakeWinShadow( chipPosition );
					break;
				}
			}
		}

		// check middle column
		if( currentCollisions[0].name == "F_110" )
		{
			for( int i = 2; i <= 35; i += 3 )
			{
				if( rolledNumber == i )
				{
					win_temp = 2 * chipValue;
					MakeWinShadow( chipPosition );
					break;
				}
			}
		}

		// check right column
		if( currentCollisions[0].name == "F_111" )
		{
			for( int i = 3; i <= 36; i += 3 )
			{
				if( rolledNumber == i )
				{
					win_temp = 2 * chipValue;
					MakeWinShadow( chipPosition );
					break;
				}
			}
		}

		GameManager.gm.TotalWin += win_temp;

	}

	/** kill chips and shadows after spin */
	void KillChips()
	{
		GameObject[] chips;
		chips = GameObject.FindGameObjectsWithTag( "CHIP" );
		foreach( GameObject chip in chips )
		{
			Destroy( chip );
		}

		GameObject[] shadows;
		shadows = GameObject.FindGameObjectsWithTag( "CHIP_WIN_SHADOW" );
		foreach( GameObject shadow in shadows )
		{
			Destroy( shadow );
		}

		rolledNumberIcon.transform.position = neutralRolledIconPosition;
	}

	/** place rolled number icon on table */
	void PlaceRolledNumber ( int roll )
	{		
		// napravi ime objekta od broja.
		string fieldName;                          

		if( roll < 10 )
		{
			fieldName = fieldNamePrefix + "0" + roll.ToString();
		}
		else
		{
			fieldName = fieldNamePrefix + roll.ToString();
		}

		GameObject chip;
		chip = GameObject.Find( fieldName );

		rolledNumberIcon.transform.position = chip.transform.position;
	}

	IEnumerator WaitForWheel ()
	{
		yield return new WaitForSeconds( 6 ); // sacekaj
		canvas.SetActive ( true );
	}

	/** camera is going back to table, do things */
	void StartMovingBackEvents()
	{
		canvasNumber.SetActive( false );
	}

	/** show rolled number */
	void showNumber ()
	{
		canvasNumber.SetActive( true );
		rolledNumberLabel.text = "It's " + roll + "!";

	}

	/** camera is back at the table */
	void CameraIsBack()
	{
		canvas.SetActive( true );	
		canvasNumber.SetActive( false );
	}

	/** make win shadow */
	void MakeWinShadow ( Vector2 chipPosition )
	{
		GameManager.gm.TotalWin += win;
		GameObject b;
		b = Instantiate (chip_win_glow, chipPosition, Quaternion.identity);		
		b.tag = "CHIP_WIN_SHADOW";
	}
}
