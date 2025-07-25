using AutoMapper;
using CleanSlice.Application.Features.Tenants.DTOs;
using CleanSlice.Shared.Contracts.Tenants.Responses;

namespace CleanSlice.Application.Common.Mappings;

public class TenantMappingProfile : Profile
{
    public TenantMappingProfile()
    {
        // DTO -> Response
        CreateMap<TenantDto, TenantResponse>();
    }
}
