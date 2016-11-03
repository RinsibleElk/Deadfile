using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model
{
    public class ApplicationModel : ModelBase
    {
        // No need to report changes or validate.
        public override int Id
        {
            get { return ApplicationId; }
            set { ApplicationId = value; }
        }

        private int _applicationId;
        public int ApplicationId
        {
            get { return _applicationId; }
            set { SetProperty(ref _applicationId, value); }
        }

        private string _localAuthority;
        [Required(ErrorMessage = "An Application must have a local authority.")]
        public string LocalAuthority
        {
            get { return _localAuthority; }
            set { SetProperty(ref _localAuthority, value); }
        }

        private string _localAuthorityReference;
        [Required(ErrorMessage = "An Application must have a local authority reference.")]
        public string LocalAuthorityReference
        {
            get { return _localAuthorityReference; }
            set { SetProperty(ref _localAuthorityReference, value); }
        }

        private DateTime _creationDate;
        [Required(ErrorMessage = "A Planning Application must have a creation date.")]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { SetProperty(ref _creationDate, value); }
        }

        private ApplicationType _applicationType;
        [Required(ErrorMessage = "A Planning Application must have a type.")]
        public ApplicationType Type
        {
            get { return _applicationType; }
            set { SetProperty(ref _applicationType, value); }
        }

        // No need to report changes or validate.
        public int JobId { get; set; }
    }

    /// <summary>
    /// All the application types.
    /// </summary>
    public static class ApplciationTypeUtils
    {
        /// <summary>
        /// All the known application types.
        /// </summary>
        public static readonly List<ApplicationType> AllApplicationTypes = Enum.GetValues(typeof(ApplicationType)).Cast<ApplicationType>().ToList();
    }
}
