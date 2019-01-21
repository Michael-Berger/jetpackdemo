using UnityEngine;

public class Goal : MonoBehaviour {

    public Scoreboard scoreBoard;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "Ball" && BoundsIsEncapsulated(transform.GetComponent<Collider>().bounds, other.bounds))
        {
            scoreBoard.AddScore();
            ResetBall(other.transform);
        }
    }

    private bool BoundsIsEncapsulated(Bounds Encapsulator, Bounds Encapsulating)
    {
        return Encapsulator.Contains(Encapsulating.min) && Encapsulator.Contains(Encapsulating.max);
    }

    private void ResetBall(Transform ball)
    {
        ball.position = Vector3.zero;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
