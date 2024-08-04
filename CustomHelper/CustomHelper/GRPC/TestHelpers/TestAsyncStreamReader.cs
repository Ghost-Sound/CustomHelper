using Grpc.Core;
using System.Threading.Channels;

namespace CustomHelper.GRPC.TestHelpers
{
    public class TestAsyncStreamReader<T>(ServerCallContext serverCallContext) : IAsyncStreamReader<T> where T : class
    {
        private readonly Channel<T> _channel = Channel.CreateUnbounded<T>();
        private readonly ServerCallContext _serverCallContext = serverCallContext;

        public T Current { get; private set; } = null!;

        public void AddMessage(T message)
        {
            if (!_channel.Writer.TryWrite(message))
            {
                throw new InvalidOperationException("Unable to write message.");
            }
        }

        public void Complete()
        {
            _channel.Writer.Complete();
        }

        public async Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            _serverCallContext.CancellationToken.ThrowIfCancellationRequested();

            if (await _channel.Reader.WaitToReadAsync(cancellationToken) &&
                _channel.Reader.TryRead(out var message))
            {
                Current = message;
                return true;
            }
            else
            {
                Current = null!;
                return false;
            }
        }
    }
}
