using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    [Header("Frame Settings")]
    int MaxRate = 999;
    public float targetFrameRate = 60.0f;

    float currentFrameTime;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine(nameof(WaitForNextFrame));
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / targetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
            {
                Thread.Sleep((int)(sleepTime * 1000));
                while (t < currentFrameTime)
                {
                    t = Time.realtimeSinceStartup;
                }
            }
        }
    }
}
