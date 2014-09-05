using System;
using UnityEngine;
using KSP.IO;

/******************************************************************************
 * Copyright (c) 2013-2014, Justin Bengtson
 * Copyright (c) 2014, Maik Schreiber
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met: 
 * 
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 * 
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 ******************************************************************************/

namespace RegexKSP {
	internal static class GUIParts {
        // styling code added by DaMichel
        private static class CurrentSkin
        {
            internal const int UNINITIALIZED = 0;
            internal const int KSP = 1;
            internal const int UNITY = 2;
        };
        private  static int currentSkin = CurrentSkin.UNINITIALIZED;
        private  static GUISkin  skin = null;
        internal static GUIStyle styleFramed = null;
        internal static GUIStyle styleFrameless = null;
        internal static GUIStyle styleNodeControlTextField = null; // prograde, normal, radial text field style
        internal static GUIStyle styleDataDisplayText = null; // DV, ejection angle etc.
        internal static GUIStyle stylePagerButtons = null;
        //internal static GUIStyle styleNonResizingButton = null;
        internal static float    leftColumnDataWidth = 110;
        internal static float    leftColumnNodeWidth = 75;
        internal static float    leftColumnWidth     = 100;
        internal static float    plusMinusButtonWidth = 50;
        internal static float    nodeControlTextFieldWidth = 100;

        internal static void initGUI(bool useKspSkin)
        {
            if (currentSkin != (useKspSkin ? CurrentSkin.KSP : CurrentSkin.UNITY))
            {
                if (useKspSkin)
                {
                    //Debug.Log("PreciseNode: switch to KSP skin");
                    Color labelColor = new Color(200, 200, 200);

                    skin = (GUISkin)GUISkin.Instantiate(HighLogic.Skin); // make a copy of the KSP skin

                    RectOffset r = skin.window.padding;
                    skin.window.padding = new RectOffset(3,2,r.top,3); // the KSP skin has large top padding and a few pixels on the other edges

                    styleFramed = skin.box;
                    //styleFramed.margin = new RectOffset(1,1,1,1); // seems to have some 1px top, left and right margin by default

                    styleFrameless = new GUIStyle();
                    styleFrameless.padding = styleFramed.padding;
                    styleFrameless.margin  = styleFramed.margin;

                    //skin.label.fontSize = 13;
                    //skin.box.fontSize = 13;
                    //skin.button.fontSize = 13;
                    //skin.textField.fontSize = 13;
            
                    skin.button.alignment = TextAnchor.MiddleCenter;
                    skin.button.padding = new RectOffset(4, 4, 2, 1);
                    //skin.button.margin  = new RectOffset(0, 0, 0, 0);
                    skin.button.fontStyle = FontStyle.Normal;
            
                    skin.textField.padding = new RectOffset(5, 4, 2, 1);
                    //skin.textField.margin = new RectOffset(0, 0, 0, 0);
                    skin.textField.alignment = TextAnchor.MiddleLeft;
                    skin.textField.fontStyle = FontStyle.Normal;
                
                    styleNodeControlTextField = new GUIStyle(skin.textField);
                    styleNodeControlTextField.normal.textColor = Color.white;
                    styleNodeControlTextField.hover.textColor = Color.white;
                    styleNodeControlTextField.active.textColor = Color.white;
                    styleNodeControlTextField.focused.textColor = Color.white;
                    //styleNodeControlTextField.stretchWidth = false;
                    //styleNodeControlTextField.fixedWidth = nodeControlTextFieldWidth;

                    skin.label.padding = new RectOffset(1, 1, 2, 1);
                    //skin.label.margin = new RectOffset(0, 0, 0, 0);
                    skin.label.alignment = TextAnchor.MiddleLeft;
                    skin.label.wordWrap = false;

                    styleDataDisplayText = new GUIStyle(skin.label);

                    skin.toggle.stretchWidth = true;
                    skin.label.normal.textColor = labelColor;

                    stylePagerButtons = new GUIStyle(skin.button);
                    stylePagerButtons.fontStyle = FontStyle.Bold;
                    stylePagerButtons.padding = new RectOffset(6, 6, 6, 4);

                    //styleNonResizingButton = new GUIStyle(skin.button);
                    //styleNonResizingButton.stretchWidth = false;
                    //styleNonResizingButton.padding = skin.button.padding.add(3, 3, 0, 0);
                    
                    currentSkin = CurrentSkin.KSP;
                }
                else
                {
                    //Debug.Log("PreciseNode: switch to Unity skin");
                    GUI.skin = null;
                    skin = (GUISkin)GUISkin.Instantiate(GUI.skin);
                    styleFramed = new GUIStyle();
                    styleFramed.padding = skin.box.padding;
                    styleFramed.margin  = skin.box.margin;
                    styleFrameless = new GUIStyle();
                    styleFrameless.padding = skin.box.padding;
                    styleFrameless.margin = skin.box.margin;
                    styleNodeControlTextField = new GUIStyle(skin.textField);
                    styleDataDisplayText = new GUIStyle(skin.label); // DV, ejection angle etc.
                    stylePagerButtons = new GUIStyle(skin.button);
                    //styleNonResizingButton = new GUIStyle(skin.button);
                    currentSkin = CurrentSkin.UNITY;
                }
            }
            //else Debug.Log("PreciseNode: keeping skin " + currentSkin);
            GUI.skin = skin;
        }

