using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;


public class AdMob : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //Request Ads
        
        string appId;

        if (Application.platform == RuntimePlatform.Android){
            appId = "ca-app-pub-7881458813262812~5402855405";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer){
            appId = "ca-app-pub-7881458813262812~8124333617";
        }
        else {
            appId = "unexpected_platform";
        }


        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        this.RequestBanner();


    }

    private void RequestBanner()
    {
        string adUnitId;

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            adUnitId = "unused";
        }

        else if (Application.platform == RuntimePlatform.Android)
        {

            //adUnitId = "ca-app-pub-3940256099942544/6300978111";  test
            adUnitId = "ca-app-pub-7881458813262812/6863222834";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

            //adUnitId = "ca-app-pub-3940256099942544/2934735716";  test
            adUnitId = "ca-app-pub-7881458813262812/4125432754";
        }
        else
        {
            adUnitId = "unexpected_platform";
        }

    // Create a 320x50 banner at the bottom of the screen.
    BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder().Build();
    // Load the banner with the request.
    bannerView.LoadAd(request);
    }

}