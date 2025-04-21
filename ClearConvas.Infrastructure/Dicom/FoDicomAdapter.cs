using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearConvas.Application.Interfaces;
using ClearConvas.Core.Models;
using FellowOakDicom.Network.Client;
using FellowOakDicom.Network;
using FellowOakDicom;
using Microsoft.Extensions.Configuration;

namespace ClearConvas.Infrastructure.Dicom
{
    public class FoDicomAdapter : Application.Interfaces.IDicomService
    {
        private readonly string _clearCanvasHost;
        private readonly int _clearCanvasPort;
        private readonly string _clearCanvasAet;
        private readonly string _clientAet;

        public FoDicomAdapter(IConfiguration configuration)
        {
            // خواندن تنظیمات از appsettings.json
            _clearCanvasHost = configuration["DicomSettings:ClearCanvasHost"];
            _clearCanvasPort = int.Parse(configuration["DicomSettings:ClearCanvasPort"]);
            _clearCanvasAet = configuration["DicomSettings:ClearCanvasAet"];
            _clientAet = configuration["DicomSettings:ClientAet"];
        }

        public async Task<List<DicomEntity>> QueryStudies(string patientId = null, string studyUid = null)
        {
            var datasets = new List<DicomEntity>();

            var cFindRequest = new DicomCFindRequest(DicomQueryRetrieveLevel.Study)
            {
                Dataset = new DicomDataset
                {
                    { DicomTag.QueryRetrieveLevel, "STUDY" },
                    { DicomTag.PatientID, patientId ?? "" },
                    { DicomTag.StudyInstanceUID, studyUid ?? "" }
                }
            };

            cFindRequest.OnResponseReceived = (request, response) =>
            {
                if (response.Status == DicomStatus.Success && response.Dataset != null)
                {
                    datasets.Add(new DicomEntity
                    {
                        StudyInstanceUID = response.Dataset.GetString(DicomTag.StudyInstanceUID),
                        PatientID = response.Dataset.GetString(DicomTag.PatientID),
                        PatientName = response.Dataset.GetString(DicomTag.PatientName),
                        StudyDate = response.Dataset.GetString(DicomTag.StudyDate)
                    });
                }
            };

            var client = DicomClientFactory.Create(_clearCanvasHost, _clearCanvasPort, false, _clientAet, _clearCanvasAet);
            await client.AddRequestAsync(cFindRequest);
            await client.SendAsync();

            return datasets;
        }

        public async Task<bool> StoreDicom(string filePath)
        {
            // منطق ذخیره فایل DICOM
            return true; // تست اولیه
        }

    }
}
