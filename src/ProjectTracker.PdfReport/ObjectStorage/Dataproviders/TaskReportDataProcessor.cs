using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;
using ProjectTracker.PdfReport.ObjectStorage.FieldBuilder;

namespace ProjectTracker.PdfReport.ObjectStorage.Dataproviders;

public class TaskReportDataProcessor
{
	public TaskReportInputDto Process(TaskReportDto _taskReportDto)
	{
		return new TaskReportInputDto
		{
			TaskId = _taskReportDto.Id,

			TaskProperties = new FieldsBuilder<TaskReportDto>()
				.Add(
					x => x.Project.Name,
					x => x.Group.Name
				)
				.AsFieldsBuilder()
				.BuildWithPrimitiveFields(_taskReportDto),

			ProjectManagerProperties = new FieldsBuilder<TaskReportEmployeeDto>()
				.BuildWithPrimitiveFields(_taskReportDto.Project.ProjectManager),

			ManagerProperties = new FieldsBuilder<TaskReportEmployeeDto>()
				.BuildWithPrimitiveFields(_taskReportDto.Project.Manager),

			Observers = new ManyFieldsBuilder<TaskReportEmployeeDto>()
				.BuildWithPrimitiveFields(_taskReportDto.Observers),

			Performers = new ManyFieldsBuilder<TaskReportEmployeeDto>()
				.BuildWithPrimitiveFields(_taskReportDto.Performers)
		};
	}
}
