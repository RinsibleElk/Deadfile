using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Entity
{
    /// <summary>
    /// Entity model for a Local Authority.
    /// </summary>
    public class LocalAuthority
    {
        public int LocalAuthorityId { get; set; }

        [Required(ErrorMessage = "You must give this Local Authority a name."),
         MaxLength(100, ErrorMessage = "Local Authority names are required to be less than 100 characters.")]
        public string Name { get; set; }

        [Url(ErrorMessage = "Invalid URL given for this Local Authority.")]
        public string Url { get; set; }
    }
}
