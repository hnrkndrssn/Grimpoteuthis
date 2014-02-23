using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Mobile;
using Grimpoteuthis.Views;

namespace Grimpoteuthis.ViewModels
{
    [DataContract]
    public class AppBootstrapper : ReactiveObject, IApplicationRootState
    {
        [DataMember] RoutingState _Router;

        public IRoutingState Router
        {
            get { return _Router; }
            set { _Router = (RoutingState)value; }
        }

        public AppBootstrapper()
        {
            Router = new RoutingState();

            var resolver = RxApp.MutableResolver;

            resolver.Register(() => new LoginView(), typeof(IViewFor<LoginViewModel>), "FullScreenLandscape");

            resolver.Register(() => new LoginViewModel(), typeof(LoginViewModel));

            resolver.RegisterConstant(this, typeof(IApplicationRootState));
            resolver.RegisterConstant(this, typeof(IScreen));
            resolver.RegisterConstant(new MainPage(), typeof(IViewFor), "InitialPage");

            Router.Navigate.Execute(new LoginViewModel(this));
        }
    }
}