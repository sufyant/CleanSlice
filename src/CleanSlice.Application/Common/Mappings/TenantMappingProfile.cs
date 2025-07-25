using AutoMapper;
using CleanSlice.Application.Features.Tenants.Commands.CreateTenant;
using CleanSlice.Application.Features.Tenants.DTOs;
using CleanSlice.Shared.Contracts.Tenants.Requests;
using CleanSlice.Shared.Contracts.Tenants.Responses;

namespace CleanSlice.Application.Common.Mappings;

public class TenantMappingProfile : Profile
{
    public TenantMappingProfile()
    {
        // DTO -> Response
        CreateMap<TenantDto, TenantResponse>();
        
        // Request -> Command
        CreateMap<CreateTenantRequest, CreateTenantCommand>();
    }
}
