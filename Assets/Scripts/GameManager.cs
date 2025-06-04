using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HitArea hitAreaDetector;
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Visual Video")]
    [SerializeField] private RawImage videoDisplayImage;

    [Header("Clips por evento")]
    [SerializeField] private VideoClip clipBola;
    [SerializeField] private VideoClip clipHit;
    [SerializeField] private VideoClip clipHomeRun;
    [SerializeField] private VideoClip clipStrike;
    [SerializeField] private VideoClip loop;

    [Header("Textos individuales")]
    [SerializeField] private TextMeshProUGUI strikeText;
    [SerializeField] private TextMeshProUGUI ballText;

    [Header("Indicador visual")]
    [SerializeField] private RawImage indicatorImage;

    [Header("Imágenes por evento")]
    [SerializeField] private Texture2D imgHit;
    [SerializeField] private Texture2D imgHomeRun;
    [SerializeField] private Texture2D imgBall1;
    [SerializeField] private Texture2D imgBall2;
    [SerializeField] private Texture2D imgBall3;
    [SerializeField] private Texture2D imgBall4;
    [SerializeField] private Texture2D imgStrike1;
    [SerializeField] private Texture2D imgStrike2;
    [SerializeField] private Texture2D imgStrike3;
    [SerializeField] private Texture2D imgBasePorBola;
    [SerializeField] private Texture2D imgGanaste;
    [SerializeField] private Texture2D imgReady;

    [Header("Duración del fade")]
    [SerializeField] private float fadeDuration = 1.0f;

    private int strikes = 0;
    private int balls = 0;
    private bool hasPlayedEventVideo = false;
    private bool isPlayable = true;

    void Start()
    {
        hitAreaDetector.OnBallHit += HandleBallHit;
        videoPlayer.loopPointReached += OnVideoFinished;

        indicatorImage.canvasRenderer.SetAlpha(0f);
        MostrarImagenConFade(imgReady);
        UpdateAllTexts();

        if (videoDisplayImage != null && videoPlayer.targetTexture != null)
            videoDisplayImage.texture = videoPlayer.targetTexture;

        // Forzar estirado total (sin AspectRatioFitter)
        RectTransform rt = videoDisplayImage.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        PlayLoop();
    }

    void Update()
    {
        ForzarEscaladoCompleto();
    }

    private void HandleBallHit(BallEvent ballEvent)
    {
        if (!hasPlayedEventVideo)
            indicatorImage.gameObject.SetActive(false);

        hasPlayedEventVideo = true;

        if (isPlayable)
        {
            isPlayable = false;

            switch (ballEvent)
            {
                case BallEvent.Ball:
                    PlayClip(clipBola);
                    balls++;
                    UpdateBallText();
                    break;

                case BallEvent.Hit:
                    PlayClip(clipHit);
                    MostrarImagenConFade(imgHit);
                    break;

                case BallEvent.HomeRun:
                    PlayClip(clipHomeRun);
                    MostrarImagenConFade(imgHomeRun);
                    break;

                case BallEvent.Strike:
                    PlayClip(clipStrike);
                    strikes++;
                    UpdateStrikeText();
                    break;
            }
        }
    }

    private void PlayClip(VideoClip clip)
    {
        if (clip == null) return;

        videoPlayer.Stop();
        videoPlayer.clip = clip;
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    private void PlayLoop()
    {
        isPlayable = true;
        videoPlayer.Stop();
        videoPlayer.clip = loop;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (videoPlayer.clip == clipHit || videoPlayer.clip == clipHomeRun || videoPlayer.clip == clipStrike || videoPlayer.clip == clipBola)
        {
            indicatorImage.texture = null;
            indicatorImage.gameObject.SetActive(false);

            if (strikes == 3)
            {
                MostrarImagenConFade(imgGanaste);
                StartCoroutine(RestartAfterDelay());
            }
            else if (balls == 4 || videoPlayer.clip == clipHit || videoPlayer.clip == clipHomeRun)
            {
                MostrarImagenConFade(imgBasePorBola);
                StartCoroutine(RestartAfterDelay());
            }
            else
            {
                PlayLoop();
            }
        }
    }

    private void UpdateAllTexts()
    {
        strikeText.text = strikes.ToString();
        ballText.text = balls.ToString();
    }

    private void UpdateStrikeText()
    {
        if (strikeText != null)
            strikeText.text = strikes.ToString();

        switch (strikes)
        {
            case 1:
                MostrarImagenConFade(imgStrike1);
                break;
            case 2:
                MostrarImagenConFade(imgStrike2);
                break;
            case 3:
                MostrarImagenConFade(imgStrike3);
                break;
        }
    }

    private void UpdateBallText()
    {
        if (ballText != null)
            ballText.text = balls.ToString();

        switch (balls)
        {
            case 1:
                MostrarImagenConFade(imgBall1);
                break;
            case 2:
                MostrarImagenConFade(imgBall2);
                break;
            case 3:
                MostrarImagenConFade(imgBall3);
                break;
            case 4:
                MostrarImagenConFade(imgBall4);
                break;
        }
    }

    private void MostrarImagenConFade(Texture2D imagen)
    {
        if (indicatorImage == null || imagen == null) return;

        indicatorImage.texture = imagen;
        indicatorImage.gameObject.SetActive(true);
        indicatorImage.CrossFadeAlpha(0f, 0f, true);
        indicatorImage.CrossFadeAlpha(1f, fadeDuration, false);
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(10f);
        Restart();
    }

    private void Restart()
    {
        strikes = 0;
        balls = 0;
        hasPlayedEventVideo = false;
        UpdateAllTexts();

        indicatorImage.texture = null;
        indicatorImage.gameObject.SetActive(false);
        indicatorImage.canvasRenderer.SetAlpha(0f);
        MostrarImagenConFade(imgReady);

        PlayLoop();
    }

    private void ForzarEscaladoCompleto()
    {
        if (videoDisplayImage != null)
        {
            RectTransform rt = videoDisplayImage.rectTransform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }
}