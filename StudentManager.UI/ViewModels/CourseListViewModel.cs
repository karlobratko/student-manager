using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

using StudentManager.DAL.Base.Repository.Models;

using StudentManager.DAL.Models;

using StudentManager.UI.Commands;

using StudentManager.UI.Services;

namespace StudentManager.UI.ViewModels;
public class CourseListViewModel : ViewModelBase {
  private readonly ICourseRepository _repository;
  private readonly IParameterNavigationService<Course> _courseEditNavigationService;

  public ObservableCollection<Course> Courses { get; }

  public ParameterNavigateCommand<Course> CreateCourseCommand { get; }
  public DelegateCommand<Course> DeleteCourseCommand { get; }
  public DelegateCommand<Course> UpdateCourseCommand { get; }

  private Course? _selectedCourse;
  public Course? SelectedCourse {
    get => _selectedCourse;
    set {
      _selectedCourse = value;
      DeleteCourseCommand.OnCanExecuteChanged();
      UpdateCourseCommand.OnCanExecuteChanged();
    }
  }

  public CourseListViewModel(ICourseRepository repository,
                             IParameterNavigationService<Course> courseEditNavigationService) {
    _repository = repository;
    _courseEditNavigationService = courseEditNavigationService;

    Courses = new ObservableCollection<Course>(repository.ReadAll());
    Courses.CollectionChanged += OnCollectionChanged;

    CreateCourseCommand = new ParameterNavigateCommand<Course>(_courseEditNavigationService);
    UpdateCourseCommand = new DelegateCommand<Course>(UpdateCourse, param => SelectedCourse is not null);
    DeleteCourseCommand = new DelegateCommand<Course>(DeleteCourse, param => SelectedCourse is not null);
  }

  private void UpdateCourse(Course? course) {
    if (SelectedCourse is not null)
      _courseEditNavigationService.Navigate(SelectedCourse);
  }

  private void DeleteCourse(Course? course) {
    MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete Course?",
                                                        "Delete Course",
                                                        MessageBoxButton.OKCancel,
                                                        MessageBoxImage.Warning,
                                                        MessageBoxResult.Cancel);
    if (messageBoxResult == MessageBoxResult.OK &&
        SelectedCourse is not null)
      _ = Courses.Remove(SelectedCourse);
  }

  private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
    switch (e.Action) {
      case NotifyCollectionChangedAction.Add:
        _ = _repository.Create(Courses[e.NewStartingIndex]);
        break;
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems is not null)
          _ = _repository.Delete(e.OldItems.OfType<Course>().ToList()[0]);
        break;
      case NotifyCollectionChangedAction.Replace:
        if (e.NewItems is not null)
          _ = _repository.Update(e.NewItems.OfType<Course>().ToList()[0]);
        break;
    }
  }
}
