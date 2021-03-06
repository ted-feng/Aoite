﻿using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Aoite.Data
{
    /// <summary>
    /// 基于 Miscsoft SQL Server Compact 的数据源单元测试管理器。
    /// </summary>
    public class MsCeTestManager : TestManagerBase
    {
        static readonly string DefaultDirectory = GA.FullPath("_databases");
        static MsCeTestManager()
        {
            GA.IO.DeleteDirectory(DefaultDirectory).Wait();
            AppDomain.CurrentDomain.UnhandledException += (ss, ee) => { GA.IO.DeleteDirectory(DefaultDirectory).Wait(); };
        }
        /// <summary>
        /// 获取数据源路径。
        /// </summary>
        public string DatabasePath { get; private set; }

        /// <summary>
        /// 随机的数据源路径，初始化一个 <see cref="Aoite.Data.MsCeTestManager"/> 类的新实例。
        /// </summary>
        public MsCeTestManager() : this(Path.Combine(DefaultDirectory, Guid.NewGuid().ToString() + ".sdf")) { }

        /// <summary>
        /// 随机的数据源路径，初始化一个 <see cref="Aoite.Data.MsCeTestManager"/> 类的新实例。
        /// </summary>
        /// <param name="databasePath">数据源路径。</param>
        public MsCeTestManager(string databasePath)
        {
            if(string.IsNullOrWhiteSpace(databasePath)) throw new ArgumentNullException(nameof(databasePath));
            this.DatabasePath = databasePath;
            GA.IO.CreateDirectory(Path.GetDirectoryName(databasePath));
            var provider = new SqlCeEngineProvider(this.DatabasePath, null);
            provider.CreateDatabase();
            var engine = new DbEngine(provider);
            this.Engine = engine;
            this.Engine.Executing += _Engine_Executing;

        }

        void _Engine_Executing(object sender, ExecutingEventArgs e)
        {
            if(e.ExecuteType != ExecuteType.NoQuery) return;
            var dbCommand = e.DbCommand;
            dbCommand.CommandText = ReplaceVarchar(dbCommand.CommandText);
        }

        internal static readonly Regex RegexReplaceVarchar = new Regex(@"\bvarchar\b", RegexOptions.Multiline | RegexOptions.Compiled);
        internal static readonly Regex RegexReplaceMax = new Regex(@"\(max\)", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// 由于 Miscsoft SQL Server Compact 不支持 varchar 数据类型和 MAX 长度，此方法将所有 varchar 转换为 nvarchar 和 2000 长度。
        /// </summary>
        /// <param name="sql">SQL 脚本。</param>
        /// <returns>新的 SQL 脚本。</returns>
        public static string ReplaceVarchar(string sql)
        {
            return RegexReplaceMax.Replace(RegexReplaceVarchar.Replace(sql, "nvarchar"), "(2000)");
        }

        /// <summary>
        /// 执行与释放或重置托管资源相关的应用程序定义的任务。
        /// </summary>
        protected override void DisposeManaged()
        {
            this.Engine = null;
            while(true)
            {
                try
                {
                    File.Delete(this.DatabasePath);
                    break;
                }
                catch(Exception)
                {
                    System.Threading.Thread.Sleep(99);
                }
            }
            base.DisposeManaged();
        }
    }
}
