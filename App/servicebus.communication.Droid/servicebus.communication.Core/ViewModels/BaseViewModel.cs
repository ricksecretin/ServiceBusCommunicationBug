using System;
using MvvmCross.ViewModels;

namespace servicebus.communication.Core
{
    public class BaseViewModel : MvxViewModel
    {
        protected IModelFacade ModelFacade { get; }

        public BaseViewModel(IModelFacade modelFacade)
        {
            ModelFacade = modelFacade;
        }
    }
}
