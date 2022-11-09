using System;

using StudentManager.UI.Stores;
using StudentManager.UI.ViewModels;

namespace StudentManager.UI.Services;

public class ModalParameterNavigationService<TParameter, TViewModel> : IParameterNavigationService<TParameter>
  where TParameter : class
  where TViewModel : ViewModelBase {
  private readonly IModalNavigationStore _modalNavigationStore;
  private readonly Func<TParameter?, TViewModel> _createViewModel;

  public ModalParameterNavigationService(IModalNavigationStore modalNavigationStore,
                                         Func<TParameter?, TViewModel> createViewModel) {
    _modalNavigationStore = modalNavigationStore;
    _createViewModel = createViewModel;
  }

  public void Navigate(TParameter? param) => _modalNavigationStore.CurrentViewModel = _createViewModel.Invoke(param);
}
