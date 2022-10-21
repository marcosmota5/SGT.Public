using System;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.HelperClasses
{
    public static class TaskExtensions
    {

        public async static void Await(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static void Await(this Task task, Action<Exception> errorCallBack)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                errorCallBack?.Invoke(ex);
            }
        }

        public async static void Await(this Task task, Action completedCallBack)
        {
            try
            {
                await task;
                completedCallBack?.Invoke();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static void Await(this Task task, Action completedCallBack, Action<Exception> errorCallBack)
        {
            try
            {
                await task;
                completedCallBack?.Invoke();
            }
            catch (Exception ex)
            {
                errorCallBack?.Invoke(ex);
            }
        }

    }
}
