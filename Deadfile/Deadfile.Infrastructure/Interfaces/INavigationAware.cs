﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Infrastructure.Interfaces
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object parameters);
        void OnNavigatedFrom();
    }
}