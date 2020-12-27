using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
	public GameObject[] player1_characters;
	public GameObject[] player2_characters;
	public int player1_selectedCharacter = 0;
	public int player2_selectedCharacter = 0;

	public void Player1_NextCharacter()
	{
		player1_characters[player1_selectedCharacter].SetActive(false);
		player1_selectedCharacter = (player1_selectedCharacter + 1) % player1_characters.Length;
		player1_characters[player1_selectedCharacter].SetActive(true);
	}

	public void Player2_NextCharacter()
	{
		player2_characters[player2_selectedCharacter].SetActive(false);
		player2_selectedCharacter = (player2_selectedCharacter + 1) % player2_characters.Length;
		player2_characters[player2_selectedCharacter].SetActive(true);
	}

	public void Player1_PreviousCharacter()
	{
		player1_characters[player1_selectedCharacter].SetActive(false);
		player1_selectedCharacter--;
		if (player1_selectedCharacter < 0)
		{
			player1_selectedCharacter += player1_characters.Length;
		}
		player1_characters[player1_selectedCharacter].SetActive(true);
	}


	public void Player2_PreviousCharacter()
	{
		player2_characters[player2_selectedCharacter].SetActive(false);
		player2_selectedCharacter--;
		if (player2_selectedCharacter < 0)
		{
			player2_selectedCharacter += player2_characters.Length;
		}
		player2_characters[player2_selectedCharacter].SetActive(true);
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("player1_selectedCharacter", player1_selectedCharacter);
		PlayerPrefs.SetInt("player2_selectedCharacter", player2_selectedCharacter);
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}

	void Update()
	{
		//for player1 
		if(Input.GetKeyDown(KeyCode.D)){
			Player1_NextCharacter();
		}

		if(Input.GetKeyDown(KeyCode.A)){
			Player1_PreviousCharacter();
		}

		//for player2
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			Player2_NextCharacter();
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			Player2_PreviousCharacter();
		}
	}
}
