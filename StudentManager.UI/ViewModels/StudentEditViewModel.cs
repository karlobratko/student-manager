﻿using System.Text;
using System.Windows;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Enums;
using StudentManager.DAL.Models;
using StudentManager.UI.Commands;

using StudentManager.UI.Services;
using StudentManager.UI.Utility;
using System.IO;
using StudentManager.UI.Extensions;

namespace StudentManager.UI.ViewModels;

public class StudentEditViewModel : ViewModelBase {
  private readonly IStudentRepository _repository;
  private readonly INavigationService _backNavigationService;

  public DelegateCommand<object> UploadPhotoCommand { get; }
  public DelegateCommand<object> SubmitStudentCommand { get; }

  public ICommand NavigateBackCommand { get; }

  public Student Student { get; set; }

  public BitmapImage Photo =>
    Student is not null &&
    Student.Image is not null
      ? Images.ByteArrayToBitmapImage(Student.Image)
      : new BitmapImage();

  public StudentEditViewModel(IStudentRepository repository, INavigationService backNavigationService) {
    _repository = repository;
    _backNavigationService = backNavigationService;

    Student = new Student();

    NavigateBackCommand = new NavigateCommand(_backNavigationService);

    UploadPhotoCommand = new DelegateCommand<object>(UploadPhoto);
    SubmitStudentCommand = new DelegateCommand<object>(SubmitStudent);
  }

  private void UploadPhoto(object? param) {
    var op = new OpenFileDialog {
      Title = "Select a picture",
      Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png"
    };

    if (op.ShowDialog() == true) {
      Student.Image = File.ReadAllBytes(op.FileName);
      OnPropertyChanged(nameof(Photo));
    }
  }

  private void SubmitStudent(object? obj) {
    if (!Student.IsValid()) {
      _ = MessageBox.Show(new StringBuilder()
                            .AppendLine("Student fields are invalid. Please review field values.")
                            .AppendLine("Validation messages:")
                            .AppendListInLines(Student.GetValidationMessages(), "- ")
                            .ToString(),
                          "Validation",
                          MessageBoxButton.OK,
                          MessageBoxImage.Warning);
      return;
    }

    if (Student.Guid == Guid.Empty) {
      CreateStatus createStatus = _repository.Create(Student);
      switch (createStatus) {
        case CreateStatus.Success:
        case CreateStatus.Recreated:
          _backNavigationService.Navigate();
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while creating new Student")
                                .AppendLine($"Status: {createStatus.ToString()}")
                                .ToString(),
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
          break;
      }
    } else {
      UpdateStatus updateStatus = _repository.Update(Student);
      switch (updateStatus) {
        case UpdateStatus.Success:
          _backNavigationService.Navigate();
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while updating Student")
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
