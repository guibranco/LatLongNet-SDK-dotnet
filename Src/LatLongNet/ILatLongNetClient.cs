using System.Threading;
using System.Threading.Tasks;

namespace LatLongNet;

public interface ILatLongNetClient
{
    Task<string> GetHelloWorldAsync(CancellationToken cancellationToken);
}
