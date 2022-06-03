using System.ComponentModel;
using UnityEngine;

public class CardType {
	#region Properties

	private CardValue value;
	private CardSuit suit;
	private string type;

	#endregion

	#region Get/Set Methods

	public CardValue GetValue() {
		return value;
	}

	public CardSuit GetSuit() {
		return suit;
	}

	public new string GetType() {
		return type;
	}

	public void SetType(CardValue value, CardSuit suit) {
		this.value = value;
		this.suit = suit;
		type = $"{value.GetDescription()}{suit.GetDescription()}";
	}

	public (CardSuit, CardSuit)? GetOppositeSuits() {
		switch (suit) {
			case CardSuit.Air:
				return (CardSuit.Earth, CardSuit.Water);
			case CardSuit.Earth:
				return (CardSuit.Air, CardSuit.Fire);
			case CardSuit.Fire:
				return (CardSuit.Water, CardSuit.Earth);
			case CardSuit.Water:
				return (CardSuit.Fire, CardSuit.Air);
		}

		return null;
	}

	#endregion

	#region Enums

	public enum CardSuit {
		Air,
		Earth,
		Fire,
		Water
	}

	public enum CardValue {
		[Description("2")]
		Two,
		[Description("3")]
		Three,
		[Description("4")]
		Four,
		[Description("5")]
		Five,
		[Description("6")]
		Six,
		[Description("7")]
		Seven,
		[Description("8")]
		Eight,
		[Description("9")]
		Nine,
		[Description("10")]
		Ten,
		[Description("J")]
		Jack,
		[Description("Q")]
		Queen,
		[Description("K")]
		King,
		[Description("A")]
		Ace,
	}

	#endregion
}
