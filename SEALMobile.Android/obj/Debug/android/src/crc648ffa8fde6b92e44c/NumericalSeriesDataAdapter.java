package crc648ffa8fde6b92e44c;


public class NumericalSeriesDataAdapter
	extends crc648ffa8fde6b92e44c.XYSeriesDataAdapter
	implements
		mono.android.IGCUserPeer,
		com.devexpress.dxcharts.BatchNumericSeriesData,
		com.devexpress.dxcharts.NumericSeriesData,
		com.devexpress.dxcharts.XYSeriesData
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_fillArguments:([DII)I:GetFillArguments_arrayDIIHandler:DevExpress.Xamarin.Android.Charts.IBatchNumericSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_fillValues:([DII)I:GetFillValues_arrayDIIHandler:DevExpress.Xamarin.Android.Charts.IBatchNumericSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_getDataCount:()I:GetGetDataCountHandler:DevExpress.Xamarin.Android.Charts.INumericSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_getArgument:(I)D:GetGetArgument_IHandler:DevExpress.Xamarin.Android.Charts.INumericSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_getValue:(I)D:GetGetValue_IHandler:DevExpress.Xamarin.Android.Charts.INumericSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Charts.Android.NumericalSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", NumericalSeriesDataAdapter.class, __md_methods);
	}


	public NumericalSeriesDataAdapter ()
	{
		super ();
		if (getClass () == NumericalSeriesDataAdapter.class)
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Charts.Android.NumericalSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", "", this, new java.lang.Object[] {  });
	}


	public int fillArguments (double[] p0, int p1, int p2)
	{
		return n_fillArguments (p0, p1, p2);
	}

	private native int n_fillArguments (double[] p0, int p1, int p2);


	public int fillValues (double[] p0, int p1, int p2)
	{
		return n_fillValues (p0, p1, p2);
	}

	private native int n_fillValues (double[] p0, int p1, int p2);


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


	public double getValue (int p0)
	{
		return n_getValue (p0);
	}

	private native double n_getValue (int p0);

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
