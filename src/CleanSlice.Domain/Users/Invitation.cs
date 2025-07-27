using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Domain.Common.ValueObjects;
using CleanSlice.Domain.Users.Events;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Users;

public sealed class Invitation : AuditableTenantEntity
{
    public Email Email { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public Guid InvitedBy { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTimeOffset? UsedAt { get; private set; }
    public Guid? UsedBy { get; private set; }

    public bool IsExpired => DateTimeOffset.UtcNow > ExpiresAt;
    public bool IsValid => !IsUsed && !IsExpired;

    private Invitation() { }

    private Invitation(
        Guid id,
        Guid tenantId,
        string email,
        Guid roleId,
        Guid invitedBy,
        string token,
        DateTimeOffset expiresAt)
    {
        Id = id;
        TenantId = tenantId;
        Email = Email.Create(email);
        RoleId = roleId;
        InvitedBy = invitedBy;
        Token = token;
        ExpiresAt = expiresAt;
        IsUsed = false;
    }

    public static Invitation Create(
        Guid id,
        Guid tenantId,
        string email,
        Guid roleId,
        Guid invitedBy,
        string token,
        DateTimeOffset expiresAt)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ValidationException(nameof(token), "Token cannot be empty");

        if (expiresAt <= DateTimeOffset.UtcNow)
            throw new ValidationException(nameof(expiresAt), "Expiration date must be in the future");

        var invitation = new Invitation(id, tenantId, email, roleId, invitedBy, token, expiresAt);

        invitation.RaiseDomainEvent(new InvitationCreatedDomainEvent(
            id, tenantId, email, roleId, invitedBy, token, expiresAt));

        return invitation;
    }

    public void MarkAsUsed(Guid userId)
    {
        if (IsUsed)
            throw new BusinessRuleViolationException("Invitation has already been used");

        if (IsExpired)
            throw new BusinessRuleViolationException("Cannot use expired invitation");

        IsUsed = true;
        UsedAt = DateTimeOffset.UtcNow;
        UsedBy = userId;

        RaiseDomainEvent(new InvitationUsedDomainEvent(Id, TenantId, Email.Value, userId));
    }

    public void Extend(DateTimeOffset newExpirationDate)
    {
        if (IsUsed)
            throw new BusinessRuleViolationException("Cannot extend used invitation");

        if (newExpirationDate <= DateTimeOffset.UtcNow)
            throw new ValidationException(nameof(newExpirationDate), "New expiration date must be in the future");

        ExpiresAt = newExpirationDate;

        RaiseDomainEvent(new InvitationExtendedDomainEvent(Id, TenantId, Email.Value, newExpirationDate));
    }
}
