using GalaSoft.MvvmLight;
using OfficeDeploymentCompanion.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDeploymentCompanion.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly MainViewModelWorkerServices WorkerServices;

        public MainViewModel(MainViewModelWorkerServices workerServices)
        {
            if (workerServices == null)
                throw new ArgumentNullException(nameof(workerServices));

            this.WorkerServices = workerServices;
        }

        public string Title
        {
            get { return "Office Deployment Companion"; }
        }
    }
}