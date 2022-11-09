using System;

using StudentManager.UI.Stores;
using StudentManager.UI.ViewModels;

namespace StudentManager.UI.Services {
  public class LayoutNavigationService<TViewModel> : INavigationService
    where TViewModel : ViewModelBase {
    private readonly IViewNavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;
    private readonly Func<NavigationBarViewModel> _createNavigationBarViewModel;

    public LayoutNavigationService(IViewNavigationStore navigationStore,
                                   Func<TViewModel> createViewModel,
                                   Func<NavigationBarViewModel> createNavigationBarViewModel) {
      _navigationStore = navigationStore;
      _createViewModel = createViewModel;
      _createNavigationBarViewModel = createNavigationBarViewModel;
    }

    public void Navigate() =>
      _navigationStore.CurrentViewModel = new LayoutViewModel(_createNavigationBarViewModel.Invoke(),
                                                              _createViewModel.Invoke());
  }
}
