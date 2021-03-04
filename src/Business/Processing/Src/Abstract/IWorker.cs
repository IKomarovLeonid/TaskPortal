namespace Processing.Abstract
{
    public interface IWorker
    {
        void PushTask(ITask task);

        void Start();

        void Stop();
    }
}
