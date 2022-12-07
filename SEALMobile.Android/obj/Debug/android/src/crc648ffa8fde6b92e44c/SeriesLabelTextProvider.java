package crc648ffa8fde6b92e44c;


public class SeriesLabelTextProvider
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.dxcharts.SeriesLabelTextProvider
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getText:(Lcom/devexpress/dxcharts/SeriesLabelValuesBase;)Ljava/lang/String;:GetGetText_Lcom_devexpress_dxcharts_SeriesLabelValuesBase_Handler:DevExpress.Xamarin.Android.Charts.ISeriesLabelTextProviderInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Charts.Android.SeriesLabelTextProvider, DevExpress.XamarinForms.Charts.Android", SeriesLabelTextProvider.class, __md_methods);
	}


	public SeriesLabelTextProvider ()
	{
		super ();
		if (getClass () == SeriesLabelTextProvider.class)
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Charts.Android.SeriesLabelTextProvider, DevExpress.XamarinForms.Charts.Android", "", this, new java.lang.Object[] {  });
	}


	public java.lang.String getText (com.devexpress.dxcharts.SeriesLabelValuesBase p0)
	{
		return n_getText (p0);
	}

	private native java.lang.String n_getText (com.devexpress.dxcharts.SeriesLabelValuesBase p0);

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
