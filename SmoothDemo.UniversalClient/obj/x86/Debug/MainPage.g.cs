﻿#pragma checksum "C:\Code\SmoothDemo\SmoothDemo.UniversalClient\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F53093E6FDC0B65866005F4BF706BE28"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmoothDemo.UniversalClient
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.splitView = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 2:
                {
                    this.HamburgerButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 40 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.HamburgerButton).Click += this.HamburgerButton_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.MenuButton5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 4:
                {
                    this.MenuButton4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 5:
                {
                    this.MenuButton3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 6:
                {
                    this.skipButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 59 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.skipButton).Click += this.skipButton_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.refreshButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 53 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.refreshButton).Click += this.refreshButton_Click;
                    #line default
                }
                break;
            case 8:
                {
                    this.restart = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 48 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.restart).Click += this.restart_Click;
                    #line default
                }
                break;
            case 9:
                {
                    this.MenuButton1 = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 10:
                {
                    this.popup = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    #line 127 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)this.popup).Tapped += this.popupClose_Tapped;
                    #line default
                }
                break;
            case 11:
                {
                    this.previous = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 119 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.previous).Click += this.previous_Click;
                    #line default
                }
                break;
            case 12:
                {
                    this.next = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 121 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.next).Click += this.next_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

