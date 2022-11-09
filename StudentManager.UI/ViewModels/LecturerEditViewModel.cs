using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Microsoft.Win32;

using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Enums;
using StudentManager.DAL.Models;
using StudentManager.UI.Commands;
using StudentManager.UI.Extensions;
using StudentManager.UI.Services;
using StudentManager.UI.Utility;

namespace StudentManager.UI.ViewModels;
public class LecturerEditViewModel : ViewModelBase {
  private readonly ILecturerRepository _repository;
  private readonly INavigationService _backNavigationService;

  public DelegateCommand<object> UploadPhotoCommand { get; }
  public DelegateCommand<object> SubmitLecturerCommand { get; }

  public ICommand NavigateBackCommand { get; }

  public Lecturer Lecturer { get; set; }

  public BitmapImage Photo =>
    Lecturer is not null &&
    Lecturer.Image is not null
      ? Images.ByteArrayToBitmapImage(Lecturer.Image)
      : new BitmapImage();

  public LecturerEditViewModel(ILecturerRepository repository, INavigationService backNavigationService) {
    _repository = repository;
    _backNavigationService = backNavigationService;

    Lecturer = new Lecturer();

    NavigateBackCommand = new NavigateCommand(_backNavigationService);

    UploadPhotoCommand = new DelegateCommand<object>(UploadPhoto);
    SubmitLecturerCommand = new DelegateCommand<object>(SubmitLecturer);
  }

  private void UploadPhoto(object? param) {
    var op = new OpenFileDialog {
      Title = "Select a picture",
      Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png"
    };

    if (op.ShowDialog() == true) {
      Lecturer.Image = File.ReadAllBytes(op.FileName);
      OnPropertyChanged(nameof(Photo));
    }
  }

  private void SubmitLecturer(object? obj) {
    if (!Lecturer.IsValid()) {
      _ = MessageBox.Show(new StringBuilder()
                            .AppendLine("Lecturer fields are invalid. Please review field values.")
                            .AppendLine("Validation messages:")
                            .AppendListInLines(Lecturer.GetValidationMessages(), "- ")
                            .ToString(),
                          "Validation",
                          MessageBoxButton.OK,
                          MessageBoxImage.Warning);
      return;
    }

    if (Lecturer.Guid == Guid.Empty) {
      CreateStatus createStatus = _repository.Create(Lecturer);
      switch (createStatus) {
        case CreateStatus.Success:
        case CreateStatus.Recreated:
          _backNavigationService.Navigate();
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while creating new Lecturer")
                                .AppendLine($"Status: {createStatus.ToString()}")
                                .ToString(),
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
          break;
      }
    } else {
      UpdateStatus updateStatus = _repository.Update(Lecturer);
      switch (updateStatus) {
        case UpdateStatus.Success:
          _backNavigationService.Navigate();
          break;
        default:
          _ = MessageBox.Show(new StringBuilder()
                                .AppendLine("Error while updating Lecturer")
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
