using System.Collections.Generic;
using UnityEngine;

public static class References {
	public static Camera camera;
	public static CameraController cameraController;
	public static GameController gameController;

	public static class Cards {
		public static Card currentCard;
		public static CardDeck cardDeck;

		public static class Slots {
			public static List<CardSlot> active = new List<CardSlot>();
			public static List<NumberCardSlot> number = new List<NumberCardSlot>();
			public static List<EnemyCardSlot> enemy = new List<EnemyCardSlot>();
			public static TempEnemyCardSlot tempEnemy;
			public static CardSlot special;
		}
	}
}
