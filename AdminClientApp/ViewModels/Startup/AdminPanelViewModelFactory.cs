using AdminClientApp.ViewModels.Essential;
using Common.Interfaces;
using System;

namespace AdminClientApp.ViewModels.Startup
{
    internal class AdminPanelViewModelFactory : IFactory<AdminPanelViewModel>
    {
        private IServiceProvider ServiceProvider { get; }

        public AdminPanelViewModelFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public AdminPanelViewModel Create()
        {
            return ServiceProvider.GetService(typeof(AdminPanelViewModel)) as AdminPanelViewModel;
        }
    }

}
