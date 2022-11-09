using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Enums;
using StudentManager.DAL.Models;
using StudentManager.UI.Commands;
using StudentManager.UI.Extensions;
using StudentManager.UI.Services;

namespace StudentManager.UI.ViewModels;
public class CourseParticipantCreateViewModel : ViewModelBase {
  private readonly IStudentRepository _studentRepository;
  private readonly ICourseParticipantRepository _courseParticipantRepository;
  private readonly IParameterNavigationService<Course> _backNavigationService;

  public DelegateCommand<object> SubmitCourseParticipantCommand { get; }

  public DelegateCommand<object> NavigateBackCommand { get; }

  private Course? _course;
  public Course? Course {
    get => _course;
    set {
      _course = value;

      if (value is not null)
        CourseParticipant.CourseFK = value.Id;
    }
  }

  public CourseParticipant CourseParticipant { get; set; }

  public IEnumerable<Student> Students { get; }

  private Student? _selectedStudent;
  public Student? SelectedStudent {
    get => _selectedStudent;
    set {
      _selectedStudent = value;

      if (value is not null)
        CourseParticipant.StudentFK = value.Id;

      SubmitCourseParticipantCommand.OnCanExecuteChanged();
    }
  }

  public CourseParticipantCreateViewModel(IStudentRepository studentRepository,
                                          ICourseParticipantRepository courseParticipantRepository,
                                          IParameterNavigationService<Course> backNavigationService) {
    _studentRepository = studentRepository;
    _courseParticipantRepository = courseParticipantRepository;
    _backNavigationService = backNavigationService;

    Students = _studentRepository.ReadAll();

    CourseParticipant = new CourseParticipant();

    NavigateBackCommand = new DelegateCommand<object>(_ => _backNavigationService.Navigate(Course));

    SubmitCourseParticipantCommand = new DelegateCommand<object>(SubmitCourse);
  }

  private void SubmitCourse(object? obj) {
    if (!CourseParticipant.IsValid()) {
      _ = MessageBox.Show(new StringBuilder()
                            .AppendLine("Course fields are invalid. Please review field values.")
                            .AppendLine("Validation messages:")
                            .AppendListInLines(CourseParticipant.GetValidationMessages(), "- ")
                            .ToString(),
                          "Validation",
                          MessageBoxButton.OK,
                          MessageBoxImage.Warning);
      return;
    }

    if (CourseParticipant.Guid == Guid.Empty) {
      CreateStatus createStatus = _courseParticipantRepository.Create(CourseParticipant);
      switch (createStatus) {
        case CreateStatus.Success:
        case CreateStatus.Recreated:
          _backNavigationService.Navigate(Course);
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while creating new Course Participant")
                                .AppendLine($"Status: {createStatus.ToString()}")
                                .ToString(),
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
          break;
      }
    } else {
      UpdateStatus updateStatus = _courseParticipantRepository.Update(CourseParticipant);
      switch (updateStatus) {
        case UpdateStatus.Success:
          _backNavigationService.Navigate(Course);
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while updating Course Participant")
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
