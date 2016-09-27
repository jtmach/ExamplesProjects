namespace DatabaseQuery
{
  using System;
  using System.Data;
  using System.Data.OleDb;
  using System.Diagnostics;
  using System.Windows;

  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      //RunAQueryAndItterateResults("SELECT TOP 100 * FROM DBC.Tables");

      DataTable resultsTable = this.RunAQueryAndReturnADatatable("SELECT TOP 100 * FROM DBC.Tables");
      MessageBox.Show(string.Format("Rows {0} returned", resultsTable.Rows.Count));
    }

    public void RunAQueryAndItterateResults(string sql)
    {
      string delimiter = ",";

      OleDbConnection connection = new OleDbConnection(Properties.Settings.Default.TeradataConnection);
      OleDbCommand command = new OleDbCommand(sql);
      command.Connection = connection;
      command.CommandTimeout = 0;

      try
      {
        connection.Open();
        OleDbDataReader dataReader = command.ExecuteReader();

        //Write the fieldName
        for (int i = 0; i < dataReader.FieldCount; i++)
        {
          //If it's not the first column put a comma in front of the field name
          Debug.Write((i == 0 ? string.Empty : delimiter) + dataReader.GetName(i));
        }

        while (dataReader.Read())
        {
          for (int i = 0; i < dataReader.FieldCount; i++)
          {
            Debug.Write((i == 0 ? string.Empty : delimiter) + dataReader[i]);
          }

          Debug.WriteLine(string.Empty); //Put a carrage return at the end of each line 
        }

        dataReader.Close();
      }
      catch (Exception exception)
      {
        Debug.WriteLine(exception.Message);
      }
      finally
      {
        if (connection.State == ConnectionState.Open)
        {
          connection.Close();
        }
      }
    }

    public DataTable RunAQueryAndReturnADatatable(string sql)
    {
      DataTable dataTable = new DataTable();

      OleDbConnection connection = new OleDbConnection(Properties.Settings.Default.TeradataConnection);
      OleDbCommand command = new OleDbCommand(sql);
      command.Connection = connection;
      command.CommandTimeout = 0;

      try
      {
        connection.Open();
        dataTable.Load(command.ExecuteReader());
      }
      catch (Exception exception)
      {
        Debug.WriteLine(exception.Message);
      }
      finally
      {
        if (connection.State == ConnectionState.Open)
        {
          connection.Close();
        }
      }

      return dataTable;
    }
  }
}
