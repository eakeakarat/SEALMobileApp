package mono.com.devexpress.dxcharts;


public class SelectionChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.dxcharts.SelectionChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSelectionChanged:(Lcom/devexpress/dxcharts/SelectionChangedInfo;)V:GetOnSelectionChanged_Lcom_devexpress_dxcharts_SelectionChangedInfo_Handler:DevExpress.Xamarin.Android.Charts.ISelectionChangedListenerInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"";
		mono.android.Runtime.register ("DevExpress.Xamarin.Android.Charts.ISelectionChangedListenerImplementor, DevExpress.Xamarin.Android.Charts", SelectionChangedListenerImplementor.class, __md_methods);
	}


	public SelectionChangedListenerImplementor ()
	{
		super ();
		if (getClass () == SelectionChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("DevExpress.Xamarin.Android.Charts.ISelectionChangedListenerImplementor, DevExpress.Xamarin.Android.Charts", "", this, new java.lang.Object[] {  });
	}


	public void onSelectionChanged (com.devexpress.dxcharts.SelectionChangedInfo p0)
	{
		n_onSelectionChanged (p0);
	}

	private native void n_onSelectionChanged (com.devexpress.dxcharts.SelectionChangedInfo p0);

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
