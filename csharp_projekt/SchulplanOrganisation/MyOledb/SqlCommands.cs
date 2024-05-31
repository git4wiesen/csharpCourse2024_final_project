using System.Data.OleDb;

namespace SchulplanOrganisation.MyOledb
{
    public static class SqlCommands
    {
        public static OleDbCommand CreateCommandSelectIdAfterInsert(this OleDbConnection conn, OleDbTransaction? transaction = null)
        {
            OleDbCommand cmdGetIdAfterInsert = conn.CreateCommand();
            if(transaction is not null)
            {
                cmdGetIdAfterInsert.Transaction = transaction;
            }
            cmdGetIdAfterInsert.CommandText = "Select @@Identity";
            return cmdGetIdAfterInsert;
        }
    }
}
