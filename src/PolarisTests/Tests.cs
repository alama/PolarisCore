using System;
using Xunit;
using Polaris.Lib.Utility;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
    {
		#region Polaris.Lib
		#region FreeList

		[Fact]
		public void PolarisLib_FreeList_TestConstructor()
		{
			FreeList<int> list = new FreeList<int>(123);
			Assert.True(list.CurrentSize == 0);
			Assert.True(list.FreeSpace == 123);
			Assert.True(list.MaxSize == 123);
		}

        [Fact]
        public void PolarisLib_FreeList_TestAdd() 
        {
			FreeList<int> list = new FreeList<int>(1);
			FreeList<int> list2 = new FreeList<int>(2);

			Assert.True(list.Add(1) == 0);
			Assert.True(list[0] == 1);
			Assert.True(list.CurrentSize == 1);
			Assert.True(list.FreeSpace == 0);
			Assert.True(list.Add(2) == -1);

			Assert.True(list2.Add(1) == 0);
			Assert.True(list2.Add(2) == 1);
			Assert.True(list2.Add(3) == -1);
			Assert.True(list2[0] == 1);
			Assert.True(list2[1] == 2);
		}

		[Fact]
		public void PolarisLib_FreeList_TestRemove()
		{
			FreeList<int> list = new FreeList<int>(1);
			FreeList<int> list2 = new FreeList<int>(2);

			list.Remove(list.Add(1));
			Assert.True(list.FreeSpace == 1);
			Assert.True(list.CurrentSize == 0);

			list2.Add(1);
			list2.Add(2);
			list2.Remove(0);
			Assert.True(list2.FreeSpace == 1);
			Assert.True(list2.CurrentSize == 1);
			Assert.True(list2[1] == 2);
			list2.Add(3);
			Assert.True(list2[0] == 3);
		}

		[Fact]
		public void PolarisLib_FreeList_TestAddClassTypes()
		{
			FreeList<List<int>> list = new FreeList<List<int>>(1);
			FreeList<List<int>> list2 = new FreeList<List<int>>(3);

			List<int> l1 = new List<int>();
			l1.Add(1);
			list.Add(l1);
			Assert.True(list[0][0] == 1);
			Assert.True(list.FreeSpace == 0);
			Assert.True(list.CurrentSize == 1);

			List<int> l2 = new List<int>();
			l2.Add(1);
			l2.Add(2);
			list2.Add(l1);
			list2.Add(l2);
			Assert.True(list2[0][0] == 1);
			Assert.True(list2[1][0] == 1);
			Assert.True(list2[1][1] == 2);
			Assert.True(list2.FreeSpace == 1);
			Assert.True(list2.CurrentSize == 2);
		}

		#endregion FreeList
		#endregion Polaris.Lib
	}
}
