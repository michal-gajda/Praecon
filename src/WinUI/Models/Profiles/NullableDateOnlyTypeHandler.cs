namespace Praecon.WinUI.Models.Profiles;

using System.Data;

using Dapper;

internal sealed class NullableDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly?>
{
    public override DateOnly? Parse(object value)
    {
        if (value is DBNull)
        {
            return default;
        }

        DateTime dateTime = (DateTime)value;

        return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly? value)
    {
        parameter.DbType = DbType.DateTime;

        if (value is null)
        {
            parameter.Value = DBNull.Value;

            return;
        }

        parameter.Value = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, hour: 0, minute: 0, second: 0, millisecond: 0, DateTimeKind.Utc);
    }
}
