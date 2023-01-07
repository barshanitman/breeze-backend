using System;
namespace backend_engine.Services
{
	public static class Extensions
	{
		
       
            public static T CloneObject<T>(this object source)
            {
                T result = Activator.CreateInstance<T>();
                //// **** made things  
                return result;
            
        }
    }
}

