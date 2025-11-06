using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;
using System.Linq.Expressions;

namespace ProjectTracker.PdfReport.ObjectStorage.FieldBuilder;

public static class ExpressionUtils
{
	public static Expression<Func<T, object?>> ToNullSafe<T>(this Expression<Func<T, object?>> expression)
	{
		return GetNullableSafeExpression(expression);
	}

	public static Expression<Func<T, object?>> ToNullSafe<T, AfterProcessType>(this Expression<Func<T, AfterProcessType?>> expression)
	{
		return GetNullableSafeExpression(expression);
	}

	private static Expression<Func<T, object?>> GetNullableSafeExpression<T>(Expression<Func<T, object?>> expression)
	{
		var parametr = expression.Parameters.First();

		var result = Expression
			.Lambda<Func<T, object?>>(
				Expression.Convert(
					MakeNullSafe(expression.Body, parametr), typeof(object)),
				parametr);

		return result;
	}

	private static Expression<Func<T, object?>> GetNullableSafeExpression<T, AfterProcessType>(Expression<Func<T, AfterProcessType?>> expression)
	{
		var parametr = expression.Parameters.First();

		var result = Expression
			.Lambda<Func<T, object?>>(
				Expression.Convert(
					MakeNullSafe(expression.Body, parametr), typeof(object)),
				parametr);

		return result;
	}

	private static Expression MakeNullSafe(Expression expression, ParameterExpression root)
	{
		if (expression is MemberExpression member)
		{
			var parent = MakeNullSafe(member.Expression!, root);

			if (member.Expression == root)
			{
				return member;
			}

			return Expression.Condition(
				Expression.Equal(parent, Expression.Constant(null)),
				Expression.Constant(null, member.Type),
				Expression.MakeMemberAccess(parent, member.Member)
			);
		}

		return expression;
	}
}