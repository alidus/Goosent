package md5c81f1b66264867e93dc51d4556676926;


public class BaseTabFragment
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Goosent.BaseTabFragment, Goosent, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BaseTabFragment.class, __md_methods);
	}


	public BaseTabFragment ()
	{
		super ();
		if (getClass () == BaseTabFragment.class)
			mono.android.TypeManager.Activate ("Goosent.BaseTabFragment, Goosent, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
