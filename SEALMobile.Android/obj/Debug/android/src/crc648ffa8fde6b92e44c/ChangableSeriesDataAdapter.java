package crc648ffa8fde6b92e44c;


public abstract class ChangableSeriesDataAdapter
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.dxcharts.ChangeableSeriesData
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_addChangedListener:(Lcom/devexpress/dxcharts/SeriesDataChangedListener;)V:GetAddChangedListener_Lcom_devexpress_dxcharts_SeriesDataChangedListener_Handler:DevExpress.Xamarin.Android.Charts.IChangeableSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"n_removeChangedListener:(Lcom/devexpress/dxcharts/SeriesDataChangedListener;)V:GetRemoveChangedListener_Lcom_devexpress_dxcharts_SeriesDataChangedListener_Handler:DevExpress.Xamarin.Android.Charts.IChangeableSeriesDataInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Charts.Android.ChangableSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", ChangableSeriesDataAdapter.class, __md_methods);
	}


	public ChangableSeriesDataAdapter ()
	{
		super ();
		if (getClass () == ChangableSeriesDataAdapter.class)
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Charts.Android.ChangableSeriesDataAdapter, DevExpress.XamarinForms.Charts.Android", "", this, new java.lang.Object[] {  });
	}


	public void addChangedListener (com.devexpress.dxcharts.SeriesDataChangedListener p0)
	{
		n_addChangedListener (p0);
	}

	private native void n_addChangedListener (com.devexpress.dxcharts.SeriesDataChangedListener p0);


	public void removeChangedListener (com.devexpress.dxcharts.SeriesDataChangedListener p0)
	{
		n_removeChangedListener (p0);
	}

	private native void n_removeChangedListener (com.devexpress.dxcharts.SeriesDataChangedListener p0);

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
