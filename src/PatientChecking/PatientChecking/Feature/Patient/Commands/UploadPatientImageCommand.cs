using MediatR;
using Microsoft.AspNetCore.Http;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Services.Image;
using PatientChecking.Services.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientChecking.Feature.Patient.Commands
{
    public class UploadPatientImageCommand : IRequest<ImageUploadingStatus>
    {
        public int PatientId { get; set; }
        public IFormFile FormFile { get; set; }
    }

    public class UploadPatientImageCommandHandler : IRequestHandler<UploadPatientImageCommand, ImageUploadingStatus>
    {
        private readonly IPatientService _patientService;
        private readonly IImageService _imageService;
        public UploadPatientImageCommandHandler(IPatientService patientService, IImageService imageService)
        {
            _patientService = patientService;
            _imageService = imageService;
        }

        public async Task<ImageUploadingStatus> Handle(UploadPatientImageCommand request, CancellationToken cancellationToken)
        {
            if (request.FormFile != null)
            {
                if (!_imageService.IsImageFile(request.FormFile))
                {
                    return ImageUploadingStatus.IsNotImageFailed;
                }

                var avatarLink = await _imageService.SaveImage(request.FormFile);

                var result = await _patientService.UploadPatientImage(request.PatientId, avatarLink);

                if (result > 0)
                {
                    return ImageUploadingStatus.UploadImageSuccessfully;
                }
                else
                {
                    return ImageUploadingStatus.UploadImageFailed;
                }
            }

            return ImageUploadingStatus.UploadImageFailed;
        }
    }
}
