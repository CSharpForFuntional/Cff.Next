
using System.Security.Cryptography.X509Certificates;
using Castle.DynamicProxy.Internal;
using Cff.Next.Abstraction;
using Cff.Next.Flurl;
using Cff.Next.Flurl.Sample;
using Flurl.Http;
using HarmonyLib;

var harmony = new Harmony("com.company.project.product");

harmony.PatchAll();

await Process<Runtime>("Hello").Run(new Runtime());


static Aff<RT, Unit> Process<RT>(string a)
    where RT : struct, IHasFlurl<RT> =>
    IHasFlurl<RT>.WriteAff(a);




public readonly partial record struct Runtime
(
    CancellationTokenSource CancellationTokenSource
) : IHasFlurl<Runtime>
{
    IFlurlClient IHas<Runtime, IFlurlClient>.It { get; }
}


[HarmonyPatch(typeof(IHasFlurl<Runtime>))]
public class Patch
{
    [HarmonyReversePatch]
    [HarmonyPatch("WriteAff")]
    public static Aff<Runtime, Unit> WriteAff(string x) => Eff(() =>
    {
        Console.WriteLine("Harmony");
        return unit;
    });
}
