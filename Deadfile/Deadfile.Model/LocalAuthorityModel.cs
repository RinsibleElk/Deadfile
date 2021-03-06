﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model
{
    /// <summary>
    /// UI model for a Local Authority.
    /// </summary>
    public class LocalAuthorityModel : ModelBase
    {
        private int _localAuthorityId;
        private string _name;
        private string _url;
        private bool _hasUrl;

        public override int Id
        {
            get { return LocalAuthorityId; }
            set { LocalAuthorityId = value; }
        }

        public int LocalAuthorityId
        {
            get { return _localAuthorityId; }
            set { SetProperty(ref _localAuthorityId, value); }
        }

        [Required(ErrorMessage = "You must give this Local Authority a name."),
         MaxLength(100, ErrorMessage = "Local Authority names are required to be less than 100 characters.")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        [Url(ErrorMessage = "Invalid URL given for this Local Authority.")]
        public string Url
        {
            get { return _url; }
            set
            {
                // This is to get cleared urls to be free of validation.
                var valueToSet = (value == "") ? null : value;
                if (SetProperty(ref _url, valueToSet))
                {
                    HasUrl = !String.IsNullOrWhiteSpace(_url);
                }
            }
        }

        public bool HasUrl
        {
            get { return _hasUrl; }
            set { SetProperty(ref _hasUrl, value); }
        }
    }
}
