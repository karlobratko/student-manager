using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Enums;
using StudentManager.DAL.Models;
using StudentManager.UI.Commands;
using StudentManager.UI.Extensions;
using StudentManager.UI.Services;

namespace StudentManager.UI.ViewModels;
public class CourseEditViewModel : ViewModelBase {
  private readonly ICourseRepository _courseRepository;
  private readonly ILecturerRepository _lecturerRepository;
  private readonly INavigationService _backNavigationService;
  private readonly IParameterNavigationService<Course> _assistantListNavigationService;
  private readonly IParameterNavigationService<Course> _courseParticipantListNavigationService;

  public DelegateCommand<object> SubmitCourseCommand { get; }

  public ICommand NavigateBackCommand { get; }
  public DelegateCommand<object> NavigateAssistantListCommand { get; }
  public DelegateCommand<object> NavigateCourseParticipantListCommand { get; }

  private Course _course;
  public Course Course {
    get => _course;
    set {
      _course = value;
      SelectedLecturer = Lecturers.FirstOrDefault(lecturer => lecturer.Id == value.HeadLecturerFK);
    }
  }

  public IEnumerable<Lecturer> Lecturers { get; }

  private Lecturer? _selectedLecturer;
  public Lecturer? SelectedLecturer {
    get => _selectedLecturer;
    set {
      _selectedLecturer = value;
      if (value is not null)
        Course.HeadLecturerFK = value.Id;

      SubmitCourseCommand.OnCanExecuteChanged();
    }
  }

  public CourseEditViewModel(ICourseRepository repository,
                             ILecturerRepository lecturerRepository,
                             INavigationService backNavigationService,
                             IParameterNavigationService<Course> assistantListNavigationService,
                             IParameterNavigationService<Course> courseParticipantListNavigationService) {
    _courseRepository = repository;
    _lecturerRepository = lecturerRepository;
    _backNavigationService = backNavigationService;
    _assistantListNavigationService = assistantListNavigationService;
    _courseParticipantListNavigationService = courseParticipantListNavigationService;

    _course = new Course();

    Lecturers = _lecturerRepository.ReadAll();

    NavigateBackCommand = new NavigateCommand(_backNavigationService);
    NavigateAssistantListCommand = new DelegateCommand<object>(_ => _assistantListNavigationService.Navigate(Course),
                                                               _ => Course.Guid != Guid.Empty);
    NavigateCourseParticipantListCommand = new DelegateCommand<object>(_ => _courseParticipantListNavigationService.Navigate(Course),
                                                                       _ => Course.Guid != Guid.Empty);
    SubmitCourseCommand = new DelegateCommand<object>(SubmitCourse);
  }

  private void SubmitCourse(object? obj) {
    if (!Course.IsValid()) {
      _ = MessageBox.Show(new StringBuilder()
                            .AppendLine("Course fields are invalid. Please review field values.")
                            .AppendLine("Validation messages:")
                            .AppendListInLines(Course.GetValidationMessages(), "- ")
                            .ToString(),
                          "Validation",
                          MessageBoxButton.OK,
                          MessageBoxImage.Warning);
      return;
    }

    if (Course.Guid == Guid.Empty) {
      CreateStatus createStatus = _courseRepository.Create(Course);
      switch (createStatus) {
        case CreateStatus.Success:
        case CreateStatus.Recreated:
          _backNavigationService.Navigate();
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while creating new Course")
                                .AppendLine($"Status: {createStatus.ToString()}")
                                .ToString(),
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
          break;
      }
    } else {
      UpdateStatus updateStatus = _courseRepository.Update(Course);
      switch (updateStatus) {
        case UpdateStatus.Success:
          _backNavigationService.Navigate();
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while updating Course")
                                .AppendLine($"Status: {updateStatus.ToString()}")
                                .ToString(),
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
          break;
      }
    }
  }
}
