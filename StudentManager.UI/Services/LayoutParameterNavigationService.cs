using System;

using StudentManager.UI.Stores;
using StudentManager.UI.ViewModels;

namespace StudentManager.UI.Services;

public class LayoutParameterNavigationService<TParameter, TViewModel> : IParameterNavigationService<TParameter>
  where TParameter : class
  where TViewModel : ViewModelBase {
  private readonly IViewNavigationStore _navigationStore;
  private readonly Func<TParameter?, TViewModel> _createViewModel;
  private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;

  public LayoutParameterNavigationService(IViewNavigationStore navigationStore,
                                          Func<TParameter?, TViewModel> createViewModel,
                                          Func<NavigationBarViewModel> createNavigationBarViewModel) {
    _navigationStore = navigationStore;
    _createViewModel = createViewModel;
    _createNavigationBarViewModel = createNavigationBarViewModel;
  }

  public void Navigate(TParameter? param) =>
    _navigationStore.CurrentViewModel = new LayoutViewModel(_createNavigationBarViewModel.Invoke(),
                                                            _createViewModel.Invoke(param));
}
