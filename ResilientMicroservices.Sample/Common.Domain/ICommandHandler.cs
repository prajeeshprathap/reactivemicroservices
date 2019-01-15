namespace Common.Domain
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand { }
}
