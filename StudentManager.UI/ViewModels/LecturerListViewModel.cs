using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Models;
using StudentManager.UI.Commands;
using StudentManager.UI.Services;

namespace StudentManager.UI.ViewModels;

public class LecturerListViewModel : ViewModelBase {
  private readonly ILecturerRepository _repository;
  private readonly IParameterNavigationService<Lecturer> _lecturerEditNavigationService;

  public ObservableCollection<Lecturer> Lecturers { get; }

  public ParameterNavigateCommand<Lecturer> CreateLecturerCommand { get; }
  public DelegateCommand<Lecturer> DeleteLecturerCommand { get; }
  public DelegateCommand<Lecturer> UpdateLecturerCommand { get; }

  private Lecturer? _selectedLecturer;
  public Lecturer? SelectedLecturer {
    get => _selectedLecturer;
    set {
      _selectedLecturer = value;
      DeleteLecturerCommand.OnCanExecuteChanged();
      UpdateLecturerCommand.OnCanExecuteChanged();
    }
  }

  public LecturerListViewModel(ILecturerRepository repository,
                               IParameterNavigationService<Lecturer> lecturerEditNavigationService) {
    _repository = repository;
    _lecturerEditNavigationService = lecturerEditNavigationService;

    Lecturers = new ObservableCollection<Lecturer>(repository.ReadAll());
    Lecturers.CollectionChanged += OnCollectionChanged;

    CreateLecturerCommand = new ParameterNavigateCommand<Lecturer>(_lecturerEditNavigationService);
    UpdateLecturerCommand = new DelegateCommand<Lecturer>(UpdateLecturer, param => SelectedLecturer is not null);
    DeleteLecturerCommand = new DelegateCommand<Lecturer>(DeleteLecturer, param => SelectedLecturer is not null);
  }

  private void UpdateLecturer(Lecturer? lecturer) {
    if (SelectedLecturer is not null)
      _lecturerEditNavigationService.Navigate(SelectedLecturer);
  }

  private void DeleteLecturer(Lecturer? lecturer) {
    MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete Lecturer?",
                                                        "Delete Lecturer",
                                                        MessageBoxButton.OKCancel,
                                                        MessageBoxImage.Warning,
                                                        MessageBoxResult.Cancel);
    if (messageBoxResult == MessageBoxResult.OK &&
        SelectedLecturer is not null)
      _ = Lecturers.Remove(SelectedLecturer);
  }

  private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
    switch (e.Action) {
      case NotifyCollectionChangedAction.Add:
        _ = _repository.Create(Lecturers[e.NewStartingIndex]);
        break;
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems is not null)
          _ = _repository.Delete(e.OldItems.OfType<Lecturer>().ToList()[0]);
        break;
      case NotifyCollectionChangedAction.Replace:
        if (e.NewItems is not null)
          _ = _repository.Update(e.NewItems.OfType<Lecturer>().ToList()[0]);
        break;
    }
  }
}

