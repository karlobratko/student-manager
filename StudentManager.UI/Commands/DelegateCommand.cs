using System;

namespace StudentManager.UI.Commands;

public class DelegateCommand<T> : CommandBase where T : class {
  private readonly Action<T?> _execute;
  private readonly Predicate<T?>? _canExecute;

  public DelegateCommand(Action<T?> execute) : this(execute, null) {
  }

  public DelegateCommand(Action<T?> execute, Predicate<T?>? canExecute) {
    _execute = execute;
    _canExecute = canExecute;
  }

  public override bool CanExecute(object? parameter) => _canExecute is null || _canExecute(parameter as T);

  public override void Execute(object? parameter) => _execute(parameter as T);
}