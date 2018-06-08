using System;
using System.Threading.Tasks;

namespace VrmArPlayer
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }

        public static async void FireAndForget<T>(this Task<T> task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }
    }
}
