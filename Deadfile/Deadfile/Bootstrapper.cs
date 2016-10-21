﻿using Microsoft.Practices.Unity;
using Prism.Unity;
using Deadfile.Views;
using System.Windows;

namespace Deadfile
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
