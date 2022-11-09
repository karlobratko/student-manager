using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Models;
using StudentManager.UI.Commands;
using StudentManager.UI.Services;

namespace StudentManager.UI.ViewModels;
public class CourseParticipantListViewModel : ViewModelBase {
  private readonly IStudentRepository _studentRepository;
  private readonly ICourseParticipantRepository _courseParticipantRepository;
  private readonly IParameterNavigationService<Course> _assistantCreateNavigationService;
  private readonly INavigationService _backNavigationService;

  public ObservableCollection<Student> CourseParticipants { get; }

  public DelegateCommand<Student> CreateCourseParticipantCommand { get; }
  public DelegateCommand<Student> DeleteCourseParticipantCommand { get; }
  public ICommand NavigateBackCommand { get; }

  public Student? _selectedCourseParticipant;
  public Student? SelectedCourseParticipant {
    get => _selectedCourseParticipant;
    set {
      _selectedCourseParticipant = value;
      DeleteCourseParticipantCommand.OnCanExecuteChanged();
    }
  }

  private Course? _course;
  public Course? Course {
    get => _course;
    set {
      _course = value;

      if (value is not null) {
        foreach (Student lecturer in _studentRepository.ReadByCourseFK(value.Id))
          CourseParticipants.Add(lecturer);
      }
    }
  }

  public CourseParticipantListViewModel(IStudentRepository studentRepository,
                                        ICourseParticipantRepository courseParticipantRepository,
                                        IParameterNavigationService<Course> assistantCreateNavigationService,
                                        INavigationService backNavigationService) {
    _studentRepository = studentRepository;
    _courseParticipantRepository = courseParticipantRepository;
    _assistantCreateNavigationService = assistantCreateNavigationService;
    _backNavigationService = backNavigationService;

    CourseParticipants = new ObservableCollection<Student>();
    CourseParticipants.CollectionChanged += OnCollectionChanged;

    CreateCourseParticipantCommand = new DelegateCommand<Student>(CreateCourseParticipant);
    DeleteCourseParticipantCommand = new DelegateCommand<Student>(DeleteCourseParticipant, param => SelectedCourseParticipant is not null);
    NavigateBackCommand = new NavigateCommand(_backNavigationService);
  }

  private void CreateCourseParticipant(Student? obj) => _assistantCreateNavigationService.Navigate(Course);

  private void DeleteCourseParticipant(Student? assistant) {
    MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete CourseParticipant?",
                                                        "Delete CourseParticipant",
                                                        MessageBoxButton.OKCancel,
                                                        MessageBoxImage.Warning,
                                                        MessageBoxResult.Cancel);
    if (messageBoxResult == MessageBoxResult.OK &&
        SelectedCourseParticipant is not null)
      _ = CourseParticipants.Remove(SelectedCourseParticipant);
  }

  private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
    switch (e.Action) {
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems is not null && Course is not null)
          _ = _courseParticipantRepository.DeleteByCourseFKAndStudentFK(Course.Id, e.OldItems.OfType<Student>().ToList()[0].Id);
        break;
    }
  }
}
