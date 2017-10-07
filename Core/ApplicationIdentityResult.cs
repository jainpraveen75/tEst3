using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ApplicationIdentityResult
    {
        public ApplicationIdentityResult(IEnumerable<string> errors, bool succeeded)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; private set; }

        public bool Succeeded { get; private set; }
    }
}
