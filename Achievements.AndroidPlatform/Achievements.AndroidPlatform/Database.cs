using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;
using System.IO;
using Android.Database;

namespace Achievements.AndroidPlatform
{
    public class SqlDatabase
    {
        /// <summary>
		/// SQLiteDatabase object sqldTemp to handle SQLiteDatabase.
		/// </summary>
		private SQLiteDatabase sqldTemp;
		/// <summary>
		/// The sSQLquery for query handling.
		/// </summary>
		private string sSQLQuery;
		/// <summary>
		/// The sMessage to hold message.
		/// </summary>
		private string sMessage;
		/// <summary>
		/// The bDBIsAvailable for database is available or not.
		/// </summary>
		private bool bDBIsAvailable;
		/// <summary>
		/// Initializes a new instance of the <see cref="MyDatabaseDemo.MyDatabase"/> class.
		/// </summary>
		public SqlDatabase ()
		{
			sMessage = "";
			bDBIsAvailable = false;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="MyDatabaseDemo.MyDatabase"/> class.
		/// </summary>
		/// <param name='sDatabaseName'>
		/// Pass your database name.
		/// </param>
		public SqlDatabase (string sDatabaseName)
		{
			try {
				sMessage = "";
				bDBIsAvailable = false;
				CreateDatabase (sDatabaseName);
			} catch (SQLiteException ex) {
				sMessage = ex.Message;
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyDatabaseDemo.MyDatabase"/> database available.
		/// </summary>
		/// <value>
		/// <c>true</c> if database available; otherwise, <c>false</c>.
		/// </value>
		public bool DatabaseAvailable {
			get{ return bDBIsAvailable;}
			set{ bDBIsAvailable = value;}
		}
		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public string Message {
			get{ return sMessage;}
			set{ sMessage = value;}
		}
		/// <summary>
		/// Creates the database.
		/// </summary>
		/// <param name='sDatabaseName'>
		/// Pass database name.
		/// </param>
		public void CreateDatabase (string sDatabaseName)
		{
			try {
				sMessage = "";
				string sLocation = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
				string sDB = Path.Combine (sLocation, sDatabaseName);
				bool bIsExists = File.Exists (sDB);		
				if (!bIsExists) {
					sqldTemp = SQLiteDatabase.OpenOrCreateDatabase (sDB, null);
					sSQLQuery = "CREATE TABLE IF NOT EXISTS " +
						"MyTable " +
						"(_id INTEGER PRIMARY KEY AUTOINCREMENT,Name VARCHAR,Age INT,Country VARCHAR);";
					sqldTemp.ExecSQL (sSQLQuery);
					sMessage = "New database is created.";
				} else {
					sqldTemp = SQLiteDatabase.OpenDatabase (sDB, null, DatabaseOpenFlags.OpenReadwrite);
					sMessage = "Database is opened.";
				}
				bDBIsAvailable = true;
			} catch (SQLiteException ex) {
				sMessage = ex.Message;
			}
		}
		/// <summary>
		/// Adds the record.
		/// </summary>
		/// <param name='sName'>
		/// Pass name.
		/// </param>
		/// <param name='iAge'>
		/// Pass age.
		/// </param>
		/// <param name='sCountry'>
		/// Pass country.
		/// </param>
		public void AddRecord (string sName, int iAge, string sCountry)
		{
			try {
				sSQLQuery = "INSERT INTO " +
					"MyTable " +
					"(Name,Age,Country)" +
					"VALUES('" + sName + "'," + iAge + ",'" + sCountry + "');";
				sqldTemp.ExecSQL (sSQLQuery);
				sMessage = "Record is saved.";
			} catch (SQLiteException ex) {
				sMessage = ex.Message;
			}
		}
		/// <summary>
		/// Updates the record.
		/// </summary>
		/// <param name='iId'>
		/// Pass record ID.
		/// </param>
		/// <param name='sName'>
		/// Pass name.
		/// </param>
		/// <param name='iAge'>
		/// Pass age.
		/// </param>
		/// <param name='sCountry'>
		/// Pass country.
		/// </param>
		public void UpdateRecord (int iId, string sName, int iAge, string sCountry)
		{
			try {
				sSQLQuery = "UPDATE MyTable " +
					"SET Name='" + sName + "',Age='" + iAge + "',Country='" + sCountry + "' " +
					"WHERE _id='" + iId + "';";
				sqldTemp.ExecSQL (sSQLQuery);
				sMessage = "Record is updated: " + iId;
			} catch (SQLiteException ex) {
				sMessage = ex.Message;
			}
		}
		/// <summary>
		/// Deletes the record.
		/// </summary>
		/// <param name='iId'>
		/// Pass ID.
		/// </param>
		public void DeleteRecord (int iId)
		{
			try {
				sSQLQuery = "DELETE FROM MyTable " +
					"WHERE _id='" + iId + "';";
				sqldTemp.ExecSQL (sSQLQuery);
				sMessage = "Record is deleted: " + iId;
			} catch (SQLiteException ex) {
				sMessage = ex.Message;
			}
		}
		/// <summary>
		/// Gets the record cursor.
		/// </summary>
		/// <returns>
		/// The record cursor.
		/// </returns>
		public ICursor GetRecordCursor ()
		{
			ICursor icTemp = null;
			try {
				sSQLQuery = "SELECT * FROM MyTable;";
				icTemp = sqldTemp.RawQuery (sSQLQuery, null);
				if (!(icTemp != null)) {
					sMessage = "Record not found.";
				}
			} catch (SQLiteException ex) {
				sMessage = ex.Message;
			}
			return icTemp;
		}
		/// <summary>
		/// Gets the record cursor by search criteria.
		/// </summary>
		/// <returns>
		/// The record cursor.
		/// </returns>
		/// <param name='sColumn'>
		/// column filed of MyTable is Name,Age,Country.
		/// </param>
		/// <param name='sValue'>
		/// Value as user input.
		/// </param>
		public ICursor GetRecordCursor (string sColumn, string sValue)
		{
			ICursor icTemp = null;
			try {
				sSQLQuery = "SELECT * FROM MyTable WHERE " + sColumn + " LIKE '" + sValue + "%';";
				icTemp = sqldTemp.RawQuery (sSQLQuery, null);
				if (!(icTemp != null)) {
					sMessage = "Record not found.";
				}
			} catch (SQLiteException ex) {
				sMessage = ex.Message;
			}
			return icTemp;
		}
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="MyDatabaseDemo.MyDatabase"/> is reclaimed by garbage collection.
		/// </summary>
		~SqlDatabase ()
		{
			try {
				sMessage = "";
				bDBIsAvailable = false;		
				sqldTemp.Close ();		
			} catch (SQLiteException ex) {
				sMessage = ex.Message;
			}
		}
	}
}