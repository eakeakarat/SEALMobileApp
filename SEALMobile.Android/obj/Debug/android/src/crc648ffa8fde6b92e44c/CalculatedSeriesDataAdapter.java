package crc648ffa8fde6b92e44c;


public class CalculatedSeriesDataAdapter
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.dxcharts.CalculatedSeriesData,
		com.devexpress.dxcharts.XYSeriesData
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getSource:()Lcom/devexpress/dxcharts/Series;:GetGetSourceHandler:DevExpress.Xamarin.Android.Charts.ICalculatedSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Charts.Android.CalculatedSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", CalculatedSeriesDataAdapter.class, __md_methods);
	}


	public CalculatedSeriesDataAdapter ()
	{
		super ();
		if (getClass () == CalculatedSeriesDataAdapter.class)
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Charts.Android.CalculatedSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", "", this, new java.lang.Object[] {  });
	}


	public com.devexpress.dxcharts.Series getSource ()
	{
		return n_getSource ();
	}

	private native com.devexpress.dxcharts.Series n_getSource ();

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
