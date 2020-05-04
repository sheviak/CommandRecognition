using System.Windows.Input;

namespace CommandRecognition.UI.Command
{
    public interface IDelegateCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}