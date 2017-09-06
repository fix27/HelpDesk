using System.Linq.Expressions;

namespace HelpDesk.Common.Helpers
{
    public static class ExpressionExtensions
    {
        public static Expression<TDelegate> AndAlso<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right)
        {
            ParameterExpression param = left.Parameters[0];
            if (ReferenceEquals(param, right.Parameters[0]))
            {
                return Expression.Lambda<TDelegate>(Expression.AndAlso(left.Body, right.Body), param);
            }
            return Expression.Lambda<TDelegate>(Expression.AndAlso(left.Body, Expression.Invoke(right, param)), param);

        }

        public static Expression<TDelegate> OrElse<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right)
        {
            ParameterExpression param = left.Parameters[0];
            if (ReferenceEquals(param, right.Parameters[0]))
            {
                return Expression.Lambda<TDelegate>(Expression.AndAlso(left.Body, right.Body), param);
            }
            return Expression.Lambda<TDelegate>(Expression.OrElse(left.Body, Expression.Invoke(right, param)), param);
        }
    }
}
