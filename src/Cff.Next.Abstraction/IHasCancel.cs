namespace Cff.Next.Abstraction;

using LanguageExt.Effects.Traits;

public interface IHasCancel<RT> : HasCancel<RT> where RT : struct, IHasCancel<RT>
{
    RT HasCancel<RT>.LocalCancel => new();
    CancellationToken HasCancel<RT>.CancellationToken => CancellationTokenSource.Token;
}


