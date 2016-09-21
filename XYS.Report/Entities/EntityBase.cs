namespace XYS.Report.Entities
{
    /// <summary>
    /// 所有自定义实体的基础接口
    /// </summary>
    public interface IEntityBase
    {

    }

    /// <summary>
    /// 数据库对象接口
    /// </summary>
    public interface IDBEntity : IEntityBase
    {

    }

    /// <summary>
    /// 传输对象接口
    /// </summary>
    public interface ITSEntity : IEntityBase
    {
    }
}
