using System;
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

public class AssistantListViewModel : ViewModelBase {
  private readonly ILecturerRepository _lecturerRepository;
  private readonly IAssistantRepository _assistantRepository;
  private readonly IParameterNavigationService<Course> _assistantCreateNavigationService;
  private readonly INavigationService _backNavigationService;

  public ObservableCollection<Lecturer> Assistants { get; }

  public DelegateCommand<Lecturer> CreateAssistantCommand { get; }
  public DelegateCommand<Lecturer> DeleteAssistantCommand { get; }
  public ICommand NavigateBackCommand { get; }

  public Lecturer? _selectedAssistant;
  public Lecturer? SelectedAssistant {
    get => _selectedAssistant;
    set {
      _selectedAssistant = value;
      DeleteAssistantCommand.OnCanExecuteChanged();
    }
  }

  private Course? _course;
  public Course? Course {
    get => _course;
    set {
      _course = value;

      if (value is not null) { 
        foreach (Lecturer lecturer in _lecturerRepository.ReadByCourseFK(value.Id))
          Assistants.Add(lecturer);
      }
    }
  }

  public AssistantListViewModel(ILecturerRepository repository,
                                IAssistantRepository assistantRepository,
                                IParameterNavigationService<Course> assistantCreateNavigationService,
                                INavigationService backNavigationService) {
    _lecturerRepository = repository;
    _assistantRepository = assistantRepository;
    _assistantCreateNavigationService = assistantCreateNavigationService;
    _backNavigationService = backNavigationService;

    Assistants = new ObservableCollection<Lecturer>();
    Assistants.CollectionChanged += OnCollectionChanged;

    CreateAssistantCommand = new DelegateCommand<Lecturer>(CreateAssistant);
    DeleteAssistantCommand = new DelegateCommand<Lecturer>(DeleteAssistant, param => SelectedAssistant is not null);
    NavigateBackCommand = new NavigateCommand(_backNavigationService);
  }

  private void CreateAssistant(Lecturer? obj) => _assistantCreateNavigationService.Navigate(Course);

  private void DeleteAssistant(Lecturer? assistant) {
    MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete Assistant?",
                                                        "Delete Assistant",
                                                        MessageBoxButton.OKCancel,
                                                        MessageBoxImage.Warning,
                                                        MessageBoxResult.Cancel);
    if (messageBoxResult == MessageBoxResult.OK &&
        SelectedAssistant is not null)
      _ = Assistants.Remove(SelectedAssistant);
  }

  private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
    switch (e.Action) {
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems is not null && Course is not null)
          _ = _assistantRepository.DeleteByCourseFKAndLecturerFK(Course.Id, e.OldItems.OfType<Lecturer>().ToList()[0].Id);
        break;
    }
  }
}
