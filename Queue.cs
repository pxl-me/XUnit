using System;
using System.Collections;
using System.Collections.Generic;

namespace CustomCollections
{
    public class Queue<TSource> : IEnumerable<TSource>
    {
        private class Node<T>
        {
            public T Value { get; set; }
            public Node<T> Next { get; set; }
            public Node<T> Prev { get; set; }

            public Node(T value)
            {
                Value = value;
            }
        }

        private class QueueIterator<T> : IEnumerator<T>
        {
            private readonly Queue<T> source;
            private T current;
            private int state = 0;
            private int i;
            public QueueIterator(Queue<T> source)
            {
                this.source = source;
            }

            public T Current { get { return current; } }
            object IEnumerator.Current { get { return Current; } } 

            public bool MoveNext()
            {
                switch (state)
                {
                    case 0:
                        i = 0;
                        state = 1;
                        goto case 1;
                    case 1:
                        if (!(i < source.Count))
                        {
                            state = 0;
                            return false;
                        }
                        current = source.Peek();
                        source.Enqueue(source.Dequeue());
                        state = 2;
                        return true;
                    case 2:
                        ++i;
                        goto case 1;
                }
                return false;
            } // перемещение на одну позицию вперед в контейнере элементов
            public void Reset()
            {
                throw new NotImplementedException();
            }  // перемещение в начало контейнера
            public void Dispose() { }
        }

        private Node<TSource> head;
        private Node<TSource> tail;

        public Queue()
        {
            head = null;
            tail = null;
        }
        public Queue(IEnumerable<TSource> collection)
        {
            foreach (TSource item in collection)
            {
                Enqueue(item);
            }
        }

        public int Count { get; private set; }

        public EventHandler<PushToQueueEventArgs<TSource>> Pushed;
        public EventHandler<PopFromQueueEventArgs<TSource>> Popped;
        public EventHandler<DeleteFromQueueEventArgs<TSource>> Deleted;

        public TSource Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException("Empty :(");

            TSource ret = head.Value;

            head = head.Next;
            Count--;

            Popped?.Invoke(this, new PopFromQueueEventArgs<TSource>(ret, $"{ret} Dequeued"));
            return ret;
        }

        private TSource deleteNode(Node<TSource> node)
        {
            var prev = node.Prev;
            var next = node.Next;

            if (prev != null)
            {
                prev.Next = next;
            }
            if (next != null)
            {
                next.Prev = prev;
            }

            Count--;
            Deleted?.Invoke(this, new DeleteFromQueueEventArgs<TSource>(node.Value, $"{node.Value} deleted"));
            return node.Value;
        }

        public TSource DeleteNodeOnCurrentPosition(int n)
        {
            if (head == null || n <= 0)
                throw new InvalidOperationException("Current list is NULL or given wrong position");

            var current = head;

            for (int i = 1; current != null && i < n; i++)
            {
                current = current.Next;
            }

            if (current == null)
                throw new InvalidOperationException("Wrong given 'n'");

            return deleteNode(current);
        }

        private TSource searchNode(Node<TSource> node)
        {
            return node.Value;
        }

        public TSource SearchNodeOnCurrentPosition(int n)
        {
            if (head == null || tail == null)
                throw new InvalidOperationException("Empty :(");

            var current = head;

            for (int i = 1; current != null && i < n; i++)
            {
                current = current.Next;
            }

            if (current == null)
            {
                throw new InvalidOperationException("Number of nodes in current queue is less than given 'n'");
            }

            return searchNode(current);
        }

        public TSource Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException("Empty :(");

            TSource ret = head.Value;

            return ret;
        }

        public void Enqueue(TSource item)
        {
            Node<TSource> newItem = new Node<TSource>(item);
            if (head == null || tail == null)
            {
                tail = newItem;
                head = newItem;
            }
            else
            {
                newItem.Prev = tail;
                tail.Next = newItem;
                tail = newItem;
            }
            Count++;
            Pushed?.Invoke(this, new PushToQueueEventArgs<TSource>(item, $"{item} enqueued"));
        }

        //public TSource[] ToArray()
        //{
        //    TSource[] array = new TSource[Count];
        //    int i = 0;

        //    foreach (TSource item in this)
        //    {
        //        array[i] = item;
        //        i++;
        //    }
        //    return array;
        //}

        public IEnumerator<TSource> GetEnumerator()
        {
            return new QueueIterator<TSource>(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new QueueIterator<TSource>(this);
        }
    }
}
