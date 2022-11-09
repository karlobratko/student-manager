using System;

using StudentManager.UI.Stores;
using StudentManager.UI.ViewModels;

namespace StudentManager.UI.Services;

public class ParameterNavigationService<TParameter, TViewModel> : IParameterNavigationService<TParameter> 
  where TParameter : class
  where TViewModel : ViewModelBase {
  private readonly IViewNavigationStore _navigationStore;
  private readonly Func<TParameter?, TViewModel> _createViewModel;

  public ParameterNavigationService(IViewNavigationStore navigationStore, Func<TParameter?, TViewModel> createViewModel) {
    _navigationStore = navigationStore;
    _createViewModel = createViewModel;
  }

  public void Navigate(TParameter? parameter) =>
    _navigationStore.CurrentViewModel = _createViewModel(parameter);
}
