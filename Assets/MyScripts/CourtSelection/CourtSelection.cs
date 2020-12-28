using UnityEngine;
using UnityEngine.SceneManagement;

public class CourtSelection : MonoBehaviour
{
	public GameObject[] courts;
	public int selectedCourt = 0;

	public void Next()
	{
		courts[selectedCourt].SetActive(false);
		selectedCourt = (selectedCourt + 1) % courts.Length;
		courts[selectedCourt].SetActive(true);
	}

	public void Previous()
	{
		courts[selectedCourt].SetActive(false);
		selectedCourt--;
		if (selectedCourt < 0)
		{
			selectedCourt += courts.Length;
		}
		courts[selectedCourt].SetActive(true);
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCourt", selectedCourt);

		if(selectedCourt == 2){
			//Load black hole scene
			SceneManager.LoadScene(4, LoadSceneMode.Single);
		}
		else if (selectedCourt == 1){
			//Load sand scene
			SceneManager.LoadScene(3, LoadSceneMode.Single);
		}
		else {
			//Load main (green court)
			SceneManager.LoadScene(2, LoadSceneMode.Single);
		}
	}
}
