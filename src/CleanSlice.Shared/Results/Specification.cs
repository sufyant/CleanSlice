using System.Linq.Expressions;
using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Shared.Results;

public abstract class Specification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        return ToExpression().Compile()(entity);
    }

    public Specification<T> And(ISpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    public Specification<T> Or(ISpecification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }

    public Specification<T> Not()
    {
        return new NotSpecification<T>(this);
    }

    public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
    {
        return specification.ToExpression();
    }
}

internal class AndSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));
        var leftInvoke = Expression.Invoke(leftExpression, parameter);
        var rightInvoke = Expression.Invoke(rightExpression, parameter);
        var andExpression = Expression.AndAlso(leftInvoke, rightInvoke);

        return Expression.Lambda<Func<T, bool>>(andExpression, parameter);
    }
}

internal class OrSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var parameter = Expression.Parameter(typeof(T));
        var leftInvoke = Expression.Invoke(leftExpression, parameter);
        var rightInvoke = Expression.Invoke(rightExpression, parameter);
        var orExpression = Expression.OrElse(leftInvoke, rightInvoke);

        return Expression.Lambda<Func<T, bool>>(orExpression, parameter);
    }
}

internal class NotSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _specification;

    public NotSpecification(ISpecification<T> specification)
    {
        _specification = specification;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = _specification.ToExpression();
        var parameter = Expression.Parameter(typeof(T));
        var invoke = Expression.Invoke(expression, parameter);
        var notExpression = Expression.Not(invoke);

        return Expression.Lambda<Func<T, bool>>(notExpression, parameter);
    }
}
