using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public static GameManager gm;
	public Text txtMoney;          // total money player has
	public Text txtBet;            // total bet ( all chips )
	public Text txtWin;            // total win ( all chips )
	public Text txtRolledNumber;   // random number
	public Text txtInfo;

	private decimal totalBet = 0;
	private decimal totalWin = 0;
	private decimal totalMoney = 1000;

	void Start () 
	{
		if (gm == null)
			gm = this.GetComponent<GameManager>();

		txtInfo.text = "";

		UpdateUI();
	}

	/** Public property for total bets */
	public decimal TotalBet
	{
		get
		{
			return totalBet;
		}
		set
		{
			totalBet = value;
		}
	}

	/** Public property for total win */
	public decimal TotalWin
	{
		get
		{
			return totalWin;
		}
		set
		{
			totalWin = value;
		}
	}

	/** Public property for total money */
	public decimal TotalMoney
	{
		get
		{
			return totalMoney;
		}
		set
		{
			totalMoney = value;
		}
	}

	/** Public method for setting info tekst ( no bet )  */
	public void SetInfoText( string txt )
	{
		txtInfo.text = txt;
	}

	/** Public method for getting random number */
	public int GetRandomNumber()
	{
		int randomNumber;

		randomNumber = Random.Range( 0, 37 );
		//randomNumber = 32;
		txtRolledNumber.text = "Rolled: " + randomNumber;
			
		return randomNumber;
	}

	/** Public property for updating UI */
	public void UpdateUI()
	{
		txtMoney.text = "Total money: " + totalMoney;
		txtBet.text = "Bet:  " + totalBet;
		txtWin.text = "Win: " + totalWin;
	}

	/** not used at this moment */
	public void UpdateUI_AfterRoll()
	{
		txtMoney.text = "Total money: " + totalMoney;
		txtBet.text = "Bet:  " + totalBet;
		//txtRolledNumber.text = "Rolled: " + 
	}

	/** Public method to determine is number black or red */
	public bool isNumberBlack ( int n )
	{
		//bool res = true;

		if( n <= 10 )
		{
			if( n % 2 == 0 )
				return true;
			else
				return false;
		}
		if( n >= 11 && n<= 19)
		{
			if( n % 2 == 0 )
				return false;
			else
				return true;
		}
		if( n >= 20 && n <= 28 )
		{
			if( n % 2 == 0 )
				return true;
			else
				return false;			
		}
		if( n >= 29 && n <= 36 )
		{
			if( n % 2 == 0 )
				return false;
			else
				return true;			
		}
		return false;
	}

	/** Public method to determine is number even or odd */
	public bool isNumberEven ( int n )
	{
		if( n % 2 == 0 )
			return true;
		else
			return false;
	}
}
