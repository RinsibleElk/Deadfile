using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;

namespace Deadfile.Infrastructure.Services
{
    public sealed class NavigationService : PropertyChangedBase, INavigationService
    {
        private readonly INavigationContainer _container;
        private readonly Stack<NavigationContext> _backStack = new Stack<NavigationContext>();
        private readonly Stack<NavigationContext> _forwardStack = new Stack<NavigationContext>();
        private bool _canGoBack;
        private bool _canGoForward;

        public void RequestNavigate(object host, string hostKey, string key, object parameters)
        {
            var targetValue = _container.GetInstance(key);
            if (targetValue == null)
                throw new ApplicationException("Failed to find a ViewModel for key " + key + " in host area " + hostKey);
            var property = host.GetType().GetProperty(hostKey, BindingFlags.Instance | BindingFlags.Public);
            var previousValue =
                property
                    .GetMethod
                    .Invoke(host, new object[0]);
            // only support one "region" being journaled at present...
            if (targetValue is IJournaled)
            {
                var context = new NavigationContext(host, hostKey, targetValue, parameters);
                _backStack.Push(context);
                _forwardStack.Clear();
                CanGoBack = _backStack.Count > 1;
                CanGoForward = false;
            }
            (previousValue as INavigationAware)?.OnNavigatedFrom();
            (targetValue as INavigationAware)?.OnNavigatedTo(parameters);
            property
                .SetMethod
                .Invoke(host, new object[] { targetValue });
        }

        public NavigationService(INavigationContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// For the moment assumes that there is only one journaled host/region pair.
        /// </summary>
        public void GoBack()
        {
            var leavingContext = _backStack.Pop();
            _forwardStack.Push(leavingContext);
            var context = _backStack.Peek();
            var targetValue = context.TargetObject;
            var parameters = context.Parameters;
            (leavingContext.TargetObject as INavigationAware)?.OnNavigatedFrom();
            (targetValue as INavigationAware)?.OnNavigatedTo(parameters);
            var host = context.Host;
            var property = host.GetType().GetProperty(context.HostKey, BindingFlags.Instance | BindingFlags.Public);
            property
                .SetMethod
                .Invoke(host, new object[] { targetValue });
            CanGoBack = _backStack.Count > 1;
            CanGoForward = true;
        }

        public bool CanGoBack
        {
            get { return _canGoBack; }
            set
            {
                if (value == _canGoBack) return;
                _canGoBack = value;
                NotifyOfPropertyChange(() => CanGoBack);
            }
        }

        public void GoForward()
        {
            var leavingContext = _backStack.Peek();
            var context = _forwardStack.Pop();
            _backStack.Push(context);
            var targetValue = context.TargetObject;
            var host = context.Host;
            var property = host.GetType().GetProperty(context.HostKey, BindingFlags.Instance | BindingFlags.Public);
            (leavingContext.TargetObject as INavigationAware)?.OnNavigatedFrom();
            (targetValue as INavigationAware)?.OnNavigatedTo(context.Parameters);
            property
                .SetMethod
                .Invoke(host, new object[] { targetValue });
            CanGoBack = _backStack.Count > 1;
            CanGoForward = _forwardStack.Count > 0;
        }

        public bool CanGoForward
        {
            get { return _canGoForward; }
            set
            {
                if (value == _canGoForward) return;
                _canGoForward = value;
                NotifyOfPropertyChange(() => CanGoForward);
            }
        }
    }
}
