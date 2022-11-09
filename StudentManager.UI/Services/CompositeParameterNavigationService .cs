using System.Collections.Generic;

namespace StudentManager.UI.Services;

public class CompositeParameterNavigationService<TParameter> : IParameterNavigationService<TParameter>
  where TParameter : class {
  private readonly IEnumerable<IParameterNavigationService<TParameter>> _navigationServices;

  public CompositeParameterNavigationService(IEnumerable<IParameterNavigationService<TParameter>> navigationServices) {
    _navigationServices = navigationServices;
  }

  public CompositeParameterNavigationService(params IParameterNavigationService<TParameter>[] navigationServices) {
    _navigationServices = navigationServices;
  }

  public void Navigate(TParameter? param) {
    foreach (IParameterNavigationService<TParameter> navigationService in _navigationServices) {
      navigationService.Navigate(param);
    }
  }
}
