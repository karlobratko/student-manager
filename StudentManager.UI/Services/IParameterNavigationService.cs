namespace StudentManager.UI.Services;
public interface IParameterNavigationService<T> where T : class {
  void Navigate(T? param);
}
