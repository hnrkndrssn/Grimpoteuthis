using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace Grimpoteuthis.ViewModels
{
    public class LoginViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment { get { return "Login"; } }
        public IScreen HostScreen { get; private set; }

        string _Username = String.Empty;
        public string Username
        {
            get { return _Username; }
            set { this.RaiseAndSetIfChanged(ref _Username, value); }
        }

        string _Password = String.Empty;
        public string Password
        {
            get { return _Password; }
            set { this.RaiseAndSetIfChanged(ref _Password, value); }
        }

        public ReactiveCommand LoginCommand { get; protected set; }

        public LoginViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? RxApp.DependencyResolver.GetService<IScreen>();

            var canLogin = this.WhenAny(
                    x => x.Username,
                    x => x.Password,
                    (user, pass) => (!String.IsNullOrWhiteSpace(user.Value) && !String.IsNullOrWhiteSpace(pass.Value)));

            LoginCommand = new ReactiveCommand(canLogin);
        }

    }
}
