# live_executer

Live Executer – this editor extension allowing to cause arbitrary (random) code in game time,
and well as in edit mode. With his help (assistance), for example, can to cause Play in the 
component Audio Source or any method (algorithm) in his component.

It works through the system assembly C #, your code is added in special (particular) method, 
and afterwards system collects this code, and causes method in Unity stream. 
Default are connected some, to a standard C# namespace, and UnityEngine, and UnityEditor. 

To open the call code window, from the tabs “Window” ( Window -> Live Executer ).

Example using
To access the target object you can take advantage of a static method "Find"  class "GameObject":
GameObject camera = GameObject.Find(“Main Camera”);
Afterwards you can use any data type
GameObject camera = GameObject.Find(“Main Camera”); 
camera.GetComponent().SomeMethod(10, “Hello”);
