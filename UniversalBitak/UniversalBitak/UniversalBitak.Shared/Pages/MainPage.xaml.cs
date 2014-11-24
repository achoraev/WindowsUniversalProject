// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
namespace UniversalBitak.Pages
{
    using SQLite;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;
    using UniversalBitak.Common;
    using UniversalBitak.Models;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.Graphics.Display;
    using Windows.Storage;
    using Windows.UI.ViewManagement;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string dbName = "ItemsDatabase.db";

        public Point initialPoint;
        public List<ItemForSql> items { get; set; }

        private NavigationHelper navigationHelper;

        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public MainPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
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
            this.navigationHelper.OnNavigatedTo(e);
            // Create Db if not exist
            //bool dbExists = await CheckDbAsync(dbName);
            //if (!dbExists)
            //{
            //    await CreateDatabaseAsync();
            //    await AddItemsAsync();
            //}

            // Get Items
            //SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            //var query = conn.Table<ItemForSql>();
            //items = await query.ToListAsync();

            //// Show users
            //ArticleList.ItemsSource = items;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ToGridViewPage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Pages.GridViewPage));
        }

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
                    Name = "Hackers",
                    Description = "Security experts testing ways to break smartphone software have found several bugs in the NFC payment system found on many handsets.",
                    Category = "Other",
                    Price = 15.50
                }                
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

        private async Task UpdateArticleTitleAsync(string oldTitle, string newTitle)
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

        private async Task DeleteArticleAsync(string name)
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

        #endregion SQLite utils


        private void MainPageManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            initialPoint = e.Position;
        }

        private void MainPageManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Point currentpoint = e.Position;
            if (initialPoint.X - currentpoint.X > 0)
            {
                this.Frame.Navigate(typeof(Pages.GridViewPage));
            }
        }
    }
}