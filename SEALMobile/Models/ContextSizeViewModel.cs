using System;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.ObjectModel;

namespace SEALMobile.Models
{
    public class ContextSizeViewModel
    {
        public ObservableCollection<ContextSize> SizeList { get; set; }

        public ContextSizeViewModel()
        {
            SizeList = new ObservableRangeCollection<ContextSize>();
            LoadMore();
        }

        void LoadMore()
        {
            SizeList.Add(new ContextSize() { Name = "Small", PolyModulusDegree = 8192, CoeffModulus = new int[] { 50, 30, 30, 50 }, scale = 30 });
            SizeList.Add(new ContextSize() { Name = "Medium", PolyModulusDegree = 8192, CoeffModulus = new int[] { 60, 40, 40, 60 }, scale = 40 });
            SizeList.Add(new ContextSize() { Name = "Large", PolyModulusDegree = 16384, CoeffModulus = new int[] { 60, 40, 40, 40, 40, 40, 40, 40, 60 }, scale = 40 });

        }
    }

    public class ContextSize
    {
        public string Name { get; set; }
        public ulong PolyModulusDegree { get; set; }
        public int[] CoeffModulus { get; set; }
        public int scale { get; set; }
    }

}
