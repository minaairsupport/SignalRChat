using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace SignalRChat
{
    public partial class MainPage : ContentPage
    {
        HubConnection _hubConnection;
        string BaseUrl = "https://10.0.2.2:5001/";
        public MainPage()
        {
            InitializeComponent();
            

        }

        async Task StartConnection()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl($"{BaseUrl}chathub").Build();
            try
            {
                await _hubConnection.StartAsync();
                _hubConnection.On<string, string>("Receive", (user, msg) =>
               {
                   Label newLabel = new Label { Text = $"{user} send {msg}", HorizontalOptions = LayoutOptions.Start, FontSize= 20 };
                   messagesStack.Children.Add(newLabel);
               });


            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            StartConnection();
        }

        private async void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            if (_hubConnection == null)
            {
                await StartConnection();
            }

            await _hubConnection.SendAsync("Receive", "Minafaw", message.Text);
        }
    }
}
