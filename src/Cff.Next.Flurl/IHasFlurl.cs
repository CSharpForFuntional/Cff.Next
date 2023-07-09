using Cff.Next.Abstraction;
using Flurl.Http;

namespace Cff.Next.Flurl;

public interface IHasFlurl<RT> : IHas<RT, IFlurlClient> where RT : struct, IHasFlurl<RT>
{
    public static Aff<RT, Unit> WriteAff(string x) => Eff(() =>
    {
        Console.WriteLine(x);
        return unit;
    });
}
