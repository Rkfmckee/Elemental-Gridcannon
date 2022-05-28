using System.Collections.Generic;
using UnityEngine;

public static class References {
	public static Camera camera;
	public static CameraController cameraController;

	public static class Cards {
		public static Card currentCard;
		public static List<CardSlot> cardSlots = new List<CardSlot>();
	}
}
