// src/2. Application/Nucleos.Application/Features/Nucleos/DTOs/NucleoDetailDto.cs
using Nucleos.Application.Features.Blocos.DTOs;

namespace Nucleos.Application.Features.Nucleos.DTOs;

public class NucleoDetailDto : NucleoDto
{
    public List<BlocoDto> Blocos { get; set; } = new List<BlocoDto>();
}