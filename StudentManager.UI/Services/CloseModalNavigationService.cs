using StudentManager.UI.Stores;

namespace StudentManager.UI.Services;

public class CloseModalNavigationService : INavigationService {
  public readonly IModalNavigationStore _modalNavigationStore;

  public CloseModalNavigationService(IModalNavigationStore modalNavigationStore) {
    _modalNavigationStore = modalNavigationStore;
  }

  public void Navigate() => _modalNavigationStore.Close();
}
