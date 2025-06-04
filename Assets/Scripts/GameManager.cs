using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HitArea hitAreaDetector;

    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Clips por evento")]
    [SerializeField] private VideoClip clipBola;
    [SerializeField] private VideoClip clipHit;
    [SerializeField] private VideoClip clipHomeRun;
    [SerializeField] private VideoClip clipStrike;
    [SerializeField] private VideoClip loop;

    [Header("Textos individuales")]
    [SerializeField] private TextMeshProUGUI strikeText;
    [SerializeField] private TextMeshProUGUI ballText;

    [SerializeField] private TextMeshProUGUI indicatorText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private int strikes = 0;
    private int balls = 0;

    private bool hasPlayedEventVideo = false;

    void Start()
    {
        hitAreaDetector.OnBallHit += HandleBallHit;
        videoPlayer.loopPointReached += OnVideoFinished;

        indicatorText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        UpdateAllTexts();

        PlayLoop(); // Inicia el video loop
    }

    private void HandleBallHit(BallEvent ballEvent)
    {
        hasPlayedEventVideo = true;

        switch (ballEvent)
        {
            case BallEvent.Ball:
                PlayClip(clipBola);
                balls++;
                UpdateBallText();
                break;

            case BallEvent.Hit:
                PlayClip(clipHit);
                indicatorText.text = "Hit!!!";
                GameOver();
                break;

            case BallEvent.HomeRun:
                PlayClip(clipHomeRun);
                indicatorText.text = "Home Run!!!";
                GameOver();
                break;

            case BallEvent.Strike:
                PlayClip(clipStrike);
                strikes++;
                UpdateStrikeText();
                break;
        }
    }

    private void PlayClip(VideoClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("No hay clip asignado.");
            return;
        }

        videoPlayer.Stop();
        videoPlayer.isLooping = false;
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }

    private void PlayLoop()
    {
        videoPlayer.Stop();
        videoPlayer.clip = loop;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // Ya no reproducimos el loop automáticamente después de cualquier video
        // Solo si NO ha sido reemplazado aún por otro video
        // (por seguridad: previene que el loop se reproduzca por error)
        // Ya no hacemos nada aquí
    }

    private void UpdateAllTexts()
    {
        UpdateStrikeText();
        UpdateBallText();
    }

    private void UpdateStrikeText()
    {
        if (strikeText != null)
            strikeText.text = $"Strikes: {strikes}";

        if (strikes == 3)
        {
            indicatorText.text = "Ganaste!!!";
            GameOver();
        }
    }

    private void UpdateBallText()
    {
        if (ballText != null)
            ballText.text = $"Balls: {balls}";

        if (balls == 4)
        {
            indicatorText.text = "Base por Bola!!!";
            GameOver();
        }
    }

    private void GameOver()
    {
        indicatorText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(RestartAfterDelay());
    }

    private void Restart()
    {
        strikes = 0;
        balls = 0;
        hasPlayedEventVideo = false;
        UpdateAllTexts();
        indicatorText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        PlayLoop(); // Solo aquí vuelve a reproducirse el loop
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(10f);
        Restart();
    }
}