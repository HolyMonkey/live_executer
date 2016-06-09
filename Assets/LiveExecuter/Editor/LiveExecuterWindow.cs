using System.Collections;
using UnityEngine;
using UnityEditor;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Text;
using System;
using System.Reflection;

public class LiveExecuter : EditorWindow {

    string SomeCode = "Debug.Log(\"Hello World!\");";
    static string UnityEngineDLLPath = "";

    string Out = " ";
    public static string Test;
    [MenuItem("Window/Live Executer")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        LiveExecuter window = (LiveExecuter)EditorWindow.GetWindow(typeof(LiveExecuter));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label(Test, EditorStyles.boldLabel);
        SomeCode = EditorGUILayout.TextArea(SomeCode, GUILayout.Height(50));
        GUILayout.BeginHorizontal();
        GUILayout.Label("UnityEngine.dll Path");
        UnityEngineDLLPath = GUILayout.TextField(UnityEngineDLLPath);
        if (GUILayout.Button("Find"))
        {
            UnityEngineDLLPath = UnityEngine.Application.dataPath.Substring(0, UnityEngine.Application.dataPath.Length - 6) +
                     @"Library/UnityAssemblies/UnityEngine.dll";
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Run!")){
            Out = Proccess(SomeCode);
        }
    }

    public string Proccess(string code)
    {
        if (!code.Contains("return"))
        {
            code += "\n " + "return true;";
        }
        return Eval(code).ToString();
    }

    public static object Eval(string sCSCode)
    {

        CSharpCodeProvider c = new CSharpCodeProvider();
        ICodeCompiler icc = c.CreateCompiler();
        CompilerParameters cp = new CompilerParameters();

        cp.ReferencedAssemblies.Add("system.dll");
        cp.ReferencedAssemblies.Add("system.xml.dll");
        cp.ReferencedAssemblies.Add("system.data.dll");
        cp.ReferencedAssemblies.Add("system.windows.forms.dll");
        cp.ReferencedAssemblies.Add("system.drawing.dll");
        cp.ReferencedAssemblies.Add(UnityEngineDLLPath);
        cp.CompilerOptions = "/t:library";
        cp.GenerateInMemory = true;

        StringBuilder sb = new StringBuilder("");
        sb.Append("using System;\n");
        sb.Append("using System.Xml;\n");
        sb.Append("using System.Data;\n");
        sb.Append("using System.Data.SqlClient;\n");
        sb.Append("using System.Windows.Forms;\n");
        sb.Append("using System.Drawing;\n");
        sb.Append("using UnityEngine;\n");
        sb.Append("namespace CodeEvaler{ \n");
        sb.Append("public class CodeEvaler{ \n");
        sb.Append("public object EvalCode(){\n");
        sb.Append(sCSCode + "\n");
        sb.Append("} \n");
        sb.Append("} \n");
        sb.Append("}\n");

        CompilerResults cr = icc.CompileAssemblyFromSource(cp, sb.ToString());
        if (cr.Errors.Count > 0)
        {
            Debug.Log("ERROR: " + cr.Errors[0].ErrorText);
            return null;
        }

        System.Reflection.Assembly a = cr.CompiledAssembly;
        object o = a.CreateInstance("CodeEvaler.CodeEvaler");

        Type t = o.GetType();
        MethodInfo mi = t.GetMethod("EvalCode");

        object s = mi.Invoke(o, null);
        return s;

    }
}
