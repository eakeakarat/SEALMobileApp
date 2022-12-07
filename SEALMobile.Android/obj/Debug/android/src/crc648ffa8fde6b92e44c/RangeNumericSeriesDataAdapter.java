package crc648ffa8fde6b92e44c;


public class RangeNumericSeriesDataAdapter
	extends crc648ffa8fde6b92e44c.XYSeriesDataAdapter
	implements
		mono.android.IGCUserPeer,
		com.devexpress.dxcharts.RangeNumericSeriesData,
		com.devexpress.dxcharts.XYSeriesData
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getDataCount:()I:GetGetDataCountHandler:DevExpress.Xamarin.Android.Charts.IRangeNumericSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_getArgument:(I)D:GetGetArgument_IHandler:DevExpress.Xamarin.Android.Charts.IRangeNumericSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_getValue1:(I)D:GetGetValue1_IHandler:DevExpress.Xamarin.Android.Charts.IRangeNumericSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_getValue2:(I)D:GetGetValue2_IHandler:DevExpress.Xamarin.Android.Charts.IRangeNumericSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Charts.Android.RangeNumericSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", RangeNumericSeriesDataAdapter.class, __md_methods);
	}


	public RangeNumericSeriesDataAdapter ()
	{
		super ();
		if (getClass () == RangeNumericSeriesDataAdapter.class)
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Charts.Android.RangeNumericSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", "", this, new java.lang.Object[] {  });
	}


	public int getDataCount ()
	{
		return n_getDataCount ();
	}

	private native int n_getDataCount ();


	public double getArgument (int p0)
	{
		return n_getArgument (p0);
	}

	private native double n_getArgument (int p0);


	public double getValue1 (int p0)
	{
		return n_getValue1 (p0);
	}

	private native double n_getValue1 (int p0);


	public double getValue2 (int p0)
	{
		return n_getValue2 (p0);
	}

	private native double n_getValue2 (int p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
