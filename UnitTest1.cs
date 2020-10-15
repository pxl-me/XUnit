using System;
using Xunit;
using CustomCollections;
using Xunit.Sdk;

namespace CustomCollections
{
    public class Tests
    {
        [Fact]
        public void Add_CheckThatEventRaisedWhenAddingAnItem()
        {
            //arrange
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);
            //act
            var receivedEvent = Assert.Raises<PushToQueueEventArgs<int>>(
                a => queue.Pushed += a,
                a => queue.Pushed -= a,
                () => { queue.Enqueue(1); });
            //assert
            Assert.NotNull(receivedEvent);
            Assert.Equal(0, receivedEvent.Arguments.PushedItem);
            Assert.Equal("0 was added", receivedEvent.Arguments.Message);
        }


        [Fact]
        public void Remove_CheckThatEventRaisedWhenRemovingAnItem()
        {
            //arrange
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);
            var delete_item = 2;
            //act
            var receivedEvent = Assert.Raises<DeleteFromQueueEventArgs<int>>(
                a => queue.Deleted += a,
                a => queue.Deleted -= a,
                () => { queue.DeleteNodeOnCurrentPosition(delete_item); });
            //assert
            Assert.NotNull(receivedEvent);
            Assert.Equal(2, receivedEvent.Arguments.DeletedItem);
            Assert.Equal("2 was removed", receivedEvent.Arguments.Message);
        }

        [Fact]
        public void Add_AddNullElement_ThrowsArgumentNullException()
        {
            //arrange
            Queue<string> queue = new Queue<string>();
            queue.Enqueue("1");
            queue.Enqueue("2");
            queue.Enqueue("3");
            queue.Enqueue("4");
            queue.Enqueue("5");
            //act & assert
            Assert.Throws<ArgumentNullException>(() => queue.Enqueue((string)null));
        }

        [Theory,
         InlineData(2), InlineData(3), InlineData(4)]
        public void Contains_CheckContainsOfElementsThatAreInCollection_ReturnTrue(int value)
        {
            bool c;
            //arrange
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);
            //act
            var searchNode = queue.SearchNodeOnCurrentPosition(value);
            if (searchNode == value) { c = true; }
            else c = false;
            //assert
            Assert.True(c);
        }
        [Theory,
         InlineData(1000), InlineData(-18), InlineData(60)]
        public void Contains_CheckContainsOfElementsThatAreNotInCollection_ReturnFalse(int value)
        {
            bool c;
            //arrange
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);
            //act
            var searchNode = queue.SearchNodeOnCurrentPosition(value);
            if (searchNode == value) { c = true; }
            else c = false;
            //assert
            Assert.False(c);
        }

        [Theory,
         InlineData(1), InlineData(5), InlineData(8)]
        public void Remove_CheckRemoveOfElementsThatAreInCollection_ReturnTrue(int value)
        {
            //arrange
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);
            //act & assert
            bool c;
            var contains = queue.DeleteNodeOnCurrentPosition(value);
            if (contains == value) { c = true; }
            else c = false;
            Assert.True(c);
        }

        [Theory,
         InlineData(20), InlineData(30), InlineData(40)]
        public void Remove_CheckRemoveOfElementsThatAreNotInCollection_ReturnFalse(int value)
        {
            //arrange
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);
            //act & assert
            bool c;
            var contains = queue.DeleteNodeOnCurrentPosition(value);
            if (contains == value) { c = true; }
            else c = false;
            Assert.False(c);
        }

        [Theory,
         InlineData(20), InlineData(30), InlineData(40)]
        public void Remove_CheckRemoveOfElementsIfCollectionIsEmpty_ReturnFalse(int value)
        {
            //arrange
            Queue<int> queue = new Queue<int>();
            //act & assert
            bool c;
            var contains = queue.DeleteNodeOnCurrentPosition(value);
            if (contains == value) { c = true; }
            else c = false;
            Assert.False(c);
        }

        //[Fact]
        //public void Remove_RemoveNullElement_ThrowsArgumentNullException()
        //{
        //    //arrange
        //    Queue<int> queue = new Queue<int>();
        //    queue.Enqueue(1);
        //    queue.Enqueue(2);
        //    queue.Enqueue(3);
        //    queue.Enqueue(4);
        //    queue.Enqueue(5);
        //    //act & assert
        //    Assert.Throws<ArgumentNullException>(() => queue.DeleteNodeOnCurrentPosition((int)null)); ---- предусмотрено в Queue.cs код чисто для примера т.к. не может быть null -> int
        //}

    }
}