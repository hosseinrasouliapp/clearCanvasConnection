using ClearConvas.Api.DTOs;
using ClearConvas.Application.Commands.CommandHandlers;
using ClearConvas.Application.Commands.CommandModels;
using ClearConvas.Application.Queries.QueriesModel;
using ClearConvas.Application.Queries.QueryHandlers;
using Microsoft.AspNetCore.Mvc;


namespace ClearConvas.Api.Controllers
{
    [Route("dicom-web")]
    [ApiController]
    
    public class DicomWebController : ControllerBase
    {
        private readonly QueryStudiesHandler _queryStudiesHandler;
        private readonly StoreDicomHandler _storeDicomHandler;

        public DicomWebController(QueryStudiesHandler queryStudiesHandler, StoreDicomHandler storeDicomHandler)
        {
            _queryStudiesHandler = queryStudiesHandler;
            _storeDicomHandler = storeDicomHandler;
        }

        [HttpGet("studies")]
        public async Task<IActionResult> QueryStudies([FromQuery] string patientId = null, [FromQuery] string studyInstanceUid = null)
        {
            var query = new DicomStudyQuery { PatientId = patientId, StudyInstanceUid = studyInstanceUid };
            var studies = await _queryStudiesHandler.Handle(query);

            var response = studies.Select(s => new DicomStudyDto
            {
                StudyInstanceUID = s.StudyInstanceUID,
                PatientID = s.PatientID,
                PatientName = s.PatientName,
                StudyDate = s.StudyDate
            });
            Console.WriteLine($"QueryStudies: {studies}");
            return Ok(response);
        }

        [HttpPost("store")]
        public async Task<IActionResult> StoreDicom([FromBody] StoreDicomCommand command)
        {
            var result = await _storeDicomHandler.Handle(command);
            if (result) return Ok("DICOM stored successfully.");
            return BadRequest("Failed to store DICOM.");
        }
    }
}
