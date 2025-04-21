namespace ClearConvas.Api.DTOs
{
    public class DicomStudyDto
    {
        public string StudyInstanceUID { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string StudyDate { get; set; }
    }
}
