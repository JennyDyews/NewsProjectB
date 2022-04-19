using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using Xamarin.Forms;

using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using News.Models;
using News.Services;

namespace News.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        NewsService service;
        NewsGroup newsgroup;
        bool isTaskRunning;

        public NewsPage()
        {
            InitializeComponent();
            UpdateUiState();
            service = new NewsService();
            newsgroup = new NewsGroup();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Code here will run right before the screen appears


            //This is making the first load of data

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {

                    await LoadNews();
                    categoryName.Text = $"News Headline for {Title} available";


                }

                catch (Exception)
                {
                    await DisplayAlert("Page crashed", "No internet connection", "try again");
                }




            });
        }



        private async Task LoadNews()
        {


            NewsCategory nCat = (NewsCategory)Enum.Parse(typeof(NewsCategory), Title);
            await Task.Run(() =>
            {

                Task<NewsGroup> t1 = service.GetNewsAsync(nCat);
                Device.BeginInvokeOnMainThread(() =>
                {
                    NewsListView.ItemsSource = t1.Result.Articles;
                });



            });
        }









        private async void refresh(object sender, EventArgs args)
        {
            await LoadNews();
        }

        private async void NewsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var newsPage = (NewsItem)e.Item;
            await Navigation.PushAsync(new ArticleView(newsPage.Url));

        }

        private void Connection_Clicked(object sender, EventArgs e)
        {

            isTaskRunning = !isTaskRunning;
            UpdateUiState();
        }

        void UpdateUiState()
        {
            runningStatusLabel.Text = isTaskRunning ? "Slow internet connection." : "Now you are connected!";
            SlowConnection.IsRunning = isTaskRunning;

        }
    }
}