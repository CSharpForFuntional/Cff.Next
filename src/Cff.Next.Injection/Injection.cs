using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cff.Next.Flurl.Sample;

public static class Injection
{
    public static void Replace(Delegate a, Delegate b)
    {
        MethodInfo methodToReplace = a.GetMethodInfo();
        MethodInfo methodToInject = b.GetMethodInfo();
        RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
        RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);
        if (methodToReplace.IsVirtual)
            ReplaceVirtualInner(methodToReplace, methodToInject);
        else
            ReplaceInner(methodToReplace, methodToInject);
    }

    static void ReplaceVirtualInner(MethodInfo methodToReplace, MethodInfo methodToInject)
    {
        unsafe
        {
            var methodDesc = (UInt64*)(methodToReplace.MethodHandle.Value.ToPointer());
            var index = (int)(((*methodDesc) >> 32) & 0xFF);
            if (IntPtr.Size == 4)
            {
                if (methodToReplace.DeclaringType != null)
                {
                    var classStart = (uint*)methodToReplace.DeclaringType.TypeHandle.Value.ToPointer();
                    classStart += 10;
                    classStart = (uint*)*classStart;
                    var tar = classStart + index;

                    var inj = (uint*)methodToInject.MethodHandle.Value.ToPointer() + 2;
#if DEBUG

                    var injInst = (byte*)*inj;
                    var tarInst = (byte*)*tar;
                    var injSrc = (int*)(injInst + 1);
                    var tarSrc = (int*)(tarInst + 1);
                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                        *tar = *inj;
#endif
                }
            }
            else
            {
                if (methodToReplace.DeclaringType != null)
                {
                    var classStart = (ulong*)methodToReplace.DeclaringType.TypeHandle.Value.ToPointer();
                    classStart += 8;
                    classStart = (ulong*)*classStart;
                    var tar = classStart + index;

                    var inj = (ulong*)methodToInject.MethodHandle.Value.ToPointer() + 1;
#if DEBUG
                    var injInst = (byte*)*inj;
                    var tarInst = (byte*)*tar;
                    var injSrc = (int*)(injInst + 1);
                    var tarSrc = (int*)(tarInst + 1);
                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                        *tar = *inj;
#endif
                }
            }
        }
    }

    static void ReplaceInner(MethodInfo methodToReplace, MethodInfo methodToInject)
    {
        unsafe
        {
            if (IntPtr.Size == 4)
            {
                var inj = (int*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                var tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;
#if DEBUG
                var injInst = (byte*)*inj;
                var tarInst = (byte*)*tar;
                var injSrc = (int*)(injInst + 1);
                var tarSrc = (int*)(tarInst + 1);

                *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    *tar = *inj;
#endif
            }
            else
            {
                ulong* inj = (ulong*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                ulong* tar = (ulong*)methodToReplace.MethodHandle.Value.ToPointer() + 1;
                *tar = *inj;
            }
        }
    }
}

