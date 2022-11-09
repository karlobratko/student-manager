using System;

using StudentManager.UI.Stores;
using StudentManager.UI.ViewModels;

namespace StudentManager.UI.Services {
  public class NavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase {
    private readonly IViewNavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;

    public NavigationService(IViewNavigationStore navigationStore, Func<TViewModel> createViewModel) {
      _navigationStore = navigationStore;
      _createViewModel = createViewModel;
    }

    public void Navigate() => _navigationStore.CurrentViewModel = _createViewModel.Invoke();
  }
}
