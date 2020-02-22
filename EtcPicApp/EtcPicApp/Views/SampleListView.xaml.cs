﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EtcPicApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SampleListView : ContentPage
    {
        public SampleListView()
        {
            InitializeComponent();
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                DoneButton.HeightRequest = 75;
            }
            else
            {

            }
        }
    }
}
