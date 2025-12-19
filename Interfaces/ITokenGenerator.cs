using StrivoLabsTest.Data.DTOs;
using StrivoLabsTest.Data.DTOs.Services;

namespace StrivoLabsTest.Interfaces
{
    public interface ITokenGenerator
    {
        TokenResponse GenerateJwtToken(ServiceModel serviceModel);
    }
}
