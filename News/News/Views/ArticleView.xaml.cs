using System.Web;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using News.Models;
using News.Services;


namespace News.Views
{
    public partial class ArticleView : ContentPage
    {
        //Here is where you show the news in Full page

        public ArticleView()
        {
            InitializeComponent();



        }


        public ArticleView(string Url)
        {
            InitializeComponent();
            BindingContext = new UrlWebViewSource
            {
                Url = HttpUtility.UrlDecode(Url)
            };


        }





    }

}

