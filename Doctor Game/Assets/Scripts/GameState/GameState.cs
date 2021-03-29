using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    enum State
    {
        playing,
        normalEnding,
        goodEnding,
        badEnding

    }

    private State currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.playing;
    }

    // Update is called once per frame
    void Update()
    {
        if (Stats.Stress > 10)
        {
            Stats.Stress = 0;
            currentState = State.badEnding;
        }

        if (Stats.Day > 7 && Stats.Stress <= 3)
        {
            currentState = State.goodEnding;
        }

        if (Stats.Day > 7 && Stats.Stress <= 3)
        {
            currentState = State.badEnding;
        }

        switch (currentState)
        {
            case State.badEnding:
                SceneManager.LoadScene("BadEnding");
                break;
            case State.normalEnding:
                SceneManager.LoadScene("NormalEnding");
                break;
            case State.goodEnding:
                SceneManager.LoadScene("GoodEnding");
                break;
        }
    }
}
