using ProjectTracker.PdfReport.ObjectStorage.Dtos.FieldBuilder;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace ProjectTracker.PdfReport.ObjectStorage.FieldBuilder;

public abstract class BaseFieldsBuilder<T>
{
	protected List<FieldDto<T>> _fields = [];
	protected FieldBuilderStatus _status = FieldBuilderStatus.Default;

	public virtual BaseFieldsBuilder<T> Add(params Expression<Func<T?, object?>>[] selectors)
	{
		if (!(_status is FieldBuilderStatus.Default || _status is FieldBuilderStatus.Fields))
		{
			throw new InvalidOperationException("Поля были уже добавлены");
		}

		var newFields = selectors.Select(
				x =>
				{
					var memberExpression = x.Body as MemberExpression;
					var propertyInfo = memberExpression?.Member as PropertyInfo;
					var name = propertyInfo?.GetCustomAttribute<DisplayAttribute>()?.Name ?? "Ошибка";
					var type = propertyInfo?.PropertyType;

					return new FieldDto<T>
					{
						Name = name,
						Selector = x.ToNullSafe().Compile(),
						FieldType = type ?? typeof(object),
					};
				}
			);

		_fields.AddRange(newFields.ToList());

		_status = FieldBuilderStatus.Fields;

		return this;
	}

	public virtual BaseFieldsBuilder<T> AddWithAfterProcess<AfterProcessType>(Expression<Func<T, AfterProcessType?>> selector, Func<AfterProcessType?, string?> after)
	{
		if (!(_status is FieldBuilderStatus.Default || _status is FieldBuilderStatus.Fields))
		{
			throw new InvalidOperationException("Поля были уже добавлены");
		}

		var memberExpression = selector.Body as MemberExpression;
		var propertyInfo = memberExpression?.Member as PropertyInfo;
		var name = propertyInfo?.GetCustomAttribute<DisplayAttribute>()?.Name ?? "Ошибка";

		var field = new FieldDto<T>
		{
			Name = name,
			Selector = selector.ToNullSafe().Compile(),
			FieldType = typeof(AfterProcessType),
			AfterProcess = value => after((AfterProcessType?)value)
		};

		_fields.Add(field);

		_status = FieldBuilderStatus.Fields;

		return this;
	}

	public ManyFieldsBuilder<T> AsManyFieldsBuilder()
	{
		return this as ManyFieldsBuilder<T> ?? throw new InvalidCastException("Не удалось преобразовать fieldsBuilder");
	}

	public FieldsBuilder<T> AsFieldsBuilder()
	{
		return this as FieldsBuilder<T> ?? throw new InvalidCastException("Не удалось преобразовать fieldsBuilder");
	}
}
