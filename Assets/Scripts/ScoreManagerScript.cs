using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerScript : MonoBehaviour
{
    public Text ScoreText;
    public Text HighScoreText;
    public GameObject scoreTxtObj;
    public GameObject panelTxtObj;
    public GameObject highscoreTxtObj;
    public AudioClip highScoreFx;
    
    private int score;
    private bool highScorePlayed;

    private int lastCheckBullet;
    private int lastCheckBilocazione;

    public static ScoreManagerScript current; //per interfecciare con altri script, in moda da richiamarla

    private void Awake() //la void viene letta quando il gioco parte
    {
        current = this; //è un istanza di questo script, serve per interfacciare altri script con questo
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        ScoreText.text = score.ToString();
        highScorePlayed = false;

        lastCheckBullet = PoteriManager.current.passiBullet;
        lastCheckBilocazione = PoteriManager.current.passiBilocazione;
    }

    public void StartScore()
    {
        InvokeRepeating("IncrementeScore", 0.1f, 0.5f);
        scoreTxtObj.SetActive(true);
    }

    private void IncrementeScore()
    {
        score += 1;
        ScoreText.text = score.ToString();

        NewHighScore();
    }

    public void DiamondScore()
    {
        int rand = Random.Range(0, 6);

        score += rand;
        ScoreText.text = score.ToString();

        NewHighScore();
    }

    private void NewHighScore()
    {
        //Se ha superato l'high Score
        if (PlayerPrefs.HasKey("highScore") && !highScorePlayed)
        {
            if (score > PlayerPrefs.GetInt("highScore"))
            {
                highscoreTxtObj.SetActive(true);
                highScorePlayed = true;
                AudioManageScript.current.PlaySound(highScoreFx);
            }
        }

        //Conta lo score per i poteri
        if (score >= lastCheckBullet)
        {
            PoteriManager.current.canBullet = true;
            lastCheckBullet += PoteriManager.current.passiBullet;
        }

        if (score >= lastCheckBilocazione)
        {
            PoteriManager.current.canBilocazione = true;
            lastCheckBilocazione += PoteriManager.current.passiBilocazione;
        }
    }

    public void StopScore()
    {
        CancelInvoke("IncrementeScore");

        //Registro il Risultato di Score
        PlayerPrefs.SetInt("Score",score);

        //registro l'high score
        if (PlayerPrefs.HasKey("highScore"))
        {
            if (score > PlayerPrefs.GetInt("highScore"))
                PlayerPrefs.SetInt("highScore", score);
                  
        }
        else
        {
            PlayerPrefs.SetInt("highScore", score);
        }

        //assegno il valore di HighScore
        HighScoreText.text = PlayerPrefs.GetInt("highScore").ToString();

        //faccio apparire il GameOverPannel
        panelTxtObj.SetActive(true);

    }
}
