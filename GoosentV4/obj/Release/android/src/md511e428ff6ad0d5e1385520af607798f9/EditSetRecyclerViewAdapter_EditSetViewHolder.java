package md511e428ff6ad0d5e1385520af607798f9;


public class EditSetRecyclerViewAdapter_EditSetViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Goosent.Adapters.EditSetRecyclerViewAdapter+EditSetViewHolder, Goosent, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EditSetRecyclerViewAdapter_EditSetViewHolder.class, __md_methods);
	}


	public EditSetRecyclerViewAdapter_EditSetViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == EditSetRecyclerViewAdapter_EditSetViewHolder.class)
			mono.android.TypeManager.Activate ("Goosent.Adapters.EditSetRecyclerViewAdapter+EditSetViewHolder, Goosent, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
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
