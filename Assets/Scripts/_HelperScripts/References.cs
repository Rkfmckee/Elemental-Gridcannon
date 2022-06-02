using System.Collections.Generic;
using UnityEngine;

public static class References {
	public static Camera camera;
	public static CameraController cameraController;
	public static PlayerController playerController;

	public static class Cards {
		public static Card currentCard;

		public static class Slots {
			public static List<CardSlot> active = new List<CardSlot>();
			public static List<CardSlot> number = new List<CardSlot>();
			public static List<CardSlot> enemy = new List<CardSlot>();
			public static List<CardSlot> special = new List<CardSlot>();
		}
	}
}
