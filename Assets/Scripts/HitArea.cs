using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    public System.Action<BallEvent> OnBallHit;
    public void Ball()
    {
        OnBallHit?.Invoke(BallEvent.Ball);
    }

    public void Hit()
    {
        OnBallHit?.Invoke(BallEvent.Hit);

    }

    public void HomeRun()
    {
        OnBallHit?.Invoke(BallEvent.HomeRun);
    }

    public void Strike()
    {
        OnBallHit?.Invoke(BallEvent.Strike);
    }
}
