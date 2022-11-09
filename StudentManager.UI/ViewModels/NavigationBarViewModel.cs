using System;
using System.Windows.Input;

using StudentManager.UI.Commands;
using StudentManager.UI.Services;
using StudentManager.UI.Stores;

namespace StudentManager.UI.ViewModels;

public class NavigationBarViewModel : ViewModelBase {
  private readonly IAccountStore _accountStore;

  public ICommand NavigateHomeCommand { get; }
  public ICommand NavigateStudentListCommand { get; }
  public ICommand NavigateLecturerListCommand { get; }
  public ICommand NavigateCourseListCommand { get; }
  public ICommand LogoutCommand { get; }

  public bool IsLoggedIn => _accountStore.IsLoggedIn;

  public NavigationBarViewModel(IAccountStore accountStore,
                                INavigationService homeNavigationService,
                                INavigationService studentListNavigationService,
                                INavigationService lecturerListNavigationService,
                                INavigationService courseListNavigationService,
                                INavigationService loginNavigationService) {
    _accountStore = accountStore;
    NavigateHomeCommand = new NavigateCommand(homeNavigationService);
    NavigateStudentListCommand = new NavigateCommand(studentListNavigationService);
    NavigateLecturerListCommand = new NavigateCommand(lecturerListNavigationService);
    NavigateCourseListCommand = new NavigateCommand(courseListNavigationService);
    LogoutCommand = new LogoutCommand(_accountStore, loginNavigationService);

    _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
  }

  private void OnCurrentAccountChanged() => OnPropertyChanged(nameof(IsLoggedIn));

  public override void Dispose() {
    base.Dispose();

    _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;

    GC.SuppressFinalize(this);
  }
}
