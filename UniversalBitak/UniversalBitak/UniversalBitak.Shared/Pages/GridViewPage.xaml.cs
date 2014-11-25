using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using UniversalBitak.Common;
using UniversalBitak.Models;
using UniversalBitak.ViewModels;

using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight;
using Parse;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace UniversalBitak.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GridViewPage : Page
    {
        private NavigationHelper navigationHelper;

        private ObservableCollection<ItemViewModel> items;

        private bool initializing;

        public GridViewPage()
            : this(new GridViewPageViewModel())
        {
        }

        public GridViewPage(GridViewPageViewModel viewModel)
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.DataContext = viewModel;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }        

        public IEnumerable<ItemViewModel> Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new ObservableCollection<ItemViewModel>();
                }
                return this.items;
            }
            set
            {
                if (this.items == null)
                {
                    this.items = new ObservableCollection<ItemViewModel>();
                }
                this.items.Clear();
                foreach (var item in value)
                {
                    this.items.Add(item);
                }
            }
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            if (ParseUser.CurrentUser != null)
            {
                this.Frame.Navigate(typeof(Pages.NewItemPage));
            }
            else
            {
                this.Frame.Navigate(typeof(Pages.LoginPage));
            }
        }

        private void ShowMessageBox(string message, string title)
        {
            MessageDialog msgDialog = new MessageDialog(message, title);

            UICommand okBtn = new UICommand("OK");
            msgDialog.Commands.Add(okBtn);

            //Cancel Button
            //UICommand cancelBtn = new UICommand("Cancel");           
            //msgDialog.Commands.Add(cancelBtn);

            msgDialog.ShowAsync();
        }

        private void OnLoginOut(object sender, RoutedEventArgs e)
        {
            ParseUser.LogOut();
        }

        private void OnItemListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemListView = (sender as ListView);
            var selectedObject = itemListView.SelectedItem;
            this.Frame.Navigate(typeof(DetailsPage), selectedObject);
        }

        private void GoToBasket(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserBasketPage));
        }

        private void ShomMyItemsForSale(object sender, RoutedEventArgs e)
        {
            this.LoadItems();            
        }

        private async Task LoadItems()
        {
            var items = await new ParseQuery<Item>()
            .Where(n => n.user == ParseUser.CurrentUser.Username)
            .FindAsync();

            this.Items = items.AsQueryable()
                    .Select(ItemViewModel.FromModel);
        }
    }
}