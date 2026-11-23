using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class CircularBufferTests
    {
        [Fact] //------------------------------------------------------------Тест 23
        public void New_Buffer_IsEmpty()
        {
            var b = new CircularBuffer<int>(4);
            Assert.Equal(4, b.Capacity);
            Assert.Equal(0, b.Count);
            Assert.False(b.IsFull);
            Assert.Empty(b.ToArray());
        }

        [Fact] //------------------------------------------------------------Тест 24
        public void Add_TwoElements_KeepsOrder()
        {
            var b = new CircularBuffer<int>(4);
            b.Add(1); b.Add(2);
            Assert.Equal(2, b.Count);
            Assert.False(b.IsFull);
            Assert.Equal(new[] { 1, 2 }, b.ToArray());
        }

        [Fact] //------------------------------------------------------------Тест 25
        public void Add_CapacityElements_IsFull()
        {
            var b = new CircularBuffer<int>(3);
            for (int i = 0; i < 3; i++) b.Add(i);
            Assert.True(b.IsFull);
        }

        [Fact]
        public void Add_BeyondCapacity_OldestElementsAreDropped()
        {
            var buffer = new CircularBuffer<int>(3);
            for (int i = 1; i <= 5; i++) buffer.Add(i);
            Assert.Equal(3, buffer.Count);
            Assert.Equal(new[] { 3, 4, 5 }, buffer.ToArray());
            Assert.Equal(3, buffer[0]);
            Assert.Equal(5, buffer[2]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void Indexer_OutOfRange_Throws(int index)
        {
            var buffer = new CircularBuffer<int>(5);
            buffer.Add(10); buffer.Add(20);
            Assert.Throws<ArgumentOutOfRangeException>(() => buffer[index]);
        }

        [Fact] //------------------------------------------------------------Тест 26
        public void Add_CapacityOne_KeepsLast()
        {
            var b = new CircularBuffer<int>(1);
            b.Add(1); b.Add(2); b.Add(3);
            Assert.Equal(new[] { 3 }, b.ToArray());
        }

        [Fact] //------------------------------------------------------------Тест 27
        public void Add_1000Elements_Capacity7_KeepsLastSeven()
        {
            var b = new CircularBuffer<int>(7);
            for (int i = 1; i <= 1000; i++) b.Add(i);
            Assert.Equal(new[] { 994, 995, 996, 997, 998, 999, 1000 }, b.ToArray());
        }

        [Fact] //------------------------------------------------------------Тест 28
        public void Clear_AfterOverflow_WorksAgain()
        {
            var b = new CircularBuffer<int>(3);
            for (int i = 1; i <= 5; i++) b.Add(i);
            b.Clear();
            b.Add(42);
            Assert.Equal(1, b.Count);
            Assert.Equal(42, b[0]);
        }

        [Theory] //------------------------------------------------------------Тест 29
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_InvalidCapacity_Throws(int capacity)
            => Assert.Throws<ArgumentOutOfRangeException>(() => new CircularBuffer<int>(capacity));

        [Fact] //------------------------------------------------------------Тест 30
        public void ToArray_ModifyingResult_DoesNotChangeBuffer()
        {
            var b = new CircularBuffer<int>(3);
            b.Add(1); b.Add(2);
            int[] copy = b.ToArray();
            copy[0] = 99;
            Assert.Equal(new[] { 1, 2 }, b.ToArray());
        }
    }
}