using System;
using System.Windows.Input;
using System.Windows;

/*
 * Using:
        private BaseCommandRelay __TestCommand { get; set; }
        public BaseCommandRelay TestCommand
        {
            get { return __TestCommand; }
        }

        public ...()
        {
            InitializeComponent();

            __TestCommand = new BaseCommandRelay((o) =>
            {
                ...
            });
 * 
 * or:
 * 
        //
        // http://joyfulwpf.blogspot.com/2009/05/mvvm-commandreference-and-keybinding.html
        //
        private BaseCommandRelay __TestCommand { get; set; }
        public ICommand TestCommand
        {
            get
            {
                if (__TestCommand == null)
                {
                    __TestCommand = new BaseCommandReference(RunSubmitCommand, CanRunSubmitCommand);
                }
                return __TestCommand;
            }
        }
 * 
 * 
 */

namespace SpchListBuilder.Base
{
    public class BaseCommandRelay : ICommand
    {
        private Action<object> __execute;
        private Func<object, bool> __canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public BaseCommandRelay(Action<object> execute) : this(execute, null) { }
        public BaseCommandRelay(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("BaseRelayCommand - execute");         //MLHIDE

            __execute = execute;
            __canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return __canExecute == null || __canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            __execute(parameter);
        }
    }

    public class BaseCommandReference : Freezable, ICommand
    {
        public event EventHandler CanExecuteChanged;
        public BaseCommandReference() { }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",                                              //MLHIDE
                typeof(ICommand),
                typeof(BaseCommandReference),
                new PropertyMetadata(OnCommandChanged)
            );

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public bool CanExecute(object parameter)
        {
            if (Command != null)
                return Command.CanExecute(parameter);
            return false;
        }

        public void Execute(object parameter)
        {
            Command.Execute(parameter);
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var __cmdRef = d as BaseCommandReference;
            ICommand __oCommand = e.OldValue as ICommand;
            ICommand __nCommand = e.NewValue as ICommand;

            if (__oCommand != null)
            {
                __oCommand.CanExecuteChanged -= __cmdRef.CanExecuteChanged;
            }
            if (__nCommand != null)
            {
                __nCommand.CanExecuteChanged += __cmdRef.CanExecuteChanged;
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return this;
            throw new NotImplementedException();
        }
    }
}
