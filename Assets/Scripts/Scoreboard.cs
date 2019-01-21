using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

    private int score = 0;
    public Text scoreValue;

	void Start () {
        score = 0;
	}

    public void AddScore()
    {
        score += 1;
        scoreValue.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
        scoreValue.text = score.ToString();
    }
}
