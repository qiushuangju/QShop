namespace Qs.Comm
{
    public struct SqlDbTypeName
    {
        public const string NVarChar = "nvarchar";
        public const string VarChar = "varchar";
        public const string NChar = "nchar";
        public const string Char = "char";
        public const string Text = "text";
        public const string Int = "int";
        public const string BigInt = "bigint";
        public const string DateTime = "datetime";
        public const string Date = "date";
        public const string SmallDateTime = "smalldatetime";
        public const string SmallDate = "smalldate";
        public const string Float = "float";
        public const string Decimal = "decimal";
        public const string Double = "double";
        public const string Bit = "bit";
        public const string Bool = "bool";
        public const string UniqueIdentifier = "uniqueidentifier";  
    }
    /// <summary>
    /// 排序类型
    /// </summary>
    public enum QueryOrderBy
    {
        Desc = 1,
        Asc = 2
    }
    /// <summary>
    /// 定时任务状态
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// 未启动
        /// </summary>
        NotRun,
        /// <summary>
        /// 正在运行
        /// </summary>
        Running
    }
    public enum LinqExpressionType
    {
        Equal = 0,//=
        NotEqual = 1,//!=
        GreaterThan,//>
        LessThan,//<
        ThanOrEqual,//>=
        LessThanOrEqual,//<=
        In,
        Contains,//Contains
        NotContains//NotContains
    }

    public enum ResponseType
    {
        ServerError = 1,
        LoginExpiration = 302,
        ParametersLack = 303,
        TokenExpiration,
        PINError,
        NoPermissions,
        NoRolePermissions,
        LoginError,
        AccountLocked,
        LoginSuccess,
        SaveSuccess,
        AuditSuccess,
        OperSuccess,
        RegisterSuccess,
        ModifyPwdSuccess,
        EidtSuccess,
        DelSuccess,
        NoKey,
        NoKeyDel,
        KeyError,
        Other
    }
}
