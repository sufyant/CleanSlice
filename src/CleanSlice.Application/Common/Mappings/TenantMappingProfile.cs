using AutoMapper;
using AutoMapper;
using CleanSlice.Application.Features.Tenants.Commands.CreateTenant;
using CleanSlice.Application.Features.Tenants.DTOs;
using CleanSlice.Domain.Tenants;
using CleanSlice.Shared.Contracts.Tenants.Requests;
using CleanSlice.Shared.Contracts.Tenants.Responses;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Common.Mappings;

public class TenantMappingProfile : Profile
{
    public TenantMappingProfile()
    {
        // Entity -> DTO
        CreateMap<Tenant, TenantDto>();

        // DTO -> Response
        CreateMap<TenantDto, TenantResponse>();

        // PagedResult mapping
        CreateMap<PagedResult<TenantDto>, PagedResult<TenantResponse>>();

        // Request -> Command
        CreateMap<CreateTenantRequest, CreateTenantCommand>();
    }
}
