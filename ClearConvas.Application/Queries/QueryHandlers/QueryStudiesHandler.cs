using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearConvas.Application.Interfaces;
using ClearConvas.Application.Queries.QueriesModel;
using ClearConvas.Core.Models;

namespace ClearConvas.Application.Queries.QueryHandlers
{
    public class QueryStudiesHandler
    {
        private readonly IDicomService _dicomService;

        public QueryStudiesHandler(IDicomService dicomService)
        {
            _dicomService = dicomService;
        }

        public async Task<List<DicomEntity>> Handle(DicomStudyQuery query)
        {
            Console.WriteLine($"Querying studies with PatientId: {query.PatientId}, StudyInstanceUid: {query.StudyInstanceUid}");
            var result = await _dicomService.QueryStudies(query.PatientId, query.StudyInstanceUid);
            Console.WriteLine($"Received {result.Count} studies.");
            return result;
        }
    }
}
