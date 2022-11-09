namespace StudentManager.UI.Stores {
  public interface IModalNavigationStore : INavigationStore {
    bool IsOpen { get; }

    void Close();
  }
}