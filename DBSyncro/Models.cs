using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace DBSyncro
{
    public class TableComparisonState
    {
        [SetsRequiredMembers]
        public TableComparisonState(string tableName)
        {
            TableName = tableName;
        }
        public required string TableName { get; set; }
        public bool AreEquals
        {
            get
            {
                return
                    ExistsInBothDB &&
                    (MissingColumns is null || MissingColumns.Count == 0 ) &&
                    (ColumnsDifferences is null || ColumnsDifferences.Values?.Count == 0);
            }
        }
        public bool ExistsInBothDB { get; set; }
        public List<string>? MissingColumns { get; set; }
        public Dictionary<string, ColumnDifferences>? ColumnsDifferences { get; set; }
    }

    public class ColumnDifferences
    {
        [SetsRequiredMembers]
        public ColumnDifferences(string propertyName)
        {
            PropertyName = propertyName;
        }
        public required string PropertyName { get; set; }
        public List<string>? differences { get; set; }
    }
}
