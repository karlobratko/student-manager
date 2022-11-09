namespace StudentManager.UI.Stores {
  public sealed class ModalNavigationStore : NavigationStoreBase, IModalNavigationStore {

    public bool IsOpen => CurrentViewModel is not null;

    public void Close() => CurrentViewModel = null;

  }
}
