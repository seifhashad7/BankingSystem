using BankingSystem.Data;
using BankingSystem.Data.Services;
using BankingSystem.Model.Contracts;
using BankingSystem.Model.Logging;
using BankingSystem.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.EntityFramework;
using System.Configuration;
using System.Data;
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
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

            base.OnStartup(e);
        }
    }
}
