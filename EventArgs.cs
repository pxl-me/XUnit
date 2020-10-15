using System;
using System.Collections;
using System.Collections.Generic;

namespace CustomCollections
{
    public sealed class PushToQueueEventArgs<T> : EventArgs
    {
        public T PushedItem { get; private set; }
        public string Message { get; private set; }

        public PushToQueueEventArgs(T value, string message)
        {
            Message = message;
            PushedItem = value;
        }
    }

    public sealed class PopFromQueueEventArgs<T> : EventArgs
    {
        public T PoppedItem { get; private set; }
        public string Message { get; private set; }

        public PopFromQueueEventArgs(T value, string message)
        {
            Message = message;
            PoppedItem = value;
        }
    }

    public sealed class DeleteFromQueueEventArgs<T> : EventArgs
    {
        public T DeletedItem { get; private set; }
        public string Message { get; private set; }

        public DeleteFromQueueEventArgs(T value, string message)
        {
            Message = message;
            DeletedItem = value;
        }
    }
}
