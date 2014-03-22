using ReactiveUI;
using System;
using System.Threading.Tasks;
using Octokit;
using System.Collections.Generic;

namespace Grimpoteuthis.ViewModels
{
    public class LoginViewModel : ReactiveObject, IRoutableViewModel
    {
        IGitHubClient _Client = new GitHubClient(new ProductHeaderValue("Grimpoteuthis"));
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
            LoginCommand.RegisterAsyncTask(async _ =>
            {
                await Login();
            });

        }

        private async Task Login()
        {
            _Client.Connection.Credentials = new Credentials(Username, Password);

            var newAuthorization = new NewAuthorization
            {
                Scopes = new List<string> { "user", "repo", "delete_repo", "notifications", "gist" },
                Note = "Grimpoteuthis"
            };

            var authorization = await _Client.Authorization.GetOrCreateApplicationAuthentication(
                "client-id-of-your-registered-github-application",
                "client-secret-of-your-registered-github-application",
                newAuthorization);

            _Client.Connection.Credentials = new Credentials(authorization.Token);
            RxApp.MutableResolver.Register(() => _Client, typeof(IGitHubClient));
        }

    }
}