		internal static void drawDoubleLabel(String text1, float width1, String text2, float width2) {
			GUILayout.BeginHorizontal();
			GUILayout.Label(text1, GUILayout.Width(width1));
            GUILayout.Label(text2, styleDataDisplayText, GUILayout.Width(width2));
			GUILayout.EndHorizontal();
		}

		internal static void drawButton(String text, Color bgColor, Action callback, GUIStyle style, params GUILayoutOption[] options) {
			Color defaultColor = GUI.backgroundColor;
			GUI.backgroundColor = bgColor;
			if(GUILayout.Button(text, style, options)) {
				callback();
			}
			GUI.backgroundColor = defaultColor;
		}

        internal static void drawButton(String text, Color bgColor, Action callback, params GUILayoutOption[] options)
        {
            drawButton(text, bgColor, callback, skin.button, options);
        }

		internal static void drawConicsControls(PreciseNodeOptions options) {
			PatchedConicSolver solver = NodeTools.getSolver();
			Color defaultColor = GUI.backgroundColor;
			// Conics mode controls
            GUILayout.BeginVertical(GUIParts.styleFramed);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Conics mode: ", GUILayout.Width(leftColumnWidth));
			    for (int mode = 0; mode <= 4; mode++) {
				    drawButton(mode.ToString(), (options.conicsMode == mode) ? Color.yellow : defaultColor, () => {
					    options.setConicsMode(mode);
				    });
			    }
                GUILayout.EndHorizontal();

			    // conics patch limit editor.
			    GUILayout.BeginHorizontal();
			    GUILayout.Label("Change conics samples:", GUILayout.ExpandWidth(true));
			    drawPlusMinusButtons(solver.IncreasePatchLimit, solver.DecreasePatchLimit);
			    GUILayout.EndHorizontal();
            GUILayout.EndVertical();
		}

		internal static void drawPlusMinusButtons(Action plus, Action minus, bool plusEnabled = true, bool minusEnabled = true) {
			bool oldEnabled = GUI.enabled;
			GUI.enabled = plusEnabled || minusEnabled;
			drawButton("+/-", GUI.backgroundColor, () => {
				switch (Event.current.button) {
					case 0:
						if (plusEnabled) {
							plus();
						}
						break;
					case 1:
						if (minusEnabled) {
							minus();
						}
						break;
				}
			}, skin.button, GUILayout.Width(plusMinusButtonWidth));
			GUI.enabled = oldEnabled;
		}
	}
}
