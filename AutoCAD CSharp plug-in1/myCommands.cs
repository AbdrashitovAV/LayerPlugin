// (C) Copyright 2016 by  
//
using System;
using System.Collections;
using System.Collections.Generic;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(AutoCAD_CSharp_plug_in1.MyCommands))]

namespace AutoCAD_CSharp_plug_in1
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an intance member then the enclosing class is 
        // intantiated for each document. If the member is a static member then
        // the enclosing class is NOT intantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.

        // Modal Command with localized name
        [CommandMethod("MyGroup", "Zigmeir", "MyCommandLocal", CommandFlags.Modal)]
        public void Zigmeir() // This method can have any name
        {
            // Put your command code here

            var a = new LayerPlugin();

            Application.ShowModalWindow(a);

         /*   Document doc = Application.DocumentManager.MdiActiveDocument;
            
            if (doc != null)
            {
                Editor ed = doc.Editor;
                var db = doc.Database;
                //ed.WriteMessage("Hello, this is your first command.");

                var layers = GetLayers(db, ed);

                var a = new AutocadPluginTestApp.MainWindow();

                Application.ShowModalWindow(a);
            }*/
        }


        private SelectionFilter SetupFilter()
        {
            TypedValue[] filterlist = new TypedValue[1];

            filterlist[0] = new TypedValue(0, "CIRCLE,LINE,POINT");

            SelectionFilter filter = new SelectionFilter(filterlist);

            return filter;
        }



        private SelectionFilter SetupFilter(ref string layerName)
        {
            TypedValue[] filterlist = new TypedValue[1];
            /*            {

                        //filterlist[0] = new TypedValue(0, "CIRCLE,LINE,POINT");
                            new TypedValue((int)DxfCode.LayerName, "0")
                        };*/

            filterlist.SetValue(new TypedValue((int)DxfCode.LayerName, layerName), 0);

            SelectionFilter filter = new SelectionFilter(filterlist);

            return filter;
        }



        


        // Modal Command with pickfirst selection
        [CommandMethod("MyGroup", "MyPickFirst", "MyPickFirstLocal", CommandFlags.Modal | CommandFlags.UsePickSet)]
        public void MyPickFirst() // This method can have any name
        {
            PromptSelectionResult result = Application.DocumentManager.MdiActiveDocument.Editor.GetSelection();
            if (result.Status == PromptStatus.OK)
            {
                // There are selected entities
                // Put your command using pickfirst set code here
            }
            else
            {
                // There are no selected entities
                // Put your command code here
            }
        }

        // Application Session Command with localized name
        [CommandMethod("MyGroup", "MySessionCmd", "MySessionCmdLocal", CommandFlags.Modal | CommandFlags.Session)]
        public void MySessionCmd() // This method can have any name
        {
            // Put your command code here
        }

        // LispFunction is similar to CommandMethod but it creates a lisp 
        // callable function. Many return types are supported not just string
        // or integer.
        [LispFunction("MyLispFunction", "MyLispFunctionLocal")]
        public int MyLispFunction(ResultBuffer args) // This method can have any name
        {
            // Put your command code here

            // Return a value to the AutoCAD Lisp Interpreter
            return 1;
        }

    }

}
