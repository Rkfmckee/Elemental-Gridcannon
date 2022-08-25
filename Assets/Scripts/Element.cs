using System.Collections.Generic;

public static class Element
{
	public static Dictionary<(ElementType, ElementType), ElementType> combinedElements = new Dictionary<(ElementType, ElementType), ElementType> {
		{(ElementType.Air, ElementType.Water), ElementType.Ice},
		{(ElementType.Water, ElementType.Air), ElementType.Ice},
		
		{(ElementType.Air, ElementType.Fire), ElementType.Lightning},
		{(ElementType.Fire, ElementType.Air), ElementType.Lightning},

		{(ElementType.Earth, ElementType.Fire), ElementType.Magma},
		{(ElementType.Fire, ElementType.Earth), ElementType.Magma},

		{(ElementType.Earth, ElementType.Water), ElementType.Mud},
		{(ElementType.Water, ElementType.Earth), ElementType.Mud}
	};

	#region Methods

	public static (ElementType, ElementType)? GetOppositeElements(ElementType element) {
		switch (element) {
			case ElementType.Air:
				return (ElementType.Earth, ElementType.Water);
			case ElementType.Earth:
				return (ElementType.Air, ElementType.Fire);
			case ElementType.Fire:
				return (ElementType.Water, ElementType.Earth);
			case ElementType.Water:
				return (ElementType.Fire, ElementType.Air);
		}

		return null;
	}

	#endregion

	#region Enums

	public enum ElementType
	{
		// Primary elements
		Air,
		Earth,
		Fire,
		Water,

		// Secondary elements
		Ice,
		Lightning,
		Magma,
		Mud
	}

	#endregion
}