using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using App1.Models;
using App1.Views;
using App1.ViewModels;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
            Send.Clicked += Send_Clicked;
            //CameraButton.Clicked += CameraButton_Clicked;
        }
        private async void Send_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                Grievence g = new Grievence();
                g.Name = Name.Text;
                g.mobile = long.Parse(MobileNo.Text);
                g.CustomGrievence = Comment.Text;
                g.imagedata = null;
                await SaveTodoItemAsync(g, true);


            }
            else
            {
               await DisplayAlert("Error", "Not connected to internet","Ok");
            }
           
         
        }

        //private async void CameraButton_Clicked(object sender, EventArgs e)
        //{
        //    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

        //    if (photo != null)
        //        PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
        //}

        public async Task SaveTodoItemAsync(Grievence item, bool isNewItem = false)
        {
            // RestUrl = http://developer.xamarin.com:8081/api/todoitems
            //var uri = new Uri(string.Format("http://localhost:8080/grievence?Name={0}&Grievence={1}&mobilenumber={2}&imagedata={3}", item.Name,item.CustomGrievence,item.mobile,item.imagedata));
            var uri = new Uri(string.Format("http://localhost:8080/grievence",string.Empty));
            try
            {
                HttpClient client = new HttpClient();
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    response = await client.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"				TodoItem successfully saved.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            //ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}