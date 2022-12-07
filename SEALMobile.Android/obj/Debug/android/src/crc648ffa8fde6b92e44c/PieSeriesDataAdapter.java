package crc648ffa8fde6b92e44c;


public class PieSeriesDataAdapter
	extends crc648ffa8fde6b92e44c.ChangableSeriesDataAdapter
	implements
		mono.android.IGCUserPeer,
		com.devexpress.dxcharts.PieSeriesData
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getDataCount:()I:GetGetDataCountHandler:DevExpress.Xamarin.Android.Charts.IPieSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_getLabel:(I)Ljava/lang/String;:GetGetLabel_IHandler:DevExpress.Xamarin.Android.Charts.IPieSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_getValue:(I)D:GetGetValue_IHandler:DevExpress.Xamarin.Android.Charts.IPieSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Charts.Android.PieSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", PieSeriesDataAdapter.class, __md_methods);
	}


	public PieSeriesDataAdapter ()
	{
		super ();
		if (getClass () == PieSeriesDataAdapter.class)
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Charts.Android.PieSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", "", this, new java.lang.Object[] {  });
	}


	public int getDataCount ()
	{
		return n_getDataCount ();
	}

	private native int n_getDataCount ();


	public java.lang.String getLabel (int p0)
	{
		return n_getLabel (p0);
	}

	private native java.lang.String n_getLabel (int p0);


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
