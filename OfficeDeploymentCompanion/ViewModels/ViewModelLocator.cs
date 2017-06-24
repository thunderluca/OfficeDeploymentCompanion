using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OfficeDeploymentCompanion.WorkerServices;

namespace OfficeDeploymentCompanion.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModelWorkerServices>();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}