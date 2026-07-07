using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public ScoreText scoreTextPlayer1, scoreTextPlayer2;
    public GameObject menuObject;
    public System.Action onStartGame;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI volumeValueText;
    public TextMeshProUGUI playModeButtonText;

    private void Start()
    {
        AdjustPlayModeButtonText();
    }
    
    public void UpdateScores(int scorePlayer1, int scorePlayer2)
    {
        scoreTextPlayer1.SetScore(scorePlayer1);
        scoreTextPlayer2.SetScore(scorePlayer2);
    }

    public void HighlightScore(int id)
    {
        if(id == 1){
            scoreTextPlayer1.Trigger();
        }
        else
        {
            scoreTextPlayer2.Trigger();
        }
    }

    public void OnStartButtonClicked()
    {
        menuObject.SetActive(false);
        onStartGame?.Invoke();
    }

    public void OnGameEnds(int winnerID)
    {
        menuObject.SetActive(true);
        winText.text = $"Player {winnerID} wins";
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        volumeValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
    }

    public void OnSwitchPlayModeButtonClicked()
    {
        GameManager.instance.SwitchPlayMode();
        AdjustPlayModeButtonText();
    }

    private void AdjustPlayModeButtonText()
    {
        switch(GameManager.instance.playMode)
        {
            case GameManager.PlayMode.PlayervsPlayer:
                playModeButtonText.text = "2 Players";
                break;

            case GameManager.PlayMode.PlayervsAI:
                playModeButtonText.text = "Player vs AI";
                break;
        }
    }
}