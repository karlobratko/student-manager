using System;

using StudentManager.UI.Stores;

namespace StudentManager.UI.ViewModels;

public class MainViewModel : ViewModelBase {
  private readonly IViewNavigationStore _navigationStore;
  private readonly IModalNavigationStore _modalNavigationStore;
  public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;
  public ViewModelBase? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;

  public bool IsModalOpen => _modalNavigationStore.IsOpen;

  public MainViewModel(IViewNavigationStore navigationStore,
                       IModalNavigationStore modalNavigationStore) {
    _navigationStore = navigationStore;
    _modalNavigationStore = modalNavigationStore;

    _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    _modalNavigationStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
  }

  private void OnCurrentViewModelChanged() => OnPropertyChanged(nameof(CurrentViewModel));
  private void OnCurrentModalViewModelChanged() {
    OnPropertyChanged(nameof(CurrentModalViewModel));
    OnPropertyChanged(nameof(IsModalOpen));
  }
}
