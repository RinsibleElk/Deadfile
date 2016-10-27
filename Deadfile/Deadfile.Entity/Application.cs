using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    public class Application
    {
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "A Planning Application must have a creation date.")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "A Planning Application must have a type.")]
        public ApplicationType Type { get; set; }

        public int JobId { get; set; }

        public virtual Job Job { get; set; }
    }
}
