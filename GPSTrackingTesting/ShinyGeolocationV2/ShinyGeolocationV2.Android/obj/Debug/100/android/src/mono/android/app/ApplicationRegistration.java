package mono.android.app;

public class ApplicationRegistration {

	public static void registerApplications ()
	{
				// Application and Instrumentation ACWs must be registered first.
		mono.android.Runtime.register ("ShinyGeolocationV2.Droid.MainApplication, ShinyGeolocationV2.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", crc6453262554876aa1fc.MainApplication.class, crc6453262554876aa1fc.MainApplication.__md_methods);
		mono.android.Runtime.register ("Shiny.ShinyAndroidApplication, Shiny.Core, Version=1.2.0.1755, Culture=neutral, PublicKeyToken=null", crc643387a08acbe69b14.ShinyAndroidApplication.class, crc643387a08acbe69b14.ShinyAndroidApplication.__md_methods);
		mono.android.Runtime.register ("Shiny.ShinyAndroidApplication`1, Shiny.Core, Version=1.2.0.1755, Culture=neutral, PublicKeyToken=null", crc643387a08acbe69b14.ShinyAndroidApplication_1.class, crc643387a08acbe69b14.ShinyAndroidApplication_1.__md_methods);
		
	}
}