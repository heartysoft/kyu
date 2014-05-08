namespace Kyu.Bus
{
    public interface IHaveCorrelationId
    {
        string CorrelationId { get; }
    }
}