using System;

using StudentManager.UI.Stores;
using StudentManager.UI.ViewModels;

namespace StudentManager.UI.Services;

public class ModalNavigationService<TViewModel> : INavigationService
  where TViewModel : ViewModelBase {
  private readonly IModalNavigationStore _modalNavigationStore;
  private readonly Func<TViewModel> _createViewModel;

  public ModalNavigationService(IModalNavigationStore modalNavigationStore, Func<TViewModel> createViewModel) {
    _modalNavigationStore = modalNavigationStore;
    _createViewModel = createViewModel;
  }

  public void Navigate() => _modalNavigationStore.CurrentViewModel = _createViewModel.Invoke();
}
