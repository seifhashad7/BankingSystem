using BankingSystem.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.EntityFramework;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Windows;

namespace BankingSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);

            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

            base.OnStartup(e);
        }
    }
}
