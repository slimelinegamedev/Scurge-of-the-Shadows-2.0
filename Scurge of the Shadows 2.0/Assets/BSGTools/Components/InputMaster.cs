/**
Copyright (c) 2014, Michael Notarnicola
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Xml.Serialization;

namespace BSGTools {
	namespace IO {
		public class InputMaster : MonoBehaviour {
			public List<Control> controls = new List<Control>();

			public bool AnyKeyDown { get; private set; }
			public bool AnyKeyHeld { get; private set; }
			public bool AnyKeyUp { get; private set; }
			public bool inputBlocked = false;


			// Update is called once per frame
			void Update() {
				if(inputBlocked)
					return;

				foreach(var c in controls)
					c.Update();

				AnyKeyDown = controls.Any(c => c.Down);
				AnyKeyHeld = controls.Any(c => c.Held);
				AnyKeyUp = controls.Any(c => c.Up);
			}

			public void ResetAll() {
				foreach(var c in controls)
					c.Reset();
			}

			internal static InputMaster CreateMaster(params Control[] controls) {
				GameObject parent = new GameObject("_INPUTMASTER");
				parent.hideFlags = HideFlags.HideInHierarchy;

				var master = parent.AddComponent<InputMaster>();
				master.controls.AddRange(controls);
				return master;
			}

			/// <summary>
			/// Clears all controls and replaces them with the provided Control objects.
			/// </summary>
			/// <param name="controls">The controls to add after clearing</param>
			public void UpdateAllControls(params Control[] controls) {
				this.controls.Clear();
				this.controls.AddRange(controls);
			}

			/// <summary>
			/// Searches the control list for the provided Control objects
			/// and replaces them with said provided object
			/// </summary>
			/// <param name="controls">The Control objects to search and replace</param>
			public void UpdateControls(params Control[] controls) {
				foreach(var c in controls) {
					int index = this.controls.IndexOf(c);
					if(index == -1)
						throw new System.ArgumentException(string.Format("Could not find Control {0} in master control list. Aborting!", c.ToString()));
					this.controls[index] = c;
				}
			}
		}


		#region IO Controls
		/// <summary>
		/// The root level class for all Control types.
		/// In essence, it's functionality represents that of the DigitalControl
		/// 
		/// Maintains three states:
		/// 
		/// Down - The control was pressed this frame.
		/// Held - True as long as the control is held down.
		/// Up = The control was released this frame.
		/// </summary>
		[XmlInclude(typeof(DigitalControl))]
		[XmlInclude(typeof(AnalogControl))]
		[XmlInclude(typeof(OneWayControl))]
		[XmlInclude(typeof(AxisControl))]
		public abstract class Control {
			#region Read/Write Properties
			[XmlAttribute("key")]
			public KeyCode Key { get; set; }
			/// <summary>
			/// A reference name for the control.
			/// </summary>
			[XmlAttribute("name")]
			public string Name { get; set; }

			/// <summary>
			/// If true, this control will not be updated,
			/// and will store its current values until updated again.
			/// This is most commonly used with the Reset function.
			/// </summary>
			[XmlIgnore]
			public bool ControlBlocked { get; set; }
			#endregion

			/// <summary>
			/// Was the control was pressed this frame?
			/// </summary>
			[XmlIgnore]
			public bool Down { get; protected set; }
			/// <summary>
			/// True as long as the control is held down.
			/// </summary>
			[XmlIgnore]
			public bool Held { get; protected set; }
			/// <summary>
			/// Was the control was released this frame?
			/// </summary>
			[XmlIgnore]
			public bool Up { get; protected set; }

			/// <summary>
			/// For serialization ONLY
			/// </summary>
			protected Control() : this(KeyCode.None, null) { }

			public Control(KeyCode key) : this(key, null) { }

			public Control(KeyCode key, string name) {
				if(key == null)
					throw new System.ArgumentNullException("Key MUST be defined!");
				this.Key = key;
				this.Name = (name == null) ? string.Format("UNNAMED_{0}", GetRandomString()) : name;
			}

			private static string GetRandomString() {
				string path = System.IO.Path.GetRandomFileName();
				path = path.Replace(".", "");
				return path;
			}

			protected internal void Update() {
				if(ControlBlocked == true)
					return;

				Down = Input.GetKeyDown(Key);
				Held = Input.GetKey(Key);
				Up = Input.GetKeyUp(Key);

				UpdateControl();
			}

			protected abstract void UpdateControl();

			/// <summary>
			/// Resets all control values to 0 or false values.
			/// </summary>
			public virtual void Reset() {
				Down = false;
				Held = false;
				Up = false;
			}

			public override int GetHashCode() {
				return Key.GetHashCode() * Name.GetHashCode();
			}

			public override bool Equals(object obj) {
				Control castedObj = obj as Control;
				if(castedObj == null)
					return false;

				if(this.GetType() != castedObj.GetType())
					return false;

				return this.Name == castedObj.Name;
			}

			public override string ToString() {
				return string.Format("{0} ({1})", Name, Key.ToString());
			}

			#region Utility Methods
			public static bool AnyHeld(params Control[] controls) {
				return controls.Any(c => c.Held);
			}

			public static bool AllHeld(params Control[] controls) {
				return controls.All(c => c.Held);
			}

			public static bool AnyUp(params Control[] controls) {
				return controls.Any(c => c.Up);
			}

			public static bool AllUp(params Control[] controls) {
				return controls.All(c => c.Up);
			}

			public static bool AnyDown(params Control[] controls) {
				return controls.Any(c => c.Down);
			}

			public static bool AllDown(params Control[] controls) {
				return controls.All(c => c.Down);
			}
			#endregion
		}

		/// <summary>
		/// Defines any control scheme that represents control
		/// status via a floating point value.
		/// </summary>
		[XmlInclude(typeof(OneWayControl))]
		[XmlInclude(typeof(AxisControl))]
		public abstract class AnalogControl : Control {
			#region Read/Write Properties
			[XmlAttribute("gravity")]
			public float Gravity { get; set; }
			[XmlAttribute("dead")]
			public float Dead { get; set; }
			[XmlAttribute("sensitivity")]
			public float Sensitivity { get; set; }
			[XmlAttribute("snap")]
			public bool Snap { get; set; }
			#endregion

			[XmlIgnore]
			public float Value { get; protected set; }
			[XmlIgnore]
			public float FixedValue { get; protected set; }
			[XmlIgnore]
			protected float realValue = 0f;

			protected AnalogControl()
				: base() {
				Gravity = 1f;
				Dead = 0f;
				Sensitivity = 1f;
			}

			public AnalogControl(KeyCode key)
				: this(key, "", 1f, 1f, 0f, false) { }

			public AnalogControl(KeyCode key, string name)
				: this(key, name, 1f, 1f, 0f, false) { }

			public AnalogControl(KeyCode key, string name, float sensitivity)
				: this(key, name, sensitivity, 1f, 0f, false) { }

			public AnalogControl(KeyCode key, string name, float sensitivity, float gravity)
				: this(key, name, sensitivity, gravity, 0f, false) { }

			public AnalogControl(KeyCode key, string name, float sensitivity, float gravity, float dead)
				: this(key, name, sensitivity, gravity, dead, false) { }

			public AnalogControl(KeyCode key, string name, float sensitivity, float gravity, float dead, bool snap)
				: base(key, name) {
				this.Sensitivity = sensitivity;
				this.Gravity = gravity;
				this.Dead = dead;
				this.Snap = snap;
			}

			public override void Reset() {
				base.Reset();
				Value = 0f;
				FixedValue = 0f;
				realValue = 0f;
			}
		}

		/// <summary>
		/// Defines a purely digital, state-based control scheme.
		/// </summary>
		public class DigitalControl : Control {

			protected DigitalControl() : base() { }

			public DigitalControl(KeyCode key) : base(key) { }

			public DigitalControl(KeyCode key, string name)
				: base(key, name) { }

			//Full functionality provided by base class Control
			protected override void UpdateControl() { }
		}

		/// <summary>
		/// Defines a control scheme that modifies a 0...1 float value.
		/// This functionality is fairly similar to Unity's own Input system.
		/// </summary>
		public class OneWayControl : AnalogControl {
			#region Constructors
			protected OneWayControl() : base() { }

			public OneWayControl(KeyCode key) : base(key) { }

			public OneWayControl(KeyCode key, string name)
				: base(key, name) { }

			public OneWayControl(KeyCode key, string name, float sensitivity)
				: base(key, name, sensitivity) { }

			public OneWayControl(KeyCode key, string name, float sensitivity, float gravity)
				: base(key, name, sensitivity, gravity) { }

			public OneWayControl(KeyCode key, string name, float sensitivity, float gravity, float dead)
				: base(key, name, sensitivity, gravity, dead) { }

			public OneWayControl(KeyCode key, string name, float sensitivity, float gravity, float dead, bool snap)
				: base(key, name, sensitivity, gravity, dead, snap) { }
			#endregion

			protected override void UpdateControl() {
				FixedValue = (Held) ? 1f : 0f;
				//realValue = (Held) ? Mathf.Lerp(0f, 1f, Time.deltaTime * Sensitivity) : Mathf.Lerp(1f, 0f, Time.deltaTime * Gravity);

				realValue = (Held) ? realValue + (Time.deltaTime * Sensitivity) : realValue - (Time.deltaTime * Gravity);
				realValue = Mathf.Clamp01(realValue);

				if(Held == false && Snap)
					realValue = 0f;

				Value = realValue;

				//We dont want to mess with the real value
				//When considering dead values
				if(Dead > 0f && realValue <= Dead)
					Value = 0f;
			}
		}

		/// <summary>
		/// Defines a control scheme that modifies a -1...0...1 float value.
		/// This functionality was purposely designed to be extremely similar to Unity's
		/// own Input system.
		/// </summary>
		public class AxisControl : AnalogControl {
			[XmlAttribute("negativeKey")]
			public KeyCode NegativeKey { get; set; }

			#region Read-Only
			[XmlIgnore]
			public bool PositiveDown { get; private set; }
			[XmlIgnore]
			public bool NegativeDown { get; private set; }
			[XmlIgnore]
			public bool PositiveHeld { get; private set; }
			[XmlIgnore]
			public bool NegativeHeld { get; private set; }
			[XmlIgnore]
			public bool PositiveUp { get; private set; }
			[XmlIgnore]
			public bool NegativeUp { get; private set; }
			#endregion

			#region Constructors
			protected AxisControl() : base() { }

			public AxisControl(KeyCode positiveKey, KeyCode negativeKey)
				: base(positiveKey) {
				this.NegativeKey = negativeKey;
			}

			public AxisControl(KeyCode positiveKey, KeyCode negativeKey, string name)
				: base(positiveKey, name) {
				this.NegativeKey = negativeKey;
			}

			public AxisControl(KeyCode positiveKey, KeyCode negativeKey, string name, float sensitivity)
				: base(positiveKey, name, sensitivity) {
				this.NegativeKey = negativeKey;
			}

			public AxisControl(KeyCode positiveKey, KeyCode negativeKey, string name, float sensitivity, float gravity)
				: base(positiveKey, name, sensitivity, gravity) {
				this.NegativeKey = negativeKey;
			}

			public AxisControl(KeyCode positiveKey, KeyCode negativeKey, string name, float sensitivity, float gravity, float dead)
				: base(positiveKey, name, sensitivity, gravity, dead) {
				this.NegativeKey = negativeKey;
			}

			public AxisControl(KeyCode positiveKey, KeyCode negativeKey, string name, float gravity, float dead, float sensitivity, bool snap) {
				this.NegativeKey = negativeKey;
			}
			#endregion

			protected override void UpdateControl() {
				PositiveDown = Input.GetKeyDown(Key);
				NegativeDown = Input.GetKeyDown(NegativeKey);
				PositiveHeld = Input.GetKey(Key);
				NegativeHeld = Input.GetKey(NegativeKey);
				PositiveUp = Input.GetKeyUp(Key);
				NegativeUp = Input.GetKeyUp(NegativeKey);

				Down = PositiveDown || NegativeDown;
				Up = PositiveUp || NegativeUp;
				Held = PositiveHeld || NegativeHeld;

				if(PositiveHeld && NegativeHeld == false)
					FixedValue = 1f;
				else if(PositiveHeld == false && NegativeHeld)
					FixedValue = -1f;
				else
					FixedValue = 0f;

				if(realValue > 0f || PositiveHeld) {
					realValue = (PositiveHeld) ? realValue + (Time.deltaTime * Sensitivity) : realValue - (Time.deltaTime * Gravity);
					realValue = Mathf.Clamp01(realValue);
				}
				else if(realValue < 0f || NegativeHeld) {
					realValue = (NegativeHeld) ? realValue - (Time.deltaTime * Sensitivity) : realValue + (Time.deltaTime * Gravity);
					realValue = Mathf.Clamp(realValue, -1f, 0f);
				}

				if(realValue > 0f && NegativeHeld && Snap)
					realValue = 0f;
				else if(realValue < 0f && PositiveHeld && Snap)
					realValue = 0f;

				Value = realValue;

				//We dont want to mess with the real value
				//When considering dead values
				if(Mathf.Abs(realValue) <= Mathf.Abs(Dead))
					Value = 0f;
			}

			public override void Reset() {
				base.Reset();
				PositiveHeld = false;
				NegativeHeld = false;

				PositiveDown = false;
				NegativeDown = false;

				PositiveUp = false;
				NegativeUp = false;
			}

			#region Utility Methods
			public static bool AnyPositiveHeld(params AxisControl[] controls) {
				return controls.Any(c => c.PositiveHeld);
			}

			public static bool AllPositiveHeld(params AxisControl[] controls) {
				return controls.All(c => c.PositiveHeld);
			}

			public static bool AnyNegativeHeld(params AxisControl[] controls) {
				return controls.Any(c => c.NegativeHeld);
			}

			public static bool AllNegativeHeld(params AxisControl[] controls) {
				return controls.All(c => c.NegativeHeld);
			}

			public static bool AnyPositiveDown(params AxisControl[] controls) {
				return controls.Any(c => c.PositiveDown);
			}

			public static bool AllPositiveDown(params AxisControl[] controls) {
				return controls.All(c => c.PositiveDown);
			}

			public static bool AnyNegativeDown(params AxisControl[] controls) {
				return controls.Any(c => c.NegativeDown);
			}

			public static bool AllNegativeDown(params AxisControl[] controls) {
				return controls.All(c => c.NegativeDown);
			}

			public static bool AnyPositiveUp(params AxisControl[] controls) {
				return controls.Any(c => c.PositiveUp);
			}

			public static bool AllPositiveUp(params AxisControl[] controls) {
				return controls.All(c => c.PositiveUp);
			}

			public static bool AnyNegativeUp(params AxisControl[] controls) {
				return controls.Any(c => c.NegativeUp);
			}

			public static bool AllNegativeUp(params AxisControl[] controls) {
				return controls.All(c => c.NegativeUp);
			}
			#endregion
		}
		#endregion
	}
}