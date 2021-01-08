package crc64c4c1f7df99cffd7d;


public class ShinyJobWorker
	extends androidx.work.ListenableWorker
	implements
		mono.android.IGCUserPeer,
		androidx.concurrent.futures.CallbackToFutureAdapter.Resolver
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_startWork:()Lcom/google/common/util/concurrent/ListenableFuture;:GetStartWorkHandler\n" +
			"n_onStopped:()V:GetOnStoppedHandler\n" +
			"n_attachCompleter:(Landroidx/concurrent/futures/CallbackToFutureAdapter$Completer;)Ljava/lang/Object;:GetAttachCompleter_Landroidx_concurrent_futures_CallbackToFutureAdapter_Completer_Handler:AndroidX.Concurrent.Futures.CallbackToFutureAdapter/IResolverInvoker, Xamarin.AndroidX.Concurrent.Futures\n" +
			"";
		mono.android.Runtime.register ("Shiny.Jobs.ShinyJobWorker, Shiny.Core", ShinyJobWorker.class, __md_methods);
	}


	public ShinyJobWorker (android.content.Context p0, androidx.work.WorkerParameters p1)
	{
		super (p0, p1);
		if (getClass () == ShinyJobWorker.class)
			mono.android.TypeManager.Activate ("Shiny.Jobs.ShinyJobWorker, Shiny.Core", "Android.Content.Context, Mono.Android:AndroidX.Work.WorkerParameters, Xamarin.AndroidX.Work.Runtime", this, new java.lang.Object[] { p0, p1 });
	}


	public com.google.common.util.concurrent.ListenableFuture startWork ()
	{
		return n_startWork ();
	}

	private native com.google.common.util.concurrent.ListenableFuture n_startWork ();


	public void onStopped ()
	{
		n_onStopped ();
	}

	private native void n_onStopped ();


	public java.lang.Object attachCompleter (androidx.concurrent.futures.CallbackToFutureAdapter.Completer p0)
	{
		return n_attachCompleter (p0);
	}

	private native java.lang.Object n_attachCompleter (androidx.concurrent.futures.CallbackToFutureAdapter.Completer p0);

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
