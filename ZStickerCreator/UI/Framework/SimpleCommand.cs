using System;
using System.Windows.Input;

namespace ZStickerCreator.UI.Framework
{
    /// <summary>
    /// WPF Command with object inheritance support.
    /// </summary>
    internal abstract class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) =>
            OnCanExecute(parameter);

        public void Execute(object parameter) =>
            OnExecute(parameter);

        protected virtual bool OnCanExecute(object param) => true;
        protected abstract void OnExecute(object param);
    }
}
