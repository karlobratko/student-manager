using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Models;
using StudentManager.DAL.Repository.Db.Sql;
using StudentManager.UI.Services;
using StudentManager.UI.Stores;
using StudentManager.UI.ViewModels;

namespace StudentManager.UI;

public static class Service {
  public static IServiceProvider Setup(IConfiguration configuration) {
    var serviceCollection = new ServiceCollection();
    ConfigureServices(serviceCollection, configuration);

    return serviceCollection.BuildServiceProvider();
  }

  private static void ConfigureServices(IServiceCollection services,
                                        IConfiguration configuration) {
    ConfigureDALServices(services, configuration);
    ConfigureUIServices(services);
  }

  private static void ConfigureDALServices(IServiceCollection services, IConfiguration configuration) {
    _ = services.AddSingleton<IAssistantRepository, AssistantSqlRepository>(
      _ => new AssistantSqlRepository(configuration.GetConnectionString("test-cs"))
    );
    _ = services.AddSingleton<ICourseParticipantRepository, CourseParticipantSqlRepository>(
      _ => new CourseParticipantSqlRepository(configuration.GetConnectionString("test-cs"))
    );
    _ = services.AddSingleton<ICourseRepository, CourseSqlRepository>(
      _ => new CourseSqlRepository(configuration.GetConnectionString("test-cs"))
    );
    _ = services.AddSingleton<ILecturerRepository, LecturerSqlRepository>(
      _ => new LecturerSqlRepository(configuration.GetConnectionString("test-cs"))
    );
    _ = services.AddSingleton<IStudentRepository, StudentSqlRepository>(
      _ => new StudentSqlRepository(configuration.GetConnectionString("test-cs"))
    );
  }

  private static void ConfigureUIServices(IServiceCollection services) {
    _ = services.AddSingleton<IAccountStore, AccountStore>()
                .AddSingleton<IViewNavigationStore, NavigationStore>()
                .AddSingleton<IModalNavigationStore, ModalNavigationStore>();

    _ = services.AddSingleton(CreateLoginNavigationService)
                .AddSingleton<CloseModalNavigationService>();

    _ = services.AddTransient(services => new HomeViewModel(services.GetRequiredService<IAccountStore>()))
                .AddTransient(services =>
                                new LoginViewModel(services.GetRequiredService<IAccountStore>(),
                                                   CreateHomeNavigationService(services)))
                .AddTransient(services =>
                                new StudentListViewModel(services.GetRequiredService<IStudentRepository>(),
                                                         CreateStudentEditNavigationService(services)))
                .AddTransient(services =>
                                new StudentEditViewModel(
                                  services.GetRequiredService<IStudentRepository>(),
                                  new CompositeNavigationService(services.GetRequiredService<CloseModalNavigationService>(),
                                                                 CreateStudentListNavigationService(services))))
                .AddTransient(services =>
                                new LecturerListViewModel(services.GetRequiredService<ILecturerRepository>(),
                                                          CreateLecturerEditNavigationService(services)))
                .AddTransient(services =>
                                new LecturerEditViewModel(
                                  services.GetRequiredService<ILecturerRepository>(),
                                  new CompositeNavigationService(services.GetRequiredService<CloseModalNavigationService>(),
                                                                 CreateLecturerListNavigationService(services))))
                .AddTransient(services =>
                                new CourseListViewModel(services.GetRequiredService<ICourseRepository>(),
                                                        CreateCourseEditNavigationService(services)))
                .AddTransient(services =>
                                new CourseEditViewModel(
                                  services.GetRequiredService<ICourseRepository>(),
                                  services.GetRequiredService<ILecturerRepository>(),
                                  new CompositeNavigationService(services.GetRequiredService<CloseModalNavigationService>(),
                                                                 CreateCourseListNavigationService(services)),
                                  new CompositeParameterNavigationService<Course>(
                                    new CloseModalParameterNavigationService<Course>(services.GetRequiredService<IModalNavigationStore>()),
                                    CreateAssistantListNavigationService(services)),
                                  new CompositeParameterNavigationService<Course>(
                                    new CloseModalParameterNavigationService<Course>(services.GetRequiredService<IModalNavigationStore>()),
                                    CreateCourseParticipantListNavigationService(services))))
                .AddTransient(services =>
                                new AssistantListViewModel(services.GetRequiredService<ILecturerRepository>(),
                                                           services.GetRequiredService<IAssistantRepository>(),
                                                           CreateAssistantCreateNavigationService(services),
                                                           CreateCourseListNavigationService(services)))
                .AddTransient(services =>
                                new AssistantCreateViewModel(
                                  services.GetRequiredService<ILecturerRepository>(),
                                  services.GetRequiredService<IAssistantRepository>(),
                                  new CompositeParameterNavigationService<Course>(
                                    new CloseModalParameterNavigationService<Course>(services.GetRequiredService<IModalNavigationStore>()),
                                    CreateAssistantListNavigationService(services))))
                .AddTransient(services =>
                                new CourseParticipantListViewModel(services.GetRequiredService<IStudentRepository>(),
                                                                   services.GetRequiredService<ICourseParticipantRepository>(),
                                                                   CreateCourseParticipantCreateNavigationService(services),
                                                                   CreateCourseListNavigationService(services)))
                .AddTransient(services =>
                                new CourseParticipantCreateViewModel(
                                  services.GetRequiredService<IStudentRepository>(),
                                  services.GetRequiredService<ICourseParticipantRepository>(),
                                  new CompositeParameterNavigationService<Course>(
                                    new CloseModalParameterNavigationService<Course>(services.GetRequiredService<IModalNavigationStore>()),
                                    CreateCourseParticipantListNavigationService(services))));

    _ = services.AddTransient(CreateNavigationBarViewModel);

    _ = services.AddSingleton<MainViewModel>()
                .AddSingleton(services => new MainWindow() {
                  DataContext = services.GetRequiredService<MainViewModel>()
                });
  }

