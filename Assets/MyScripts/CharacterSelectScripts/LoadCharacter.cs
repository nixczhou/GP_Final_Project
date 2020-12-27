using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
	public GameObject[] player1_characterPrefabs;
	public GameObject[] player2_characterPrefabs;

	void Start()
	{
		//for initialize player 1 gameobject after character select
		int player1_selectedCharacter = PlayerPrefs.GetInt("player1_selectedCharacter");
		GameObject player1_spawn = player1_characterPrefabs[player1_selectedCharacter];
		player1_spawn.SetActive(true);
		GameScript.player1 = player1_spawn;


		//for initialize player 2 gameobject after character select
		int player2_selectedCharacter = PlayerPrefs.GetInt("player2_selectedCharacter");
		GameObject player2_spawn = player2_characterPrefabs[player2_selectedCharacter];
		player2_spawn.SetActive(true);
		GameScript.player2 = player2_spawn;
		
	}
}
