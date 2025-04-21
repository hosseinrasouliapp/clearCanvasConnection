using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearConvas.Application.Commands.CommandModels;
using ClearConvas.Application.Interfaces;

namespace ClearConvas.Application.Commands.CommandHandlers
{
    public class StoreDicomHandler
    {
        private readonly IDicomService _dicomService;

        public StoreDicomHandler(IDicomService dicomService)
        {
            _dicomService = dicomService;
        }

        public async Task<bool> Handle(StoreDicomCommand command)
        {
            // منطق ذخیره فایل DICOM
            Console.WriteLine($"Storing DICOM file at path: {command.FilePath}");
            return await _dicomService.StoreDicom(command.FilePath);
        }
    }
}
