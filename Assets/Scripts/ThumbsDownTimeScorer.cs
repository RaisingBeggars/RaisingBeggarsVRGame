using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class ThumbsDownTimeScorer : MonoBehaviour
{
    [Header("Thumbs Down Gestures")]
    public StaticHandGesture leftThumbDownGesture;
    public StaticHandGesture rightThumbDownGesture;

    [Header("Coin Settings")]
    public long coinPenalty = 5000;   // 양손 thumbs down 시 5000 코인 차감

    private bool leftIsDown = false;
    private bool rightIsDown = false;
    private bool hasDeductedThisCycle = false;

    // 왼손 Thumbs Down 시작
    public void OnLeftDownStarted()
    {
        leftIsDown = true;
        TryDeduct();
    }

    // 왼손 Thumbs Down 종료
    public void OnLeftDownEnded()
    {
        leftIsDown = false;
        ResetCycle();
    }

    // 오른손 Thumbs Down 시작
    public void OnRightDownStarted()
    {
        rightIsDown = true;
        TryDeduct();
    }

    // 오른손 Thumbs Down 종료
    public void OnRightDownEnded()
    {
        rightIsDown = false;
        ResetCycle();
    }

    // 양손이 Down 되었을 때만 1번 차감
    private void TryDeduct()
    {
        if (leftIsDown && rightIsDown && !hasDeductedThisCycle)
        {
            CoinManager.Instance.TrySpend(coinPenalty);
            hasDeductedThisCycle = true;
        }
    }

    // 포즈 종료 시 다음 사이클 준비
    private void ResetCycle()
    {
        if (!leftIsDown && !rightIsDown)
            hasDeductedThisCycle = false;
    }
}
