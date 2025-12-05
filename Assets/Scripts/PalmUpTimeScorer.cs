using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class PalmUpTimeScorer : MonoBehaviour
{
    [Header("Palm Up Gestures (Left / Right)")]
    public StaticHandGesture leftPalmUpGesture;
    public StaticHandGesture rightPalmUpGesture;

    [Header("Coin Settings")]
    public long coinPerSecond = 1000;     // 1초마다 1000 코인 지급

    private bool isLeftHeld = false;
    private bool isRightHeld = false;

    private float timer = 0f;

    void Update()
    {
        int activeHands = 0;

        if (isLeftHeld) activeHands++;
        if (isRightHeld) activeHands++;

        if (activeHands == 0)
        {
            // 포즈가 유지되는 손이 없다면 타이머 초기화 또는 유지
            return;
        }

        // Palm Up 유지 중일 때 시간 누적
        timer += Time.deltaTime;

        // 1초가 지났다면 코인 지급
        while (timer >= 1f)
        {
            CoinManager.Instance.AddCoin(activeHands * coinPerSecond);
            timer -= 1f;   // 1초 단위 차감 (여러 초 누적 처리 가능)
        }
    }

    // 왼손 Palm Up 시작
    public void OnLeftPalmUpStarted()
    {
        isLeftHeld = true;
    }

    // 왼손 Palm Up 종료
    public void OnLeftPalmUpEnded()
    {
        isLeftHeld = false;
    }

    // 오른손 Palm Up 시작
    public void OnRightPalmUpStarted()
    {
        isRightHeld = true;
    }

    // 오른손 Palm Up 종료
    public void OnRightPalmUpEnded()
    {
        isRightHeld = false;
    }
}
