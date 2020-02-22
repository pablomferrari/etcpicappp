﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EtcPicApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MaterialListView : ContentPage
    {
        public MaterialListView()
        {
            InitializeComponent();
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                DoneButton.HeightRequest = 75;
            }
        }
    }
}
