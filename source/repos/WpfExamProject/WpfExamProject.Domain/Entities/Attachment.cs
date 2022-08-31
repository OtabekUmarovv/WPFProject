using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfExamProject.Domain.Commons;

namespace WpfExamProject.Domain.Entities
{
    public class Attachment : Auditable
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }
}
