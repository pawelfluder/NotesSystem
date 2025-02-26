namespace SharpOperationsProg.Operations.Date;

public interface IDateOperations
{
    bool TryParse(
        string date,
        out DateTime result);

    string DateTimeToString(
        DateTime date);

    string ToYear(string dateString);
    string UderscoreDate(DateTime date);
}