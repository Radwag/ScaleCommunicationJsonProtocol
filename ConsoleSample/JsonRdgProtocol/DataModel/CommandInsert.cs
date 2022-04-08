using System.Collections.Generic;
using System.Text;

namespace ConsoleSample.JsonRdgProtocol.DataModel
{
    public class CommandInsert
    {
        private string Command = "DbInsert";
        private string Table { get; set; }
        private List<ObjectModel> ObjectsList = new List<ObjectModel>();

        public void SetDestinationTable(string table)
        {
            Table = table;
        }

        public void AddObject(string dbName, object value)
        {
            ObjectModel @object = new ObjectModel
            {
                DbName = dbName,
                Value = value
            };
            ObjectsList.Add(@object);
        }

        public string BuildQuery()
        {
            StringBuilder query = new StringBuilder();
            query.Append("{");
            query.Append("\"Command\":\"" + Command + "\"");
            query.Append(", \"Table\":\"" + Table + "\"");
            foreach (var objectModel in ObjectsList)
            {
                query.Append(", \"" + objectModel.DbName + "\":\"" + objectModel.Value + "\"");
            }

            query.Append("}");
            return query.ToString();
        }
    }
}