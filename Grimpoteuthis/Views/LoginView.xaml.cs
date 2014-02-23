using Grimpoteuthis.ViewModels;
using ReactiveUI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Grimpoteuthis.Views
{
    public sealed partial class LoginView : Page, IViewFor<LoginViewModel>
    {

        public LoginViewModel ViewModel
        {
            get { return (LoginViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
                    DependencyProperty.Register("ViewModel", 
                        typeof(LoginViewModel), 
                        typeof(LoginView), 
                        new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (LoginViewModel)value; }
        }

        public LoginView()
        {
            this.InitializeComponent();

            this.Bind(ViewModel, x => x.Username, x => x.Username.Text);
            this.Bind(ViewModel, x => x.Password, x => x.Password.Password);

            this.BindCommand(ViewModel, x => x.LoginCommand, x => x.LoginButton);
        }
    }
}
