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

    [Header("Textos individuales")]
    [SerializeField] private TextMeshProUGUI strikeText;
    [SerializeField] private TextMeshProUGUI ballText;
    [SerializeField] private TextMeshProUGUI hitText;
    [SerializeField] private TextMeshProUGUI homeRunText;

    private int strikes = 0;
    private int balls = 0;
    private int hits = 0;
    private int homeRuns = 0;

    void Start()
    {
        hitAreaDetector.OnBallHit += HandleBallHit;
        UpdateAllTexts();
    }

    private void HandleBallHit(BallEvent ballEvent)
    {
        switch (ballEvent)
        {
            case BallEvent.Ball:
                PlayClip(clipBola);
                balls++;
                UpdateBallText();
                break;

            case BallEvent.Hit:
                PlayClip(clipHit);
                hits++;
                UpdateHitText();
                break;

            case BallEvent.HomeRun:
                PlayClip(clipHomeRun);
                homeRuns++;
                UpdateHomeRunText();
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
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }

    private void UpdateAllTexts()
    {
        UpdateStrikeText();
        UpdateBallText();
        UpdateHitText();
        UpdateHomeRunText();
    }

    private void UpdateStrikeText()
    {
        if (strikeText != null)
            strikeText.text = $"Strikes: {strikes}";
    }

    private void UpdateBallText()
    {
        if (ballText != null)
            ballText.text = $"Balls: {balls}";
    }

    private void UpdateHitText()
    {
        if (hitText != null)
            hitText.text = $"Hits: {hits}";
    }

    private void UpdateHomeRunText()
    {
        if (homeRunText != null)
             homeRunText.text = $"Home Runs: {homeRuns}";
    }
}