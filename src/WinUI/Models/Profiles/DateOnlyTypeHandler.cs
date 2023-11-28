namespace Praecon.WinUI.Models.Profiles;

using System.Data;
using Dapper;

internal sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        if (value is DBNull)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var dateTime = (DateTime)value;

        return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.DateTime;
        parameter.Value = new DateTime(value.Year, value.Month, value.Day, hour: 0, minute: 0, second: 0, millisecond: 0, DateTimeKind.Utc);
    }
}
