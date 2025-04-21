using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearConvas.Application.Queries.QueriesModel
{
    public class DicomStudyQuery
    {
        public string PatientId { get; set; }
        public string StudyInstanceUid { get; set; }
    }
}
