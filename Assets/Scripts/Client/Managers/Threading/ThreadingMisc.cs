using System.Threading.Tasks;
using System;
public static class ThreadingMisc
{
    public static void Threader(Action target)
    {
        Task.Run(target);
    }
}
