package crc648ffa8fde6b92e44c;


public class XYSeriesDataAdapter
	extends crc648ffa8fde6b92e44c.ChangableSeriesDataAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Charts.Android.XYSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", XYSeriesDataAdapter.class, __md_methods);
	}


	public XYSeriesDataAdapter ()
	{
		super ();
		if (getClass () == XYSeriesDataAdapter.class)
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Charts.Android.XYSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", "", this, new java.lang.Object[] {  });
	}

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
