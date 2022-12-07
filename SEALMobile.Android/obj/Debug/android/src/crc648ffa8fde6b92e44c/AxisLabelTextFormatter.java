package crc648ffa8fde6b92e44c;


public class AxisLabelTextFormatter
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.devexpress.dxcharts.AxisLabelTextFormatter
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_format:(Ljava/lang/Object;)Ljava/lang/String;:GetFormat_Ljava_lang_Object_Handler:DevExpress.Xamarin.Android.Charts.IAxisLabelTextFormatterInvoker, DevExpress.Xamarin.Android.Charts\n" +
			"";
		mono.android.Runtime.register ("DevExpress.XamarinForms.Charts.Android.AxisLabelTextFormatter, DevExpress.XamarinForms.Charts.Android", AxisLabelTextFormatter.class, __md_methods);
	}


	public AxisLabelTextFormatter ()
	{
		super ();
		if (getClass () == AxisLabelTextFormatter.class)
			mono.android.TypeManager.Activate ("DevExpress.XamarinForms.Charts.Android.AxisLabelTextFormatter, DevExpress.XamarinForms.Charts.Android", "", this, new java.lang.Object[] {  });
	}


	public java.lang.String format (java.lang.Object p0)
	{
		return n_format (p0);
	}

	private native java.lang.String n_format (java.lang.Object p0);

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
