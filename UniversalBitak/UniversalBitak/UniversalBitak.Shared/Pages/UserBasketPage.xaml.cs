using UniversalBitak.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using UniversalBitak.Models;
using System.Threading.Tasks;
using Windows.Storage;
using UniversalBitak.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace UniversalBitak.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserBasketPage : Page
    {
        private const string dbName = "ItemsDatabase.db";

        public List<ItemForSql> items { get; set; }

        private NavigationHelper navigationHelper;

        public UserBasketPage()
            :this(new BasketPageViewModel())
        {
        }

        public UserBasketPage(BasketPageViewModel viewModel)
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.ViewModel = viewModel;
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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ViewModel.Item = e.Parameter as ItemViewModel;
            this.navigationHelper.OnNavigatedTo(e);
            // Create Db if not exist
            
            bool dbExists = await CheckDbAsync(dbName);
            if (dbExists)
            {
                if (this.ViewModel.Item != null)
                {
                    await AddItemsAsync();
                }                
            }
            else
            {
                await CreateDatabaseAsync();
                await AddItemsAsync();
            }


            // Get Items
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            var query = conn.Table<ItemForSql>();
            items = await query.ToListAsync();

            // Show users
            this.ItemBasketList.ItemsSource = items;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        public BasketPageViewModel ViewModel
        {
            get
            {
                return (BasketPageViewModel)this.DataContext;
            }
            set
            {
                this.DataContext = value;
            }
        }

        #endregion

        #region SQLite utils
        private async Task<bool> CheckDbAsync(string dbName)
        {
            bool dbExist = true;

            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(dbName);
            }
            catch (Exception)
            {
                dbExist = false;
            }

            return dbExist;
        }

        private async Task CreateDatabaseAsync()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            await conn.CreateTableAsync<ItemForSql>();
        }

        private async Task AddItemsAsync()
        {
            // Create a Items list
            var list = new List<ItemForSql>()
            {
                new ItemForSql()
                {
                    Name = this.ViewModel.Item.itemName,
                    Description = this.ViewModel.Item.itemDescription,
                    Category = this.ViewModel.Item.itemCategory,
                    Price = this.ViewModel.Item.itemPrice,
                    Picture = this.ViewModel.Item.itemPicture.Url.ToString()
                },
                
            };

            // Add rows to the Item Table
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            await conn.InsertAllAsync(list);
        }        

        private async Task SearchItemByTitleAsync(string title)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);

            AsyncTableQuery<ItemForSql> query = conn.Table<ItemForSql>().Where(x => x.Name.Contains(title));
            List<ItemForSql> result = await query.ToListAsync();
            foreach (var item in result)
            {
                // ...
            }

            var allArticles = await conn.QueryAsync<ItemForSql>("SELECT * FROM Articles");
            foreach (var article in allArticles)
            {
                // ...
            }

            var otherArticles = await conn.QueryAsync<ItemForSql>(
                "SELECT Content FROM Articles WHERE Title = ?", new object[] { "Hackers, Creed" });
            foreach (var article in otherArticles)
            {
                // ...
            }
        }

        private async Task UpdateItemNameAsync(string oldTitle, string newTitle)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);

            var item = await conn.Table<ItemForSql>()
                .Where(x => x.Name == oldTitle).FirstOrDefaultAsync();
            if (item != null)
            {
                item.Name = newTitle;

                // Update record
                await conn.UpdateAsync(item);
            }
        }

        private async Task DeleteItemsAsync(string name)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);

            var article = await conn.Table<ItemForSql>().Where(x => x.Name == name).FirstOrDefaultAsync();
            if (article != null)
            {
                // Delete record
                await conn.DeleteAsync(article);
            }
        }

        private async Task DropTableAsync(string name)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            await conn.DropTableAsync<ItemForSql>();
        }

        private void RefreshListView()
        {
            this.ItemBasketList.ItemsSource = items;
            this.Frame.Navigate(typeof(UserBasketPage));
        }

        #endregion SQLite utils

        private async void OnHoldingListItem(object sender, HoldingRoutedEventArgs e)
        {
            var itemListView = (sender as ListView);
            var selectedObject = (itemListView.SelectedValue as ItemForSql).Name;
            await DeleteItemsAsync(selectedObject);
            RefreshListView();
        }

        private async void OnSelectListItem(object sender, SelectionChangedEventArgs e)
        {
            var itemListView = (sender as ListView);
            var selectedObject = (itemListView.SelectedValue as ItemForSql).Name;
            await DeleteItemsAsync(selectedObject);
            RefreshListView();
        }
    }
}
