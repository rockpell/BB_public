using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour
{
    private const string androidGameId = "2676632"; // 플레이 스토어 번호
    private const string rewardedVideoId = "video"; // 광고 번호

    private void Start()
    {
        Advertisement.Initialize(androidGameId);
    }

    public void ShowAds()
    {
        Invoke("ShowRewardedAd", 0.5f);
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady(rewardedVideoId))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(rewardedVideoId, options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}