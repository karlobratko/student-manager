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
public class AssistantCreateViewModel : ViewModelBase {
  private readonly ILecturerRepository _lecturerRepository;
  private readonly IAssistantRepository _assistantRepository;
  private readonly IParameterNavigationService<Course> _backNavigationService;

  public DelegateCommand<object> SubmitAssistantCommand { get; }

  public DelegateCommand<object> NavigateBackCommand { get; }

  private Course? _course;
  public Course? Course {
    get => _course;
    set {
      _course = value;

      if (value is not null)
        Assistant.CourseFK = value.Id;
    }
  }

  public Assistant Assistant { get; set; }

  public IEnumerable<Lecturer> Lecturers { get; }

  private Lecturer? _selectedLecturer;
  public Lecturer? SelectedLecturer {
    get => _selectedLecturer;
    set {
      _selectedLecturer = value;

      if (value is not null)
        Assistant.LecturerFK = value.Id;

      SubmitAssistantCommand.OnCanExecuteChanged();
    }
  }

  public AssistantCreateViewModel(ILecturerRepository lecturerRepository,
                                  IAssistantRepository assistantRepository,
                                  IParameterNavigationService<Course> backNavigationService) {
    _lecturerRepository = lecturerRepository;
    _assistantRepository = assistantRepository;
    _backNavigationService = backNavigationService;

    Lecturers = _lecturerRepository.ReadAll();

    Assistant = new Assistant();

    NavigateBackCommand = new DelegateCommand<object>(_ => _backNavigationService.Navigate(Course));

    SubmitAssistantCommand = new DelegateCommand<object>(SubmitCourse);
  }

  private void SubmitCourse(object? obj) {
    if (!Assistant.IsValid()) {
      _ = MessageBox.Show(new StringBuilder()
                            .AppendLine("Course fields are invalid. Please review field values.")
                            .AppendLine("Validation messages:")
                            .AppendListInLines(Assistant.GetValidationMessages(), "- ")
                            .ToString(),
                          "Validation",
                          MessageBoxButton.OK,
                          MessageBoxImage.Warning);
      return;
    }

    if (Assistant.Guid == Guid.Empty) {
      CreateStatus createStatus = _assistantRepository.Create(Assistant);
      switch (createStatus) {
        case CreateStatus.Success:
        case CreateStatus.Recreated:
          _backNavigationService.Navigate(Course);
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while creating new Assistant")
                                .AppendLine($"Status: {createStatus.ToString()}")
                                .ToString(),
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
          break;
      }
    } else {
      UpdateStatus updateStatus = _assistantRepository.Update(Assistant);
      switch (updateStatus) {
        case UpdateStatus.Success:
          _backNavigationService.Navigate(Course);
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while updating Assistant")
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
