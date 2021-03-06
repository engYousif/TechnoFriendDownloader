﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TechnoFriend
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="technodb")]
	public partial class DataClasses1DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertDefine(Define instance);
    partial void UpdateDefine(Define instance);
    partial void DeleteDefine(Define instance);
    partial void InsertSettingsTable(SettingsTable instance);
    partial void UpdateSettingsTable(SettingsTable instance);
    partial void DeleteSettingsTable(SettingsTable instance);
    #endregion
		
		public DataClasses1DataContext() : 
				base(global::TechnoFriend.Properties.Settings.Default.technodbConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Define> Defines
		{
			get
			{
				return this.GetTable<Define>();
			}
		}
		
		public System.Data.Linq.Table<SettingsTable> SettingsTables
		{
			get
			{
				return this.GetTable<SettingsTable>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Define")]
	public partial class Define : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Server;
		
		private string _Server_Name;
		
		private string _File_Path;
		
		private System.Nullable<int> _Port;
		
		private string _Username;
		
		private string _Password;
		
		private System.Nullable<long> _File_Size;
		
		private System.Nullable<System.DateTime> _Last_Modified;
		
		private System.Nullable<int> _Speed;
		
		private string _Unit;
		
		private System.Nullable<bool> _isChecked;
		
		private double _progress;
		
		private bool _isStarted;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnServerChanging(string value);
    partial void OnServerChanged();
    partial void OnServer_NameChanging(string value);
    partial void OnServer_NameChanged();
    partial void OnFile_PathChanging(string value);
    partial void OnFile_PathChanged();
    partial void OnPortChanging(System.Nullable<int> value);
    partial void OnPortChanged();
    partial void OnUsernameChanging(string value);
    partial void OnUsernameChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnFile_SizeChanging(System.Nullable<long> value);
    partial void OnFile_SizeChanged();
    partial void OnLast_ModifiedChanging(System.Nullable<System.DateTime> value);
    partial void OnLast_ModifiedChanged();
    partial void OnSpeedChanging(System.Nullable<int> value);
    partial void OnSpeedChanged();
    partial void OnUnitChanging(string value);
    partial void OnUnitChanged();
    partial void OnisCheckedChanging(System.Nullable<bool> value);
    partial void OnisCheckedChanged();
    partial void OnprogressChanging(double value);
    partial void OnprogressChanged();
    partial void OnisStartedChanging(bool value);
    partial void OnisStartedChanged();
    #endregion
		
		public Define()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="server", Storage="_Server", DbType="NVarChar(50)")]
		public string Server
		{
			get
			{
				return this._Server;
			}
			set
			{
				if ((this._Server != value))
				{
					this.OnServerChanging(value);
					this.SendPropertyChanging();
					this._Server = value;
					this.SendPropertyChanged("Server");
					this.OnServerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Server_Name", DbType="NVarChar(200)")]
		public string Server_Name
		{
			get
			{
				return this._Server_Name;
			}
			set
			{
				if ((this._Server_Name != value))
				{
					this.OnServer_NameChanging(value);
					this.SendPropertyChanging();
					this._Server_Name = value;
					this.SendPropertyChanged("Server_Name");
					this.OnServer_NameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_File_Path", DbType="NVarChar(500)")]
		public string File_Path
		{
			get
			{
				return this._File_Path;
			}
			set
			{
				if ((this._File_Path != value))
				{
					this.OnFile_PathChanging(value);
					this.SendPropertyChanging();
					this._File_Path = value;
					this.SendPropertyChanged("File_Path");
					this.OnFile_PathChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Port", DbType="Int")]
		public System.Nullable<int> Port
		{
			get
			{
				return this._Port;
			}
			set
			{
				if ((this._Port != value))
				{
					this.OnPortChanging(value);
					this.SendPropertyChanging();
					this._Port = value;
					this.SendPropertyChanged("Port");
					this.OnPortChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Username", DbType="NVarChar(50)")]
		public string Username
		{
			get
			{
				return this._Username;
			}
			set
			{
				if ((this._Username != value))
				{
					this.OnUsernameChanging(value);
					this.SendPropertyChanging();
					this._Username = value;
					this.SendPropertyChanged("Username");
					this.OnUsernameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Password", DbType="NVarChar(50)")]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_File_Size", DbType="BigInt")]
		public System.Nullable<long> File_Size
		{
			get
			{
				return this._File_Size;
			}
			set
			{
				if ((this._File_Size != value))
				{
					this.OnFile_SizeChanging(value);
					this.SendPropertyChanging();
					this._File_Size = value;
					this.SendPropertyChanged("File_Size");
					this.OnFile_SizeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Last_Modified", DbType="Date")]
		public System.Nullable<System.DateTime> Last_Modified
		{
			get
			{
				return this._Last_Modified;
			}
			set
			{
				if ((this._Last_Modified != value))
				{
					this.OnLast_ModifiedChanging(value);
					this.SendPropertyChanging();
					this._Last_Modified = value;
					this.SendPropertyChanged("Last_Modified");
					this.OnLast_ModifiedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Speed", DbType="Int")]
		public System.Nullable<int> Speed
		{
			get
			{
				return this._Speed;
			}
			set
			{
				if ((this._Speed != value))
				{
					this.OnSpeedChanging(value);
					this.SendPropertyChanging();
					this._Speed = value;
					this.SendPropertyChanged("Speed");
					this.OnSpeedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Unit", DbType="NVarChar(50)")]
		public string Unit
		{
			get
			{
				return this._Unit;
			}
			set
			{
				if ((this._Unit != value))
				{
					this.OnUnitChanging(value);
					this.SendPropertyChanging();
					this._Unit = value;
					this.SendPropertyChanged("Unit");
					this.OnUnitChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_isChecked")]
		public System.Nullable<bool> isChecked
		{
			get
			{
				return this._isChecked;
			}
			set
			{
				if ((this._isChecked != value))
				{
					this.OnisCheckedChanging(value);
					this.SendPropertyChanging();
					this._isChecked = value;
					this.SendPropertyChanged("isChecked");
					this.OnisCheckedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_progress")]
		public double progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				if ((this._progress != value))
				{
					this.OnprogressChanging(value);
					this.SendPropertyChanging();
					this._progress = value;
					this.SendPropertyChanged("progress");
					this.OnprogressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_isStarted")]
		public bool isStarted
		{
			get
			{
				return this._isStarted;
			}
			set
			{
				if ((this._isStarted != value))
				{
					this.OnisStartedChanging(value);
					this.SendPropertyChanging();
					this._isStarted = value;
					this.SendPropertyChanged("isStarted");
					this.OnisStartedChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SettingsTable")]
	public partial class SettingsTable : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _path;
		
		private System.Nullable<System.DateTime> _dueDate;
		
		private System.Nullable<System.DateTime> _startDate;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnpathChanging(string value);
    partial void OnpathChanged();
    partial void OndueDateChanging(System.Nullable<System.DateTime> value);
    partial void OndueDateChanged();
    partial void OnstartDateChanging(System.Nullable<System.DateTime> value);
    partial void OnstartDateChanged();
    #endregion
		
		public SettingsTable()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_path", DbType="NVarChar(250)")]
		public string path
		{
			get
			{
				return this._path;
			}
			set
			{
				if ((this._path != value))
				{
					this.OnpathChanging(value);
					this.SendPropertyChanging();
					this._path = value;
					this.SendPropertyChanged("path");
					this.OnpathChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dueDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> dueDate
		{
			get
			{
				return this._dueDate;
			}
			set
			{
				if ((this._dueDate != value))
				{
					this.OndueDateChanging(value);
					this.SendPropertyChanging();
					this._dueDate = value;
					this.SendPropertyChanged("dueDate");
					this.OndueDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_startDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> startDate
		{
			get
			{
				return this._startDate;
			}
			set
			{
				if ((this._startDate != value))
				{
					this.OnstartDateChanging(value);
					this.SendPropertyChanging();
					this._startDate = value;
					this.SendPropertyChanged("startDate");
					this.OnstartDateChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
