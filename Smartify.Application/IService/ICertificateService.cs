using Smartify.Application.DTO.Certificate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface ICertificateService
    {
        Task<IEnumerable<CertificateDTO>> GetAllCertificatesAsync();
        Task<CertificateDTO?> GetCertificateByIdAsync(int id);
        Task<CertificateDTO?> AddCertificateAsync(CreateCertificateDTO certificateDTO);
        Task UpdateCertificateAsync(UpdateCertificateDTO certificateDTO);
        Task DeleteCertificateAsync(int id);
    }
}
