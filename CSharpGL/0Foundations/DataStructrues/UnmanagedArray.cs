﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;


namespace CSharpGL
{

    /// <summary>
    /// unmanaged huge array.
    /// <para>Check http://www.cnblogs.com/bitzhuwei/p/huge-unmanged-array-in-csharp.html </para>
    /// </summary>
    /// <typeparam name="T">sbyte, byte, char, short, ushort, int, uint, long, ulong, float, double, decimal, bool or other struct types. enum not supported.</typeparam>
    public sealed unsafe class UnmanagedArray<T> : UnmanagedArrayBase where T : struct
    {
        /// <summary>
        /// How many <see cref="UnmanagedArray&lt;T&gt;"/> objects allocated?
        /// <para>Only used for debugging.</para>
        /// </summary>
        private static int thisTypeAllocatedCount = 0;
        /// <summary>
        /// How many <see cref="UnmanagedArray&lt;T&gt;"/> objects released?
        /// <para>Only used for debugging.</para>
        /// </summary>
        private static int thisTypeDisposedCount = 0;

        /// <summary>
        /// unmanaged array.
        /// </summary>
        /// <param name="count"></param>
        public UnmanagedArray(int count)
            : base(count, Marshal.SizeOf(typeof(T)))
        {
            UnmanagedArray<T>.thisTypeAllocatedCount++;
        }

        // Do not try to use less effitient way of accessing elements as we're using OpenGL.
        // 既然要用OpenGL，就不要试图才用低效的方式了。
        ///// <summary>
        ///// 获取或设置索引为<paramref name="index"/>的元素。
        ///// <para>如果要处理的元素数目较大，请使用unsafe方式(<see cref="UnmanagedArrayFastAccessHelper"/>)。</para>
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //public unsafe T this[int index]
        //{
        //    get
        //    {
        //        if (index < 0 || index >= this.Length)
        //            throw new IndexOutOfRangeException("index of UnmanagedArray is out of range");

        //        var pItem = this.Header + (index * elementSize);
        //        var obj = Marshal.PtrToStructure(pItem, typeof(T));
        //        T result = (T)obj;
        //        //T result = Marshal.PtrToStructure<T>(pItem);// works in .net 4.5.1
        //        return result;
        //    }
        //    set
        //    {
        //        if (index < 0 || index >= this.Length)
        //            throw new IndexOutOfRangeException("index of UnmanagedArray is out of range");

        //        var pItem = this.Header + (index * elementSize);
        //        Marshal.StructureToPtr(value, pItem, true);
        //        //Marshal.StructureToPtr<T>(value, pItem, true);// works in .net 4.5.1
        //    }
        //}

        /// <summary>
        /// Dispose unmanaged resources
        /// </summary>
        protected override void DisposeUnmanagedResources()
        {
            base.DisposeUnmanagedResources();

            UnmanagedArray<T>.thisTypeDisposedCount++;
        }

        // Do not try to use less effitient way of accessing elements as we're using OpenGL.
        ///// <summary>
        ///// 按索引顺序依次获取各个元素。
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<T> Elements()
        //{
        //    for (int i = 0; i < this.Length; i++)
        //    {
        //        yield return this[i];
        //    }
        //}
    }

    /// <summary>
    /// Base type of unmanaged array.
    /// <para>Similar to array in <code>int array[Length];</code></para>
    /// </summary>
    public abstract class UnmanagedArrayBase : IDisposable
    {
        /// <summary>
        /// How many <see cref="UnmanagedArrayBase"/> allocated?
        /// <para>Only used for debugging.</para>
        /// </summary>
        public static int allocatedCount = 0;

        /// <summary>
        /// How many <see cref="UnmanagedArrayBase"/> released?
        /// <para>Only used for debugging.</para>
        /// </summary>
        public static int disposedCount = 0;

        /// <summary>
        /// Start position of array; Head of array; first element's position of array.
        /// <para><code>array</code> in <code>int array[Length];</code></para>
        /// </summary>
        public IntPtr Header { get; private set; }

        /// <summary>
        /// How many elements?
        /// <para><code>Length</code> in <code>int array[Length];</code></para>
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// 单个元素的字节数。
        /// <para>How manay bytes for one element of array?</para>
        /// </summary>
        protected int elementSize;

        /// <summary>
        /// 申请到的字节数。（元素的总数 * 单个元素的字节数）。
        /// <para>How many bytes for total array?</para>
        /// <para>Length * elementSize</para>
        /// </summary>
        public int ByteLength
        {
            get { return (this.Length * this.elementSize); }
        }

        /// <summary>
        /// Base type of unmanaged array.
        /// <para>Similar to array in <code>int array[Length];</code></para>
        /// </summary>
        /// <param name="elementCount">How many elements?</param>
        /// <param name="elementSize">How manay bytes for one element of array?</param>
        //[MethodImpl(MethodImplOptions.Synchronized)]//这造成死锁，不知道是为什么 Dead lock, Why?
        protected UnmanagedArrayBase(int elementCount, int elementSize)
        {
            this.Length = elementCount;
            this.elementSize = elementSize;

            int memSize = elementCount * elementSize;
            this.Header = Marshal.AllocHGlobal(memSize);

            UnmanagedArrayBase.allocatedCount++;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        } // end sub

        /// <summary>
        /// Destruct instance of the class.
        /// </summary>
        ~UnmanagedArrayBase()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Backing field to track whether Dispose has been called.
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Dispose managed and unmanaged resources of this instance.
        /// </summary>
        /// <param name="disposing">If disposing equals true, managed and unmanaged resources can be disposed. If disposing equals false, only unmanaged resources can be disposed. </param>
        private void Dispose(bool disposing)
        {
            if (this.disposedValue == false)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    //DisposeManagedResources();
                } // end if

                // Dispose unmanaged resources.
                DisposeUnmanagedResources();

                UnmanagedArrayBase.disposedCount++;
            } // end if

            this.disposedValue = true;
        } // end sub

        #endregion

        /// <summary>
        /// Dispose unmanaged resources
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
            Marshal.FreeHGlobal(this.Header);

            this.Header = IntPtr.Zero;
        }

        ///// <summary>
        ///// Dispose managed resources
        ///// </summary>
        //protected virtual void DisposeManagedResources()
        //{
        //}

        /// <summary>
        /// return string.Format("head: {0}, element count: {1}, byte length: {2}",
        ///     this.Header, this.Length, this.ByteLength);
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("head: {0}, element count: {1}, byte length: {2}",
                this.Header, this.Length, this.ByteLength);
        }
    }
}