  private static NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider services) =>
    new(services.GetRequiredService<IAccountStore>(),
        CreateHomeNavigationService(services),
        CreateStudentListNavigationService(services),
        CreateLecturerListNavigationService(services),
        CreateCourseListNavigationService(services),
        CreateLoginNavigationService(services));

  private static INavigationService CreateHomeNavigationService(IServiceProvider services) =>
    new LayoutNavigationService<HomeViewModel>(services.GetRequiredService<IViewNavigationStore>(),
                                               services.GetRequiredService<HomeViewModel>,
                                               services.GetRequiredService<NavigationBarViewModel>);
  private static INavigationService CreateLoginNavigationService(IServiceProvider services) =>
    new NavigationService<LoginViewModel>(services.GetRequiredService<IViewNavigationStore>(),
                                          services.GetRequiredService<LoginViewModel>);
  private static INavigationService CreateStudentListNavigationService(IServiceProvider services) =>
    new LayoutNavigationService<StudentListViewModel>(services.GetRequiredService<IViewNavigationStore>(),
                                                      services.GetRequiredService<StudentListViewModel>,
                                                      services.GetRequiredService<NavigationBarViewModel>);

  private static IParameterNavigationService<Student> CreateStudentEditNavigationService(IServiceProvider services) =>
    new ModalParameterNavigationService<Student, StudentEditViewModel>(services.GetRequiredService<IModalNavigationStore>(),
                                                              param => {
                                                                StudentEditViewModel viewModel = services.GetRequiredService<StudentEditViewModel>();
                                                                if (param is not null)
                                                                  viewModel.Student = param;
                                                                return viewModel;
                                                              });

  private static INavigationService CreateLecturerListNavigationService(IServiceProvider services) =>
    new LayoutNavigationService<LecturerListViewModel>(services.GetRequiredService<IViewNavigationStore>(),
                                                       services.GetRequiredService<LecturerListViewModel>,
                                                       services.GetRequiredService<NavigationBarViewModel>);

  private static IParameterNavigationService<Lecturer> CreateLecturerEditNavigationService(IServiceProvider services) =>
    new ModalParameterNavigationService<Lecturer, LecturerEditViewModel>(services.GetRequiredService<IModalNavigationStore>(),
                                                                         param => {
                                                                           LecturerEditViewModel viewModel = services.GetRequiredService<LecturerEditViewModel>();
                                                                           if (param is not null)
                                                                             viewModel.Lecturer = param;
                                                                           return viewModel;
                                                                         });

  private static INavigationService CreateCourseListNavigationService(IServiceProvider services) =>
    new LayoutNavigationService<CourseListViewModel>(services.GetRequiredService<IViewNavigationStore>(),
                                                     services.GetRequiredService<CourseListViewModel>,
                                                     services.GetRequiredService<NavigationBarViewModel>);

  private static IParameterNavigationService<Course> CreateCourseEditNavigationService(IServiceProvider services) =>
    new ModalParameterNavigationService<Course, CourseEditViewModel>(services.GetRequiredService<IModalNavigationStore>(),
                                                                     param => {
                                                                       CourseEditViewModel viewModel = services.GetRequiredService<CourseEditViewModel>();
                                                                       if (param is not null)
                                                                         viewModel.Course = param;
                                                                       return viewModel;
                                                                     });

  private static IParameterNavigationService<Course> CreateAssistantListNavigationService(IServiceProvider services) =>
    new LayoutParameterNavigationService<Course, AssistantListViewModel>(services.GetRequiredService<IViewNavigationStore>(),
                                                                         param => {
                                                                           AssistantListViewModel viewModel = services.GetRequiredService<AssistantListViewModel>();
                                                                           if (param is not null)
                                                                             viewModel.Course = param;
                                                                           return viewModel;
                                                                         },
                                                                         services.GetRequiredService<NavigationBarViewModel>);

  private static IParameterNavigationService<Course> CreateAssistantCreateNavigationService(IServiceProvider services) =>
    new ModalParameterNavigationService<Course, AssistantCreateViewModel>(services.GetRequiredService<IModalNavigationStore>(),
                                                                          param => {
                                                                            AssistantCreateViewModel viewModel = services.GetRequiredService<AssistantCreateViewModel>();
                                                                            if (param is not null)
                                                                              viewModel.Course = param;
                                                                            return viewModel;
                                                                          });

  private static IParameterNavigationService<Course> CreateCourseParticipantListNavigationService(IServiceProvider services) =>
    new LayoutParameterNavigationService<Course, CourseParticipantListViewModel>(services.GetRequiredService<IViewNavigationStore>(),
                                                                         param => {
                                                                           CourseParticipantListViewModel viewModel = services.GetRequiredService<CourseParticipantListViewModel>();
                                                                           if (param is not null)
                                                                             viewModel.Course = param;
                                                                           return viewModel;
                                                                         },
                                                                         services.GetRequiredService<NavigationBarViewModel>);
  private static IParameterNavigationService<Course> CreateCourseParticipantCreateNavigationService(IServiceProvider services) =>
    new ModalParameterNavigationService<Course, CourseParticipantCreateViewModel>(services.GetRequiredService<IModalNavigationStore>(),
                                                                          param => {
                                                                            CourseParticipantCreateViewModel viewModel = services.GetRequiredService<CourseParticipantCreateViewModel>();
                                                                            if (param is not null)
                                                                              viewModel.Course = param;
                                                                            return viewModel;
                                                                          });
}
