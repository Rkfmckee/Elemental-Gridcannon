using System.ComponentModel;
using static Element;

public class CardType {
	#region Properties

	private CardValue value;
	private ElementType suit;
	private string type;

	#endregion

	#region Get/Set Methods

	public CardValue GetValue() {
		return value;
	}

	public ElementType GetSuit() {
		return suit;
	}

	public new string GetType() {
		return type;
	}

	public void SetType(CardValue value, ElementType suit) {
		this.value = value;
		this.suit  = suit;
		type       = $"{value.GetDescription()}{suit.GetDescription()}";
	}

	#endregion

	#region Enums

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
