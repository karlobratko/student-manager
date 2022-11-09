using StudentManager.UI.Stores;

namespace StudentManager.UI.Services;

public class CloseModalParameterNavigationService<TParameter> : IParameterNavigationService<TParameter>
  where TParameter : class {
  public readonly IModalNavigationStore _modalNavigationStore;

  public CloseModalParameterNavigationService(IModalNavigationStore modalNavigationStore) {
    _modalNavigationStore = modalNavigationStore;
  }

  public void Navigate(TParameter? param) => _modalNavigationStore.Close();
}
