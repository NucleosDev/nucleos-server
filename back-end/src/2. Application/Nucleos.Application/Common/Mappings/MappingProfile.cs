using AutoMapper;
using Nucleos.Application.Features.Blocos.DTOs;
using Nucleos.Application.Features.Listas.Commands;
using Nucleos.Application.Features.Listas.DTOs;
using Nucleos.Application.Features.ItensLista.DTOs;
using Nucleos.Application.Features.Nucleos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ========== BLOCOS ==========
        CreateMap<Bloco, BlocoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NucleoId, opt => opt.MapFrom(src => src.NucleoId))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo))
            .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo ?? string.Empty))
            .ForMember(dest => dest.Posicao, opt => opt.MapFrom(src => src.Posicao))
            .ForMember(dest => dest.Configuracoes, opt => opt.MapFrom(src => src.Configuracoes ?? "{}"))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        // ========== NÚCLEOS ==========
        CreateMap<Nucleo, NucleoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo))
            .ForMember(dest => dest.CorDestaque, opt => opt.MapFrom(src => src.CorDestaque))
            .ForMember(dest => dest.ImagemCapa, opt => opt.MapFrom(src => src.ImagemCapa))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<Nucleo, NucleoListDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo))
            .ForMember(dest => dest.CorDestaque, opt => opt.MapFrom(src => src.CorDestaque))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        
        CreateMap<Nucleo, NucleoDetailDto>()
            .IncludeBase<Nucleo, NucleoDto>();
        
        // ========== LISTAS ==========
        CreateMap<Lista, ListaDto>();
        CreateMap<CreateListaCommand, Lista>();
        
        // ========== ITENS DE LISTA ==========
        CreateMap<ItemLista, ItemListaDto>();
        
        // ========== CATEGORIAS ==========
        CreateMap<Categoria, CategoriaDto>();
    }
}