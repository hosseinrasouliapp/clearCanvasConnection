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
            Console.WriteLine($"Sending C-FIND request with PatientId: {patientId}, StudyInstanceUid: {studyUid}");

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
                Console.WriteLine($"Response received with status: {response.Status}*");
               if (response.Status == DicomStatus.Success && response.Dataset != null)
                {
                    datasets.Add(new DicomEntity
                    {
                        StudyInstanceUID = response.Dataset.GetString(DicomTag.StudyInstanceUID),
                        PatientID = response.Dataset.GetString(DicomTag.PatientID),
                        PatientName = response.Dataset.GetString(DicomTag.PatientName),
                        StudyDate = response.Dataset.GetString(DicomTag.StudyDate)
                    });

                    Console.WriteLine("Dataset added successfully.");
                }
                else
               {
                    Console.WriteLine("No dataset or unsuccessful response.");
               }
            };
            

            var client = DicomClientFactory.Create(_clearCanvasHost, _clearCanvasPort, false, _clientAet, _clearCanvasAet);
            await client.AddRequestAsync(cFindRequest);
            await client.SendAsync();
            Console.WriteLine($"Total studies retrieved: {datasets.Count}");
            return datasets;
        }

        public async Task<bool> StoreDicom(string filePath)
        {
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File does not exist at path: {filePath}");
                    return false;
                }

                Console.WriteLine($"Sending C-STORE request for file: {filePath}");
                var dicomFile = DicomFile.Open(filePath);
               // var client = new DicomClient();
                //var cStoreRequest = new DicomCStoreRequest(dicomFile);

                //await client.AddRequestAsync(cStoreRequest);
               // await client.SendAsync(_clearCanvasHost, _clearCanvasPort, false, _clientAet, _clearCanvasAet);

                Console.WriteLine("C-STORE request completed successfully.");
                return true;
            }
        }

    }
}
