using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearConvas.Core.Models;

namespace ClearConvas.Application.Interfaces
{
    public interface IDicomService
    {
        Task<List<DicomEntity>> QueryStudies(string patientId = null, string studyUid = null);
        Task<bool> StoreDicom(string filePath);
    }
}
