﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using INavigationService = Deadfile.Infrastructure.Interfaces.INavigationService;

namespace Deadfile.Infrastructure.Services
{
    public sealed class NavigationService : PropertyChangedBase, INavigationService
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly INavigationContainer _container;
        private readonly Stack<NavigationContext> _backStack = new Stack<NavigationContext>();
        private readonly Stack<NavigationContext> _forwardStack = new Stack<NavigationContext>();
        private bool _canGoBack;
        private bool _canGoForward;

        public void RequestNavigate(object host, string hostKey, string key, object parameters)
        {
            try
            {
                var targetValue = _container.GetInstance(key);
                if (targetValue == null)
                    throw new ApplicationException("Failed to find a ViewModel for key " + key + " in host area " +
                                                   hostKey);
                ActuallyNavigate(host, hostKey, targetValue, parameters);
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception thrown during Navigate, {0}, {1}", e, e.StackTrace);
                throw;
            }
        }

        private void ActuallyNavigate(object host, string hostKey, object targetValue, object parameters)
        {
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

        public void RequestDeactivate(object host, string hostKey)
        {
            ActuallyNavigate(host, hostKey, null, null);
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

        public void FallBack()
        {
            var leavingContext = _backStack.Pop();
            _forwardStack.Clear();
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
            CanGoForward = false;
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

        public void Teardown()
        {
            _backStack.Clear();
            _forwardStack.Clear();
        }

        public void SetCurrentNavigationParameters(object newParameters)
        {
            var contextToEdit = _backStack.Pop();
            _backStack.Push(new NavigationContext(contextToEdit.Host, contextToEdit.HostKey, contextToEdit.TargetObject, newParameters));
        }
    }
}
