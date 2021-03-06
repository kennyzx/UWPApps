﻿using Microsoft.Advertising.WinRT.UI;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdvertisementPage : Page
    {
        public AdvertisementPage()
        {
            this.InitializeComponent();
            this.Loaded += (s, e)=> { AddInterstitialAd(); };

        }

        private void AddInterstitialAd()
        {
            interstitialAd = new InterstitialAd();
            interstitialAd.ErrorOccurred += OnAdError;
            interstitialAd.AdReady += OnAdReady;
            interstitialAd.Cancelled += OnAdCancelled;
            interstitialAd.Completed += OnAdCompleted;

            interstitialAd.RequestAd(AdType.Video, "d25517cb-12d4-4699-8bdc-52040c712cab", "test");
        }

        private void OnAdReady(object sender, object e)
        {
            // The ad is ready to show; show it.
            interstitialAd.Show();
        }

        // This is an event handler for the interstitial ad. It is triggered when the interstitial ad is cancelled.
        private void OnAdCancelled(object sender, object e)
        {
            
        }

        // This is an event handler for the interstitial ad. It is triggered when the interstitial ad has completed playback.
        private void OnAdCompleted(object sender, object e)
        {
            
        }

        // This is an error handler for the interstitial ad.
        private void OnAdError(object sender, AdErrorEventArgs e)
        {
            
        }

        InterstitialAd interstitialAd;
    }
}
