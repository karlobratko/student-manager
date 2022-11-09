using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.UI.ViewModels {
  public class LayoutViewModel : ViewModelBase {
    public NavigationBarViewModel NavigationBarViewModel { get; set; }
    public ViewModelBase ContentViewModel { get; set; }

    public LayoutViewModel(NavigationBarViewModel navigationBarViewModel,
                           ViewModelBase contentViewModel) {
      NavigationBarViewModel = navigationBarViewModel;
      ContentViewModel = contentViewModel;
    }

    public override void Dispose() {
      base.Dispose();

      NavigationBarViewModel.Dispose();
      ContentViewModel.Dispose();

      GC.SuppressFinalize(this);
    }
  }
}
