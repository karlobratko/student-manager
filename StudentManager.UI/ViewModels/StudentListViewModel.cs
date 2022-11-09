using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Models;
using StudentManager.UI.Commands;
using StudentManager.UI.Services;

namespace StudentManager.UI.ViewModels;

public class StudentListViewModel : ViewModelBase {
  private readonly IStudentRepository _repository;
  private readonly IParameterNavigationService<Student> _studentEditNavigationService;

  public ObservableCollection<Student> Students { get; }

  public ParameterNavigateCommand<Student> CreateStudentCommand { get; }
  public DelegateCommand<Student> DeleteStudentCommand { get; }
  public DelegateCommand<Student> UpdateStudentCommand { get; }

  private Student? _selectedStudent;
  public Student? SelectedStudent {
    get => _selectedStudent;
    set {
      _selectedStudent = value;
      DeleteStudentCommand.OnCanExecuteChanged();
      UpdateStudentCommand.OnCanExecuteChanged();
    }
  }

  public StudentListViewModel(IStudentRepository repository,
                              IParameterNavigationService<Student> studentEditNavigationService) {
    _repository = repository;
    _studentEditNavigationService = studentEditNavigationService;

    Students = new ObservableCollection<Student>(repository.ReadAll());
    Students.CollectionChanged += OnCollectionChanged;

    CreateStudentCommand = new ParameterNavigateCommand<Student>(_studentEditNavigationService);
    UpdateStudentCommand = new DelegateCommand<Student>(UpdateStudent, param => SelectedStudent is not null);
    DeleteStudentCommand = new DelegateCommand<Student>(DeleteStudent, param => SelectedStudent is not null);
  }

  private void UpdateStudent(Student? student) {
    if (SelectedStudent is not null)
      _studentEditNavigationService.Navigate(SelectedStudent);
  }

  private void DeleteStudent(Student? student) {
    MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete Student?",
                                                        "Delete Student",
                                                        MessageBoxButton.OKCancel,
                                                        MessageBoxImage.Warning,
                                                        MessageBoxResult.Cancel);
    if (messageBoxResult == MessageBoxResult.OK &&
        SelectedStudent is not null)
      _ = Students.Remove(SelectedStudent);
  }

  private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
    switch (e.Action) {
      case NotifyCollectionChangedAction.Add:
        _ = _repository.Create(Students[e.NewStartingIndex]);
        break;
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems is not null)
          _ = _repository.Delete(e.OldItems.OfType<Student>().ToList()[0]);
        break;
      case NotifyCollectionChangedAction.Replace:
        if (e.NewItems is not null)
          _ = _repository.Update(e.NewItems.OfType<Student>().ToList()[0]);
        break;
    }
  }
}
